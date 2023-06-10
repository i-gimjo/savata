using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class audioplayer : MonoBehaviour
{
    
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void OnMouseUp()
    {
        
        audioSource.Play();
    }

}
