using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景を時間経過で移動させる処理・背景をループさせるようにする必要あり
/// 更新日時:0414
/// </summary>
public class BackGroundTimeMove : MonoBehaviour {

    [SerializeField, Tooltip("移動する速さ")]
    private float moveSpeed;

    [SerializeField, Tooltip("ループする時の座標実軸")]
    private float loopPos;

    [SerializeField, Tooltip("ループした際の座標軸")]
    private float reternPos;

    [SerializeField, Tooltip("横移動する場合")]
    private bool isLateralmovement;


    void FixedUpdate() {
        TimeMove();
    }//FixedUpdate

    /// <summary>
    /// 時間経過で移動させる処理
    /// </summary>
    private void TimeMove() {
        Vector3 addPos = this.transform.localPosition;
        switch (isLateralmovement) {
            case true:
                addPos.x -= moveSpeed;
                if (this.transform.localPosition.x < loopPos) {
                    addPos.x = reternPos;
                }//if
                break;
            case false:
                addPos.y -= moveSpeed;
                if (this.transform.localPosition.y < loopPos) {
                    addPos.y = reternPos;
                }//if
                break;
        }//switch
        this.transform.localPosition = addPos;
    }//TimeMove

}//BackGroundTimeMove
