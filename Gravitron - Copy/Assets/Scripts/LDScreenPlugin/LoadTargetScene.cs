using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTargetScene : MonoBehaviour
{
    public void LoadScene(int num)
    {
        LoadingScreenManager.LoadScene(num);
    }
}
