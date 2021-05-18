using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一定範囲を移動するトゲギミック処理
/// 更新日時:0416
/// </summary>
public class WorkThorn : MovingGimmick {

    private new void Start() {
        _movingObject = this.gameObject;
        base.Start();
    }//Start

    private void FixedUpdate() {
        (_xPositionNow, _yPositionNow) = Move(_movingObject, _xPositionNow, _yPositionNow);
    }//Update

}//WorkThorn
          