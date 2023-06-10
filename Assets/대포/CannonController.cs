using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{

    public float doubleClickTimeThreshold = 0.3f;
    private float lastClickTime;

    public float rotationSpeed = 1;
    public float BlastPower = 5;

    public GameObject Cannonball;
    public Transform ShotPoint;
    public Transform StartPosition;
    private LineRenderer lineRenderer;

    public GameObject Explosion;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, StartPosition.position);
    }
    private void Update()
    {
        float horizontalRotation = Input.GetAxis("Mouse X"); //마우스의 x값을 받아와서 y축을 움직임
        float verticalRotation = Input.GetAxis("Mouse Y"); //마우스의 y값을 받아와서 z축을 움직임
        Debug.Log("H" + horizontalRotation);
        Debug.Log("V" + verticalRotation);

        verticalRotation = Mathf.Clamp(verticalRotation, -2f, 2f); //상하 움직임
        horizontalRotation = Mathf.Clamp(horizontalRotation, -2f, 3f); //좌우 움직임



        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +
            new Vector3(0, horizontalRotation * rotationSpeed, -verticalRotation * rotationSpeed));


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Time.time - lastClickTime < doubleClickTimeThreshold)
            {
                Throw();
            }
            lastClickTime = Time.time;
        }

    }

    private void Throw()
    {
        GameObject CreatedCannonball = Instantiate(Cannonball, ShotPoint.position, ShotPoint.rotation);
        CreatedCannonball.GetComponent<Rigidbody>().velocity = ShotPoint.transform.up * BlastPower;

        // Added explosion for added effect
        Destroy(Instantiate(Explosion, ShotPoint.position, ShotPoint.rotation), 2);

    }



}
