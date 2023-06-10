using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Default defaultscript;


    private void Start()
    {
        if(gameObject.activeSelf)
        {
            // �ڽ� ������Ʈ ��������
            for (int i = 0; i < transform.childCount; i++)
            {
                // 1�� �ڽ� ������Ʈ�� �����ϰ� ��Ȱ��ȭ
                if (i != 0 && i != 1)
                {
                    Transform child = transform.GetChild(i);
                    child.gameObject.SetActive(false);
                }
            }
        }
        defaultscript = transform.GetChild(1).gameObject.GetComponent<Default>();
    }
    
    private void Update()
    {
        if(defaultscript.test)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
