using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageCamCtrl : MonoBehaviour
{
    public enum CamPosition
    {
        None,
        SetTile,
    }

    public static bool FirstSetTile = true;
    static CamPosition CamPos = CamPosition.SetTile;

    Vector3 SpeedVel;
    public Vector3 TempCamPos;

    public float CamMoveSpeed = 0.3f;

    public static void ResetAll()
    {
        FirstSetTile = true;
    }

    void Awake()
    {
        TempCamPos = transform.position;
    }

    void Start()
    {
        
    }

    void Update()
    {
        MoveCamera();
    }

    public static CamPosition GetCamPosition()
    {
        return CamPos;
    }

    public static void SetCamPosition(CamPosition Value)
    {
        CamPos = Value;
    }

    void MoveCamera()
    {
        if (CamPos == CamPosition.SetTile)
        {
            transform.position = Vector3.SmoothDamp(transform.position, transform.position - new Vector3(0, transform.position.y, 0), ref SpeedVel, CamMoveSpeed);
            if (Vector3.Distance(transform.position, transform.position - new Vector3(0, transform.position.y, 0)) <= 0.01f)
                transform.position = transform.position - new Vector3(0, transform.position.y, 0);
        }
        else if(CamPos == CamPosition.None)
        {
            transform.position = Vector3.SmoothDamp(transform.position, TempCamPos, ref SpeedVel, CamMoveSpeed);
            if (Vector3.Distance(transform.position, TempCamPos) <= 0.01f)
            {
                transform.position = TempCamPos;
            }
        }
    }

}
