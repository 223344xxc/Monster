using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRayMgr : MonoBehaviour
{
    public static GameObject HitObject;
    public static GameObject InvenSlotObject;
    public GameObject TempHitObject;
    public GameObject TempInvenSlot;

    static InvenSlot Temp;

    public static bool ChangeTile = false;
    public static bool TagBlock = false;
    public static bool FreezSlot = false;

    bool MouseDown = false;

    void Start()
    {
    }
    void Update()
    {
        GetRayObject();
        CopyObjects();
    }

    public static void ResetAll()
    {
        ChangeTile = false;
        TagBlock = false;
    }

    void CopyObjects()
    {
        TempHitObject = HitObject;
        TempInvenSlot = InvenSlotObject;
    }

    void GetRayObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //if (hit.collider.tag == "TempCollider" && !FreezSlot)
            //{
            //    InvenSlotObject = hit.collider.gameObject.transform.parent.gameObject;
            //}
            if (hit.collider.tag == "Block")
            {
                HitObject = hit.collider.gameObject;
                TagBlock = true;
            }
            else
            {
                TagBlock = false;
                HitObject = null;
            }
        }
    }
    
    public static void ResetSelObtion()
    {
        if (!BossStageCamCtrl.FirstSetTile)
        {
            BossStageCamCtrl.SetCamPosition(BossStageCamCtrl.CamPosition.None);
            BossCtrl.SelectPlayerAttack = false;
            BossCtrl.UseItem = false;
        }
    }

    public static void ChangeTileFuntion()
    {
        Temp = InvenSlotObject.GetComponent<InvenSlot>();
        if (TagBlock && Temp.Item != InvenSlot.ItemType.None && HitObject.GetComponent<Block>().Type !=  Block.BlockType.Wall + (int)Temp.Item && Temp.UseType == InvenSlot.UseItemType.Tile)
        {
            HitObject.GetComponent<Block>().Type = Block.BlockType.Wall + (int)Temp.Item;
            Temp.ItemCount -= 1;
            if (Temp.ItemCount <= 0)
            {
                Temp.ItemCount = 0;
                Temp.Item = InvenSlot.ItemType.None;
            }
            ResetSelObtion();
        }
    }
}
