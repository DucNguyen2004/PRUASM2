using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void EnterGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
