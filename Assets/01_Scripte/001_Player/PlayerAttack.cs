using ConfigDataDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機の攻撃用の処理
/// 更新日時 :20211026
/// </summary>
public class PlayerAttack : MonoBehaviour {
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;
    private PlayerAnimator _pAnimator;
    private Renderer _renderer;
    private StageStatusManagement _stageClearMgmt;

    private readonly float ATTACK_TIME = 0.4f;//攻撃反映時間

    private float _attackTimer;//攻撃を行い始めてからの時間

    void Start() {
        _animator = this.GetComponent<Animator>();
        _boxCollider2D = this.GetComponent<BoxCollider2D>();
        _pAnimator = transform.parent.gameObject.GetComponent<PlayerAnimator>();
        _renderer = this.GetComponent<Renderer>();
        _stageClearMgmt = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
    }//Start

    void Update() {
        if(Time.timeScale == 0)//ポーズ中の場合は停止
            return;
        switch (_stageClearMgmt.StageStatus) {
            case EnumStageStatus.Normal:
            case EnumStageStatus.BossBattle:
                AttackBegin();
                break;
        }//switch
        AttackEnd();
        _attackTimer += Time.deltaTime;
    }//Update

    /// <summary>
    /// 攻撃を始める処理
    /// </summary>
    private void AttackBegin() {
        if(_attackTimer < ATTACK_TIME * 1.75||//攻撃終了後の一定時間
            _pAnimator.AniDamage)
            return;
        if (ConfigManager.Instance.config.GetKeyDown(ConfigData.Attack.String)) {//入力判定
            _animator.SetBool("AniEffect", true);
            _boxCollider2D.enabled = true;
            _renderer.enabled = true;
            _pAnimator.AudioManager.PlaySE("CutAttack");
            _attackTimer = 0;
            _pAnimator.AniAttack = true;
        }//if
    }//AttackBegin

    /// <summary>
    /// 攻撃を終了させる処理
    /// </summary>
    private void AttackEnd() {
        if (!_pAnimator.AniAttack)
            return;
        if(_attackTimer > ATTACK_TIME) {
            _animator.SetBool("AniEffect", false);
            _boxCollider2D.enabled = false;
            _pAnimator.AniAttack = false;
            _renderer.enabled = false;
        }//if 
    }//AttackEnd

}//PlayerAttack
