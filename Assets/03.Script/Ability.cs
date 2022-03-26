using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
 
    public float MaxHp;
    public float STR;
    private float Hp;
    public float MoveSpeed;//보스일 경우 스킬 쿨타임 플래이어일 경우 이동속도

    void Awake()
    {
        Hp = MaxHp;    
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Damaged(float Damage)
    {
        if (gameObject.tag == "Player" && Damage >= 0)
            PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Hit_Player);
        Hp -= Damage;
        if (Hp < 0)
            Hp = 0;
        else if (Hp > MaxHp)
            Hp = MaxHp;
    }

    public float GetHp()
    {
        return Hp;
    }

    public void SetHp(float Hp)
    {
        this.Hp = Hp;
    }
}
