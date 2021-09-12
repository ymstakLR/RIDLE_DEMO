using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Aタイプの敵共通の処理
/// 更新日時:20210911
/// </summary>
public class EnemyTypeA : EnemyParent {
    public GameObject SideDecisionObject { get; set; }//enemyの子オブジェクトSideDecisionを取得
    public EnemySideDecision SideDecisionScript { get; set; }//sideDecisionObjectオブジェクトのスクリプト取得
    public EnemyWork EnemyWork { get; set; }//EnemyWorkスクリプト

    private EnemyBodyTrigger _eBodyTrigger;
    protected EnemyUnderTrigger _eUnderTrigger;
    private EnumStageStatus _beforeStageStatus;

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
        EnemyRendererHide();
        if (!AniMiss) {
            ParentUpdate();
            UnderTriggerCheack();
        } else {
            EnemyMissBlinking();
            SideDecisionObject.GetComponent<Collider2D>().enabled = false;
            _eUnderTrigger.IsRise = false;
            _eUnderTrigger.IsGimmickJump = false;//現状trueになるときはないが、レイヤー順番変更の処理を理解しやすくするために記述している(0502)
        }//if
    }//TypeAUpdate

    /// <summary>
    /// 敵の非表示化
    /// </summary>
    private void EnemyRendererHide() {
        if (_stageClearManagement.StageStatus == _beforeStageStatus)
            return;
        if (_stageClearManagement.StageStatus == EnumStageStatus.Normal) {
            _renderer.enabled = true;
        } else {
            _renderer.enabled = false;
        }//if
        _beforeStageStatus = _stageClearManagement.StageStatus;
    }//EnemyRendererHide

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

    private void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.tag != "Stage" &&
           col.gameObject.tag != "PlatformEffector" &&
           col.gameObject.tag != "Gimmick")
            return;
        if (AniMiss && !_isMissEnd && _eUnderTrigger.IsUnderTrigger) {
            RB2D.velocity = new Vector2(0, 0);
            _isMissEnd = true;
        }//if
    }//OnCollisionStay2D

}//EnemyTypeA
