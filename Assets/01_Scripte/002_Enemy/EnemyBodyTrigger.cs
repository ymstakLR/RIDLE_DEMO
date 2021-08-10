using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDamageType {
    None = 0,
    Trampling = 1,
    Slashing = 2
}

/// <summary>
/// 敵の攻撃判定用の処理
/// 更新日時:0602
/// </summary>
public class EnemyBodyTrigger : MonoBehaviour {
    public EnemyDamageType EnemyDamageType { get; set; }

    public bool IsStageTouch { get; set; }//現在,BossEnemy1でしか使用していない。

    private void OnTriggerEnter2D(Collider2D col) {
        if (this.gameObject.layer == LayerMask.NameToLayer("EnemyAttack")) {
            if (col.gameObject.tag == "PlayerTrampling") {
                EnemyDamageType = EnemyDamageType.Trampling;
            }
            if (col.gameObject.tag == "PlayerSlashing") {
                EnemyDamageType = EnemyDamageType.Slashing;
            }
        }//if
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "StageEdge") {
            IsStageTouch = true;
        }//if
    }//OnTriggerEnter2D

}//EnemyBodyTrigger
