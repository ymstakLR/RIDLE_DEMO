using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵の横から触れた場合の判定処理
/// 更新日時:0602
/// </summary>
public class EnemySideDecision : MonoBehaviour {
    public bool SideDecisionCol { get; set; }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "Enemy" ||
            col.gameObject.tag == "StageEdge" || col.gameObject.tag == "Spring") {//ステージタグに触れた場合
            SideDecisionCol = true;
        }//if
    }//OnCollisionStay2D

    private void OnTriggerStay2D(Collider2D col) {//EnterからStayに変更した(0523)
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "Enemy" ||
            col.gameObject.tag == "StageEdge" || col.gameObject.tag == "Spring") {//ステージタグに触れた場合
            SideDecisionCol = true;
        }//if
    }//OnTriggerStay2D

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "Enemy" ||
            col.gameObject.tag == "StageEdge" || col.gameObject.tag == "Spring") {//ステージタグに触れた場合
            SideDecisionCol = false;
        }//if
    }//OnTriggerEnter2D

}//EnemySideDecisionCOl
