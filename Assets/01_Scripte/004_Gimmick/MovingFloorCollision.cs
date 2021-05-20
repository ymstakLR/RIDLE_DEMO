using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CicleMovingObjectで移動させる床の処理
/// 更新日時:0416
/// </summary>
public class MovingFloorCollision : MonoBehaviour {

    private void Update() {
        this.transform.localPosition = new Vector3(0, 0, 0);
    }//Update

    private void OnCollisionEnter2D(Collision2D col) {//0129現状時期のみの処理しか記載していないので敵オブジェクトの処理を後日記載していく
        if (col.gameObject.tag != "Player")
            return;
        
        bool isUnderTrigger =false;
        int angle = (int)col.gameObject.transform.localEulerAngles.z;

        foreach (Transform child in col.gameObject.transform) {
            if (child.name.ToString() == "UnderTrigger") {
                isUnderTrigger = child.GetComponent<BaseUnderTrigger>().IsUnderTrigger; 
            }//if
        }//foreach

        if(isUnderTrigger == false &&(angle !=180&&angle!= 270)) {
            return;
        }//if
        col.transform.SetParent(transform.parent);
    }//OnCollisionEnter2D

    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            
            col.transform.SetParent(null);
        }//if
    }//OnCollisionExit2D
}
