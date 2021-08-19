using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成ダメージオブジェクトの共通処理
/// 更新日時:0819
/// </summary>
public class GenerateDamageObjBase : MonoBehaviour {
    private const int OBJ_DELETE_COUNT = 5;//球を消去するためのカウント定数

    private int _objDeleteCounter;//玉の消去する用のカウント変数

    /// <summary>
    /// ボールの消滅処理
    /// </summary>
    protected void GenerateDamageObjDestroy() {
        if(_objDeleteCounter >= OBJ_DELETE_COUNT) {
            Destroy(this.gameObject);
        }//if
    }//BallDestroy

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Stage") {
            _objDeleteCounter++;
        }//if
        if(col.gameObject.tag == "StageEdge") {
            _objDeleteCounter = OBJ_DELETE_COUNT;
        }
    }//OnTriggerEnter2D

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Stage") {
            _objDeleteCounter--;
        }//if
    }//OnTriggerExit2D

}//DamageBallBase

