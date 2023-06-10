using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObject : MonoBehaviour
{
    public GameObject getObject;
    void Start()
    {
        PassFaceObject instance = PassFaceObject.GetInstance();
        GameObject transferredObject = instance.objectToPass;
        transferredObject.transform.parent = getObject.transform; // objectToPass를 getobject의 자식으로 설정
    
        transferredObject.transform.localPosition = Vector3.zero; // x=0, y=0, z=0
        transferredObject.transform.localScale = new Vector3(0.004f, 0.004f, 0.004f); // x=0.004, y=0.004, z=0.004
    
    }
}
