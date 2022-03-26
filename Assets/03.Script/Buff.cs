using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public BuffMgr.BuffType buff;
    public int Turn;
    public float Value;
    public float TempValue;

    bool FirstSetBuff = true;

    Ability Object_Ability;

    void Start()
    {
    }

    void Update()
    {

    }

    public void SetBuff(Ability Object_Ability = null, BuffMgr.BuffType buff = BuffMgr.BuffType.None, int Turn = 0, float Value = 0)
    {
        this.buff = buff;
        this.Turn = Turn;
        this.Value = Value;
        this.Object_Ability = Object_Ability;
        Copy_TempValue();
        Set_Buff_Effect();
    }

    public void Copy_TempValue()
    {
        if (FirstSetBuff)
        {
            switch (buff)
            {
                case BuffMgr.BuffType.None:
                    break;
                case BuffMgr.BuffType.Slow:
                    TempValue = Object_Ability.MoveSpeed;
                    break;
                case BuffMgr.BuffType.STRUp:
                    TempValue = Object_Ability.STR;
                    break;
                default:
                    break;
            }
            FirstSetBuff = false;
        }
    }

    public void Set_Buff_Effect()
    {
        switch (buff)
        {
            case BuffMgr.BuffType.None:
                break;
            case BuffMgr.BuffType.Slow:
                Object_Ability.MoveSpeed = Value;
                break;
            case BuffMgr.BuffType.STRUp:
                Object_Ability.STR = Value;
                break;
            default:
                break;
        }
    }

    public void DeleteBuff()
    {
        switch(buff)
        {
            case BuffMgr.BuffType.None:
                break;
            case BuffMgr.BuffType.Slow:
                Object_Ability.MoveSpeed = TempValue;
                break;
            case BuffMgr.BuffType.STRUp:
                Object_Ability.STR = TempValue;
                break;
            default:
                break;
        }
    }
}
