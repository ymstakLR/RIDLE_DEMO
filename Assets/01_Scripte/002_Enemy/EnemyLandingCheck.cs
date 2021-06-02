using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵がステージに触れている判定処理
/// 更新日時:0602
/// </summary>
public class EnemyLandingCheck : MonoBehaviour {
    private bool _isLandingCheck;
    public bool IsLandingCheck { get { return _isLandingCheck; } }


    private void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "PlatformEffector") {
            _isLandingCheck = true;
        }//if

    }//OnTriggerStay2D

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "PlatformEffector") {
            _isLandingCheck = false;
        }//if
    }//OnTriggerExit2D

}//EnemyLandingCheck
