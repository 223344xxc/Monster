using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum BlockType
    {
        None, //기본 블럭
        Fire,
        Ice,
        Wall,
        AttackTile,
        HealItem,
        STRUp,
        TestItem4,
    }

    public BlockType Type;

    public int AttackCount = 0;

    public bool MoveObject = false;
    public bool Invisible = false;
    public bool IsSnow = false;

    public Sprite[] TileSprite = new Sprite[8];
    public Sprite Snow;
    SpriteRenderer Renderer;

    void Awake()
    {
        AttackCount = 0;
        Renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        SetTile();
        TileSprite[0] = Renderer.sprite;
        if (Invisible)
            gameObject.SetActive(false);
    }

    void Update()
    {
        ChackTile();
        if (IsSnow)
            Renderer.sprite = Snow;
    }

    void SetTile()
    {
        TileSprite[1] = BlockMgr.FireTile;
        TileSprite[2] = BlockMgr.IceTile;
        TileSprite[3] = BlockMgr.WallTile;
    }
    void ChackTile()
    {
        if (Type == BlockType.Fire)
        {
            Renderer.sprite = BlockMgr.FireTile;
            gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
        else if (Type != BlockType.Fire)
        {
            Renderer.sprite = TileSprite[(int)Type];
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        if (Type == BlockType.AttackTile && AttackCount >= 2)
        {
            Type = BlockType.None;
            AttackCount = 0;
        }
    }
}
