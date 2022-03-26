using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCtrl : MonoBehaviour
{
    public GameObject Dialogue;
    public GameObject[] Slots;

    void Start()
    {
        for (int i = 0; i < Slots.Length; i++)
            Slots[i].SetActive(false);
    }

    void Update()
    {
        if (!Dialogue.activeSelf)
        {
            for (int i = 0; i < Slots.Length; i++)
                Slots[i].SetActive(true);
        }
    }
}
