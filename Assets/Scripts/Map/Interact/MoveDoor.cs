using System;
using System.Collections;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    [SerializeField] Vector3 closedLocalPosition;
    Vector3 openOffset = new Vector3(0f, 3f, 0f);
    Vector3 openLocalPosition;

    bool useExplicitOpenPosition = false;
    bool useLocalSpace = true;

    float duration = 2f;

    AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    bool isOpen = false;
    Coroutine moving;

    void Awake()
    {
        // 초기값 보호: 인스펙터에서 값이 없으면 기본 설정
        if (useLocalSpace)
            closedLocalPosition = transform.localPosition;
        else
            closedLocalPosition = transform.position;

        if (!useExplicitOpenPosition)
            openLocalPosition = closedLocalPosition + openOffset;
    }

    // 외부에서 호출할 수 있는 API
    public void Toggle()
    {
        if (moving != null) StopCoroutine(moving);
        Vector3 target = isOpen ? closedLocalPosition : openLocalPosition;
        moving = StartCoroutine(MoveTo(target));
        isOpen = !isOpen;
    }

    public void Open()
    {
        if (isOpen) return;
        if (moving != null) StopCoroutine(moving);
        Vector3 target = openLocalPosition;
        moving = StartCoroutine(MoveTo(target));
        isOpen = true;
    }

    public void Close()
    {
        if (!isOpen) return;
        if (moving != null) StopCoroutine(moving);
        Vector3 target = closedLocalPosition;
        moving = StartCoroutine(MoveTo(target));
        isOpen = false;
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start = useLocalSpace ? transform.localPosition : transform.position;
        float t = 0f;

        // 짧은 duration 처리
        if (duration <= 0f)
        {
            if (useLocalSpace) 
                transform.localPosition = target; 
            else 
                transform.position = target;
            moving = null;
            yield break;
        }

        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            float eval = curve.Evaluate(k);
            Vector3 next = Vector3.LerpUnclamped(start, target, eval);

            if (useLocalSpace)
                transform.localPosition = next;
            else
                transform.position = next;

            yield return null;
        }

        // 정확하게 맞춰줌
        if (useLocalSpace) transform.localPosition = target; else transform.position = target;
        moving = null;
    }

    public void Interact()
    {
        throw new NotImplementedException();
    }
}