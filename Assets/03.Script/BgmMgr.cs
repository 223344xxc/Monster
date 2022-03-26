using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmMgr : MonoBehaviour
{
    public enum BgmType
    {
        BossStage,
        PuzzleStage,
    }

    public static AudioClip[] Copy_Bgm;
    public AudioClip[] Bgm;
    public static AudioSource Audio;
    public static float Volum;
    static bool trig = false;
    static BgmType PlayingBgm;

    void Awake()
    {
        Audio = GetComponent<AudioSource>();
        Copy_Bgm_Clip();
    }

    void Start()
    {
    }
    
    void Update()
    {
        ChackBgm();
    }

    void ChackBgm()
    {
        if (!Audio.isPlaying && trig && !PlayerCtrl.IsBossDie)
            Start_Bgm(PlayingBgm);
    }

    void Copy_Bgm_Clip()
    {
        Copy_Bgm = new AudioClip[Bgm.Length];
        for (int i = 0; i < Bgm.Length; i++)
        {
            Copy_Bgm[i] = Bgm[i];
        }
    }

    public static void Stop_Bmg()
    {
        Audio.Stop();
    }

    public static void Start_Bgm(BgmType Bgm)
    {
        trig = true;
        switch (Bgm)
        {
            case BgmType.BossStage:
                Volum = 0.1f;
                break;
            case BgmType.PuzzleStage:
                Volum = 0.3f;
                break;
            default:
                break;
        }
        Audio.PlayOneShot(Copy_Bgm[(int)Bgm] ,Volum);
        PlayingBgm = Bgm;
    }
}
