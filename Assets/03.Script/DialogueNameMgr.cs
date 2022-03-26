using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueNameMgr : MonoBehaviour
{
    public GameObject NameObject;
    public Text NameText;

    public static int NameCount = -1;

    public string[] Names;
    string Name;
    void Awake()
    {
        NameText = NameObject.GetComponent<Text>();
    }

    void Start()
    {
    }

    void Update()
    {
        ChackName();
    }

    void ChackName()
    {
        if (NameCount < Names.Length - 1)
        {
            Name = Names[NameCount];
        }
        else
            NameCount -= 1;
        Debug.Log(Names.Length + " aa " + NameCount);
        NameText.text = Name;
    }
}
