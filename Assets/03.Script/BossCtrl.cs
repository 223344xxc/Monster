using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossCtrl : MonoBehaviour
{
    enum AttackType
    {
        None,
        OneTile,
        Line,
    };

    enum SkillType
    {
        Ice_Ball,
        Ice_Lance,
        Ice_Blast,
        Snow,
        Ice_Thom,
        None
    }

    int TempNum;

    public float TempHP;
    public float Time_Count;
    public float Select_Time_Count;
    public float TempHPsub;
    public float RedTime;
    public float RedDelTime = 0.3f;
    float TempMoveSpeed;
    float Stun_Ice_Lance_Time;
    float Stun_Ice_Lance_Attack_Count = 0;
    float ClearCount = 0;


    public static bool SelectPlayerAttack = false;
    public static bool UseItem = false;
    public static bool AttackBoss = false;
    public static bool RedCtrl = false;
    public static bool Stun_Attack = false;
    public static bool Stun_Trigger = true;
    public bool Attack = false;
    public bool NowAttack = false;
    public bool UseItemTemp = UseItem;
    public bool AttackBossTemp = AttackBoss;
    public bool SpecialAttack = false;
    public bool More_Stun_Attack = false;
    static bool isDamaged = true;
    bool TempButtontransform = true;
    public static bool Stun_Attack_Trigger = true;

    public Sprite[] HpBars;

    public GameObject Ice_Lance; Vector3 Lance_Ofset = new Vector3(0, 1.35f, -1);
    public GameObject Ice_Blast; Vector3 Blast_Ofset = Vector3.zero; Vector3 Blast_rot_Ofset = Vector3.zero;
    public GameObject Ice_Thom;
    public GameObject Snow;
    public GameObject BulletLine;
    public GameObject AttackButton;
    public GameObject ItemButton;
    public GameObject Stun_Ice_Lance;
    public GameObject Ice_Ball;
    public GameObject SelectButtonUI;
    public GameObject UIBackGround;
    public GameObject BossHPBar;
    public GameObject Red;
    GameObject TempEffect;

    public static PlayerCtrl Player_Ctrl;

    public Image BossHPBarImage;

    public static int AtIdx = 0;
    public static int AttackStack = 0;
    public int BossAttackSkillCount = 0;
    public int[] Attacks;
    public int AtIdx_Temp;
    public int Attack_Idx_Length;
    public int ThomCount = 1;
    public int IceThomCollTurn = 5;
    public int CollTurn_Ice_Thom = 0;
    public int MoreSkill = 1;

    public static Vector3 Stun_Ice_Lance_Pos;
    public static Vector2[][] Attack_Idx;

    public static Ability Boss_Ability;

    public static void ResetAll()
    {
        AtIdx = 0;
        AttackStack = 0;
        SelectPlayerAttack = false;
        UseItem = false;
        AttackBoss = false;
        RedCtrl = false;
        Stun_Attack = false;
        Stun_Trigger = true;
        Stun_Attack_Trigger = true;
    }



    void Awake()
    {
        Boss_Ability = GetComponent<Ability>();
        AttackButton = GameObject.Find("AttackButton");
        ItemButton = GameObject.Find("ItemButton");
        SelectButtonUI = GameObject.Find("SelectButtonUI");
        UIBackGround = GameObject.Find("UIBackGround");
        BossHPBar = GameObject.Find("BossHPBar");
        Red = GameObject.Find("Red");
        Player_Ctrl = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        DelButton();
        BossHPBarImage = BossHPBar.GetComponent<Image>();
    }

    void Start()
    {   
        TempHP = Boss_Ability.GetHp();
        AtIdx = 0;
        Attack_Idx = new Vector2[BossAttackSkillCount][];
        TempMoveSpeed = Boss_Ability.MoveSpeed;
        if (SpecialAttack)
            LoadBossAttack(BossAttackSkillCount, SkillType.Ice_Thom);
        else
            LoadBossAttack(BossAttackSkillCount);
        BgmMgr.Start_Bgm(BgmMgr.BgmType.BossStage);
    }

    void Update()
    {
        if(PlayerCtrl.IsBossDie)
        {
            Red_Ctrl();
            BossClear();
            return;
        }
        Stun_Ice_Lance_();
        Red_Ctrl();
        if (BossStageCamCtrl.GetCamPosition() == BossStageCamCtrl.CamPosition.SetTile)
            return;
        if (SelPlayer())
            return;
        Time_Count += Time.deltaTime;

        if(Time_Count >= Boss_Ability.MoveSpeed)//skill cooltime
        {
            Start_Attack();
        }
        UpdateHpBar();
        if (Boss_Ability.GetHp() <= 0 && !PlayerCtrl.IsBossDie)
        {
            BgmMgr.Stop_Bmg();
            PlayerCtrl.IsBossDie = true;
            PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.BossDeath);
            return;
        }
    }

    void BossClear()
    {
        ClearCount += Time.deltaTime;
        if (ClearCount >= 3)
        {
            PlayerPrefs.SetString("Scene", "Boss_Clear");
            SceneManager.LoadScene("Loading");
        }
    }

    void Stun_Ice_Lance_()
    {
        if(Stun_Attack)
        {
            Stun_Ice_Lance_Time = Boss_Ability.MoveSpeed;
            Stun_Ice_Lance_Attack_Count += Time.deltaTime;
            if (Stun_Attack_Trigger)
            {
                Stun_Ice_Lance_Pos = new Vector3(Player_Ctrl.Player_MoveCtrl.PosIdx.x, Player_Ctrl.Player_MoveCtrl.PosIdx.y, 0);
                OneTileInitBulletLine();
                Stun_Attack_Trigger = false;
            }
            if(Stun_Ice_Lance_Attack_Count >= Stun_Ice_Lance_Time + 0.2f)
            {
                Instantiate(Stun_Ice_Lance).transform.position = BlockMgr.Blocks[(int)Stun_Ice_Lance_Pos.y][(int)Stun_Ice_Lance_Pos.x].transform.position + new Vector3(0, 0, -1);
                Stun_Ice_Lance_Attack_Count = 0;
                Stun_Attack = false;
            }
        }
        if (!Stun_Attack_Trigger && !Stun_Attack_Trigger && Stun_Trigger)
        {
            Stun_Ice_Lance_Attack_Count += Time.deltaTime;
            if (Stun_Ice_Lance_Attack_Count >= 1f)
            {
                Stun_Attack = true;
                Stun_Attack_Trigger = true;
                Stun_Trigger = false;
            }
        }
    }

    void InitSpecialThomAttack()
    {
        if (Player_Ctrl.Player_MoveCtrl.PosIdx.x <= 0)
            Player_Ctrl.Player_MoveCtrl.PosIdx.x += 1;
        else if (Player_Ctrl.Player_MoveCtrl.PosIdx.x >= BlockMgr.LineX - 1)
            Player_Ctrl.Player_MoveCtrl.PosIdx.x -= 1;
        if (Player_Ctrl.Player_MoveCtrl.PosIdx.y <= 0)
            Player_Ctrl.Player_MoveCtrl.PosIdx.y += 1;
        else if (Player_Ctrl.Player_MoveCtrl.PosIdx.y >= BlockMgr.LineY - 1)
            Player_Ctrl.Player_MoveCtrl.PosIdx.y -= 1;
        Player_Ctrl.Player_MoveCtrl.StartWalk(BlockMgr.Blocks[(int)Player_Ctrl.Player_MoveCtrl.PosIdx.y][(int)Player_Ctrl.Player_MoveCtrl.PosIdx.x].transform.position);
        for(int y = 0; y < BlockMgr.Blocks_Com.Length; y++)
        {
            for (int x = 0; x < BlockMgr.Blocks_Com[y].Length; x++)
            {
                if (x == 0 || x == BlockMgr.LineX - 1 || y == 0 || y == BlockMgr.LineY - 1)
                    BlockMgr.Blocks_Com[y][x].Type = Block.BlockType.Wall;
            }
        }
        SpecialAttack = false;
    }

    void ResetSpecialThomAttack()
    {
        for (int y = 0; y < BlockMgr.Blocks_Com.Length; y++)
        {
            for (int x = 0; x < BlockMgr.Blocks_Com[y].Length; x++)
            {
                if (x == 0 || x == BlockMgr.LineX - 1 || y == 0 || y == BlockMgr.LineY - 1)
                    if (BlockMgr.Blocks_Com[y][x].Type == Block.BlockType.Wall)
                        BlockMgr.Blocks_Com[y][x].Type = Block.BlockType.None;
            }
        }
    }

    void Red_Ctrl()
    {
        if (RedCtrl)
        {
            Red.transform.position = gameObject.transform.position - new Vector3(0, 0, 1);
            RedTime += Time.deltaTime;
            if(RedTime >= RedDelTime)
            {
                RedCtrl = false;
                RedTime = 0;
            }
        }
        else
            Red.transform.position = gameObject.transform.position + new Vector3(0, 0, 1);
    }

    void UpdateHpBar()
    {
        if (TempHP != Boss_Ability.GetHp())
        {
            TempHP = Boss_Ability.GetHp();
            if (Boss_Ability.GetHp() <= 0)
            {
                BossHPBar.SetActive(false);
            }
            else if (Boss_Ability.GetHp() > 0 && Boss_Ability.GetHp() < Boss_Ability.MaxHp / HpBars.Length)
            {
                BossHPBar.SetActive(true);
                BossHPBarImage.sprite = HpBars[0];
            }
            else
            {
                BossHPBar.SetActive(true);
                if((int)Boss_Ability.GetHp() / (int)(Boss_Ability.MaxHp / HpBars.Length - 1) < HpBars.Length && (int)Boss_Ability.GetHp() / (int)(Boss_Ability.MaxHp / HpBars.Length - 1) >= 0)
                BossHPBarImage.sprite = HpBars[(int)Boss_Ability.GetHp() / (int)(Boss_Ability.MaxHp / HpBars.Length - 1)];
            }
        }
    }

    bool SelPlayer()
    {
        UseItemTemp = UseItem;
        AttackBossTemp = Attack;
        if (SelectPlayerAttack)
        {
            Select_Time_Count += Time.deltaTime;
            if (Select_Time_Count >= 1f)
            {
                SelectButton();
                return true;
            }
            else
                return true;
        }
        else
            return false;
    }

    void SelectButton()
    {
        if (TempButtontransform)
        {
            AttackButton.transform.position -= new Vector3(0, 10000, 0);
            ItemButton.transform.position -= new Vector3(0, 10000, 0);
            SelectButtonUI.transform.position -= new Vector3(0, 10000, 0);
            UIBackGround.transform.position -= new Vector3(0, 10000, 0);
            TempButtontransform = !TempButtontransform;
            Player_Ctrl.SelectButton = true;
            Player_Ctrl.PlayerBuff.BuffTurnCtrl(); //턴 증가
            CollTurn_Ice_Thom += 1;
            if (CollTurn_Ice_Thom == IceThomCollTurn)
            {
                SpecialAttack = true;
                LoadBossAttack(BossAttackSkillCount, SkillType.Ice_Thom);
                CollTurn_Ice_Thom = 0;
            }
            if (CollTurn_Ice_Thom == 2)
            {
                MoreSkill = 2;
            }
        }
       
        if(UseItem)
        {
            BossStageCamCtrl.SetCamPosition(BossStageCamCtrl.CamPosition.SetTile);
            ResetButton();
        }
        else if(AttackBoss)
        {
            Player_Ctrl.Player_MoveCtrl.isAttack = true;
            ResetButton();
        }
    }

    void ResetButton()
    {
        SelectPlayerAttack = false;
        TempButtontransform = true;
        UseItem = false;
        AttackBoss = false;
        Player_Ctrl.SelectButton = false;
        Select_Time_Count = 0f;
        DelButton();
    }

    void DelButton()
    {
        AttackButton.transform.position += new Vector3(0, 10000, 0);
        ItemButton.transform.position += new Vector3(0, 10000, 0);
        SelectButtonUI.transform.position += new Vector3(0, 10000, 0);
        UIBackGround.transform.position += new Vector3(0, 10000, 0);
    }

    void LoadBossAttack(int AttackCount, SkillType Skill = SkillType.None)
    {
        bool Ice_Ball = false;
        if (Skill == SkillType.Ice_Thom)
            Boss_Ability.MoveSpeed = 0.5f;

        else
            Boss_Ability.MoveSpeed = TempMoveSpeed;
        Attacks = new int[AttackCount];
        if (Skill == SkillType.None)
        {
            for (int i = 0; i < Attacks.Length; i++)
            {
                if (i < 3 && !Ice_Ball)
                {
                    Attacks[i] = Random.Range((int)SkillType.Ice_Ball, (int)SkillType.Ice_Blast + MoreSkill);
                    if (Attacks[i] == (int)SkillType.Ice_Ball)
                        Ice_Ball = true;
                }
                else
                {
                    Attacks[i] = Random.Range((int)SkillType.Ice_Lance, (int)SkillType.Ice_Blast + MoreSkill);
                    if (Attacks[i] == (int)SkillType.Ice_Ball)
                        Debug.Break();
                }
                if (MoreSkill == 2 && Attacks[i] == (int)SkillType.Ice_Blast + 1)
                    MoreSkill = 1;
            }
        }
        else
        {
            for (int i = 0; i < Attacks.Length; i++)
            {
                Attacks[i] = (int)Skill;
            }
        }
        Time_Count -= (0.3f * AttackCount) - Boss_Ability.MoveSpeed;
    }

    void ReLoadBossAttacks(float DelayTime = 0)
    {
        Time_Count = DelayTime;
        NowAttack = false;
        if (AttackStack >= Attacks.Length - 1)
        {
            if (SpecialAttack)
            {
                LoadBossAttack(BossAttackSkillCount, SkillType.Ice_Thom);
            }
            else
                LoadBossAttack(BossAttackSkillCount);
            AttackStack = 0;
            ThomCount = 1;
            SelectPlayerAttack = true;
        }
        else
        {
            AttackStack += 1;
            if (AttackStack / 3 > 0 && AttackStack % 3 == 0)
                ThomCount += 1;
            if (ThomCount == 4)
                ResetSpecialThomAttack();
        }
    }
   

    void Start_Attack()
    {
        isDamaged = true;
        switch (Attacks[AttackStack])
        {
            case (int)SkillType.Ice_Lance:
                if (!NowAttack)
                {
                    NowAttack = true;
                    InitAttackRange(AttackType.OneTile, 5);
                    InitBulletLine();
                }
                if (Time_Count >= Boss_Ability.MoveSpeed + Boss_Ability.MoveSpeed)//bullit line time
                {
                    Skill_Ice_Lance();
                    ReLoadBossAttacks();
                }
                break;
            case (int)SkillType.Ice_Blast:
                if (!NowAttack)
                {
                    NowAttack = true;
                    InitAttackRange(AttackType.Line);
                    InitBulletLine();
                }
                if (Time_Count >= Boss_Ability.MoveSpeed + Boss_Ability.MoveSpeed)//bullit line time
                {
                    Skill_Ice_Blast();
                    ReLoadBossAttacks();
                }
                break;
            case (int)SkillType.Ice_Ball:
                if(!NowAttack)
                {
                    NowAttack = true;
                    InitAttackRange(AttackType.None);
                }
                if (Time_Count >= Boss_Ability.MoveSpeed + Boss_Ability.MoveSpeed)
                {
                    NowAttack = false;
                    Instantiate(Ice_Ball).transform.position = gameObject.transform.position;
                    PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Ice_Ball);
                    Boss_Attack();
                    ReLoadBossAttacks();
                }
                break;
            case (int)SkillType.Ice_Thom:
                if(!NowAttack)
                {
                    if (SpecialAttack)
                    {
                        InitSpecialThomAttack();
                        Time_Count -= Boss_Ability.MoveSpeed * 2;
                    }
                    InitAttackRange(AttackType.OneTile, ThomCount, 1, 1, 1, 1);
                    if (Time_Count >= 0)
                    {
                        NowAttack = true;
                        InitBulletLine();
                    }
                }
                if(Time_Count >= Boss_Ability.MoveSpeed + Boss_Ability.MoveSpeed)
                {
                    Skill_Ice_Thom();
                    ReLoadBossAttacks();
                }
                break;
            case (int)SkillType.Snow:
                if(!NowAttack)
                {
                    NowAttack = true;
                    InitAttackRange(AttackType.None);
                }
                if (Time_Count >= Boss_Ability.MoveSpeed + Boss_Ability.MoveSpeed)//bullit line time
                {
                    PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Snow);
                    Skill_Snow();
                    Boss_Attack();
                    ReLoadBossAttacks(-0.5f);
                }
                break;
            default:
                break;
        }
    }

    void Skill_Snow()
    {
        for (int i = 0; i < BlockMgr.Blocks[0].Length; i++)
        {
            Instantiate(Snow).transform.position = BlockMgr.Blocks[0][i].transform.position;
        }
        Player_Ctrl.PlayerBuff.GiveBuff(BuffMgr.BuffType.Slow, 2, 0.3f);
    }

    void Skill_Ice_Blast()
    {
        TempEffect = Instantiate(Ice_Blast);
        TempEffect.transform.position = Blast_Ofset;
        TempEffect.transform.eulerAngles = Blast_rot_Ofset;
        Blast_rot_Ofset = Vector3.zero;
    }

    void Skill_Ice_Lance()
    {
        for (int i = 0; i < Attack_Idx[AttackStack].Length; i++)
        {
            TempEffect = Instantiate(Ice_Lance);
            TempEffect.transform.position = BlockMgr.Blocks[(int)Attack_Idx[AttackStack][i].y][(int)Attack_Idx[AttackStack][i].x].transform.position;
            TempEffect.transform.position += Lance_Ofset;
        }
    }

    void Skill_Ice_Thom()
    {
        for (int i = 0; i < Attack_Idx[AttackStack].Length; i++)
        {
            TempEffect = Instantiate(Ice_Thom);
            TempEffect.transform.position = BlockMgr.Blocks[(int)Attack_Idx[AttackStack][i].y][(int)Attack_Idx[AttackStack][i].x].transform.position;
            TempEffect.transform.position -= new Vector3(0, 0, 1);
        }
    }
    void InitAttackRange(AttackType at_Type, int count = 1, int MinX = 0, int MinY = 0, int MaxX = 0, int MaxY = 0)
    {
        TempNum = Random.Range(0, 2);

        switch (at_Type)
        {
            case AttackType.None:
                Attack_Idx[AttackStack] = new Vector2[0];
                break;
            case AttackType.OneTile:
                Attack_Idx[AttackStack] = new Vector2[count];
                for (int i = 0; i < Attack_Idx[AttackStack].Length; i++)
                {  
                    Attack_Idx[AttackStack][i].x = Random.Range(0 + MinX, BlockMgr.LineX - MaxX);
                    Attack_Idx[AttackStack][i].y = Random.Range(0 + MinY, BlockMgr.LineY - MaxY);
                    for (int c = 0; c < Attack_Idx[AttackStack].Length; c++)
                        if (i < Attack_Idx[AttackStack].Length && i >= 0)
                            if (Attack_Idx[AttackStack][i].x == Attack_Idx[AttackStack][c].x && Attack_Idx[AttackStack][i].y == Attack_Idx[AttackStack][c].y && i != c)
                                i -= 1;
                }
                break;
            case AttackType.Line:
                if (TempNum == 0) // x 라인
                {
                    Attack_Idx[AttackStack] = new Vector2[BlockMgr.LineX];
                    TempNum = Random.Range(0, BlockMgr.LineY);
                    for (int i = 0; i < Attack_Idx[AttackStack].Length; i++)
                        Attack_Idx[AttackStack][i] = new Vector2(i, TempNum);

                    Blast_Ofset = new Vector3((BlockMgr.Blocks[(int)Attack_Idx[AttackStack][0].y][(int)Attack_Idx[AttackStack][0].x].transform.position.x 
                        + BlockMgr.Blocks[(int)Attack_Idx[AttackStack][Attack_Idx[AttackStack].Length - 1].y][(int)Attack_Idx[AttackStack][Attack_Idx[AttackStack].Length - 1].x].transform.position.x / 2)
                        ,BlockMgr.Blocks[(int)Attack_Idx[AttackStack][0].y][(int)Attack_Idx[AttackStack][0].x].transform.position.y, -1);

                    TempNum = Random.Range(0, 2);
                    if(TempNum == 1)
                        Blast_rot_Ofset = new Vector3(0, 0, 180);
                }
                else if (TempNum == 1) // y 라인
                {
                    Attack_Idx[AttackStack] = new Vector2[BlockMgr.LineY];
                    TempNum = Random.Range(0, BlockMgr.LineX);
                    for(int i = 0; i < Attack_Idx[AttackStack].Length; i++)
                        Attack_Idx[AttackStack][i] = new Vector2(TempNum, i);
                    Blast_Ofset = new Vector3(BlockMgr.Blocks[(int)Attack_Idx[AttackStack][0].y][(int)Attack_Idx[AttackStack][0].x].transform.position.x
                        ,(BlockMgr.Blocks[(int)Attack_Idx[AttackStack][0].y][(int)Attack_Idx[AttackStack][0].x].transform.position.y
                        + BlockMgr.Blocks[(int)Attack_Idx[AttackStack][Attack_Idx[AttackStack].Length - 1].y][(int)Attack_Idx[AttackStack][Attack_Idx[AttackStack].Length - 1].x].transform.position.y) / 2, -1);
                    TempNum = Random.Range(0, 2);
                    if (TempNum == 1)
                        Blast_rot_Ofset = new Vector3(0, 0, 90);
                    else
                        Blast_rot_Ofset = new Vector3(0, 0, -90);

                }
                break;
            default:
                break;
        }
    }

    void InitBulletLine()
    {
        for (int i = 0; i < Attack_Idx[AttackStack].Length; i++)
        {
            Instantiate(BulletLine).transform.position = BlockMgr.Blocks[(int)Attack_Idx[AttackStack][i].y][(int)Attack_Idx[AttackStack][i].x].transform.position + new Vector3(0, 0, -1);
        }
    }

    void OneTileInitBulletLine()
    {
        Instantiate(BulletLine).transform.position = BlockMgr.Blocks[(int)Stun_Ice_Lance_Pos.y][(int)Stun_Ice_Lance_Pos.x].transform.position + new Vector3(0, 0, -1);
    }

    public static void Boss_Attack()
    {
        if (isDamaged)
        {
            for (int i = 0; i < Attack_Idx[AtIdx].Length; i++)
            {
                if (Attack_Idx[AtIdx][i].x == Player_Ctrl.Player_MoveCtrl.PosIdx.x && Attack_Idx[AtIdx][i].y == Player_Ctrl.Player_MoveCtrl.PosIdx.y)
                {
                    Player_Ctrl.Player_Ability.Damaged(Boss_Ability.STR);
                    Player_Ctrl.RedCtrl = true;
                }
            }
            isDamaged = false;
            AtIdx += 1;
        if (AtIdx == Attack_Idx.Length)
            AtIdx = 0;
        }
  
    }

    public static void Stun_Ice_Lance_Attack()
    {
        if (Stun_Ice_Lance_Pos.x == Player_Ctrl.Player_MoveCtrl.PosIdx.x && Stun_Ice_Lance_Pos.y == Player_Ctrl.Player_MoveCtrl.PosIdx.y)
        {
            Player_Ctrl.Delete_Stun();
            Player_Ctrl.Player_Ability.Damaged(Boss_Ability.STR);
            Player_Ctrl.RedCtrl = true;
        }
    }
}
