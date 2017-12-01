using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject warning;
    public Image healthBar;
    public static int lives;
    public int maxLives;
    public static GameManager instance;
    public GameObject playerPrefab;
    GameObject currentPlayer;
    Vector3 respawnPosition;
    public string livesString = "Lives: ";
    public Text livesText;
    public GameObject gameOver;
    public GameObject deathParticle;
    public static void IncreaseLives(int amount)
    {
        lives += amount;
        instance.UpdateLivesText();
    }
    public void Start()
    {
        Time.timeScale = 1;
        instance = this;
        if (instance != this)
            Destroy(gameObject);

        currentPlayer = FindObjectOfType<Character>().gameObject;
        respawnPosition = currentPlayer.transform.position;
        lives = 4;
        UpdateLivesText();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPaused();
        }
        if(currentPlayer != null)
        {
            UpdateHealthBar();
        }
    }
    private void UpdateHealthBar()
    {
        
        Character chara = currentPlayer.GetComponent<Character>();
        float ratio = chara.hp / chara.maxHp;
        healthBar.rectTransform.localScale = new Vector3(1, ratio, 1);
    }
    public static void Death()
    {
        lives--;
        if(lives <= 0)
        {
            instance.StartCoroutine("Restart");
        }
        else
        instance.StartCoroutine("Respawn");
    }

    void UpdateLivesText()
    {
        livesText.text = livesString + lives;
    }
    public IEnumerator Restart()
    {
        Instantiate(deathParticle, currentPlayer.transform.position, currentPlayer.transform.rotation);
        Destroy(currentPlayer);
        UpdateLivesText();
        gameOver.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
    public CameraController mapCam;
    public IEnumerator Respawn()
    {
        Instantiate(deathParticle, currentPlayer.transform.position, currentPlayer.transform.rotation);
        Destroy(currentPlayer);
        UpdateLivesText();
        yield return new WaitForSeconds(3f);
        GameObject go = Instantiate(playerPrefab, respawnPosition, Quaternion.identity);
        currentPlayer = go;
        Camera.main.GetComponent<CameraController>().target = go.transform;
        mapCam.target = go.transform;
    }
    public static bool isPaused = false;
    public GameObject pauseMenu;
    public void SetPaused()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        if (isPaused)
        {
            Time.timeScale = 0;          
        }
        else
        {
            //FindObjectOfType<Character>().jumpCount += 1;
            Time.timeScale = 1;
        }
    }
    public void RestartCurrentLevel()
    {
        LoadingScreenManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

}
