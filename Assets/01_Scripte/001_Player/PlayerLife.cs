using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 自機の体力についての処理
/// 更新日時:0602
/// </summary>
public class PlayerLife : MonoBehaviour {
    private PlayerAnimator _pAnimator;

    private readonly float RECOVERY_TIMER = 15;

    private Image[] _life;
    private ItemProperty _itemProperty;

    private float _recoveryTime;
    private float _recoveryLifeAnimate;

    private void Start() {
        _pAnimator = this.GetComponent<PlayerAnimator>();
        _life = new Image[4];
        for (int i = 0; i < _life.Length; i++) {
             string path = "UI/UIItemReference/Life (" + (i+1) + ")";
            _life[i] = GameObject.Find(path).GetComponent<Image>();
        }//for
        _itemProperty = GameObject.Find("UI").GetComponent<ItemProperty>();
        for (int i = 0; i < _life.Length; ++i) {
            _life[i].enabled = true;
            _life[i].fillAmount = 1;
        }//for
        _recoveryLifeAnimate = 0;
    }//Start

    private void FixedUpdate() {
        LifeAnimate();
    }//FixedUpdate

    /// <summary>
    /// 体力回復中のアニメーションの再生処理
    /// </summary>
    private void LifeAnimate() {
        if (_life[0].fillAmount == 0 || _life[_life.Length - 1].fillAmount == 1)
            return;
        for (int i = 0; i < _life.Length; ++i) {
            if (_life[i].fillAmount != 1) {
                _life[i].fillAmount += 1 / (RECOVERY_TIMER * 55);
                _recoveryLifeAnimate = _life[i].fillAmount;
                return;
            }//if
        }//for
    }//LifeAnimate
    

    private void Update() {
        LifeRecoveryJudge();
    }//Update

    /// <summary>
    /// 体力の回復判定処理
    /// </summary>
    private void LifeRecoveryJudge() {
        if (_life[_life.Length - 1].fillAmount == 1) {
            _recoveryLifeAnimate = 0;
            return;
        }//if
        if (_recoveryTime < RECOVERY_TIMER) {
            _recoveryTime += Time.deltaTime;
            return;
        }//if
        LifeRecovery();
    }//LifeRecoveryJudge

    /// <summary>
    /// 体力の回復処理
    /// </summary>
    private void LifeRecovery() {
        for (int i = 0; i < _life.Length; ++i) {
            if (_life[i].fillAmount !=1) {
                _life[i].fillAmount = 1;
                _recoveryTime = 0;
                return;
            }//if
        }//for
    }//LifeRecovery

    //自機の体力を1減らす
    public void LifeDecrease() {
        _itemProperty.PlayerMissCount += 1;
        for (int i = _life.Length - 1; 0 <= i; --i) {
            if (_life[1].fillAmount==0) {//ゲームミスになる
                _life[0].fillAmount = 0;
                _pAnimator.AudioManager.PlaySE("PlayerMiss_Hide");
                this.GetComponent<PlayerAnimator>().AniMiss = true;
                return;
            }//if
            if (_life[i].fillAmount ==1) {//ダメージを受ける
                _life[i].fillAmount = _recoveryLifeAnimate;
                _pAnimator.AudioManager.PlaySE("PlayerDamage");
                return;
            }//if
            _life[i].fillAmount = 0;
        }//for
    }//LifeDecrease

}//PlayerLife
