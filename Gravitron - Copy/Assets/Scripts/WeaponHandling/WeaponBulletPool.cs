using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UpGamesWeapon2D.Weapons
{
    public class BulletPoolInstance
    {
        static GameObject masterPool;
        List<GameObject> pool;
        GameObject pooledObject;
        public GameObject parent;
        public BulletPoolInstance(GameObject _pooledObject, int poolSize, string weaponName = "")
        {
            if (masterPool == null)
            {
                masterPool = new GameObject("BulletPools");
            }
            parent = new GameObject(_pooledObject.name + "_" + weaponName + "_BulletPool");
            parent.transform.SetParent(masterPool.transform);
            pool = new List<GameObject>();
            pooledObject = _pooledObject;
            for (int i = 0; i < poolSize; i++)
            {
                GameObject go = MonoBehaviour.Instantiate(_pooledObject);
                go.transform.SetParent(parent.transform);
                go.SetActive(false);
                pool.Add(go);
            }
        }
        public GameObject SpawnObjectAt(Transform trans, Quaternion rotation)
        {

            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].SetActive(true);
                    pool[i].transform.rotation = rotation;
                    pool[i].transform.position = trans.position;
                    return pool[i];
                }
            }
            GameObject go = MonoBehaviour.Instantiate(pooledObject, trans.position, rotation);
            go.transform.SetParent(parent.transform);
            pool.Add(go);
            return go;
        }
        IEnumerator Destruction()
        {
            while (HasActiveMembers)
            {
                yield return new WaitForSeconds(2);
            }
            
            MonoBehaviour.Destroy(parent);

        }
        bool HasActiveMembers
        {
            get
            {
                for(int i= 0; i< pool.Count; i++)
                {
                    if (pool[i].activeInHierarchy)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}

