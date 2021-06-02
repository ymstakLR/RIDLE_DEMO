using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵9の移動処理
/// 更新日時:0603
/// </summary>
public class Enemy9 : EnemyTypeA {
    private EnemySideDecision _sideDecision;
    private EnemyUnderDecisionTrigger _underDecisionTrigger;

    private float _distancePlayerX;
    private float _distancePlayerY;

    private new void Start() {
        base.Start();

        _sideDecision = SideDecisionObject.GetComponent<EnemySideDecision>();
        _underDecisionTrigger = this.transform.Find("UnderDecision").GetComponent<EnemyUnderDecisionTrigger>();
            
        EnemyMissFoll = -10;//ミス時の落下速度を反映
        EnemyAnimator.SetBool("AniFlight", false);
        EnemyAnimator.SetBool("AniDescent", false);
        EnemySpeed = 6;
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
    }//Update

    /// <summary>
    /// 移動方法を選択する処理
    /// </summary>
    private void MoveType() {
        _distancePlayerX =(this.transform.position.x - _playerObject.transform.position.x);
        _distancePlayerY =(this.transform.position.y - _playerObject.transform.position.y);
        //もし自機が範囲内にいなかったら
        if ((_distancePlayerX > 10 || _distancePlayerX <-10)||
            (_distancePlayerY >30 || _distancePlayerY <-30)) {
            WallMove();
        } else {
            SideDecisionScript.SideDecisionCol = false;
            SkyMove();
        }//if
    }//MoveType

    /// <summary>
    /// 地面移動時の処理
    /// </summary>
    private void WallMove() {
        LayerChange("Enemy");
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -10));
        if (_eUnderTrigger.IsUnderTrigger) {
            EnemyAnimator.SetBool("AniFlight", false);
            EnemyAnimator.SetBool("AniDescent", false);
            EnemySpeed = EnemyWork.LandWork(
                workType: "Left_Right",
                enemySpeed: EnemySpeed,
                rb2d: RB2D,
                transform: EnemyTransform,
                enemySideDecision: SideDecisionScript,
                enemyUnderDecisionTrigger: _underDecisionTrigger);
        } else {
            EnemyAnimator.SetBool("AniDescent", true);
            _eUnderTrigger.IsRise = false;
            _underDecisionTrigger.UnderDecisionTri = false;
        }//if
    }//WallMove

    /// <summary>
    /// 空中移動時の処理
    /// </summary>
    private void SkyMove() {
        LayerChange("FlyEnemy");
        EnemySpeed = EnemyWork.SkyDiagonalMove(
            EnemyAnimator, _eUnderTrigger, _playerObject, RB2D,
            this.transform, EnemySpeed, _distancePlayerX, _distancePlayerY);
    }//SkyMove

    /// <summary>
    /// レイヤー属性を変更する処理
    /// </summary>
    /// <param name="layerName"></param>
    private void LayerChange(string layerName) {
        this.gameObject.layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in transform) {
            if(child.gameObject.name != "BodyTrigger") {
                child.gameObject.layer = LayerMask.NameToLayer(layerName);
            }//if
        }//foreach
    }//LayerChange

}//Enemy9
