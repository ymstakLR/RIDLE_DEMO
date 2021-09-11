using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機の体に接触したときの処理
/// 更新日時:20210911
/// </summary>
public class PlayerBodyTrigger : MonoBehaviour {
    private PlayerAnimator _pAnimator;
    private PlayerLife _pLife;
    private RotatingArrow _rotatingArrow;
    private SpriteRenderer _spriteRenderer;
    private StageStatusManagement _stageClearManagement;

    private readonly float RECOVERY_TIME = 2;//ミスから回復するまでの時間

    private float _recoveryTimer;//ミス判定中の時間
    private float _rendererEnableTime;

    private bool _isDamage;

    void Start() {
        _pAnimator = this.transform.parent.GetComponent<PlayerAnimator>();
        _pLife = this.transform.parent.GetComponent<PlayerLife>();
        _rotatingArrow = GameObject.Find("UI/UIItemReference/ArrowToGoal").GetComponent<RotatingArrow>();
        _stageClearManagement = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
        _spriteRenderer = this.transform.parent.GetComponent<SpriteRenderer>();
    }//Start

    void Update() {
        Damage();
    }//Update

    private void Damage() {
        if (!_isDamage || _pAnimator.AniMiss)
            return;
        _recoveryTimer += Time.deltaTime;
        //ミスから回復する
        if (_recoveryTimer > RECOVERY_TIME) {
            _spriteRenderer.enabled = true;
            _recoveryTimer = (float)0.1;//ミス直後に表示させる必要があるので0から変更した
            _rendererEnableTime = 0;
            _isDamage = false;
            return;
        }//if
        //アニメーションの回復
        if (_recoveryTimer > RECOVERY_TIME - 1) {
            _pAnimator.AniDamage = false;
        }//if
        //自機の表示・非表示の変更
        if (_recoveryTimer <= _rendererEnableTime)
            return;
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
        _rendererEnableTime += 0.1f;
    }//Damage

    private void OnTriggerEnter2D(Collider2D col) {
        //アイテム取得した場合
        if(col.gameObject.tag == "Item") {
            col.gameObject.GetComponent<ItemManager>().PlayerGetItem();
        }//if
    }//OnTriggerEnter2D

    private void OnTriggerStay2D(Collider2D col) {
        EnemyTouch(col);
        GoalTouch(col);
    }//OnTriggerStay2D

    /// <summary>
    /// 敵に接触したときの処理
    /// </summary>
    /// <param name="col"></param>
    private void EnemyTouch(Collider2D col) {
        if (!_isDamage && 
            (col.gameObject.tag == "EnemyAttack" ||col.gameObject.tag == "DamageGimmick")) {
            _pAnimator.AniDamage = true;
            _isDamage = true;
            //AttackEffect.EffectGenerate("Burest", this.transform.parent.position, this.transform.parent.gameObject, true);
            _pLife.LifeDecrease();
        }//if
    }//EnemyTouch

    /// <summary>
    /// ゴールに触れたときの処理
    /// </summary>
    /// <param name="col"></param>
    private void GoalTouch(Collider2D col) {
        if (col.gameObject.tag != "Goal")//ゴールに触れていない場合処理を抜ける
            return;
        if (!_rotatingArrow.IsTouchGoal && !_rotatingArrow.IsTouchKey) {//&& !_rotatingArrow.IsTouchKeyを追加した(0525) SEを鳴らさないため
            _rotatingArrow.IsTouchGoal = true;
            _pAnimator.AudioManager.PlaySE("KeyGet");
        }//if
        if (!_rotatingArrow.IsTouchKey)
            return;
        switch (_stageClearManagement.StageStatus) {
            case EnumStageStatus.Normal:
                _stageClearManagement.StageStatus = EnumStageStatus.GoalMove;
                break;
        }//switch
    }//GoalAndKeyTouch

}//BodyTrigger
