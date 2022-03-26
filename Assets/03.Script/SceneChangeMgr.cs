using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeMgr : MonoBehaviour
{
    public string NowSceneName;
    public string NextSceneName;
    public GameObject Player;

    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        
    }

  
    void Update()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) < 0.1f)
            LoadNextScene();
    }

    public void LoadNextScene()
    {
        PlayerPrefs.SetString("Scene", NextSceneName);
        SceneManager.LoadScene("Loading");
    }

    public void ReLoadScene()
    {
        PlayerPrefs.SetString("Scene", NowSceneName);
        SceneManager.LoadScene("Loading");
    }

}
