using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵10の移動処理
/// 更新日時:20210910
/// </summary>
public class Enemy10 : EnemyTypeA {

    private EnemyUnderDecisionTrigger _underDecisionTrigger;
    private EnemyLandingCheck _landingCheck;

    private readonly int JUMP_POWER_MAX = 250;//ジャンプ量の最大値
    private readonly int JUMP_TIME = 3;//ジャンプ処理に遷移するためのタイマー

    private float _jumpPower;//現在のジャンプ量
    private float _jumpTimer;//現在のジャンプタイマー
    private float _pastTPY;//記憶用変数

    private bool _isFall;//落下判定

    private new void Start() {
        base.Start();
        _enemyScore = 200;
        _underDecisionTrigger = this.transform.Find("UnderDecision").GetComponent<EnemyUnderDecisionTrigger>();
        _landingCheck = this.transform.Find("LandCheck").GetComponent<EnemyLandingCheck>();
        _jumpTimer = Random.Range(0f, JUMP_TIME);
        EnemyMissFoll = -10;//ミス時の落下速度を反映
        EnemySpeed = 4;
        if (this.GetComponent<Transform>().localScale.x < 0) {
            EnemySpeed = -EnemySpeed;
        }//if
    }//Start

    private void FixedUpdate() {
        if (!AniMiss) {
            MoveType();
        }//if
    }//FixedUpdate

    void Update() {
        TypeAUpdate();
    }//UPdate

    /// <summary>
    /// 移動処理判定
    /// 左右の地面移動とジャンプを切り替える
    /// </summary>
    private void MoveType() {
        if (_jumpTimer < JUMP_TIME) {
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -15));
            EnemySpeed = EnemyWork.LandWork(
                workType: "Left_Right",
                enemySpeed: EnemySpeed,
                rb2d: RB2D,
                transform: EnemyTransform,
                enemySideDecision: SideDecisionScript,
                enemyUnderDecisionTrigger: _underDecisionTrigger);
            ColliderChange(new Vector2(0.55f, -1.3f), new Vector2(3.8f, 3), CapsuleDirection2D.Horizontal);
        } else { 
            (_jumpPower, _jumpTimer, _isFall) = EnemyWork.Jump(
                EnemyAnimator, _eUnderTrigger, _landingCheck, RB2D, transform,
                EnemySpeed, _jumpPower, _pastTPY, _jumpTimer, JUMP_POWER_MAX, _isFall);
            ColliderChange(new Vector2(0.55f, -0.5f), new Vector2(3.8f, 4.73f), CapsuleDirection2D.Vertical);
        }//if
        _pastTPY = this.transform.position.y;
        _jumpTimer += Time.deltaTime;
    }//MoveType

}//Enemy10
