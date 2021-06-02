using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///触れたタブごとの処理変更ようの列挙型になる
///追加するごとに見直しが必要になる
public enum BodyType {
    wait = 0,
    stage = 1,
    platformEffect = 2,
    gimmick = 3
}//BodyType

///プレイヤーの当たり判定処理
///更新日時:0602
public class PlayerBody : MonoBehaviour {
    public BodyType IsBody { get; set; }//触れているタグごとの数値取得

    private PlayerUnderTrigger _pUnderTrigger;

    private float _pastTPY;

    private void Start() {
        _pUnderTrigger = this.transform.parent.Find("UnderTrigger").GetComponent<PlayerUnderTrigger>();
    }//Start

    private void Update() {
        _pastTPY = this.transform.parent.position.y;
    }//Upadate

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Stage") {
            IsBody = BodyType.stage;
        }//if
        if (col.gameObject.tag == "PlatformEffector" && _pastTPY > this.transform.parent.position.y) {
            IsBody = BodyType.platformEffect;
        }//if 
        if (col.gameObject.tag == "Gimmick") {
            IsBody = BodyType.gimmick;
        }//if
    }//OnCollisionEnter2D

    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Stage") {
            IsBody = BodyType.wait;
        }//if
        if (col.gameObject.tag == "PlatformEffector") {
            IsBody = BodyType.wait;
        }//if
        if (_pUnderTrigger.IsUnderTrigger) {//ジャンプ中の場合
            IsBody = BodyType.stage;
        }//if
    }//OnCollisionExit2D

}//Body
