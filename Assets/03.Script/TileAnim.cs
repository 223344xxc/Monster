using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnim : MonoBehaviour
{
    public Sprite []Anim;
    public int Event = -1;
    public float Delay = 0.5f;
    public float TimeCount = 0;
    public int AnimCount = 0;
    public bool Event_Trigget = true;
    public SpriteRenderer Renderer;
    public bool StartAnim = true;
    public bool IsLoop = true;
    void Awake()
    {   
        Renderer = gameObject.GetComponent<SpriteRenderer>(); 
    }

    void Start()
    {
    }
    void Update()
    {
        if (StartAnim)
            LoopAnim();
    }

    void LoopAnim()
    {
        TimeCount += Time.deltaTime;
        if(Delay <= TimeCount)
        {
            TimeCount = 0;
            if(Anim.Length - 1 > AnimCount)
            {
                AnimCount += 1;
            }
            else
            {
                AnimCount = 0;
            }
            Renderer.sprite = Anim[AnimCount];
        }
        if(IsLoop == false)
        {
            if (Anim.Length - 1 <= AnimCount)
                Destroy(gameObject);
        }
    }
}
