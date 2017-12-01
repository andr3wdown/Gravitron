using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public GameObject[] planets;
    public int planetAmount;
    public LayerMask planetLayer;
    int pc = 0;//current planet count
    public float crawlerRadius;
    public float jumpMax;
    public float jumpMin;
    public float planetSizeMax = 6.0f;
    public float planetSizeMin = 4.0f;
    int iter = 0;
    public GameObject planet;
    List<GameObject> spawnedPlanets = new List<GameObject>();
    PlanetGenerator pg;
	void Awake ()
    {
        
        pg = GetComponent<PlanetGenerator>();
        GenerateLevel();
	}
    void GenerateLevel()
    {
        GameObject crawler = Instantiate(new GameObject(), Vector2.zero, Quaternion.identity);
        SpawnPlanet(crawler.transform);
        while(pc < planetAmount && iter < 1000)
        {
            crawler.transform.rotation = Quaternion.Euler(0,0,Random.Range(0f,360f));
            Vector2 candidate = crawler.transform.position + crawler.transform.up * Random.Range(jumpMin, jumpMax);
            crawler.transform.position = candidate;
            if (IsFree(candidate, crawlerRadius))
            {
                
                SpawnPlanet(crawler.transform);
            }
            iter++;
        }
    }
    void SpawnPlanet(Transform pos)
    {
        int index = Random.Range(0, planets.Length);
        GameObject go = Instantiate(planet, pos.position, pos.rotation);
        Texture2D tex = pg.GenerateImage();
        float scale = Random.Range(4f, 6f);
        float mass = Map(scale, 4f, 6f, 25000, 50000);
        float gravityDist = Map(scale, 4f, 6f, 115f, 175f);
        go.GetComponent<GravityBody>().gravityDistance = gravityDist;
        go.GetComponent<Rigidbody2D>().mass = mass;
        go.transform.localScale = new Vector3(scale, scale, scale);
        go.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
        pc++;
    }
    public float Map(float scale, float fMin, float fMax, float sMin, float sMax)
    {      
        return (scale - fMin) / (fMax - fMin) * (sMax - sMin) + sMin;
    }
    
    bool IsFree(Vector2 checkPos, float checkRadius = 10)
    {
        Collider2D c = Physics2D.OverlapCircle(checkPos, checkRadius, planetLayer);
        return c == null;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, crawlerRadius);
        Debug.DrawLine(transform.position, transform.position + transform.up * jumpMax);
        Debug.DrawLine(transform.position, transform.position + transform.up * jumpMin, Color.green);
    }

}
