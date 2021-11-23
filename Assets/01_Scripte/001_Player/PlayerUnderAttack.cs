using ConfigDataDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 足部分の攻撃判定処理
/// 更新日時:20211123
/// </summary>
public class PlayerUnderAttack : MonoBehaviour {
    private PlayerJump _pJump;
    private PlayerWork _pWork;

    public int UnderAttackPower { get; set; }//ジャンプ中に敵などを踏みつけたときの反動

    private void Start() {
        _pJump = transform.parent.gameObject.GetComponent<PlayerJump>();
        _pWork = transform.parent.gameObject.GetComponent<PlayerWork>();
        UnderAttackPower = 0;//初期値 
    }//Start

    private void OnTriggerEnter2D(Collider2D col) {
        if(this.transform.parent.localScale.y < 0)
            return;
        if (col.gameObject.tag != "Stage" && 
            col.gameObject.tag !="Enemy")//タグを動的に変更する際はこの処理の修正を行う必要がある
            return;
        PlayerTagChange(col,"Player");
        AttackEnemyJudge(col);
    }//OnTriggerEnter2D


    private void OnTriggerExit2D(Collider2D col) {
        PlayerTagChange(col, "PlayerTrampling");
    }//OnTriggerExit2D

    /// <summary>
    /// タグの変更をする処理
    /// </summary>
    /// <param name="col">触れた当たり判定</param>
    /// <param name="tagName">変更するタグ名</param>
    private void PlayerTagChange(Collider2D col, string tagName) {
        if (col.gameObject.tag == "Stage"||col.gameObject.tag == "PlatformEffector"||col.gameObject.tag =="Gimmick") {
            this.gameObject.tag = tagName;
            //this.gameObject.layer = LayerMask.NameToLayer(tagName);
        }//if
    }//PlayerTagChange

    /// <summary>
    /// 敵を攻撃しているかの判定処理
    /// </summary>
    /// <param name="col">触れた当たり判定</param>
    private void AttackEnemyJudge(Collider2D col) {
        if (col.gameObject.tag != "Enemy"&&col.gameObject.tag != "EnemyAttack")
            return;
        if (_pJump.JumpTypeFlag == EnumJumpTypeFlag.flipFall ||
           _pJump.JumpTypeFlag == EnumJumpTypeFlag.wallFlipFall)//↑の条件文にまとめれるが見づらくなるので分割した
            return;
        AttackEnemy();
    }//EnemyAttackJudge

    /// <summary>
    /// 敵を攻撃したときの処理
    /// </summary>
    private void AttackEnemy() {
        if (ConfigManager.Instance.config.GetKey(ConfigData.NormalJump.String) || 
            ConfigManager.Instance.config.GetKey(ConfigData.FlipJump.String)) {
            UnderAttackPower = 2;
        } else {
            UnderAttackPower = 1;
        }//if 
        _pJump.JumpInputLimit();
    }//AttackEnemy

}//UnderAttack
