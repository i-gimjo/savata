using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform camerPosition;

    void Update()
    {
        transform.position = camerPosition.position;
    }
}
