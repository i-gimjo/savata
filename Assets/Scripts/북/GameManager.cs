using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    public float blinkDuration = 0.5f; // ������ ���� �ð�
    public float blinkInterval = 1f; // ������ ����
    private string[] answers; //������ ������ �迭
    private int[] Ans;
    private string[] clickedObjects; // Ŭ���� ��ü�� ��ȣ�� ������ �迭
    private int clickCount = 0; // Ŭ���� Ƚ���� ������ ����
    public int answer = 3; //���� ���䰳��
    private int AnimationNum = 0;
    public Animator animator;
    public Image StartImage;

    private void Start()
    {

        StartImage.gameObject.SetActive(false);


        // answers �迭�� ������ �ε��� ����
        Ans = new int[answer];      
        answers = new string[answer];
        clickedObjects = new string[answer];

        for (int i = 0; i < answer; i++)
        {
            Ans[i] = Random.Range(0, objects.Count);
            answers[i] = objects[Ans[i]].name;
        }


        // ������Ʈ ������ ����
        StartCoroutine(delayTime());
    }
    private IEnumerator delayTime()
    {
        StartImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        StartImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        StartCoroutine(BlinkObjects());

    }

    private IEnumerator BlinkObjects()
    {
        for (int i = 0; i < answers.Length; i++)
        {
            int index = Ans[i];
            Debug.Log(objects[index]);
            GameObject obj = objects[index];
            obj.SetActive(false); // ������Ʈ Ȱ��ȭ
            yield return new WaitForSeconds(blinkDuration);
            obj.SetActive(true); // ������Ʈ ��Ȱ��ȭ
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    public void ObjectClicked(GameObject clickedObject)
    {
        clickedObjects[clickCount] = clickedObject.name;
        clickCount++;

        if (clickCount == answers.Length)
        {
            bool isEqual = Enumerable.SequenceEqual(clickedObjects, answers);
            // Ŭ���� ������Ʈ id �迭�� answers �迭�� ������ Clear ó��
            if (isEqual)
            {

                AnimationNum++;
                animator.SetInteger("Stage", AnimationNum);

                if (answer < 6)
                {
                    answer++;
                    Start(); 
                    
                }
                else
                {
                    Debug.Log("GameClear");
                }
            }
            else // �ٸ��� Wrong ó��
            {
                GameOver();
            }

            // Ŭ���� ������Ʈ id �迭�� clickCount �ʱ�ȭ
            clickedObjects = new string[answer];
            clickCount = 0;
        }



    }

   

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}


