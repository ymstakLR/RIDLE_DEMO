using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵下側が触れた場合の判定処理(向き変更用)
/// 更新日時:0602
/// </summary>
public class EnemyUnderDecisionTrigger : MonoBehaviour {
    public bool UnderDecisionTri { get; set; }

    private void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "PlatformEffector") {
            UnderDecisionTri = false;
        }//if
    }//OnTriggerStay2D

    private void OnTriggerExit2D(Collider2D col) {
        if(col.gameObject.tag == "Stage" || col.gameObject.tag == "PlatformEffector") {
            UnderDecisionTri = true;
        }//if
    }//OnTriggerExit2D

}//EnemyUnderDecisionTrigger
