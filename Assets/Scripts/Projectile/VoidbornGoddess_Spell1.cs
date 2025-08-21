using UnityEngine;

public class VoidbornGoddess_Spell1 : MonoBehaviour
{
    private void Update()
    {
        if(GetComponent<SpriteRenderer>().color.a == 0)
            Destroy(gameObject);
    }
}
