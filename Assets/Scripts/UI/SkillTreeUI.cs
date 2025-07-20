using TMPro;
using UnityEngine;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform descUIRectTransform;
    [SerializeField]
    private TextMeshProUGUI descUIText;

    // position
    private Vector2 upPosition = Vector2.zero;
    private Vector2 downPosition = new Vector2(0f, -310f);

    private float elapsedTime;
    private float clampT;
    private float t;

    // conditional variable
    public bool descUIActiveCheck = false;
    public bool up = false;
    public bool down = false;

    void Update()
    {
        if (up)
        {
            elapsedTime += Time.deltaTime;
            clampT = Mathf.Clamp01(elapsedTime);
            t = Mathf.SmoothStep(0f, 1f, clampT);
            descUIRectTransform.anchoredPosition = Vector2.Lerp(downPosition, upPosition, t);

            if(t >= 1f)
            {
                up = false;
                descUIRectTransform.anchoredPosition = upPosition;
                elapsedTime = 0;
            }
        }

        if(down)
        {
            elapsedTime += Time.deltaTime;
            clampT = Mathf.Clamp01(elapsedTime);
            t = Mathf.SmoothStep(0f, 1f, clampT);
            descUIRectTransform.anchoredPosition = Vector2.Lerp(upPosition, downPosition, t);

            if (t >= 1f)
            {
                down = false;
                descUIRectTransform.anchoredPosition = downPosition;
                elapsedTime = 0;
            }
        }
    }

    public void OnClickSkillNode(int nodeID)
    {
        if (down)
            return;

        if (!descUIActiveCheck)
            up = true;

        descUIActiveCheck = true;

        descUIText.text = nodeID.ToString();
    }

    public void DownDescUIAnmation()
    {
        if (down || up)
            return;

        descUIActiveCheck = false;
        down = true;
    }

}
