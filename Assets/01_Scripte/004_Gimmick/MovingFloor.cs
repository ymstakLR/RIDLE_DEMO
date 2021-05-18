using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 移動する床の処理
/// 更新日時:0416
/// </summary>
public class MovingFloor : MovingGimmick {

    private new void Start() {
        _movingObject = transform.parent.gameObject;
        base.Start();
    }//Start

    private void FixedUpdate() {
        (_xPositionNow, _yPositionNow)= Move(_movingObject, _xPositionNow, _yPositionNow);
    }//Update

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            col.transform.SetParent(_movingObject.transform);
        }//if
    }//OnCollisionEnter2D

    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            col.transform.SetParent(null);
        }//if
    }//OnCollisionExit2D

}//MovingFloor
