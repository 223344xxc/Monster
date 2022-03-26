using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrl : MonoBehaviour
{
    public GameObject Player;
    Vector3 TempPos;

    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        TempPos = Player.transform.position;
        TempPos -= new Vector3(0, 0, 10);
        gameObject.transform.position = TempPos;
    }
}
