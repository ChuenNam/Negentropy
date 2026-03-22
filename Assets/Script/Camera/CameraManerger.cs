using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManerger : MonoBehaviour
{
    public static CameraManerger Instance { get; private set; }
    
    private void Awake()   
    {
        if (Instance == null)  
            Instance = this;
        else   
            Destroy(gameObject);
    }

    public GameObject PlayerCamera;
}