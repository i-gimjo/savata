using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class FaceLandmarksController : MonoBehaviour
{
    private string url = "http://13.124.65.108:5000";
    public float[,] landmarks;

    // Start is called before the first frame update
    void Start()
    {
        // Load test image
		Texture2D tex = Resources.Load<Texture2D>("iu_face");

        // Convert Texture2D to byte array
        byte[] bytes = tex.EncodeToPNG();

        // Send put request to server
		StartCoroutine(Put(bytes));
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

    IEnumerator Put(byte[] imageBytes)
    {
        if (!IsValidImage(imageBytes))
        {
            yield break;
        }

        // Create UnityWebRequest object and set method to PUT
        UnityWebRequest request = UnityWebRequest.Put(url, imageBytes);
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
        GameObject calObj = GameObject.Find("Calculator");
        PartsCalculator partsCalculator = calObj.GetComponent<PartsCalculator>();
        if (partsCalculator != null)
        {
            partsCalculator.CalculateParts(landmarks);
        }
        else
        {
            Debug.LogError("PartsCalculator not found.");
        }


        // Return landmarks
        //yield return landmarks;

        // Print landmarks to console
        /*for (int i = 0; i < landmarks.GetLength(0); i++)
        {
            Debug.Log("Landmark " + i + ": (" + landmarks[i, 0] + ", " + landmarks[i, 1] + ")");
        }*/
    }
}
