using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// MovingRockの下側当たり判定
/// 更新日時:0416
/// </summary>
public class MovingRock_Under : MonoBehaviour {

    private MovingRock _movingRock;

    private void Start() {
        _movingRock = this.transform.parent.GetComponent<MovingRock>();
    }//Start

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Stage") {
            _movingRock.IsUnderTrigger = true;
        }//if
    }//OnTriggerEnter2D

    private void OnTriggerExit2D(Collider2D col) {
        _movingRock.IsSideTrigger = false;
    }//OnTriggerExit2D

}//MovingRock_Under
