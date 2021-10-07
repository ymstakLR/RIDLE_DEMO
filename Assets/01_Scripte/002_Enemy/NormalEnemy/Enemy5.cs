using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵5の移動・攻撃処理
/// 更新日時:20211007
/// </summary>
public class Enemy5 : EnemyTypeA {
    private GameObject _genarateObject;
    private EnemyUnderDecisionTrigger _underDecisionTrigger;//underDecisionObjectオブジェクトのスクリプト取得

    protected float _workTime;

    public new void Start() {
        base.Start();
        _enemyScore = 200;
        _genarateObject = (GameObject)Resources.Load("GameObject/Enemy5Ball");
        _underDecisionTrigger = this.transform.Find("UnderDecision").GetComponent<EnemyUnderDecisionTrigger>();
        EnemySpeed = 6;
        if (this.GetComponent<Transform>().localScale.x < 0)
            EnemySpeed = -EnemySpeed;
        EnemyMissFoll = -10;
        _workTime = Random.Range(0f, 2.5f);
    }//Start

    private void FixedUpdate() {
        if (!AniMiss) {
            Work();
        }//if
    }//FixedUpdate

    // Update is called once per frame
    void Update() {
        TypeAUpdate();
    }//Update

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Work() {
        RB2D.AddForce(new Vector2(0, -10));
        _workTime += Time.deltaTime;
        if (_workTime < 2.5) {
            EnemySpeed = EnemyWork.LandWork(
                workType: "Left_Right",
                enemySpeed: EnemySpeed,
                rb2d: RB2D,
                transform: EnemyTransform,
                enemySideDecision: SideDecisionScript,
                enemyUnderDecisionTrigger: _underDecisionTrigger);
        } else if (_workTime < 3.5) {
            RB2D.velocity = Vector3.zero;
            EnemyAnimator.SetBool("AniWait", true);
        } else if (_workTime < 4.5) {
            Attack();
            EnemyAnimator.SetBool("AniAttack", true);
        } else if (_workTime < 5.5) {
            EnemyAnimator.SetBool("AniAttack", false);
        } else {
            EnemyAnimator.SetBool("AniWait", false);
            _workTime = 0;
        }//if
    }//Work

    /// <summary>
    /// 攻撃処理
    /// </summary>
    public void Attack() {
        if (EnemyAnimator.GetBool("AniAttack"))
            return;
        float generatePositionX;
        int generateScaleX;

        if (this.transform.localScale.x < 0) {
            generatePositionX = this.transform.position.x - 2;
            generateScaleX = 1;//この値をgenerateObjectのScaleXに乗算する
        } else {
            generatePositionX = this.transform.position.x + 2;
            generateScaleX = -1;
        }//if
        AudioManager.PlaySE("Enemy_Fire");
        GameObject instanse = (GameObject)Instantiate(
            _genarateObject, new Vector2(generatePositionX, this.transform.position.y + (float)0.5), Quaternion.identity);
        instanse.transform.localScale = new Vector2(
            generateScaleX * instanse.transform.localScale.x, instanse.transform.localScale.y);
        instanse.transform.localRotation = Quaternion.Euler(0, 0, this.transform.localEulerAngles.z);
    }//Attack

}//Enemy5
