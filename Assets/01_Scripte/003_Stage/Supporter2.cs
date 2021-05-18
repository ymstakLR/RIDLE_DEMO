using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 味方2の処理
/// 更新日時:0415
/// </summary>
public class Supporter2 : Enemy2 {//Supporterで親クラスを作るよりEnemyを継承したほうが簡単なのでEnemyを継承した(1106)
    private Result _result;

    private new void Start() {
        base.Start();
        _result = GameObject.Find("UI/ResultText").GetComponent<Result>();
        EnemySpeed = 10;
    }//Start

    private void FixedUpdate() {
        if (!_result.IsSupporterMove)
            return;
        if (EnemySpeed > 0) {
            EnemySpeed = -EnemySpeed;
        }//if
        base.Work();
    }//FixedUpdate

    private void Update() {
        if (this.transform.localScale.x > 0) {//左向きにする
            this.transform.localScale = new Vector2(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y);
        }//if
        base.ParentUpdate();
    }//Update

}//Supporter2
