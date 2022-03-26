using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpImageCtrl : MonoBehaviour
{
    public GameObject HelpImage;
    void Start()
    {
        HelpImage.SetActive(false);
    }

    void Update()
    {
        if (DialogueNameMgr.NameCount == 13)
            HelpImage.SetActive(true);
        if (DialogueNameMgr.NameCount == 15)
            HelpImage.SetActive(false);
    }
}
