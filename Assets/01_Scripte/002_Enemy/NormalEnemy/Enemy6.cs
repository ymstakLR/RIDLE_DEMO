using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵6の移動・攻撃処理
/// 更新日時:0602
/// </summary>
public class Enemy6 : Enemy5 {

    new void Start() {
        base.Start();
    }//Start

    private void FixedUpdate() {
        if (!AniMiss) { 
            base.Work();
            DirectionChange();
        }//if
    }//FixedUpdate

    void Update() {
        base.TypeAUpdate();
    }//Update

    /// <summary>
    /// 向きを変更する処理
    /// </summary>
    private void DirectionChange() {
        if (_workTime != 0)
            return;
        float playerPositionX = _playerObject.GetComponent<Transform>().transform.position.x;
        if ((this.transform.position.x > playerPositionX & EnemySpeed > 0) ||
            (this.transform.position.x < playerPositionX & EnemySpeed < 0)) {
            EnemySpeed = -EnemySpeed;
            EnemyWork.WorkStart(-this.transform.localScale.x, RB2D);
        }//if
    }//DirretiooChange
}//Enemy6
