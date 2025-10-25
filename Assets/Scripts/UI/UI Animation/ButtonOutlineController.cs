using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class ButtonOutlineController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Image outline; // Outline 이미지 (SpriteRenderer가 아님!)

    private bool isClicked = false; 
    private static ButtonOutlineController currentButton = null;

    private Coroutine blinkCoroutine;

    void OnEnable()
    {
        if (outline != null)
            outline.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        if (outline != null)
        {
            outline.gameObject.SetActive(false);
        }

        isClicked = false;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked) ShowOutline(true);
        
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
        if (outline == null || !gameObject.activeInHierarchy) return;

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
            t += Time.unscaledDeltaTime * 2f;
            float alpha = Mathf.Abs(Mathf.Sin(t));
            color.a = alpha;
            outline.color = color;

            yield return null;
        }
    }
}
