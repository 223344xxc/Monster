using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCtrl : MonoBehaviour
{
    public GameObject TargetObject;
    public GameObject Player;

    //public BossCtrl Boss;
    public PlayerCtrl Player_Ctrl;
    
    Vector3 AttackVel;
    
    public float AttackMoveTime = 0.05f;
    public float ReturnRadius = 0.5f;
    
    bool forAttack = true;


    void Awake()
    {
        TargetObject = GameObject.FindWithTag("Boss");
        Player = GameObject.FindWithTag("Player");
        //Boss = TargetObject.GetComponent<BossCtrl>();
        Player_Ctrl = Player.GetComponent<PlayerCtrl>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        rotationAttack();
        MoveAttack();
    }

    void MoveAttack()
    {
        if (forAttack)
            transform.position = Vector3.SmoothDamp(transform.position, TargetObject.transform.position, ref AttackVel, AttackMoveTime);
        else
            transform.position = Vector3.SmoothDamp(transform.position, Player.transform.position, ref AttackVel, AttackMoveTime);
        if (Vector3.Distance(transform.position, TargetObject.transform.position) <= ReturnRadius && forAttack)
        {
            BossCtrl.Boss_Ability.Damaged(Player_Ctrl.Player_Ability.STR);
            PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Attack_Player);
            BossCtrl.RedCtrl = true;
            forAttack = false;
        }
        else if (Vector3.Distance(transform.position, Player.transform.position) <= ReturnRadius && !forAttack)
        {
            Destroy(gameObject);
            forAttack = true;
        }
    }

    void rotationAttack()
    {
        transform.eulerAngles += new Vector3(0, 0, -20);
    }
}
