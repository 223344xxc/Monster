using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDialogue : MonoBehaviour
{
    public DialogueTrigger Trigger;
    public string NextSceneName;

    void Start()
    {
        Trigger.TriggerDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(DialogueNameMgr.NameCount == -1)
        {
            PlayerPrefs.SetString("Scene", NextSceneName);
            SceneManager.LoadScene("Loading");
        }
    }
}
