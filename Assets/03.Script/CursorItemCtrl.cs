using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorItemCtrl : MonoBehaviour
{
    public static bool isMove = false;
    public static Image image;
    public static InvenSlot.ItemType ChangeTileItem;

    void Awake()
    {
        image = gameObject.GetComponent<Image>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (isMove)
            transform.position = Input.mousePosition;
        else
            transform.position = new Vector3(0, 10000, 0);
    }
}
