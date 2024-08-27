using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gameOverTtl;
    [SerializeField] private TMP_Text txtMenuHighScore;
    //Powerup related variables below
    [SerializeField] private Transform powerUpSpawnpoint;
    [SerializeField] private GameObject nukeSymbol;
    private float nukeOffset = 0.25f;
    private float nukeHeight = -0.3f;
    public Image countdown;

    [Header ("Gameplay")]
    [SerializeField] private TMP_Text txtHealth;
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private TMP_Text txtHighScore;
    private Player player;
    private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        
        GameManager.GetInstance().OnGameStart += GameStarted;
        GameManager.GetInstance().OnGameOver += GameOver;
    }

    public void UpdateHealth(float currentHealth)
    {
        txtHealth.SetText(currentHealth.ToString());
    }

    public void UpdateScore()
    {
        txtScore.SetText(scoreManager.GetScore().ToString());
    }

    public void UpdateHighScore()
    {
        txtHighScore.SetText(scoreManager.GetHighScore().ToString());
        txtMenuHighScore.SetText($"High Score: {scoreManager.GetHighScore()}");
    }

    public void GameStarted()
    {
        player = GameManager.GetInstance().GetPlayer();
        player.health.OnHealUpdate += UpdateHealth;

        menuPanel.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        gameOverTtl.gameObject.SetActive(true);
        menuPanel.gameObject.SetActive(true);
    }

    public void UpdatePowerups()
    {
        //Function to make the nuke powerups spawn in a pretty order on the side of the screen

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Powerup"))
        {
            Destroy(item);
        }

        for (int i = 0;  i < GameManager.GetInstance().nukes; i++)
        {
            if (i%2 == 0)
            {
                Instantiate(nukeSymbol, powerUpSpawnpoint.position + new Vector3(-nukeOffset, nukeHeight * i, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(nukeSymbol, powerUpSpawnpoint.position + new Vector3(nukeOffset, nukeHeight * i, 0), Quaternion.identity);
            }
        }
    }

    public void UpdateCountdown()
    {
        //Updates the screen icon in relation to time left

        countdown.fillAmount = GameManager.GetInstance().fillPercentage;
    }
}
