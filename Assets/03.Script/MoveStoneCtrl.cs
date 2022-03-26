using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStoneCtrl : MonoBehaviour
{
    public ObjectMoveCtrl MoveStone_MoveCtrl;
    public Vector2 TempIdx = Vector2.zero;
    void Awake()
    {
        MoveStone_MoveCtrl = GetComponent<ObjectMoveCtrl>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (!MoveStone_MoveCtrl.isMove || MoveStone_MoveCtrl.NowWalk)
            MoveStone_MoveCtrl.Walk();
    }

    public void MoveObject_Move(ObjectMoveCtrl.Distance dis)
    {
        MoveStone_MoveCtrl.MoveCtrl(dis);
        BlockMgr.Blocks_Com[(int)MoveStone_MoveCtrl.PosIdx.y][(int)MoveStone_MoveCtrl.PosIdx.x].MoveObject = true;
        BlockMgr.Blocks_Com[(int)TempIdx.y][(int)TempIdx.x].MoveObject = false;
        TempIdx = MoveStone_MoveCtrl.PosIdx;
    }
}
