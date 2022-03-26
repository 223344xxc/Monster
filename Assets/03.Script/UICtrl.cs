using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemButtonDown()
    {
        PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Normal_Click);
        PlayerCtrl.MouseReset = true;
        BossCtrl.UseItem = true;
        BossCtrl.AttackBoss = false;
    }

    public void AttackButtonDown()
    {
        PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Normal_Click);
        PlayerCtrl.MouseReset = true;
        BossCtrl.AttackBoss = true;
        BossCtrl.UseItem = false;
    }
}
