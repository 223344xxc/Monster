using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice_Ball_Ctrl : MonoBehaviour
{
    GameObject Player;
    PlayerCtrl Player_Ctrl;

    public float Ice_Ball_Move_Speed = 1f;
    public float Ice_Ball_Rot_Speed = 1f;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Player_Ctrl = Player.GetComponent<PlayerCtrl>();
        Destroy(gameObject, 5);
    }
    void Update()
    {
        MoveCtrl();
    }

    void MoveCtrl()
    {
        transform.Rotate(0, 0, Ice_Ball_Rot_Speed);
        transform.position += (Player.transform.position - transform.position).normalized * 0.01f * Ice_Ball_Move_Speed;
        if (Vector3.Distance(transform.position, Player.transform.position) <= 0.5f)
        {
            PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Stun_Ice_Lance);
            Player_Ctrl.Set_Stun();
            BossCtrl.Stun_Trigger = true;
            BossCtrl.Stun_Attack = true;
            BossCtrl.Stun_Attack_Trigger = true;
            Destroy(gameObject);
        }
    }
}
