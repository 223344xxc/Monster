using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject Weaphon;
    public GameObject[] Hart;
    public GameObject Red;
    public GameObject GroundRed;
    public GameObject Player_Stun;

    public SpriteRenderer Player_Stun_Renderer;

    public Sprite[] HartImage;
    public Sprite Stun_Up;
    public Sprite Stun_Down;
    public Sprite Stun_Side;

    public Image[] HartBarSprites;

    public Vector3 MouseDown;
    public Vector3 MouseUp;
    public Vector3 MoveDistance;

    public static bool MouseReset = false;
    public static bool IsBossDie = false;
    public bool TempMouseReset = false;
    public bool DoteDamage;
    public bool SelectButton = false;
    public bool RedCtrl = false;
    public bool IsStun = false;

    public float DoteDamageCount = 0f;
    public float RedDelTime = 0.2f;
    float RedTime;
    public BuffMgr PlayerBuff;


    int num;

    public Ability Player_Ability;
    public Animator Player_Animator;
    public ObjectMoveCtrl Player_MoveCtrl;

    public static void ResetAll()
    {
        MouseReset = false;
        IsBossDie = false;
    }

    void Awake()
    {
        Player_Animator = GetComponent<Animator>();
        Player_Ability = GetComponent<Ability>();
        Player_MoveCtrl = GetComponent<ObjectMoveCtrl>();
        Player_Stun = GameObject.Find("PlayerStun");
        Player_Stun_Renderer = Player_Stun.GetComponent<SpriteRenderer>();
        PlayerBuff = GetComponent<BuffMgr>();
        DoteDamage = false;
        Red = GameObject.Find("PlayerRed");
        GroundRed = GameObject.Find("GroundRed");
        InitUI();
    }

    void Start()
    {
        Player_MoveCtrl.TpObject(1, 1);
        Player_Stun.SetActive(false);
    }

    void Update()
    {
        if (IsBossDie)
        {
            return;
        }
        if(Player_Ability.GetHp() <= 0)
        {
            BossStageCamCtrl.SetCamPosition(BossStageCamCtrl.CamPosition.SetTile);
            BossStageCamCtrl.FirstSetTile = true;
            BossCtrl.ResetAll();
            BossStageCamCtrl.ResetAll();
            CamRayMgr.ResetAll();
            ResetAll();
            PlayerPrefs.SetString("Scene", "Boss");
            SceneManager.LoadScene("Loading");
        }
        TempMouseReset = MouseReset;
        Red_Ctrl();
        Attack();
        if (BossStageCamCtrl.GetCamPosition() == BossStageCamCtrl.CamPosition.SetTile || SelectButton)
        {
            ResetMousePos();
            return;
        }
        if (Player_MoveCtrl.isMove && !IsStun)
        {
            Move();
        }
        if (!Player_MoveCtrl.isMove || Player_MoveCtrl.NowWalk)
        {
            Player_MoveCtrl.Walk();
        }

        Player_MoveCtrl.ChackPos();

        if (DoteDamage)
            ChackDoteDamage();
        UpdateUI();
    }

    public void Set_Stun()
    {
        IsStun = true;
        Player_Stun.SetActive(true);
        Player_MoveCtrl.StopWalk();
        Player_Animator.speed = 0;
        switch(Player_Animator.GetInteger("WalkAnim"))
        {
            case 0:
                Player_Stun_Renderer.sprite = Stun_Down;
                break;
            case 1:
                Player_Stun_Renderer.sprite = Stun_Side;
                break;
            case 2:
                Player_Stun_Renderer.sprite = Stun_Side;
                break;
            case 3:
                Player_Stun_Renderer.sprite = Stun_Up;
                break;
            case 4:
                Player_Stun_Renderer.sprite = Stun_Down;
                break;
            default:
                break;
        }
    }

    public void Delete_Stun()
    {
        if (IsStun)
        {
            IsStun = false;
            Player_Stun.SetActive(false);
            Player_MoveCtrl.StartWalk(BlockMgr.Blocks[(int)Player_MoveCtrl.PosIdx.y][(int)Player_MoveCtrl.PosIdx.x].transform.position);
            Player_Animator.speed = 1;
        }
    }

    void ResetMousePos()
    {
        MouseDown = Vector3.zero;
        MouseUp = Vector3.zero;
        MoveDistance = Vector3.zero;
    }

    void Red_Ctrl()
    {
        if (RedCtrl)
        {
            GroundRed.SetActive(true);
            Red.SetActive(true);
            RedTime += Time.deltaTime;
            if (RedTime >= RedDelTime)
            {
                RedCtrl = false;
                RedTime = 0;
            }
        }
        else
        {
            GroundRed.SetActive(false);
            Red.SetActive(false);
        }
    }
    void InitUI()
    {
        HartBarSprites = new Image[Hart.Length];
        for(int i = 0; i < HartBarSprites.Length; i++)
        {
            HartBarSprites[i] = Hart[i].GetComponent<Image>();
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < Hart.Length; i++)
        {
            HartBarSprites[i].sprite = HartImage[0];
        }
        if (Player_Ability.GetHp() >= Player_Ability.MaxHp)
        {
            Player_Ability.SetHp(Player_Ability.MaxHp);
            for (int i = 0; i < Hart.Length; i++)
                Hart[i].SetActive(true);
        }
        else if (Player_Ability.GetHp() <= 0)
        {
            Player_Ability.SetHp(0);
            for (int i = 0; i < Hart.Length; i++)
                Hart[i].SetActive(false);
        }
        if (Player_Ability.GetHp() % 2 != 0)
        {
            num = 1;
            HartBarSprites[(int)Player_Ability.GetHp() / 2].sprite = HartImage[1];
        }
        else if (Player_Ability.GetHp() % 2 == 0)
        {
            num = 0;
            HartBarSprites[((int)Player_Ability.GetHp() / 2) - 1].sprite = HartImage[0];
        }
        for (int i = 0; i < Hart.Length; i++)
            Hart[i].SetActive(false);
        for(int i = 0; i < num + (int)(Player_Ability.GetHp()/2);i++)
        {
            Hart[i].SetActive(true);
        }
    }

    void Attack()
    {
        if (Player_MoveCtrl.isAttack)
        {
            Player_MoveCtrl.isAttack = false;
            Instantiate(Weaphon).transform.position = transform.position;
        }
    }

    void ChackDoteDamage()
    {
        if (Vector3.Distance(Player_MoveCtrl.DoteTile, transform.position) >= 1.5)
        {
            DoteDamage = false;
            return;
        }
        DoteDamageCount += Time.deltaTime;
        if(DoteDamageCount >= 1f && Player_Ability.GetHp() > 0)
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
        if (Input.GetMouseButtonUp(0) && !MouseReset)
        {
            MouseUp = Input.mousePosition;
            MoveDistance = MouseUp - MouseDown;

            if (MoveDistance.magnitude <= 100)
                return;

            Player_MoveCtrl.MoveCtrl(Player_MoveCtrl.SelectDistance(MoveDistance));
        }
        else if (Input.GetMouseButtonUp(0) && MouseReset)
            MouseReset = false;
    }

}
