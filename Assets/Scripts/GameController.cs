using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public int score = 0;
    public int currentPillar = 1;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pillarText;
    public bool isLose = false;

    public int currentWave = 1;
    public int mediumWave = 6;
    public int hardWave = 11;

    public bool isChangeCameraPos = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddPillar()
    {
        currentPillar++;
        pillarText.text = "PILLAR: " + currentPillar.ToString();
    }
    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "SCORE: " + score.ToString();
    }

    public void Lose()
    {
        Debug.Log("lose");
        PlayerPrefs.SetInt("point", score);
    }
}
