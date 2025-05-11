using UnityEngine;

public class teststst : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 4f);
            if (hit)
                Debug.Log($"{hit.collider.gameObject.name}");
        }
    }
}
