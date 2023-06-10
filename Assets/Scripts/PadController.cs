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
        playableDirector = object1.GetComponent<PlayableDirector>(); // object1�� PlayableDirector ������Ʈ
        playableDirector.stopped += OnTimelineStopped;
        object2.SetActive(false);
    }

    private void Update()
    {
        if (isTimelineFinished)
        {
            // object1�� Ȱ��ȭ��Ű�� ������ ����
            object1.SetActive(false);
            object2.SetActive(true);
            
        }
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        // 2page Ÿ�Ӷ����� ������ �� ȣ��Ǵ� �Լ�
        if (director.playableAsset.name == "2page")
        {
            isTimelineFinished = true;
        }
    }
}
