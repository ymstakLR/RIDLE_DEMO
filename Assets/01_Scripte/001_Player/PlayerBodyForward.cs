using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 前面の当たり判定用の処理
/// 更新日時:0602
/// </summary>
public class PlayerBodyForward : MonoBehaviour {
    public bool IsBodyForward { get; set; }//Stageタグに触れたかの判定
    private PlayerUnderTrigger _pUnderTrigger;

    private void Start() {
        _pUnderTrigger = this.transform.parent.transform.Find("UnderTrigger").GetComponent<PlayerUnderTrigger>();
    }//Start

    private void OnTriggerStay2D(Collider2D col) {
        if(col.gameObject.tag == "Stage") {
            IsBodyForward = true;
        }//if
    }//OnTriggerEnter2D

    private void OnTriggerExit2D(Collider2D col) {
        if(col.gameObject.tag == "Stage") {
            IsBodyForward = false;
        }//if
    }//OnTriggerExit2D

}//BodyTrigger