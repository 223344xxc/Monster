using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePlayerCtrl : MonoBehaviour
{
    public Vector3 MouseDown;
    public Vector3 MouseUp;
    public Vector3 MoveDistance;

    public bool IsDialogue = true;
    public bool DoteDamage;
    public static bool ReadDialogue = true;
    public static bool ResetMouse = false;

    public float DoteDamageCount = 0f;

    public DialogueTrigger Dialogue_Start_Button;

    public ObjectMoveCtrl Player_MoveCtrl;
    public Ability Player_Ability;
    void Awake()
    {
        Player_MoveCtrl = GetComponent<ObjectMoveCtrl>();
        Player_Ability = GetComponent<Ability>();
        if (IsDialogue)
            Dialogue_Start_Button = GameObject.Find("StartButton").GetComponent<DialogueTrigger>();
    }

    void Start()
    {
        BgmMgr.Start_Bgm(BgmMgr.BgmType.PuzzleStage);
        if (IsDialogue)
            Dialogue_Start_Button.TriggerDialogue();
    }
    void Update()
    { 
        if (ReadDialogue && IsDialogue)
            return;
        if (Player_MoveCtrl.isMove)
            Move();
        if (!Player_MoveCtrl.isMove || Player_MoveCtrl.NowWalk)
            Player_MoveCtrl.Walk();

        Player_MoveCtrl.ChackPos();

        if (DoteDamage)
            ChackDoteDamage();
    }

    void ChackDoteDamage()
    {
        if (Vector3.Distance(Player_MoveCtrl.DoteTile, transform.position) >= 1.5)
        {
            DoteDamage = false;
            return;
        }
        DoteDamageCount += Time.deltaTime;
        if (DoteDamageCount >= 1f && Player_Ability.GetHp() > 0)
        {
            DoteDamageCount = 0;
            Player_Ability.Damaged(1);
        }
    }

    void Move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseDown = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0) && !ResetMouse)
        {
            MouseUp = Input.mousePosition;
            MoveDistance = MouseUp - MouseDown;

            if (MoveDistance.magnitude <= 5)
                return;
            Player_MoveCtrl.MoveCtrl(Player_MoveCtrl.SelectDistance(MoveDistance));
        }
        else if (Input.GetMouseButtonUp(0) && ResetMouse)
        {
            MouseDown = Vector3.zero;
            MouseUp = Vector3.zero;
            MoveDistance = Vector3.zero;
            ResetMouse = false;
        }
    }
}
