using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class TargetController : MonoBehaviour
{
   private PlayableDirector timeline; // Timeline 애니메이션을 재생하는 PlayableDirector 컴포넌트
   public void TimeLineStart()
   {
      gameObject.SetActive(true);
      timeline = GetComponent<PlayableDirector>(); // PlayableDirector 컴포넌트
      timeline.Play();
   }

   public void TargetOut()
   {
      gameObject.SetActive(false);
      timeline = GetComponent<PlayableDirector>(); // PlayableDirector 컴포넌트
      timeline.Stop();
   }


}