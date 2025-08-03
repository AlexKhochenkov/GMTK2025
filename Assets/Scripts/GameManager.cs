using System;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Score score;
    [SerializeField] private GameObject endScreen;

    public static GameManager Instance { get; private set; }

    [field: SerializeField] public bool GameStarted { get; private set; }

    public void StartGame()
    {
        GameStarted = true;
    }

    void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        PlayerController.Player.Died += new PlayerController.DiedEvent(EndGame);
    }

    void OnDestroy()
    {
        if(PlayerController.Player != null)
            PlayerController.Player.Died -= new PlayerController.DiedEvent(EndGame);
        Instance = null;
    }

    void EndGame()
    {
        if (!GameStarted)
        {
            return;
        }
        GameStarted = false;
        endScreen.SetActive(true);
        score.SetBestScore();
    }

    void Update()
    {
        if (GameStarted)
            score.AddScore(Time.deltaTime);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
