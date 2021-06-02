using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵3の処理
/// 更新日時:0602
/// </summary>
public class Enemy3 : EnemyTypeA {

    public new void Start() {
        base.Start();//EnemyParentのStartを行う
        _enemyScore = 100;
        EnemyMissFoll = -10;//ミス時の落下速度を反映
        EnemySpeed = 4;
        if (this.GetComponent<Transform>().localScale.x < 0) {
            EnemySpeed = -EnemySpeed;
        }//if
        EnemyWork.SkyTime = Random.Range(0, 0.5f);
    }//Start

    private void FixedUpdate() {
        if (!AniMiss) {
            Work();
        }//if
    }//FixedUpdate

    void Update() {//EnemyWorkの処理を行う
        TypeAUpdate();//Work()をこの下に記述するとミスした後に移動処理が継続して実行されてしまう
    }//Update

    /// <summary>
    /// 移動処理 同種でも使用する
    /// </summary>
    public void Work() {
        EnemySpeed = EnemyWork.SkyHorizontalMove(
            workType: "Left_Right",
            enemySpeed: EnemySpeed,
            wobblingTime: (float)0.5,
            wobblingSpeed: (float)0.1,
            rb2d: RB2D,
            animator: EnemyAnimator,
            transform: EnemyTransform,
            enemySideDecision: SideDecisionScript);
    }//Work

}//Enemy3
