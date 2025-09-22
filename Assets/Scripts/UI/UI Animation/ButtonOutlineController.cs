using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

// 선택된 버튼이 깜빡이는 효과를 위한 스크립트.
public class ButtonOutlineController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Image outline; // Outline 이미지 (SpriteRenderer가 아님!)

    private bool isClicked = false; // 클릭 상태를 저장하기 위한 변수
    private static ButtonOutlineController currentButton = null;

    private Coroutine blinkCoroutine;

    void OnEnable()
    {
        if (outline != null)
            outline.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked) ShowOutline(true);
        Debug.Log(" / Alpha: " + outline.color.a);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked) ShowOutline(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!isClicked) ShowOutline(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!isClicked) ShowOutline(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 이전에 선택된 버튼은 해제
        if(currentButton != null && currentButton != this)
        {
            currentButton.isClicked = false;
            currentButton.ShowOutline(false);
        }
        isClicked = true;
        ShowOutline(true);
        currentButton = this;
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
