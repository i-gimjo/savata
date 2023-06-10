using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default: MonoBehaviour
{

    private AudioSource audioSource;

    private Vector3 mousePosition;
    private float offsetX, offsetY, offsetZ;
    public Vector3 Position_2;
    private static bool mouseButtonReleased;
    public string objName; 
    public string combObj;
     
    private string nextObj;

    public bool test = false;
    private bool isPlaying = false;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isPlaying = false;


    }
    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mouseButtonReleased = false;
        mousePosition = Input.mousePosition - GetMousePos();

    }
    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
    }

    private void OnMouseUp()
    {
        mouseButtonReleased = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        string thisGameobjectName;
        string collisionGameobjectName;

        thisGameobjectName = gameObject.name;
        collisionGameobjectName = collision.gameObject.name;

        if (mouseButtonReleased && thisGameobjectName == objName && collisionGameobjectName == combObj)
        {
            audioSource.Play();
            isPlaying = true;

            gameObject.transform.localPosition = Position_2;
            gameObject.transform.SetParent(collision.transform);
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
            mouseButtonReleased = false;
            
            collisionGameobjectName = objName.Substring(name.IndexOf("_")+1);
            gameObject.name = combObj + collisionGameobjectName;

            /*
            foreach (Transform childTransform in parentTransform)
            {
                if (childTransform.CompareTag(nextObj))
                {
                    GameObject nextObject = childTransform.gameObject;
                    nextObject.SetActive(true);
                    break;
                }              
            }
            */

        }
    }


    private void Update()
    {
            if(isPlaying && !audioSource.isPlaying)
            {
                test = true;
                isPlaying = false;
            }

    }
   

}




