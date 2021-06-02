using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵下側が触れた場合の判定処理(地面接触用)
/// 更新日時:0602
/// </summary>
public class EnemyUnderTrigger : BaseUnderTrigger {

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Stage"||
            col.gameObject.tag == "PlatformEffector"||
            col.gameObject.tag == "Gimmick") {
            IsUnderTrigger = true;
        }//if
    }//OnTriggerEnter2D

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Stage" || 
            col.gameObject.tag == "PlatformEffector"||
            col.gameObject.tag == "Gimmick") {
            IsUnderTrigger = false;
        }//if

    }//OnTriggerExit2D

}//EnemyUnderTrigger
