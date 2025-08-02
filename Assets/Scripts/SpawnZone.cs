using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpawnZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out SpawnedObject obj))
        {
            obj.AddIter();
        }
    }
}
