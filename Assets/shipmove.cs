using UnityEngine;
using UnityEngine.Animations;

public class shipmove : MonoBehaviour
{
    public Animator animator;
    public AnimationClip Moveforward;

    private bool isPlaying = false;

    // 이미지 타겟을 인식했을 때 호출되는 콜백
    public void OnImageTargetDetected()
    {
        if (!isPlaying)
        {
            // moveforward 애니메이션을 3번 재생
            animator.Play(Moveforward.name, 0, 0);
            animator.Play(Moveforward.name, 0, 1f / 3f);
            animator.Play(Moveforward.name, 0, 2f / 3f);

            isPlaying = true;
        }
    }
}
