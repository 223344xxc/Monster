using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
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
        Change_Scene();
    }

    void Change_Scene()
    {
        if (transform.position.x == Player.transform.position.x && transform.position.y == Player.transform.position.y)
            SceneManager.LoadScene("Clear");
    }

}
