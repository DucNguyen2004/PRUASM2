using UnityEngine;

public class Bridge : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            GameController.Instance.isLose = true;
        }
    }
}

