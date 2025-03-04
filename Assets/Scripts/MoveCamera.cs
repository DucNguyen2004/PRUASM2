using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform firstPersonPosition;  // Assign the first-person camera position
    public Transform sideViewPosition;     // Assign the 2D side-view camera position
    private bool isSideView = false;       // Track current view

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isSideView = !isSideView; // Toggle camera view
            GameController.Instance.isChangeCameraPos = isSideView;
        }

        // Smoothly move the camera between positions
        if (isSideView)
        {
            transform.position = Vector3.Lerp(transform.position, sideViewPosition.position, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, sideViewPosition.rotation, Time.deltaTime * 5);
        }
        else
        {
            transform.position = firstPersonPosition.position;
        }
    }
}
