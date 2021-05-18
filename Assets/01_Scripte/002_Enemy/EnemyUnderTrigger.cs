using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵下側が触れた場合の判定処理(地面接触用)
/// 更新日時:0408
/// </summary>
public class EnemyUnderTrigger : BaseUnderTrigger {

    //条件文を見やすくするようにする returnを使う(1104)
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Stage"||
            col.gameObject.tag == "PlatformEffector"||
            col.gameObject.tag == "Gimmick") {
            IsUnderTrigger = true;
        }//if
    }//OnTriggerEnter2D

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Stage" || 
            col.gameObject.tag == "PlatformEffector"||
            col.gameObject.tag == "Gimmick") {//Gimmickタグの確認中(必要ない可能性あり(0408))
            IsUnderTrigger = false;
        }//if

    }//OnTriggerExit2D

}//EnemyUnderTrigger
