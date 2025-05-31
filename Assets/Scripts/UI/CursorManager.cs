using UnityEngine;       

//커서 이미지 바꾸는 

public class CursorManager : MonoBehaviour
{
    public Texture2D customCursor;
    public Vector2 hotspot = Vector2.zero; // 커서 기준점 (왼쪽 위면 0,0 / 가운데면 이미지의 절반)

    void Start()
    {
        Cursor.SetCursor(customCursor, hotspot, CursorMode.Auto);   //커스텀 이미지사용, 커서 이미지 안에서 실제로 클릭되는 지점, 기본 모드
    }
}
