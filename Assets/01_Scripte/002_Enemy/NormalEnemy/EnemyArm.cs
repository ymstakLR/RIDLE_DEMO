using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵4のアーム処理
/// 更新日時:0409
/// </summary>
public class EnemyArm : MonoBehaviour {

    private readonly float JUDGMENT_RECOVERY_TIME = 2;//アームに再び当たり判定を表示させる時間数
    private readonly float UNCATCH_TIME = (float)0.25;

    private float _uncatchTimer;
    public float UncatchTimer { set { _uncatchTimer = value; } }

    private void Start() {
        _uncatchTimer = JUDGMENT_RECOVERY_TIME*3;//タイマーの処理を変更すれば初期化が必要なくなると考える タイムを減少させて処理を行うなど(0929)
    }//Start

    private void Update() {
        ArmReboot();
    }//Update

    /// <summary>
    /// アームの値を再起動させる
    /// </summary>
    private void ArmReboot() {
        if (_uncatchTimer > JUDGMENT_RECOVERY_TIME * 2)
            return;
        if (_uncatchTimer > JUDGMENT_RECOVERY_TIME) {
            this.GetComponent<CapsuleCollider2D>().enabled = true;
            this.GetComponent<Animator>().SetBool("AniWork", false);
            _uncatchTimer *= 3;
        } else {
            _uncatchTimer += Time.deltaTime;
            this.transform.parent.GetComponent<Animator>().SetBool("AniWork", false);
        }//if
    }//ArmReboot

    private void OnTriggerEnter2D(Collider2D col) {
        PlayerCatch(col);
    }//OnTriggerStay2D

    /// <summary>
    /// 自機に当たったときの判定
    /// </summary>
    /// <param name="col"></param>
    private void PlayerCatch(Collider2D col) {
        if (col.gameObject.tag != "PlayerBody")
            return;
        this.transform.parent.GetComponent<Animator>().SetBool("AniWork", true);
        Transform player = col.transform.parent;
        player.SetParent(this.gameObject.transform, true);//自機をアームオブジェクトの子要素にする
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        player.GetComponent<Transform>().localRotation = Quaternion.Euler(0, 0, 0);
        player.GetComponent<Transform>().localPosition = new Vector2(0, -1);
        player.GetComponent<Transform>().localScale = new Vector2(player.GetComponent<Transform>().localScale.x, 1);
        this.GetComponent<Animator>().SetBool("AniWork", true);
        player.GetComponent<Animator>().SetBool("AniCapture", true);
        player.GetComponent<PlayerManager>().enabled = false;
        player.GetComponent<PlayerCapture>().enabled = true;
    }//PlayerCatch

}//EnemyArm
