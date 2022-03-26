using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonCtrl : MonoBehaviour
{
    public SceneChangeMgr ChangeMgr;

    void Awake()
    {
        ChangeMgr = GameObject.Find("SceneChanger").GetComponent<SceneChangeMgr>();    
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PuzzleReset()
    {
        ChangeMgr.ReLoadScene();
    }
}
