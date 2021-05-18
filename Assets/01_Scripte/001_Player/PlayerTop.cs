using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 頭部分の当たり判定取得用の処理
/// 更新日時:0405
/// </summary>
public class PlayerTop : MonoBehaviour {
    public bool IsStageStay { get; set; }

    private void OnCollisionStay2D(Collision2D col) {
        if(col.gameObject.tag == "Stage") {
            IsStageStay = true;
        }//if
    }//OnCollisionStay2D

    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Stage") {
            IsStageStay = false;
        }//if
    }//OnCollisionExit2D

}//PlayerTop
