using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    GameObject Player;

    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) <= 0.1f)
            Destroy(gameObject);
    }
}
