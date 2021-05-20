using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnderTriggerの変数を使用する
/// 更新日時:0406 処理の必要性を調査中
/// </summary>
public class PlayerUnderTriggerPE : PlayerUnderTrigger {

    private Animator _animator;
    private PlayerUnderTrigger _pUnderTrigger;
    private PlayerJump _pJump;
    private PlayerTop _pTop;

    private float _fallTimer;//落下に有した時間
    private float _pastTPY;//1フレーム前のpositionYの値

    public int _passingEnterNum;//ジャンプ後のPlatformEffectorタグの通過回数

    private new void Start() {
        base.Start();
        _animator = this.transform.parent.GetComponent<Animator>();
        _pJump = this.transform.parent.GetComponent<PlayerJump>();
        _pTop = this.transform.parent.transform.Find("Top").GetComponent<PlayerTop>();
        _pUnderTrigger = this.transform.parent.transform.Find("UnderTrigger").GetComponent<PlayerUnderTrigger>();
    }//Start

    private void Update() {
        LandingCheack();
    }//Update

    /// <summary>
    /// 地面に触れているかの判定
    /// </summary>
    private void LandingCheack() {
        //情報受け渡し(レイヤー変更時にこのスクリプトを参照するため)
        IsGimmickJump = _pUnderTrigger.IsGimmickJump;
        IsRise = _pUnderTrigger.IsRise;

        if (this.transform.parent.localEulerAngles.z != 0)
            return;
        if ((_pTop.IsStageStay || (!_animator.GetBool("AniJump") && !_animator.GetBool("AniFall")) ||
            _pUnderTrigger.IsUnderTrigger)) {
            _fallTimer = 0;
            return;
        }//if
        AniFallEndJudge();
    }//LandingCheack

    /// <summary>
    /// 落下処理継続中に終了させる処理
    /// </summary>
    private void AniFallEndJudge() {
        if (_pastTPY == this.transform.parent.position.y) {
            if (this.transform.parent.transform.localScale.y < 0)
                return;
            if (_fallTimer > 0.02) {//アニメーションが落下中の場合だったら//比較値は元々0.02だった(不具合を発見したら元に戻す(0426))
                _pUnderTrigger.IsUnderTrigger = true;
                Debug.Log("IsUnderTriggerTrue");
                _pAnimator.AniFall = false;//ここの処理を別スクリプトで行えるようにする(0115)
            } else {
                _fallTimer += Time.deltaTime;
            }//if
        } else {
            _fallTimer = 0;
        }//if
        _pastTPY = this.transform.parent.position.y;
    }//AniFallJudge


    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag != "PlatformEffector")
            return;
    }//OnTriggerEnter2D

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag != "PlatformEffector")
            return;
        _pUnderTrigger.IsRise = true;
        _pUnderTrigger.IsUnderTrigger = false;
        _pJump.IsJump = true;
    }//OnTriggerExit2D

}//UnderTrigger
