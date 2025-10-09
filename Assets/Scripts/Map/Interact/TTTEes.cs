using UnityEngine;

public class TTTEes : MonoBehaviour
{
    [SerializeField] private MoveDoor door;

    private void OnDisable()
    {
        door.Open();
    }
}
