using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScreenService : MonoBehaviour, ILoadScreenService, IScene
{
    public void StartLoad()
    {
        Debug.LogError("Hellow");
    }

    public void StopLoad()
    {
        
    }
}
