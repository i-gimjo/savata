using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMerge : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isPlaying;
    
    private Transform child1;
    private Transform child2;

    private bool hasCollided = false; // 충돌 여부를 나타내는 변수

    private void Start()
    {
        isPlaying = false;
        child1 = transform.GetChild(0); // 1번 자식 오브젝트
        child2 = transform.GetChild(1);

        if (gameObject.activeSelf)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i != 0 && i != 1)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (!child2.GetComponent<ObjectMove>().clicked &&!hasCollided && 
        child1.GetComponent<Collider>().bounds.Intersects(child2.GetComponent<Collider>().bounds))
        {
            hasCollided = true;
            Debug.Log("충돌");
            audioSource = child2.GetComponent<AudioSource>();

            audioSource.Play();
            isPlaying = true;
            child2.localPosition = child2.GetComponent<SetPosition>().Position_2;
            child2.GetComponent<Collider>().enabled = false;
            child2.SetParent(child1);


            StartCoroutine(DoAfterCollision());
        }
    }

    private IEnumerator DoAfterCollision()
    {
        yield return new WaitWhile(() => isPlaying && audioSource.isPlaying);
        child1 = transform.GetChild(0); // 1번 자식 오브젝트
        child2 = transform.GetChild(1);
        child2.gameObject.SetActive(true);
        hasCollided = false;
    }
    
}
