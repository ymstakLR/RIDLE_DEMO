using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy4
/// 壁に張り付いてクレーンを伸ばす（棒のクレーンにアームがある）
/// 自機がアームに触れたら、自機を動けなくして捕獲アニメーションにする
/// クレーンをEnemy4のほうに縮ませる
/// 自機がEnemy4に触れたら自機にダメージ
/// 更新日時:20210906
/// </summary>
public class Enemy4 : EnemyParent {
    private GameObject _armObject;//アームオブジェクト
    private GameObject _craneObject;//クレーンオブジェクト

    private EnemyArm _enemyArm;
    private EnemyCrane _enemyCrane;
  
    private float _longestScaleSize;//クレーンの伸ばす長さの最大値 値は別スクリプトで個別に設定する

    private new void Start() {
        base.Start();//EnemyParentのStartを行う
        _armObject = this.gameObject.transform.Find("Enemy4_Arm").gameObject;
        _craneObject = this.gameObject.transform.Find("Enemy4_Crane").gameObject;
        _enemyArm = _armObject.GetComponent<EnemyArm>();
        _enemyCrane = _craneObject.GetComponent<EnemyCrane>();
        _longestScaleSize = this.transform.parent.GetComponent<ObjectAppearanceManager>().ObjectFloatList[0];
        EnemyMissFoll = 0;//ミス時の落下速度を反映
        EnemySpeed = 0;//スピード値を反映
    }//Start

    private void FixedUpdate() {
        _enemyCrane.Crane(_craneObject,_armObject,_longestScaleSize);
        _enemyCrane.Arm(_armObject,EnemyAnimator);
    }//FixedUpdate

    private void Update() {
        ParentUpdate();
        EnemyRendererHide();
        if (AniMiss) {
            this.GetComponent<EnemyParent>().enabled = false;
        }//if
    }//Update

    /// <summary>
    /// 敵の非表示化
    /// </summary>
    private void EnemyRendererHide() {//非表示化するRendererを探索できるような処理に変更する(20210906
        if (_stageClearManagement.StageStatus == EnumStageStatus.Normal) {
            this.GetComponent<Renderer>().enabled = true;
            _craneObject.GetComponent<Renderer>().enabled = true;
            _armObject.GetComponent<Renderer>().enabled = true;
        } else {
            this.GetComponent<Renderer>().enabled = false;
            _craneObject.GetComponent<Renderer>().enabled = false;
            _armObject.GetComponent<Renderer>().enabled = false;
        }//if
    }//EnemyRendererHide

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "PlayerAttack") {//自機の攻撃に触れた場合
            _armObject.GetComponent<Animator>().SetBool("AniWork", true);
            _armObject.GetComponent<CapsuleCollider2D>().enabled = false;
            _armObject.GetComponent<EnemyArm>().enabled = false;
        }//if
    }//OnTriggerEnter2D

    private void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.tag == "Stage" && AniMiss) {
            this.GetComponent<CapsuleCollider2D>().enabled = false;
        }//if
    }//OnTriggerStay2D

}//Enemy4
