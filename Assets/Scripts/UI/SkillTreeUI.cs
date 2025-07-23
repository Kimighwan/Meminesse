using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : UIBase
{
    [SerializeField]
    private RectTransform descUIRectTransform;
    [SerializeField]
    private TextMeshProUGUI descUIText;
    [SerializeField]
    private RectTransform contentRectTransform;

    [Header("Skill descUI Position")]
    private Vector2 upPosition = Vector2.zero;
    private Vector2 downPosition = new Vector2(0f, -310f);

    [Header("Scroll position to move")]
    private Vector2 rightPosition = new Vector2(-1400f, 292f);    // Scroll Right Position to move right
    private Vector2 leftPosition = new Vector2(-0f, 292f);     // Scroll Left Position to move right

    private float elapsedTime;
    private float clampT;
    private float t;

    // conditional variable
    public bool descUIActiveCheck = false;
    public bool up = false;
    public bool down = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

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

    public override void SetCurrentButton(GameObject gb)
    {
        base.SetCurrentButton(gb);
    }

    public void MoveRightScroll()
    {
        contentRectTransform.anchoredPosition = rightPosition;
    }

    public void MoveLeftScroll()
    {
        contentRectTransform.anchoredPosition = leftPosition;
    }
}
