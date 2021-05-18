using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Aタイプの敵共通の処理
/// 更新日時:0413
/// </summary>
public class EnemyTypeA : EnemyParent {
    public GameObject SideDecisionObject { get; set; }//enemyの子オブジェクトSideDecisionを取得
    public EnemySideDecision SideDecisionScript { get; set; }//sideDecisionObjectオブジェクトのスクリプト取得
    public EnemyWork EnemyWork { get; set; }//EnemyWorkスクリプト

    private EnemyBodyTrigger _eBodyTrigger;
    protected EnemyUnderTrigger _eUnderTrigger;

    private float _pastPosY;

    private bool _isMissEnd;//ミス処理が終了したときの判定

    public new void Start() {
        base.Start();
        SideDecisionObject = this.transform.Find("SideDecision").gameObject;
        SideDecisionScript = SideDecisionObject.GetComponent<EnemySideDecision>();
        EnemyWork = this.gameObject.GetComponent<EnemyWork>();
        EnemyWork.WorkStart(EnemyTransform.localScale.x, RB2D);
        _eBodyTrigger = this.transform.Find("BodyTrigger").GetComponent<EnemyBodyTrigger>();
        _eUnderTrigger = this.transform.Find("UnderTrigger").GetComponent<EnemyUnderTrigger>();
    }//Start

    /// <summary>
    /// 各種敵共通のUpdate処理
    /// </summary>
    protected void TypeAUpdate() {
        if (!AniMiss) {
            ParentUpdate();
            UnderTriggerCheack();
        } else {
            EnemyErasureMiss();
            SideDecisionObject.GetComponent<Collider2D>().enabled = false;
            _eUnderTrigger.IsRise = false;
            _eUnderTrigger.IsGimmickJump = false;//現状trueになるときはないが、レイヤー順番変更の処理を理解しやすくするために記述している(0502)
        }//if
    }//TypeAUpdate

    /// <summary>
    /// 敵の非表示化
    /// </summary>
    private void EnemyErasureMiss() {
        if (_stageClearManagement.StageStatus != EnumStageStatus.Normal) {
            this.GetComponent<Renderer>().enabled = false;
        } else {
            this.GetComponent<Renderer>().enabled = true;
        }//if
    }//EnemyErasureMiss

    /// <summary>
    /// 敵のUnderTriggerの判定を更新する処理
    /// </summary>
    private void UnderTriggerCheack() {
        if (_pastPosY > this.transform.position.y) {
            _eUnderTrigger.IsRise = false;
        } else {
            _eUnderTrigger.IsRise = true;
        }//if
        _pastPosY = this.transform.position.y;
    }//UnderTriggerCheack

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Stage" ||
           col.gameObject.tag == "PlatformEffector" ||
           col.gameObject.tag == "Gimmick") {
            if (AniMiss && !_isMissEnd) {
                RB2D.velocity = new Vector2(0, 0);
                _isMissEnd = true;
                Debug.Log("EnemyName_"+this.gameObject.name+"_MissEndObject_" + col.gameObject.name);
            }//if
        }//if
    }//OnCollisionEnter2D

}//EnemyTypeA
