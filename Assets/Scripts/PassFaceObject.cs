using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassFaceObject : MonoBehaviour
{
    private static PassFaceObject instance;
    public GameObject objectToPass;

    private void Awake()
    {
        // objectToPass 인스턴스가 이미 존재하는지 확인하고, 이미 존재한다면 새로 생성하지 않고 기존 인스턴스를 사용합니다.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(objectToPass); // 씬 전환 시에도 오브젝트가 유지되도록 설정합니다.
        }
        else
        {
            Destroy(objectToPass);
        }
    }

    public static PassFaceObject GetInstance()
    {
        return instance;
    }
}
