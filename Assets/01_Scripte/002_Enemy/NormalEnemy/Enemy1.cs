using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵1の移動処理
/// 更新日時:0603
/// </summary>
public class Enemy1 : EnemyTypeA {

    private GameObject _underDecisionObject;//enemyの子オブジェクトUnderDecisionを取得
    private EnemyUnderDecisionTrigger _underDecisionTrigger;//underDecisionObjectオブジェクトのスクリプト取得

    public new void Start() {
        base.Start();
        _enemyScore = 100;
        _underDecisionObject = this.transform.Find("UnderDecision").gameObject;
        _underDecisionTrigger = _underDecisionObject.GetComponent<EnemyUnderDecisionTrigger>();
        EnemyMissFoll = -10;//ミス時の落下速度を反映
        EnemySpeed = 6;//スピード値を反映
        if (this.GetComponent<Transform>().localScale.x < 0) {
            EnemySpeed = -EnemySpeed;
        }//if
    }//Start

    private void FixedUpdate() {
        if (!AniMiss) {
            Work();
        }//if
    }//FixedUpdate

    void Update() {
        TypeAUpdate();
    }//Update

    /// <summary>
    /// 移動処理　同種でも使用する
    /// </summary>
    public void Work() {
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -15));
        EnemySpeed = EnemyWork.LandWork(
                workType: "Left_Right",
                enemySpeed: EnemySpeed,
                rb2d: RB2D,
                transform: EnemyTransform,
                enemySideDecision: SideDecisionScript,
                enemyUnderDecisionTrigger: _underDecisionTrigger);
    }//Work

}//Enemy1
