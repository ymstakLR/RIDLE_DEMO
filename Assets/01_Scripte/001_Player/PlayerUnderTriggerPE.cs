using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnderTriggerの変数を使用する
/// 更新日時:0602
/// </summary>
public class PlayerUnderTriggerPE : PlayerUnderTrigger {

    private Animator _animator;
    private PlayerUnderTrigger _pUnderTrigger;
    private PlayerJump _pJump;

    private new void Start() {
        base.Start();
        _animator = this.transform.parent.GetComponent<Animator>();
        _pJump = this.transform.parent.GetComponent<PlayerJump>();
        _pUnderTrigger = this.transform.parent.transform.Find("UnderTrigger").GetComponent<PlayerUnderTrigger>();
    }//Start

    private void Update() {
        //情報受け渡し(レイヤー変更時にこのスクリプトを参照するため)
        IsGimmickJump = _pUnderTrigger.IsGimmickJump;
        IsRise = _pUnderTrigger.IsRise;
    }//Update


    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag != "PlatformEffector")
            return;
        if (!_pUnderTrigger.IsJumpUp) {
            _pUnderTrigger.IsUnderTrigger = true;
            _pAnimator.AniFall = false;
        }//if
    }//OnTriggerEnter2D

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag != "PlatformEffector")
            return;
        _pUnderTrigger.IsRise = true;
        _pUnderTrigger.IsUnderTrigger = false;
        _pJump.IsJump = true;
    }//OnTriggerExit2D

}//UnderTrigger
