using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵の攻撃判定用の処理
/// 更新日時:0408
/// </summary>
public class EnemyBodyTrigger : MonoBehaviour {
    public bool IsEnemyDamage { get; set; }
    public bool IsStageTouch { get; set; }//現在,BossEnemy1でしか使用していない。今後の使用状況次第ではほかのクラスに役割を移動させる(0512)

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "PlayerAttack" && this.gameObject.layer == LayerMask.NameToLayer("EnemyAttack")) {
            IsEnemyDamage = true;
        }//if
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "StageEdge") {
            IsStageTouch = true;
        }//if
    }//OnTriggerEnter2D

}//EnemyBodyTrigger
