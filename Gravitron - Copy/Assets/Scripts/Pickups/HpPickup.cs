using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPickup : Pickup
{
    public int LivesToGive;
    bool activated = false;
	public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Character>() != null && !activated)
        {
            activated = true;
            GameManager.IncreaseLives(LivesToGive);
            Destroy(gameObject);
        }
    }

}
