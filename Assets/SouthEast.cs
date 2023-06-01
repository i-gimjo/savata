using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouthEastEffect : MonoBehaviour
{
    public Animator animator;
    public List<GameObject> effects = new List<GameObject>();

  

    public void StartAnim()
    {
        foreach (GameObject obj in effects)
        {
            obj.SetActive(false);
        }

        animator.Play("southeast");
        OnAnimationEnd();
    }

    private void OnAnimationEnd()
    {
        foreach (GameObject obj in effects)
        {
            obj.SetActive(true);
        }
    }
}
