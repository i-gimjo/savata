using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("ship"))
        {
            Debug.Log("�浹");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    
}
