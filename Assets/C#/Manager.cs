using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    public TMP_Text coinText;
    public TMP_Text countdownText; 
    public int coin;
    public GameObject pauseButton;
    public GameObject resumeButton;

    [HideInInspector]
    public bool isPaused;

    private void Awake()
    {
            Instance = this;
    }

    void Start()
    {
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        coin = PlayerPrefs.GetInt("coin", coin);

        StartCoroutine(StartGameAfterCountdown());
        StartCoroutine(InitialDelay());
    }

    void Update()
    {
        coinText.text = "Coin: " + coin.ToString();
    }

    public void AddCoin(int num)
    {
        coin += num;
        PlayerPrefs.SetInt("coin", coin);
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("coin", coin); 
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
    }

    public void NopauseGame()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
    }

    public void loadScene(int numberScene)
    {
        SceneManager.LoadScene(numberScene);
    }

    IEnumerator StartGameAfterCountdown()
    {
        countdownText.gameObject.SetActive(true); 
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false); 
    }

    IEnumerator InitialDelay()
    {
        isPaused = true; 
        yield return new WaitForSeconds(4f); 
        isPaused = false; 
    }
}
