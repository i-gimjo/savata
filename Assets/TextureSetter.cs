using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSetter : MonoBehaviour
{
    public Texture2D[] textures; // 적용할 텍스쳐 리스트
    public GameObject[] parts; // 재질을 적용할 파트 리스트

    private void Start()
    {
        Material[] mats = new Material[parts.Length]; // 복사한 재질을 저장할 배열
        Material originalMat = parts[0].GetComponent<Renderer>().material; // 재질을 복사할 오브젝트

        for (int i = 0; i < parts.Length; i++)
        {
            mats[i] = new Material(originalMat); // 재질 복사
            mats[i].mainTexture = textures[i]; // 텍스쳐 적용
            parts[i].GetComponent<Renderer>().material = mats[i]; // 복사한 재질 적용
        }
    }
}
