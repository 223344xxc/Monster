using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundMgr : MonoBehaviour
{
    public enum SoundClip
    {
        Ice_Lance,
        Ice_Blast,
        Hit_Player,
        Attack_Player,
        Heal,
        Normal_Click,
        Ice_Thom,
        Power_Buff,
        Snow,
        Stun_Ice_Lance,
        Ice_Ball,
        BossDeath,
    }

    public static AudioSource Audio;
    public static AudioClip[] Sounds;
    public AudioClip[] Clips;
    static float Volum;

    void Awake()
    {
        Audio = GetComponent<AudioSource>();
        CopyClips();
    }

    void Start()
    {

    }
    void Update()
    {
        
    }

    void CopyClips()
    {
        Sounds = new AudioClip[Clips.Length];
        for (int i = 0; i < Clips.Length; i++)
        {
            Sounds[i] = Clips[i];
        }
    }

    public static void Play_AudioClip(int ClipNum)
    {
        switch((SoundClip)ClipNum)
        {
            case SoundClip.Ice_Lance:
                Volum = 0.05f;
                break;
            default:
                Volum = 0.25f;
                break;
            //case SoundClip.Ice_Blast:
            //    Volum = 0.5f;
            //    break;
            //case SoundClip.Hit_Player:
            //    Volum = 0.5f;
            //    break;
            //case SoundClip.Attack_Player:
            //    Volum = 0.5f;
            //    break;
            //case SoundClip.Heal:
            //    Volum = 0.5f;
            //    break;
            //case SoundClip.Normal_Click:
            //    Volum = 0.5f;
            //    break;
            //case SoundClip.Ice_Thom:
            //    Volum = 0.5f;
            //    break;
            //case SoundClip.Power_Buff:
            //    Volum = 0.5f;
            //    break;
            //case SoundClip.Snow:
            //    Volum = 0.5f;
            //    break;
            //case SoundClip.Stun_Ice_Lance:
            //    Volum = 0.5f;
            //    break;
            //case SoundClip.Ice_Ball:
            //    Volum = 0.5f;
            //    break;
        }
        Audio.PlayOneShot(Sounds[ClipNum], Volum);
    }
}
