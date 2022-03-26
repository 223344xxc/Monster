using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipAnimMgr : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public void Attack()
    {
        BossCtrl.Boss_Attack();
    }

    public void Stun_Lance_Attack()
    {
        BossCtrl.Stun_Ice_Lance_Attack();

    }

    public void Play_Sound(int AudioClipNum)
    {
        PlayerSoundMgr.Play_AudioClip(AudioClipNum);
    }
}
