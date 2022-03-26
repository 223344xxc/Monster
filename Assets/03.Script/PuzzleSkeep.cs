using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSkeep : MonoBehaviour
{
    public SceneChangeMgr Mgr;

    void Awake()
    {
        Mgr = GameObject.Find("SceneChanger").GetComponent<SceneChangeMgr>();    
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SkeepPuzzle()
    {
        Mgr.LoadNextScene();
    }
}
