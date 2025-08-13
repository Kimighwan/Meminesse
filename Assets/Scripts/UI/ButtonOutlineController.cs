using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

// 선택된 버튼이 깜빡이는 효과를 위한 스크립트.
public class ButtonOutlineController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Image outline; // Outline 이미지 (SpriteRenderer가 아님!)

    private Coroutine blinkCoroutine;

    void Start()
    {
        if (outline != null)
            outline.gameObject.SetActive(false); // 시작 시 꺼두기
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowOutline(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShowOutline(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowOutline(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ShowOutline(false);
    }

    private void ShowOutline(bool isShow)
    {
        if (outline == null) return;

        outline.gameObject.SetActive(isShow);

        if (isShow)
        {
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
            blinkCoroutine = StartCoroutine(BlinkOutline());
        }
        else
        {
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        }
    }

    private IEnumerator BlinkOutline()
    {
        Color color = outline.color;
        float t = 0f;

        while (true)
        {
            // 알파값 0~1 사이로 부드럽게 왕복
            t += Time.deltaTime * 2f;
            float alpha = Mathf.Abs(Mathf.Sin(t));
            color.a = alpha;
            outline.color = color;

            yield return null;
        }
    }
}
