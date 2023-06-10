using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AvatarGenerator : MonoBehaviour
{
    //스프라이트를 표시하기 위한 변수
    public SpriteRenderer noseSprite;
    public SpriteRenderer eyeRightSprite;
    public SpriteRenderer eyeLeftSprite;
    public SpriteRenderer eyebrowRightSprite;
    public SpriteRenderer eyebrowLeftSprite;
    public SpriteRenderer mouthSprite;
    public GameObject faceObject;
    public GameObject avatarObject;

    //스프라이트/오브젝트 배열 생성을 위한 변수
    public SpriteAtlas facepartsAtlas;
    public Sprite[] noseSprites;
    public Sprite[] eyeSprites;
    public Sprite[] eyebrowSprites;
    public Sprite[] mouthSprites;
    //public GameObject[] faceModels;

    private Sprite[] GetSpritesByPrefix(SpriteAtlas atlas, string prefix)
    {
        List<Sprite> sprites = new List<Sprite>();
        Sprite[] allSprites = new Sprite[atlas.spriteCount];
        atlas.GetSprites(allSprites);

        foreach (Sprite sprite in allSprites)
        {
            if (sprite.name.StartsWith(prefix))
            {
                sprites.Add(sprite);
            }
        }
        return sprites.ToArray();
    }

    public void SpriteAssignment() {
        //스프라이트 아틀라스 초기화
        facepartsAtlas = Resources.Load<SpriteAtlas>("total_faceparts");

        // 스프라이트 배열에 스프라이트 아틀라스에서 가져온 스프라이트들을 할당함
        noseSprites = GetSpritesByPrefix(facepartsAtlas, "nose");
        eyeSprites = GetSpritesByPrefix(facepartsAtlas, "eye");
        eyebrowSprites = GetSpritesByPrefix(facepartsAtlas, "eyebrow");
        mouthSprites = GetSpritesByPrefix(facepartsAtlas, "mouth");

        // faceModels 배열에 각 face 파츠의 모델을 할당함
        /*faceModels = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            string faceModelPath = "face/face_" + i; // face 파츠의 모델 파일 경로
            faceModels[i] = Resources.Load<GameObject>(faceModelPath);
        }*/
    }

    // avatarObject에 face 파츠의 모델을 자식으로 로드
    /*public void LoadFaceModel(int partIndex)
    {
        GameObject faceModel = Instantiate(faceModels[partIndex]);
        faceModel.transform.SetParent(faceObject.transform);
    }*/

    // 파츠별로 스프라이트 및 모델을 변경
    public void SettingPart(string partName, int partIndex)
    {
        switch (partName)
        {
            case "nose":
                noseSprite.sprite = noseSprites[partIndex];
                break;
            case "eye":
                eyeRightSprite.sprite = eyeSprites[partIndex];
                eyeLeftSprite.sprite = eyeSprites[partIndex];
                break;
            case "eyebrow":
                eyebrowRightSprite.sprite = eyebrowSprites[partIndex];
                eyebrowLeftSprite.sprite = eyebrowSprites[partIndex];
                break;
            case "mouth":
                mouthSprite.sprite = mouthSprites[partIndex];
                break;
            /*case "face":
                LoadFaceModel(partIndex);
                break;*/
        }
    }

    // 아바타를 조합하여 저장하는 메서드
    /*public GameObject CombineAvatar(int partIndex)
    {
        // 새로운 게임 오브젝트를 생성하여 아바타 부분들을 조합
        GameObject avatarObject = new GameObject("Avatar");
        avatarObject.transform.position = Vector3.zero;

        // 스프라이트 부분들을 조합
        GameObject spriteObject = new GameObject("Sprite");
        spriteObject.transform.SetParent(avatarObject.transform);

        noseSprite.transform.SetParent(spriteObject.transform);
        eyeRightSprite.transform.SetParent(spriteObject.transform);
        eyeLeftSprite.transform.SetParent(spriteObject.transform);
        eyebrowRightSprite.transform.SetParent(spriteObject.transform);
        eyebrowLeftSprite.transform.SetParent(spriteObject.transform);
        mouthSprite.transform.SetParent(spriteObject.transform);

        // 얼굴 obj 파일을 조합
        //GameObject faceModel = Instantiate(faceModels[partIndex]);
        //faceModel.transform.SetParent(avatarObject.transform);

        return avatarObject;
    }*/

}
