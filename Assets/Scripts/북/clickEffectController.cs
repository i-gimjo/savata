using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class clickEffectController : MonoBehaviour, IPointerClickHandler
{
    public float clickEffectDuration = 0.2f; // Ŭ�� ����Ʈ�� ���ӵǴ� �ð�
    public Image clickEffectPrefab; // Ŭ�� ����Ʈ�� �����? ������
    public GameManager gameManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Ŭ�� ����Ʈ ����
        Image clickEffect = Instantiate(clickEffectPrefab, transform.position, Quaternion.identity);
        clickEffect.transform.SetParent(transform.parent, false);
        clickEffect.transform.position = transform.position;

        // ���� �ð��� ���� �� Ŭ�� ����Ʈ ����
        Destroy(clickEffect.gameObject, clickEffectDuration);

        gameManager.ObjectClicked(gameObject);
    }
}
