using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject Slot;
    GameObject Inven;

    public static InvenSlot[] Slots;
    RectTransform[] Slots_Rect;

    public static Sprite[] ItemImage;
    public Sprite[] Image;

    public float Blank = 1.5f;
    public Vector3 StartPos = Vector3.zero;
    public Vector3 NullPos = new Vector3(10000, 10000, 0);

    public int MaxSlots = 30;

    void Awake()
    {
        Inven = GameObject.Find("Inventory");
        Slot = Inven.transform.Find("Slot").gameObject;
        Image[0] = null;
        CopyImage();
        InitInventory();
        SetingSlots(4, new Vector3(0,-3f,0));
        InitMyItme();
        InvenSlot.GiveSlotItem(InvenSlot.ItemType.AttackTile, 1);
        InvenSlot.GiveSlotItem(InvenSlot.ItemType.HealPotion, 3);
        InvenSlot.GiveSlotItem(InvenSlot.ItemType.STRUp, 1);
    }



    void Start()
    {
    }

    void Update()
    {
    }


    void InitMyItme()
    {
        InvenSlot.GiveSlotItem((InvenSlot.ItemType)PlayerPrefs.GetInt("ShopItem"), 1);
    }

    void InitInventory()
    {
        Slots = new InvenSlot[MaxSlots];
        Slots_Rect = new RectTransform[MaxSlots];
        for(int i = 0; i < Slots.Length; i++)
        {
            Slots[i] = Instantiate(Slot, Inven.transform).GetComponent<InvenSlot>();
            Slots[i].Item = InvenSlot.ItemType.None;
            Slots[i].ItemCount = 0;
            Slots[i].gameObject.transform.position = NullPos;
            Slots_Rect[i] = Slots[i].GetComponent<RectTransform>();
        }
    }

    void SetingSlots(int Line, Vector3 StartPos)
    {
        float yCount = 0;
        float xCount = 0;
        for(int i = 0; i < Slots.Length; i++,xCount++)
        {
            if (xCount >= 4)
                yCount += 1;
            xCount = xCount % Line;
            Slots[i].transform.position = StartPos + new Vector3((Blank * xCount), -(yCount * Blank), 10);
            Slots[i].InitTempCollider();
        }
    }

    void CopyImage()
    {
        ItemImage = new Sprite[Image.Length];
        for(int i = 0; i < ItemImage.Length; i++)
        {
            ItemImage[i] = Image[i];
        }
    }
}
