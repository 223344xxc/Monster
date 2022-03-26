using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMgr : MonoBehaviour
{ 
    public enum BuffType
    {
        None,
        Slow,
        STRUp,
    }

    public Buff[] buff = new Buff[0];

    Buff[] TempBuff;

    Ability Object_Ability;

    void Awake()
    {
        Object_Ability = GetComponent<Ability>();
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void UpdateBuff(ref float HP, ref float ATR, ref float MoveSpeed)
    {

    }

    public void BuffTurnCtrl()
    {
        for(int i = 0; i < buff.Length; i ++)
        {
            buff[i].Turn -= 1;
            if(buff[i].Turn <= 0)
            {
                DeleteBuff(i);
            }
        }
    }

    //같은 버프가 2개 적용될 경우 가장 마지막에 적용된 버프로 덮어 쓴다
    public void GiveBuff(BuffType buffType = BuffType.None, int Turn = 0, float Value = 0)
    {
        for (int i = 0; i < buff.Length; i++)
            if (buff[i].buff == buffType)
            {
                buff[i].SetBuff(Object_Ability, buffType, Turn, Value);
                return;
            }

        TempBuff = new Buff[buff.Length];

        for (int i = 0; i < buff.Length; i++)
            TempBuff[i] = buff[i];

        buff = new Buff[buff.Length + 1];

        for (int i = 0; i < TempBuff.Length; i++)
            buff[i] = TempBuff[i];

        buff[buff.Length - 1] = gameObject.AddComponent<Buff>();
        buff[buff.Length - 1].SetBuff(Object_Ability, buffType, Turn, Value);
    }

    public void DeleteBuff(int BuffIndex)
    {
        int SubCount = 0;
        TempBuff = new Buff[buff.Length];
        buff[BuffIndex].DeleteBuff();
        DestroyImmediate(buff[BuffIndex]);
        for (int i = 0; i < buff.Length; i++)
            TempBuff[i] = buff[i];
        buff = new Buff[buff.Length - 1];

        for (int i = 0; i < TempBuff.Length; i++)
        {
            if (i == BuffIndex)
                continue;
            else
                buff[SubCount] = TempBuff[i];
            SubCount += 1;
        }
    }
}
