using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour
{
    public enum ItemType
    {
        None,
        AttackTile,
        HealPotion,
        STRUp,
        TestItem4,
    };

    public enum UseItemType
    {
        None,
        Active,
        Tile,
    }

    public ItemType Item = ItemType.None;
    public UseItemType UseType = UseItemType.None;

    public GameObject TempCollider;
    public GameObject TempUi;
    public GameObject Player;

    public PlayerCtrl Player_Com;

    public bool ButtonDown = false;
    public float ButtonDownTime = 0;
    public int ItemCount = 1;

    public Image ItemSprite;
    
    public Text ItemCount_Text;

    void Awake()
    {
        TempUi = GameObject.Find("TempUi");
        ItemCount_Text = transform.Find("Text").GetComponent<Text>();
        ItemSprite = transform.Find("Image").GetComponent<Image>();
        Player = GameObject.FindWithTag("Player");
        Player_Com = Player.GetComponent<PlayerCtrl>();
    }

    void Start()
    {

    }


    void Update()
    {
        CountTime();
        ChangeImage();
        ItemCountUpdate();
    }

    public void InitTempCollider()
    {
        Instantiate(TempCollider,transform).transform.position = transform.position;
    }

    void ItemCountUpdate()
    {
        if (ItemCount <= 0)
        {
            ItemCount_Text.text = "";
            return;
        }
        ItemCount_Text.text = ItemCount.ToString("f0");
    }

    void ChangeImage()
    {
        if (Item == ItemType.None)
        {
            ItemCount = 0;
        }
        if (ItemCount <= 0)
        {
            ItemSprite.sprite = null;
            ItemSprite.gameObject.SetActive(false);
            return;
        }
        else
        {
            ItemSprite.gameObject.SetActive(true);
        }

        ItemSprite.sprite = Inventory.ItemImage[(int)Item];
    }

    void CountTime()
    {
        if (ButtonDownTime >= 0.01f && Item != ItemType.None && ItemCount > 0)
        {
            CursorItemCtrl.isMove = true;
            CursorItemCtrl.image.sprite = ItemSprite.sprite;
            CursorItemCtrl.ChangeTileItem = Item;
        }
        if (ButtonDown)
            ButtonDownTime += Time.deltaTime;
    }

    void UseItem()
    {
        switch(Item)
        {
            case ItemType.HealPotion:
                PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Heal);
                Player_Com.Player_Ability.Damaged(-4);
                break;
            default:
                break;
        }
        ItemCount -= 1;
        PlayerCtrl.MouseReset = true;   
        CamRayMgr.ResetSelObtion();
    }

    public void OnButtonDown()
    {
        if (BossStageCamCtrl.GetCamPosition() == BossStageCamCtrl.CamPosition.None)
            return;
        if (UseType == UseItemType.Active)
        {
            if (BossStageCamCtrl.FirstSetTile)
                return;
            else
            {
                UseItem();
                return;
            }
        }
        ButtonDown = true;
        CamRayMgr.FreezSlot = true;
        CamRayMgr.InvenSlotObject = gameObject;
    }

    public void OnButtonUp()
    {
        if (BossStageCamCtrl.GetCamPosition() == BossStageCamCtrl.CamPosition.None)
            return;
        CamRayMgr.ChangeTileFuntion();
        CursorItemCtrl.isMove = false;
        CamRayMgr.FreezSlot = false;
        ButtonDown = false;
        ButtonDownTime = 0;
    }


    public static void GiveSlotItem(ItemType Item, int count = 1)
    {
        bool NotFind = true;
        InvenSlot slot = null;
        for(int i = 0; i < Inventory.Slots.Length; i++)
        {
            if (Inventory.Slots[i].Item == Item)
            {
                NotFind = false;
                slot = Inventory.Slots[i];
                break;
            }
        }
        if(NotFind)
        {
            for (int i = 0; i < Inventory.Slots.Length; i++)
            {
                if (Inventory.Slots[i].Item == ItemType.None)
                {
                    slot = Inventory.Slots[i];
                    break;
                }
            }
        }

        switch(Item)
        {
            case ItemType.None:return;
            case ItemType.AttackTile:
                slot.Item = ItemType.AttackTile;
                slot.ItemCount += count;
                slot.UseType = UseItemType.Tile;
                break;
            case ItemType.HealPotion:
                slot.Item = ItemType.HealPotion;
                slot.ItemCount += count;
                slot.UseType = UseItemType.Active;
                break;
            case ItemType.STRUp:
                slot.Item = ItemType.STRUp;
                slot.ItemCount += count;
                slot.UseType = UseItemType.Tile;
                break;
            default:
                break;
        }
    }
}
