using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Andr3wDown.Math;

public class EnemyWaveManager : MonoBehaviour
{
    public GameObject victory;
    public Text text;
    public Wave[] waves;
    public int waveIndex = 0;
    public List<Transform> locations;
    public GameObject[] enemies;
    public GameObject nextWavePrefab;
    public GameObject bossWavePrefab;
    public GameObject messagePad;
    public AudioSource music;
    public AudioClip bossMusic;
    public GameObject boss;
    bool ended = false;
    public static Character chara;
    public void Start()
    {     
        SpawnWave(waves[waveIndex]);
        waveIndex++;
    }
    bool started = false;
    public void Update()
    {
        //10 11
        if(waveIndex == 11 && Enemy.enemyList.Count < 1)
        {
            ended = true;
            StartCoroutine(WinProcess(5));
        }
        if(waveIndex == 10 && Enemy.enemyList.Count < 1)
        {
            if (!started && !ended)
            {
                started = true;
                StartCoroutine(SpawnBoss(5));
            }
           
        }
        if(chara == null)
        {
            FindPlayer(3);
        }
        if(waveIndex < 10)
        {
            if (!ended && chara != null)
            {
                text.text = "Wave: " + waveIndex;
                if (Enemy.enemyList.Count < 1)
                {
                    if (!started)
                    {
                        started = true;
                        StartCoroutine(WaveSpawnProcess(5));
                    }
                }

            }
        }
   
        
    }
    IEnumerator FindPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        chara = FindObjectOfType<Character>();
    }
    IEnumerator WinProcess(float delay)
    {
        victory.SetActive(true);
        yield return new WaitForSeconds(delay);       
        SceneManager.LoadScene(0);     
    }
    IEnumerator SpawnBoss(float delay)
    {
        music.clip = bossMusic;
        music.Play();
        GameObject go = Instantiate(bossWavePrefab);
        go.transform.SetParent(messagePad.transform);
        yield return new WaitForSeconds(delay);
        GameObject g = Instantiate(boss, GravityBody.GetSpawnPos(), MathOperations.LookAt2D(transform.position, chara.transform.position, 90));
        chara.ConfigurePointers();
        chara.hp = chara.maxHp;
        waveIndex++;
        started = false;
    }
    IEnumerator WaveSpawnProcess(float delay)
    {
        GameObject go = Instantiate(nextWavePrefab);
        go.transform.SetParent(messagePad.transform);
        yield return new WaitForSeconds(delay);
        SpawnWave(waves[waveIndex]);
        waveIndex++;
        started = false;
    }
    void SpawnWave(Wave waveToSpawn)
    {
        
        for(int i = 0; i < enemies.Length; i++)
        {
            for(int j = 0; j < waveToSpawn.amounts[i]; j++)
            {
                Instantiate(enemies[i], GravityBody.GetSpawnPos(), MathOperations.LookAt2D(transform.position, chara.transform.position, 90));
            }
        }
        chara.ConfigurePointers();
        chara.hp = chara.maxHp;
    }
    public Vector2 GetBossPos(float rad, float checkRad)
    {
        if(chara != null)
        {
            int iter = 0;
            while (iter < 1000)
            {
                if(chara.closestPlanet != null)
                {
                    GravityBody planet = chara.closestPlanet.GetComponent<GravityBody>();
                    if (planet != null)
                    {
                        Vector2 checkVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
                        if (!Physics2D.OverlapCircle((Vector2)chara.closestPlanet.position + checkVector * chara.closestPlanet.GetComponent<GravityBody>().gravityDistance * rad, checkRad, planet.planetLayer))
                        {
                            return (Vector2)chara.closestPlanet.position + checkVector * chara.closestPlanet.GetComponent<GravityBody>().gravityDistance * rad;
                        }
                    }
                    else
                    {
                        return GravityBody.GetSpawnPos(rad, checkRad);
                    }
                }
                else
                {
                    return GravityBody.GetSpawnPos(rad, checkRad);
                }
               
            
            }
        }
        return Vector2.zero;
    }
}
[System.Serializable]
public class Wave
{
    public int[] amounts = new int[3];
}
