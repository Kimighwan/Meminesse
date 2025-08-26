using UnityEngine;

public class Heart : MonoBehaviour
{
    public GameObject empty;
    public GameObject half;
    public GameObject full;

    public void SetState(int state) // 0=빈 하트, 1=반 하트, 2=풀 하트
    {
        empty.SetActive(state == 0);
        half.SetActive(state == 1);
        full.SetActive(state == 2);
    }
}
