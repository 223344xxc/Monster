using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMgr : MonoBehaviour
{
    public GameObject[] TempBlocks;
    public GameObject[] C_Blank;
    public static GameObject[][] Blocks;
    public static GameObject[] MoveObjects;
    public static ObjectMoveCtrl[] MoveObjects_MoveCtrl;
    public static MoveStoneCtrl[] MoveObjects_StoneCtrl;
    public static Block[][] Blocks_Com;

    public static float Blank;
    public float Fire_Idx_Delay;
    float Fire_Idx_Time = 0;
    public static int LineY = 0;
    public static int LineX = 0;
    public int Fire_Idx = 0;

    public Sprite[] NorMalTiles = new Sprite[6];  
    public Sprite[] Fire_Sprite;
    public static Sprite[] Pow_Tiles = new Sprite[4];
    public static Sprite FireTile;
    public static Sprite WallTile;
    public static Sprite IceTile;

    void Awake()
    {
        TempBlocks = GameObject.FindGameObjectsWithTag("Block");
        MoveObjects = GameObject.FindGameObjectsWithTag("MoveObject");
        GetTileSprite();
        CountBlank();
        LoadBlock();
        SetComponent();
    }

    void Start() { }
    void Update() { Update_Fire_Anim_Idx(); }

    void Update_Fire_Anim_Idx()
    {
        Fire_Idx_Time += Time.deltaTime;
        if(Fire_Idx_Time >= Fire_Idx_Delay)
        {
            if(Fire_Idx < Fire_Sprite.Length)
            {
                FireTile = Fire_Sprite[Fire_Idx];
                Fire_Idx += 1;
                Fire_Idx_Time = 0;
                if (Fire_Idx == Fire_Sprite.Length)
                    Fire_Idx = 0;
            }
        }
    }

    void GetTileSprite()
    {
        FireTile = GameObject.Find("Fire").GetComponent<SpriteRenderer>().sprite;
        WallTile = GameObject.Find("Wall").GetComponent<SpriteRenderer>().sprite;
        IceTile = GameObject.Find("Ice").GetComponent<SpriteRenderer>().sprite;
    }

    //블럭간의 간격을 구합니다
    void CountBlank()
    {
        int count = 0;
        for (int i = 0; i < TempBlocks.Length; i++)
        {
            if(TempBlocks[i].transform.position.y == 0)
            {
                count += 1;
            }
        }
        C_Blank = new GameObject[count];
        count = 0;
        for (int i = 0; i < TempBlocks.Length; i++)
        {
            if (TempBlocks[i].transform.position.y == 0)
            {
                C_Blank[count] = TempBlocks[i];
                count += 1;
            }
        }

        for (int i = 0; i < C_Blank.Length; i++)
        {
            for (int c = 1; c < C_Blank.Length; c++)
            {
                if (C_Blank[c - 1].transform.position.x > C_Blank[c].transform.position.x)
                {
                    GameObject temp = C_Blank[c - 1];
                    C_Blank[c - 1] = C_Blank[c];
                    C_Blank[c] = temp;
                }
                else
                    continue;
            }
        }

        if (C_Blank[0].transform.position.x == 0)
            Blank = C_Blank[1].transform.position.x;
        else
            Blank = C_Blank[0].transform.position.x;
    }

    //블럭을 로딩합니다
    void LoadBlock()
    {

        //블럭 초기화
        Blocks = new GameObject[0][];
        GameObject FirstBlock = TempBlocks[0];

        //기준블럭 검색
        for (int i = 0; i < TempBlocks.Length; i++)
        {
            if (FirstBlock.transform.position.x >= TempBlocks[i].transform.position.x && FirstBlock.transform.position.y >= TempBlocks[i].transform.position.y)
            {
                FirstBlock = TempBlocks[i];
            }
        }
        float[] TempY = new float[1000];//최대 범위 지정
        float[] TempX = new float[1000];//최대 범위 지정
        float[] TempPos = new float[TempBlocks.Length];

        InitList(TempY);
        InitList(TempX);

        TempY[0] = TempBlocks[0].transform.position.y;
        TempX[0] = TempBlocks[0].transform.position.x;

        for (int i = 0; i < TempPos.Length; i++)
        {
            TempPos[i] = TempBlocks[i].transform.position.y;
        }
        FindCount(TempY, TempPos);

        for (int i = 0; i < TempPos.Length; i++)
        {
            TempPos[i] = TempBlocks[i].transform.position.x;
        }
        FindCount(TempX, TempPos);

        LineY = GetLength(TempY);
        LineX = GetLength(TempX);

        SetGround(LineY, LineX);
    }

    //블럭을 2차원 배열에 초기화 합니다
    void SetGround(int Y, int X)
    {
        //블럭간 간격
        float tempY = 0;
        float tempX = 0;

        Blocks = new GameObject[Y][];
        Blocks_Com = new Block[Y][];

        for (int i = 0; i < Y; i++)
        {
            Blocks[i] = new GameObject[X];
            Blocks_Com[i] = new Block[X];
        }

        for (int i = 0; i < Y; i++)
        {
            for (int c = 0; c < X; c++)
            {
                for (int a = 0; a < TempBlocks.Length; a++)
                {
                    if (TempBlocks[a].transform.position.x == tempX && TempBlocks[a].transform.position.y == tempY)
                    {
                        Blocks[i][c] = TempBlocks[a];
                        break;
                    }
                }
                tempX += Blank;
            }
            tempX = 0;
            tempY += Blank;
        }
    }

    int GetLength(float[] temp)
    {
        int count = 0;
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] != -1)
                count += 1;
        }
        return count;
    }

    //서로다른 좌표의 갯수를 구합니다
    void FindCount(float[] Temp, float[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            for (int c = 0; c <= Temp.Length; c++)
            {
                if (c == Temp.Length)
                {
                    for (int t = 0; t < Temp.Length; t++)
                    {
                        if (Temp[t] == -1)
                        {
                            Temp[t] = pos[i];
                            break;
                        }
                    }
                    break;
                }
                if (Temp[c] == pos[i])
                    break;
            }
        }
    }

    void InitList(float[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i] = -1;
        }
    }

    //Block 컴포넌트를 구합니다
    void SetComponent()
    {
        for(int y = 0; y < Blocks.Length; y++)
        {
            for(int x = 0; x < Blocks[y].Length; x++)
            {
                Blocks_Com[y][x] = Blocks[y][x].GetComponent<Block>();
            }
        }

        MoveObjects_MoveCtrl = new ObjectMoveCtrl[MoveObjects.Length];
        MoveObjects_StoneCtrl = new MoveStoneCtrl[MoveObjects.Length];
        
        for(int i = 0; i < MoveObjects_MoveCtrl.Length; i++)
        {
            MoveObjects_MoveCtrl[i] = MoveObjects[i].GetComponent<ObjectMoveCtrl>();
            MoveObjects_StoneCtrl[i] = MoveObjects[i].GetComponent<MoveStoneCtrl>();
        }
    }
}
