using UnityEngine;       

//커서 이미지 바꾸는 클래스 
// 프로젝트 세팅에 커서 넣어놔서 사실 불필요함
public class CursorManager : MonoBehaviour
{
    public Texture2D customCursor;
    public Vector2 hotspot = Vector2.zero;

    void Start()
    {
        Cursor.SetCursor(customCursor, hotspot, CursorMode.ForceSoftware);
    }
}
