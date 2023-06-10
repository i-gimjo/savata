using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FaceLandmarksController : MonoBehaviour
{
    private AvatarGenerator avatarGenerator;
    private static PartsCalculator partsCalculator;

    private string url = "http://54.180.29.90:5000";
    public float[,] landmarks;

    //화면 전환을 위한 필드
    //gameobject 삭제부분을 canvas로 바꾸고 canvasgroup(canvas에 컴포넌트로 추가)
    public Canvas WebCamScreen;
    public Canvas ShowAvatarScreen;
    public CanvasGroup WebCamCanv;
    public CanvasGroup ShowAvatarCanv;

    public Camera webCamCamera;
    public Camera showAvatarCamera;

    public RawImage webcamImage;

    private WebCamTexture webcamTexture;

    void Awake()
    {
        avatarGenerator = GetComponent<AvatarGenerator>();
        if (partsCalculator == null) // 체크 후 생성 및 초기화
        {
            partsCalculator = gameObject.AddComponent<PartsCalculator>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Send put request to server
        StartCoroutine(RequestCameraPermission());
        
        WebCamCanv.alpha = 1;
        WebCamCanv.interactable = true;
        WebCamScreen.enabled = true;
        webCamCamera.enabled = true;

        ShowAvatarCanv.alpha = 0;
        ShowAvatarCanv.interactable = false;
        ShowAvatarScreen.enabled = false;
        showAvatarCamera.enabled = false;
       
    }

    private void TransitionScreen()
    {
           
        ShowAvatarCanv.alpha = 1;
        ShowAvatarCanv.interactable = true;
        ShowAvatarScreen.enabled = true;
        showAvatarCamera.enabled= true;


        WebCamCanv.alpha = 0;
        WebCamCanv.interactable = false;
        WebCamScreen.enabled = false;
        webCamCamera.enabled = false;



    }

    public void LoadScene()
    {
        
        SceneManager.LoadScene("SecondScene");
    }

    IEnumerator RequestCameraPermission()
    {
        // Request permission
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        // Check permission
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.LogError("Camera permission denied");
            yield break;
        }

        // Start camera
        webcamTexture = new WebCamTexture();
        webcamImage.texture = webcamTexture; // Assign webcam texture to the RawImage
        webcamTexture.Play();

        // Wait until camera starts
        while (!webcamTexture.didUpdateThisFrame)
        {
            yield return null;
        }
    }

    private bool IsValidImage(byte[] imageData)
    {
        // Check if image data is null or empty
        if (imageData == null || imageData.Length == 0)
        {
            Debug.LogError("Invalid image data: null or empty");
            return false;
        }

        // Check if image data can be decoded to a texture
        Texture2D tex = new Texture2D(2, 2);
        if (!tex.LoadImage(imageData))
        {
            Debug.LogError("Invalid image data: cannot be decoded to a texture");
            return false;
        }
        return true;
    }

    public void CaptureImage()
    {
        // Check if webcamTexture is valid
        if (webcamTexture == null || !webcamTexture.isPlaying)
        {
            Debug.LogError("WebcamTexture is not valid or not playing");
            return;
        }

        // Get texture from camera
        Texture2D tex = new Texture2D(webcamTexture.width, webcamTexture.height);
        tex.SetPixels(webcamTexture.GetPixels());
        tex.Apply();

        // Convert Texture2D to byte array
        byte[] bytes = tex.EncodeToPNG();

        // Send put request to server
        StartCoroutine(Put(bytes));
        /*GameObject avatarObject = avatarGenerator.CombineAvatar(partsCalculator.face);*/

        TransitionScreen();
    }

    IEnumerator Put(byte[] imageBytes)
    {
        if (!IsValidImage(imageBytes))
        {
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequest.Put(url, imageBytes))
        {
            request.method = "PUT";
            request.SetRequestHeader("Content-Type", "application/octet-stream");
            request.SetRequestHeader("Accept", "application/json");

            // Send the request and wait for the response
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                yield break;
            }

            // Parse JSON response
            JObject response = JObject.Parse(request.downloadHandler.text);
            JArray landmarksArray = (JArray)response["landmarks"];

            // Convert JArray to 2D float array
            landmarks = new float[landmarksArray.Count, 2];
            for (int i = 0; i < landmarksArray.Count; i++)
            {
                landmarks[i, 0] = (float)landmarksArray[i][0];
                landmarks[i, 1] = (float)landmarksArray[i][1];
            }

            // Call the function to calculate the parts
            if (partsCalculator != null)
            {
                partsCalculator.CalculateParts(landmarks);
                GenerateAvatarFace();
            }
            else
            {
                Debug.LogError("PartsCalculator not found.");
            }
        }
    }

    void GenerateAvatarFace()
    {
        // Call the function to generate avatar
        if (avatarGenerator != null)
        {
            avatarGenerator.SpriteAssignment();

            //Debug.Log("nose : " + partsCalculator.nose);
            avatarGenerator.SettingPart("nose", partsCalculator.nose);
            //Debug.Log("eye : " + partsCalculator.eye);
            avatarGenerator.SettingPart("eye", partsCalculator.eye);
            //Debug.Log("eyebrow : " + partsCalculator.eyebrow);
            avatarGenerator.SettingPart("eyebrow", partsCalculator.eyebrow);
            //Debug.Log("mouth : " + partsCalculator.mouth);
            avatarGenerator.SettingPart("mouth", partsCalculator.mouth);
            //Debug.Log("face : " + partsCalculator.face);
            //avatarGenerator.SettingPart("face", partsCalculator.face);

            // Remove other face objects except the selected face
            Transform faceTransform = avatarGenerator.faceObject.transform;
            for (int i = 0; i < faceTransform.childCount; i++)
            {
                Transform child = faceTransform.GetChild(i);
                // Check if the child is not the selected face
                if (child.gameObject.name != "face_" + partsCalculator.face)
                {
                    DestroyImmediate(child.gameObject);
                    i--; // Decrement the counter to handle the child removal
                }
            }
            // face에 따라 스프라이트 렌더러의 위치 조정

            avatarGenerator.noseSprite.transform.localPosition = new Vector3(200f, -70f, -45f);
            avatarGenerator.eyeRightSprite.transform.localPosition = new Vector3(400f, 60f, -30f);
            avatarGenerator.eyeLeftSprite.transform.localPosition = new Vector3(0f, 60f, -30f);
            avatarGenerator.eyebrowRightSprite.transform.localPosition = new Vector3(400f, 220f, -5f);
            avatarGenerator.eyebrowLeftSprite.transform.localPosition = new Vector3(0f, 220f, -5f);
            avatarGenerator.mouthSprite.transform.localPosition = new Vector3(200f, -200f, 0f);

            switch (partsCalculator.face)
            {
                case 0:
                    avatarGenerator.noseSprite.transform.localPosition = new Vector3(200f, -70f, 0f);
                    avatarGenerator.eyeRightSprite.transform.localPosition = new Vector3(400f, 60f, 5f);
                    avatarGenerator.eyeLeftSprite.transform.localPosition = new Vector3(0f, 60f, 5f);
                    avatarGenerator.eyebrowRightSprite.transform.localPosition = new Vector3(400f, 220f, 20f);
                    avatarGenerator.eyebrowLeftSprite.transform.localPosition = new Vector3(0f, 220f, 20f);
                    avatarGenerator.mouthSprite.transform.localPosition = new Vector3(200f, -200f, 50f);
                    break;
                case 1:
                    avatarGenerator.noseSprite.transform.localPosition = new Vector3(200f, -70f, 0f);
                    avatarGenerator.eyeRightSprite.transform.localPosition = new Vector3(400f, 60f, 5f);
                    avatarGenerator.eyeLeftSprite.transform.localPosition = new Vector3(0f, 60f, 5f);
                    avatarGenerator.eyebrowRightSprite.transform.localPosition = new Vector3(400f, 220f, 20f);
                    avatarGenerator.eyebrowLeftSprite.transform.localPosition = new Vector3(0f, 220f, 20f);
                    avatarGenerator.mouthSprite.transform.localPosition = new Vector3(200f, -200f, 50f);
                    break;
                case 2:
                    avatarGenerator.noseSprite.transform.localPosition = new Vector3(200f, -70f, -45f);
                    avatarGenerator.eyeRightSprite.transform.localPosition = new Vector3(400f, 60f, -30f);
                    avatarGenerator.eyeLeftSprite.transform.localPosition = new Vector3(0f, 60f, -30f);
                    avatarGenerator.eyebrowRightSprite.transform.localPosition = new Vector3(400f, 220f, -5f);
                    avatarGenerator.eyebrowLeftSprite.transform.localPosition = new Vector3(0f, 220f, -5f);
                    avatarGenerator.mouthSprite.transform.localPosition = new Vector3(200f, -200f, 0f);
                    break;
                case 3:
                    avatarGenerator.noseSprite.transform.localPosition = new Vector3(200f, -70f, -45f);
                    avatarGenerator.eyeRightSprite.transform.localPosition = new Vector3(400f, 60f, -30f);
                    avatarGenerator.eyeLeftSprite.transform.localPosition = new Vector3(0f, 60f, -30f);
                    avatarGenerator.eyebrowRightSprite.transform.localPosition = new Vector3(400f, 220f, -5f);
                    avatarGenerator.eyebrowLeftSprite.transform.localPosition = new Vector3(0f, 220f, -5f);
                    avatarGenerator.mouthSprite.transform.localPosition = new Vector3(200f, -200f, 0f);
                    break;
                case 4:
                    avatarGenerator.noseSprite.transform.localPosition = new Vector3(200f, -70f, -45f);
                    avatarGenerator.eyeRightSprite.transform.localPosition = new Vector3(400f, 60f, -30f);
                    avatarGenerator.eyeLeftSprite.transform.localPosition = new Vector3(0f, 60f, -30f);
                    avatarGenerator.eyebrowRightSprite.transform.localPosition = new Vector3(400f, 220f, -5f);
                    avatarGenerator.eyebrowLeftSprite.transform.localPosition = new Vector3(0f, 220f, -5f);
                    avatarGenerator.mouthSprite.transform.localPosition = new Vector3(200f, -200f, 0f);
                    break;
            }

            Invoke("LoadScene", 3f);
        }
        else
        {
            Debug.LogError("AvatarGenerator not found.");
        }
    }
}

