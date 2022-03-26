using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTileSeting : MonoBehaviour
{
    public GameObject YesButton;
    public GameObject NoButton;
    public GameObject BackUI;
    public GameObject UIBackGround;
    public BossStageCamCtrl Cam;

    void Awake()
    {
        Cam = GameObject.Find("Main Camera").GetComponent<BossStageCamCtrl>();
        SetButtons(true);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnButtonDown()
    {
        PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Normal_Click);
        SetButtons(false);
    }

    public void OnYesButtonDown()
    {
        PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Normal_Click);
        SetButtons(true);
        DelObject();
        BossStageCamCtrl.SetCamPosition(BossStageCamCtrl.CamPosition.None);
        BossStageCamCtrl.FirstSetTile = false;
        PlayerCtrl.MouseReset = true;
    }
    public void OnNoButtonDown()
    {
        PlayerSoundMgr.Play_AudioClip((int)PlayerSoundMgr.SoundClip.Normal_Click);
        SetButtons(true);
    }

    void DelObject()
    {
        Destroy(YesButton);
        Destroy(NoButton);
        Destroy(BackUI);
        Destroy(gameObject);
    }

    void SetButtons(bool ButtonPos)
    {
        if(ButtonPos)
        {
            YesButton.transform.position += new Vector3(0, 10000, 0);
            NoButton.transform.position += new Vector3(0, 10000, 0);
            BackUI.transform.position += new Vector3(0, 10000, 0);
            UIBackGround.transform.position = new Vector3(UIBackGround.transform.position.x, 10000, UIBackGround.transform.position.z);
        }
        else
        {
            YesButton.transform.position -= new Vector3(0, 10000, 0);
            NoButton.transform.position -= new Vector3(0, 10000, 0);
            BackUI.transform.position -= new Vector3(0, 10000, 0);
            UIBackGround.transform.position = new Vector3(UIBackGround.transform.position.x, 0, UIBackGround.transform.position.z);
        }
    }


}
