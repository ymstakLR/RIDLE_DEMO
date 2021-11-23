using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnderTriggerの変数を使用する
/// 更新日時:20211117
/// </summary>
public class PlayerUnderTriggerPE : PlayerUnderTrigger {

    private Animator _animator;
    private PlayerUnderTrigger _pUnderTrigger;
    private PlayerJump _pJump;

    private bool _isPEExit;

    private new void Start() {
        base.Start();
        _animator = this.transform.parent.GetComponent<Animator>();
        _pJump = this.transform.parent.GetComponent<PlayerJump>();
        _pUnderTrigger = this.transform.parent.transform.Find("UnderTrigger").GetComponent<PlayerUnderTrigger>();
        _isPEExit = true;
    }//Start

    private void Update() {
        //情報受け渡し(レイヤー変更時にこのスクリプトを参照するため)
        IsGimmickJump = _pUnderTrigger.IsGimmickJump;
        IsRise = _pUnderTrigger.IsRise;
    }//Update

    private void OnTriggerStay2D(Collider2D col) {//元々はOnTriggerEnter2Dの処理だった 不具合があった場合上記のコメントになっている処理を確認する(0805)
        if (col.gameObject.tag != "PlatformEffector")
            return;
        if (this.transform.parent.localEulerAngles.z != 0)
            return;
        if (!_pUnderTrigger.IsJumpUp && _isPEExit) {
            _pUnderTrigger.IsUnderTrigger = true;
            _pAnimator.AniFall = false;
            _isPEExit = false;
        }//if
    }//OnTriggerStay2D

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag != "PlatformEffector")
            return;
        _pUnderTrigger.IsRise = true;
        _pUnderTrigger.IsUnderTrigger = false;
        _pJump.IsJump = true;
        _isPEExit = true;
    }//OnTriggerExit2D

}//UnderTrigger
