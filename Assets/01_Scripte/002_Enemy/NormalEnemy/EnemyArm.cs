using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵4のアーム処理
/// 更新日時:20210819
/// </summary>
public class EnemyArm : MonoBehaviour {

    private Animator _thisAnimator;
    private Animator _parentAnimator;
    private EnemyParent _parentEnemyParentScript;

    private readonly float JUDGMENT_RECOVERY_TIME = 2;//アームに再び当たり判定を表示させる時間数
    private readonly float UNCATCH_TIME = (float)0.25;

    private float _uncatchTimer;
    public float UncatchTimer { set { _uncatchTimer = value; } }

    private void Start() {
        _thisAnimator = this.GetComponent<Animator>();
        _parentAnimator = this.transform.parent.GetComponent<Animator>();
        _parentEnemyParentScript = this.transform.parent.GetComponent<EnemyParent>();
        _uncatchTimer = JUDGMENT_RECOVERY_TIME*3;//タイマーの処理を変更すれば初期化が必要なくなると考える タイムを減少させて処理を行うなど(0929)
    }//Start

    private void Update() {
        ArmReboot();
        ArmStopCheck();
    }//Update

    /// <summary>
    /// アームの値を再起動させる
    /// </summary>
    private void ArmReboot() {
        if (_uncatchTimer > JUDGMENT_RECOVERY_TIME * 2)
            return;
        if (_uncatchTimer > JUDGMENT_RECOVERY_TIME) {
            this.GetComponent<CapsuleCollider2D>().enabled = true;
            _thisAnimator.SetBool("AniWork", false);
            _uncatchTimer *= 3;
        } else {
            _uncatchTimer += Time.deltaTime;
            _parentAnimator.SetBool("AniWork", false);
        }//if
    }//ArmReboot

    /// <summary>
    /// アーム停止をチェックする処理
    /// </summary>
    private void ArmStopCheck() {
        if (_parentEnemyParentScript.AniMiss) {
            _thisAnimator.SetBool("AniWork", true);
            _parentEnemyParentScript.enabled = false;
        }//if
    }//ArmStopCheck


    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag != "PlayerBody")
            return;
        _parentAnimator.SetBool("AniWork", true);
        _thisAnimator.SetBool("AniWork", true);
        PlayerCatch(col.transform.parent);
    }//OnTriggerStay2D

    /// <summary>
    /// 自機を捕獲した際の自機に対する処理
    /// </summary>
    /// <param name="col"></param>
    private void PlayerCatch(Transform player) {
        player.SetParent(this.gameObject.transform, true);//自機をアームオブジェクトの子要素にする
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        player.localRotation = Quaternion.Euler(0, 0, 0);
        player.localPosition = new Vector2(0, -1);
        player.localScale = new Vector2(player.GetComponent<Transform>().localScale.x, 1);
        player.GetComponent<Animator>().SetBool("AniCapture", true);
        player.GetComponent<PlayerManager>().enabled = false;
        player.GetComponent<PlayerCapture>().enabled = true;
    }//PlayerCatch

}//EnemyArm
