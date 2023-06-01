using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectController : MonoBehaviour
{
    public GameObject object1; // 1번 오브젝트
    public GameObject object2; // 2번 오브젝트
    private PlayableDirector timeline; // Timeline 애니메이션을 재생하는 PlayableDirector 컴포넌트

    private bool isTimelineFinished = false;
    private void Start()
    {
        timeline = object1.GetComponent<PlayableDirector>(); // object1의 PlayableDirector 컴포넌트
        timeline.stopped += OnTimelineStopped;
        object2.SetActive(false); 
    }

    private void Update()
    {
        // Timeline 애니메이션이 재생 중인 경우
        if (isTimelineFinished)
        {
            object1.SetActive(false);
            object2.SetActive(true);
        }
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        if(director.playableAsset.name == transform.gameObject.name)
        {
            isTimelineFinished = true;
        }
    }



 
}