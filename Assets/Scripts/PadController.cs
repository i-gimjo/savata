using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PadController : MonoBehaviour
{
    private PlayableDirector playableDirector;
    public GameObject object1;
    public GameObject object2; 


    private bool isTimelineFinished = false;

    private void Start()
    {
        playableDirector = object1.GetComponent<PlayableDirector>(); // object1의 PlayableDirector 컴포넌트
        playableDirector.stopped += OnTimelineStopped;
        object2.SetActive(false);
    }

    private void Update()
    {
        if (isTimelineFinished)
        {
            // object1을 활성화시키는 동작을 수행
            object1.SetActive(false);
            object2.SetActive(true);
            
        }
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        // 2page 타임라인이 끝났을 때 호출되는 함수
        if (director.playableAsset.name == "2page")
        {
            isTimelineFinished = true;
        }
    }
}
