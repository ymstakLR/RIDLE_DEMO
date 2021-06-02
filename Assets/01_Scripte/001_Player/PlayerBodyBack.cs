using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背面の当たり判定用の処理
/// 更新日時:0602
/// </summary>
public class PlayerBodyBack : MonoBehaviour {
    public bool IsBodyBack { get; set; }//Stage,PlatformEffectorタグに触れたときの判定

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "PlatformEffector" && this.transform.parent.transform.localEulerAngles.z == 0)
            return;
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "PlatformEffector"||
            col.gameObject.tag == "Gimmick") {
            IsBodyBack = true;
        }//if
    }//OnTriggerEnter2D

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "PlatformEffector")
            IsBodyBack = false;
    }//OnTriggerExit2D

}//BodyBack
