using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCtrl : MonoBehaviour
{
    public DialogueTrigger trigger;
    public GameObject Text;

    void Awake()
    {
        Text.SetActive(false);    
    }

    void Start()
    {
        trigger.TriggerDialogue();
    }

    void Update()
    {
        if (DialogueNameMgr.NameCount == -1)
            Text.SetActive(true);
    }
}
