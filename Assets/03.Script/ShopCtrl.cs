using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCtrl : MonoBehaviour
{
    public DialogueTrigger Dialogue_Start_Button;

    void Awake()
    {
        Dialogue_Start_Button = GameObject.Find("StartButton").GetComponent<DialogueTrigger>();
    }
    void Start()
    {
        Dialogue_Start_Button.TriggerDialogue();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
