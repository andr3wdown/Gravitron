using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpGamesWeapon2D.Weapons;

public class PoolManager : MonoBehaviour
{
    public List<BulletPoolInstance> pools = new List<BulletPoolInstance>();
    public GameObject[] poolableObjects;
    private void Start()
    {
        for(int i = 0; i < poolableObjects.Length; i++)
        {
            pools.Add(new BulletPoolInstance(poolableObjects[i], 100));
        }
    }
}
