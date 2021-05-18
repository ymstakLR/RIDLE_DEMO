using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///触れたタブごとの処理変更ようの列挙型になる
///追加するごとに見直しが必要になる(0928)
public enum BodyType {
    wait = 0,
    stage = 1,
    platformEffect = 2,
    gimmick = 3
}//BodyType

///アタッチしているBodyの当たり判定がステージ配置で左右されると考えられる
///PlatformEffectorタグで自機が動けなくなる箇所を当たり判定で強引に解決させた
///当たり判定を変更できなくなる場合は新たにステージ床のみの当たり判定にしてそうオブジェクト数を増やす必要がある(0927
///更新日時:0401
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

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Stage") {
            IsBody = BodyType.stage;
        }//if
        if (col.gameObject.tag == "PlatformEffector" && _pastTPY > this.transform.parent.position.y) {
            IsBody = BodyType.platformEffect;
        }//if 
        if (col.gameObject.tag == "Gimmick") {
            IsBody = BodyType.gimmick;
        }//if
    }//OnTriggerEnter2D

    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Stage") {
            IsBody = BodyType.wait;
            //return;//不具合が出たらコメント解除して動きを確認する
        }//if
        if (col.gameObject.tag == "PlatformEffector") {
            IsBody = BodyType.wait;
        }//if
        if (_pUnderTrigger.IsUnderTrigger) {//ジャンプ中の場合
            IsBody = BodyType.stage;
        }//if
    }//OnCollisionExit2D



}//Body
