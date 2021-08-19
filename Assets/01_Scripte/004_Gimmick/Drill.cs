using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ判定のドリル処理
/// 更新日時:0819
/// </summary>
public class Drill : GenerateDamageObjBase {
    private readonly float MOVING_SPEED = 1500;

    private void Start() {
        if (0 < transform.localScale.x) {
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.left * MOVING_SPEED);
        } else {
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.left * -MOVING_SPEED);
        }//if
    }//Start

    private void FixedUpdate() {
        DrillWork();
    }//FixedUpdate

    private void Update() {
        GenerateDamageObjDestroy();
    }//Update

    private void DrillWork() {
        this.transform.localScale = new Vector2(this.transform.localScale.x,this.transform.localScale.y);
    }//DrillWork

}//Drill
