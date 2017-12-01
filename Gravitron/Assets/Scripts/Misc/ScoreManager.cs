using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public string scoreString = "Score: ";
    public static int score;
    public static ScoreManager instance;
    Character chara;
    public Image bar;
    private void Start()
    {
        chara = FindObjectOfType<Character>();
        instance = this;
        if (instance != this)
        {
            Destroy(gameObject);
        }
        UpdateScore();
    }
    public void Update()
    {
        if (chara == null)
        {
            StartCoroutine(FindChara(3.01f));
        }
        else
            UpdateFuelBar();
    }
    void UpdateFuelBar()
    {
        float ratio = (float)chara.jumpCount / (float)chara.maxJumpCount;
        bar.rectTransform.localScale = new Vector3(1, ratio, 1);
    }
    IEnumerator FindChara(float delay)
    {
        yield return new WaitForSeconds(delay);
        chara = chara = FindObjectOfType<Character>();
    }
    public static void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        instance.UpdateScore();
    }
    void UpdateScore()
    {
        scoreText.text = scoreString + score.ToString();
    }
}
