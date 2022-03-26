using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopButtonCtrl : MonoBehaviour
{
    public InvenSlot.ItemType item;
  

    public Sprite[] ItemImage;
    Image image;

    void Awake()
    {
        image = transform.Find("Image").GetComponent<Image>();    
    }

    void Start()
    {
    }

    void Update()
    {
        ChackImage();
    }

    void ChackImage()
    {
        image.sprite = ItemImage[(int)item];
    }

    public void OnButtonDown()
    {
        PlayerPrefs.SetInt("ShopItem", (int)item);
        SceneManager.LoadScene("NextShop");
    }
}
