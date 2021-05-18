using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ダメージボールの共通処理
/// 更新日時:0416
/// </summary>
public class DamageBallBase : MonoBehaviour {
    private float _destroyTimer;//ダメージ球のしょきょするための時間
    private bool _isDestroy;//消去するための判定

    /// <summary>
    /// ボールの消滅処理
    /// </summary>
    protected void BallDestroy() {
        if (!_isDestroy)
            return;
        if (_destroyTimer < (float)0.1) {
            _destroyTimer += Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }//if
    }//BallDestroy

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "PlayerBody")
            _isDestroy = true;
    }//OnTriggerEnter2D

}//DamageBallBase

