using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵7の移動処理
/// 更新日時:0603
/// </summary>
public class Enemy7 : Enemy3 {

    private float _directionTime;

    new void Start() {
        base.Start();
        _directionTime = Random.Range(0f, 4f);
    }//Start

    private void FixedUpdate() {
        if (!AniMiss) {
            base.Work();
            DirectionChange();
        }//it
    }//FixedUpdate


    void Update() {
        base.TypeAUpdate();
    }//Update

    /// <summary>
    /// 向きを変更する処理
    /// </summary>
    private void DirectionChange() {
        _directionTime += Time.deltaTime;
        if (_directionTime < 4)
            return;
        float playerPositionX = _playerObject.GetComponent<Transform>().transform.position.x;
        if ((this.transform.position.x > playerPositionX & EnemySpeed > 0) ||
            (this.transform.position.x < playerPositionX & EnemySpeed < 0)) {
            EnemySpeed = -EnemySpeed;
            this.transform.localScale = new Vector2(-(this.transform.localScale.x), this.transform.localScale.y);
            EnemyWork.WorkStart(-this.transform.localScale.x, RB2D);
        }//if
        _directionTime = 0;
    }//DirectionChange
}//Enemy7
