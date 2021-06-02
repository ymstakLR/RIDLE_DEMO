using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ判定のドリル処理
/// 更新日時:0416
/// </summary>
public class Drill : MonoBehaviour {
    private readonly float MOVING_SPEED = 1500;

    private float _movingSpeed;
    private float _destroyTime;

    private bool _isDestroy;

    private void Start() {
        if (0 < transform.localScale.x) {
            _movingSpeed = MOVING_SPEED;
        } else {
            _movingSpeed = -MOVING_SPEED;
        }//if
        this.GetComponent<Rigidbody2D>().AddForce(Vector2.left * _movingSpeed);
    }//Start

    private void FixedUpdate() {
        DrillWork();
    }//FixedUpdate

    private void Update() {
        DrillDestroy();
    }//Update

    private void DrillWork() {
        this.transform.localScale = new Vector2(this.transform.localScale.x,this.transform.localScale.y);
    }//DrillWork

    /// <summary>
    /// ドリルを消滅させる処理
    /// </summary>
    private void DrillDestroy() {
        if ((this.transform.localScale.y <= 0.2)||
            (this.transform.localScale.x <= 0.2 && _movingSpeed == MOVING_SPEED)||
            (this.transform.localScale.x >= 0.2 && _movingSpeed != MOVING_SPEED)) {
            Destroy(this.gameObject);
        }//if
        if (!_isDestroy)
            return;
        if (_destroyTime < (float)0.25) {
            _destroyTime += Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }//if
    }//DrillDestroy

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Stage") {
            _isDestroy = true;
        }//if
    }//OnTriggerEnter2D


}//Drill
