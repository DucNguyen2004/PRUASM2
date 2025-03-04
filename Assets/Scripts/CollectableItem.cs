using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.AddScore(5);
            Destroy(gameObject);
        }
    }
}
