using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Pickup
{
    public int scoreToGive;
   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Character>() != null)
        {
            ScoreManager.AddScore(scoreToGive);
            Destroy(gameObject);
        }
    }

}
