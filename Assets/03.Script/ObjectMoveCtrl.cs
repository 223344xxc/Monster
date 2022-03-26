using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveCtrl : MonoBehaviour
{
    public enum Distance
    {
        None,   //없음
        Left,   //오른쪽
        Right,  //왼쪽
        Up,     //위
        Down    //아래
    }

    public Vector3 Target;
    public Vector3 WarpTemp = Vector3.zero;
    public Vector3 DoteTile;
    Vector3 NowSpeed = Vector3.zero;

    public Vector2 PosIdx = Vector2.zero;
    public Vector2 TempPos = Vector2.zero;
    public Vector2 BackPos;
    public Vector2 TempAttackPos = Vector2.zero;
    public Vector2 Object_Start_Pos;

    public bool isMove = true;
    public bool NowWalk = false;
    public bool isAttack = false;
    public bool DoteDamage;
    bool TileAttack = true;

    public float DelayTime = 1;

    public Animator Object_Animator;
    public Ability Object_Ability;
    public BuffMgr Object_BuffMgr;

    void Awake()
    {
        Object_Animator = GetComponent<Animator>();
        Object_Ability = GetComponent<Ability>();
        Object_BuffMgr = GetComponent<BuffMgr>();
    }

    void Start()
    {
        TpObject(Object_Start_Pos.x, Object_Start_Pos.y);
    }
    void Update()
    {
        
    }
    public void TpObject(float x = 0, float y = 0)
    {
        if (gameObject.tag == "MoveObject")
        {
            BlockMgr.Blocks_Com[(int)y][(int)x].MoveObject = true;
            TempPos.x = x;
            TempPos.y = y;
            BackPos = TempPos;
        }
        PosIdx = new Vector2(x, y);
        transform.position = BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position;
        transform.position += new Vector3(0, 0, -1);
    }
    public void ChackPos()
    {
        if (transform.position.x >= (BlockMgr.Blocks[0].Length * BlockMgr.Blank) - (BlockMgr.Blank / 2.0f))
        {
            StopWalk();
            transform.position = new Vector3(-BlockMgr.Blank / 2.0f + 0.01f, transform.position.y, transform.position.z);
            SetTarget(Distance.Right);
            StartWalk(BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position, Distance.Right);
        }
        else if (transform.position.x <= -(BlockMgr.Blank / 2.0f))
        {
            StopWalk();
            transform.position = new Vector3((BlockMgr.Blocks[(int)PosIdx.y].Length * BlockMgr.Blank) - BlockMgr.Blank / 2.0f - 0.01f, transform.position.y, transform.position.z);
            SetTarget(Distance.Left);
            StartWalk(BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position, Distance.Left);
        }
        else if (transform.position.y >= (BlockMgr.Blocks.Length * BlockMgr.Blank) - (BlockMgr.Blank / 2.0f))
        {
            StopWalk();
            transform.position = new Vector3(transform.position.x, -BlockMgr.Blank / 2.0f + 0.01f, transform.position.z);
            SetTarget(Distance.Up);
            StartWalk(BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position, Distance.Up);
        }
        else if (transform.position.y <= -(BlockMgr.Blank / 2.0f))
        {
            StopWalk();
            transform.position = new Vector3(transform.position.x, (BlockMgr.Blocks.Length * BlockMgr.Blank) - BlockMgr.Blank / 2.0f - 0.01f, transform.position.z);
            SetTarget(Distance.Down);
            StartWalk(BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position, Distance.Down);
        }
    }

    public void StopWalk()
    {
        DelayTime = 1;
        isMove = true;
        NowWalk = false;
    }

    void ChackBlock()
    {
        if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].Type == Block.BlockType.Fire)
        {
            DoteTile = BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position;
            DoteDamage = true;
        }
        else if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].Type == Block.BlockType.AttackTile && TileAttack)
        {
            isAttack = true;
            TileAttack = false;
            BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].AttackCount += 1;
            TempAttackPos.x = (int)PosIdx.x;
            TempAttackPos.y = (int)PosIdx.y;
        }
        else if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].Type == Block.BlockType.STRUp)
        {
            PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Power_Buff);
            Object_BuffMgr.GiveBuff(BuffMgr.BuffType.STRUp, 2, 20);
            BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].Type = Block.BlockType.None;
        }
        else if (TempAttackPos.x != (int)PosIdx.x || TempAttackPos.y != (int)PosIdx.y)
            TileAttack = true;
    }

    public void Walk()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Target, ref NowSpeed, Object_Ability.MoveSpeed * DelayTime);

        if (Vector3.Distance(transform.position, Target) <= 0.1f)
        {
            DelayTime = 1;
            isMove = true;
            if (gameObject.tag == "Player")
                Object_Animator.SetInteger("WalkAnim", (int)Distance.None);
            BackPos = PosIdx;
            ChackBlock();
        }
        if (Vector3.Distance(transform.position, Target) <= 0.0001f)
        {
            if (gameObject.tag == "Player")
                Object_Animator.speed = 1;
            transform.position = Target;
            NowWalk = false;
            DelayTime = 1;
        }

        return;
    }
    public void StartWalk(Vector3 TargetObject, Distance dis = Distance.None)
    {
        Target = TargetObject + new Vector3(0, 0, transform.position.z);
        isMove = false;
        NowWalk = true;
        if (gameObject.tag == "MoveObject")
        {
            BlockMgr.Blocks_Com[(int)BackPos.y][(int)BackPos.x].MoveObject = false;
        }
        if (gameObject.tag == "Player")
        {
            if (dis == Distance.Right)
                gameObject.transform.localScale = new Vector3(-Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            else if (dis == Distance.Left)
                gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            Object_Animator.SetInteger("WalkAnim", (int)dis);
        }
    }

    public void MoveCtrl(Distance dis, bool Move = false)
    {
        if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].Type == Block.BlockType.Ice && gameObject.tag == "Player" && !Move)
        {
            switch (dis)
            {
                case Distance.Right:
                    if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x - 1].Type != Block.BlockType.Wall && !BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x - 1].MoveObject && !BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x + 1].MoveObject)
                    {
                        return;
                    }
                    break;
                case Distance.Left:
                    if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x + 1].Type != Block.BlockType.Wall && !BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x + 1].MoveObject && !BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x - 1].MoveObject)
                    {
                        return;
                    }
                    break;
                case Distance.Up:
                    if (BlockMgr.Blocks_Com[(int)PosIdx.y - 1][(int)PosIdx.x].Type != Block.BlockType.Wall && !BlockMgr.Blocks_Com[(int)PosIdx.y - 1][(int)PosIdx.x].MoveObject && !BlockMgr.Blocks_Com[(int)PosIdx.y + 1][(int)PosIdx.x].MoveObject)
                    {
                        return;
                    }
                    break;
                case Distance.Down:
                    if (BlockMgr.Blocks_Com[(int)PosIdx.y + 1][(int)PosIdx.x].Type != Block.BlockType.Wall && !BlockMgr.Blocks_Com[(int)PosIdx.y + 1][(int)PosIdx.x].MoveObject && !BlockMgr.Blocks_Com[(int)PosIdx.y - 1][(int)PosIdx.x].MoveObject)
                    {
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
        if (gameObject.tag == "Player")
        {
            for (int i = 0; i < BlockMgr.MoveObjects.Length; i++)
            {
                switch (dis)
                {
                    case Distance.Right:
                        if (PosIdx.x + 1 == BlockMgr.MoveObjects_MoveCtrl[i].PosIdx.x && PosIdx.y == BlockMgr.MoveObjects_MoveCtrl[i].PosIdx.y)
                        {
                            BlockMgr.MoveObjects_StoneCtrl[i].MoveObject_Move(dis);
                            if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].Type == Block.BlockType.Ice)
                                MoveCtrl(Distance.Left, true);
                            return;
                        }
                        break;
                    case Distance.Left:
                        if (PosIdx.x - 1 == BlockMgr.MoveObjects_MoveCtrl[i].PosIdx.x && PosIdx.y == BlockMgr.MoveObjects_MoveCtrl[i].PosIdx.y)
                        {
                            BlockMgr.MoveObjects_StoneCtrl[i].MoveObject_Move(dis);
                            if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].Type == Block.BlockType.Ice)
                                MoveCtrl(Distance.Right, true);
                            return;
                        }
                        break;
                    case Distance.Up:
                        if (PosIdx.x == BlockMgr.MoveObjects_MoveCtrl[i].PosIdx.x && PosIdx.y + 1 == BlockMgr.MoveObjects_MoveCtrl[i].PosIdx.y)
                        {
                            BlockMgr.MoveObjects_StoneCtrl[i].MoveObject_Move(dis);
                            if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].Type == Block.BlockType.Ice)
                                MoveCtrl(Distance.Down, true);
                            return;
                        }
                        break;
                    case Distance.Down:
                        if (PosIdx.x == BlockMgr.MoveObjects_MoveCtrl[i].PosIdx.x && PosIdx.y - 1 == BlockMgr.MoveObjects_MoveCtrl[i].PosIdx.y)
                        {
                            BlockMgr.MoveObjects_StoneCtrl[i].MoveObject_Move(dis);
                            if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].Type == Block.BlockType.Ice)
                                MoveCtrl(Distance.Up, true);
                            return;
                        }
                        break;
                    case Distance.None:
                        break;
                }
            }
        }
        SetTarget(dis);
        if ((int)PosIdx.y >= 0 && (int)PosIdx.y < BlockMgr.Blocks.Length && (int)PosIdx.x >= 0 && (int)PosIdx.x < BlockMgr.Blocks[0].Length)
            StartWalk(BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position, dis);
    }
    bool SetTarget(Distance dis)
    {
        TempPos = PosIdx;
        switch (dis)
        {
            case Distance.Right:
                if (PosIdx.x < BlockMgr.LineX - 1)
                {
                    PosIdx.x += 1;
                    DelayTime += 1;
                }
                else
                {
                    if (BlockMgr.Blocks_Com[(int)PosIdx.y][0].Type != Block.BlockType.Wall)
                    {
                        DelayTime += 1;
                        WarpTemp = BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position + new Vector3(BlockMgr.Blank, 0, 0);
                        StartWalk(WarpTemp, dis);
                        PosIdx = new Vector2(-1, PosIdx.y);
                        return false;
                    }
                    else if (BlockMgr.Blocks_Com[(int)PosIdx.y][0].Type == Block.BlockType.Wall || BlockMgr.Blocks_Com[(int)PosIdx.y][0].MoveObject)
                    {
                        StartWalk(BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position, dis);
                        return false;
                    }
                }
                break;

            case Distance.Left:
                if (PosIdx.x > 0)
                {
                    PosIdx.x -= 1;
                    DelayTime += 1;
                }
                else
                {
                    if (BlockMgr.Blocks_Com[(int)PosIdx.y][BlockMgr.Blocks[(int)PosIdx.y].Length - 1].Type != Block.BlockType.Wall)
                    {
                        DelayTime += 1;
                        WarpTemp = BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position - new Vector3(BlockMgr.Blank, 0, 0);
                        StartWalk(WarpTemp, dis);
                        PosIdx = new Vector2(BlockMgr.Blocks[(int)PosIdx.y].Length, PosIdx.y);
                        return false;
                    }
                    else if (BlockMgr.Blocks_Com[(int)PosIdx.y][BlockMgr.Blocks[(int)PosIdx.y].Length - 1].Type == Block.BlockType.Wall || BlockMgr.Blocks_Com[(int)PosIdx.y][BlockMgr.Blocks[(int)PosIdx.y].Length - 1].MoveObject)
                    {
                        StartWalk(BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position, dis);
                        return false;
                    }
                }

                break;

            case Distance.Down:
                if (PosIdx.y > 0)
                {
                    PosIdx.y -= 1;
                    DelayTime += 1;
                }
                else
                {
                    if (BlockMgr.Blocks_Com[BlockMgr.Blocks.Length - 1][(int)PosIdx.x].Type != Block.BlockType.Wall)
                    {
                        DelayTime += 1;
                        WarpTemp = BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position - new Vector3(0, BlockMgr.Blank, 0);
                        StartWalk(WarpTemp, dis);
                        PosIdx = new Vector2(PosIdx.x, BlockMgr.Blocks.Length);
                        return false;
                    }
                    else if (BlockMgr.Blocks_Com[BlockMgr.Blocks.Length - 1][(int)PosIdx.x].Type == Block.BlockType.Wall || BlockMgr.Blocks_Com[BlockMgr.Blocks.Length][(int)PosIdx.x].MoveObject)
                    {
                        StartWalk(BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position, dis);
                        return false;
                    }
                }
                break;

            case Distance.Up:
                if (PosIdx.y < BlockMgr.LineY - 1)
                {
                    PosIdx.y += 1;
                    DelayTime += 1;
                }
                else
                {
                    if (BlockMgr.Blocks_Com[0][(int)PosIdx.x].Type != Block.BlockType.Wall)
                    {
                        DelayTime += 1;
                        WarpTemp = BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position + new Vector3(0, BlockMgr.Blank, 0);
                        StartWalk(WarpTemp, dis);
                        PosIdx = new Vector2(PosIdx.x, -1);
                        return false;
                    }
                    else if (BlockMgr.Blocks_Com[0][(int)PosIdx.x].Type == Block.BlockType.Wall || BlockMgr.Blocks_Com[0][(int)PosIdx.x].MoveObject)
                    {
                        StartWalk(BlockMgr.Blocks[(int)PosIdx.y][(int)PosIdx.x].transform.position, dis);
                        return false;
                    }
                }
                break;
            case Distance.None:
                return false;
            default:
                return false;
        }

        if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].Type == Block.BlockType.Ice && !BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].MoveObject)
        {
            DelayTime -= 0.5f;
            SetTarget(dis);
            return true;
        }
        else if (BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].Type == Block.BlockType.Wall || BlockMgr.Blocks_Com[(int)PosIdx.y][(int)PosIdx.x].MoveObject)
        {
            PosIdx = TempPos;
            DelayTime -= 1;
        }
        else
            DelayTime -= 1;
        return true;
    }

    public Distance SelectDistance(Vector3 MoveDistance)
    {
        if (Mathf.Abs(MoveDistance.x) > Mathf.Abs(MoveDistance.y))
        {
            if (MoveDistance.x < 0)
                return Distance.Left;
            else
                return Distance.Right;
        }
        else
        {
            if (MoveDistance.y < 0)
                return Distance.Down;
            else
                return Distance.Up;
        }
    }
}
