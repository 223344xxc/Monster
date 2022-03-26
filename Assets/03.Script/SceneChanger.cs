using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public float LoadTime = 0;

    void Awake()
    {
        LoadTime = 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        LoadTime += Time.deltaTime;
        if (LoadTime >= 2)
            SceneManager.LoadScene(PlayerPrefs.GetString("Scene"));
    }
}
