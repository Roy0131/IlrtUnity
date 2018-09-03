using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour 
{
    private void Awake()
    {
        Debug.Log("Driver!!!");
        DontDestroyOnLoad(gameObject);
    }
}