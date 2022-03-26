using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextShopCtrl : MonoBehaviour
{
    SpriteRenderer renderer;
    public GameObject DialogueBox;
    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();        
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (DialogueNameMgr.NameCount == 2)
            renderer.sprite = null;
        if (DialogueNameMgr.NameCount == -1 && !DialogueBox.activeSelf)
        {
            PlayerPrefs.SetString("Scene", "Boss_Dialogue");
            SceneManager.LoadScene("Loading");
        }
    }
}
