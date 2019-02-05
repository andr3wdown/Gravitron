using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapToggler : MonoBehaviour
{
    bool active = true;
    public GameObject miniMap;
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            active = !active;
        }
        miniMap.SetActive(active);
	}
   
}
