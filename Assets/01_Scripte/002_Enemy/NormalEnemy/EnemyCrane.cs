using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemyのクレーン攻撃の一連の処理を記述する
/// 更新日時:0413
/// </summary>
public class EnemyCrane : MonoBehaviour {

    private readonly float SHORTEST_SCALE_SIZE = (float)1.5;//クレーンの最短サイズ
    private readonly float JUDGMENT_RECOVERY_TIME = 2;//アームに再び当たり判定を表示させる時間数

    private float _judgmentRecoveryTimer;//アームの当たり判定用のタイマー

    private float _craneSpeed;//クレーンの移動スピード
    private bool _cranesTretch = true;//クレーンを伸ばすかの判定

    private bool _playerUncatch;
    public bool PlayerUncatch { set { _playerUncatch = value; } }

    /// <summary>
    /// 通常　LongestScaleSizeまでクレーンを伸ばす
    /// 条件：時期がアームに触れた場合
    /// SHORTEST_SCALE_SIZEまでクレーンを縮ませる
    /// </summary>
    public void Crane(GameObject crane,GameObject arm,float longestScaleSize) {
        CraneJudge(longestScaleSize);
        CraneExpansion(crane, arm);
    }//Crane

    /// <summary>
    /// クレーン内の判定処理
    /// </summary>
    /// <param name="longestScaleSize"></param>
    private void CraneJudge(float longestScaleSize) {
        if (_cranesTretch) {//クレーンを伸ばす場合
            _craneSpeed += Time.deltaTime * 8;//クレーンを伸ばす
        } else {
            _craneSpeed -= Time.deltaTime * 8;//クレーンを縮ませる
        }//if

        if (_craneSpeed > longestScaleSize && _cranesTretch) {//クレーンが伸び切っている場合
            _cranesTretch = false;
        } else if (SHORTEST_SCALE_SIZE > _craneSpeed && !_cranesTretch) {//クレーンが縮み切っている場合
            _cranesTretch = true;
        }//if
    }//CraneJudge

    /// <summary>
    /// クレーンの伸縮処理
    /// </summary>
    /// <param name="crane"></param>
    /// <param name="arm"></param>
    private void CraneExpansion(GameObject crane, GameObject arm) {
        float craneScaleY = -_craneSpeed;//クレーンのスケール(長さ)の値
        float cranePositionY = ((craneScaleY / 2) + 1);//クレーンの縦軸の配置箇所の値
        float armPositionY = craneScaleY + 1;//アームの縦軸の配置箇所の値
        crane.GetComponent<Transform>().localScale = new Vector2(crane.GetComponent<Transform>().localScale.x, craneScaleY);
        crane.GetComponent<Transform>().localPosition = new Vector2(crane.GetComponent<Transform>().localPosition.x, cranePositionY);
        arm.GetComponent<Transform>().localPosition = new Vector2(arm.GetComponent<Transform>().localPosition.x, armPositionY);
    }//CraneExpansion

    /// <summary>
    /// 自機が捕獲解除されたときのアーム処理
    /// アームの当たり判定、アニメーション
    /// </summary>
    /// <param name="arm">クレーンオブジェクト</param>
    /// <param name="enemyArm">アームオブジェクト</param>
    /// <param name="enemy">敵オブジェクト</param>
    public void Arm(GameObject arm,Animator animator) {
        if (!_playerUncatch) //自機が捕獲解除されたとき
            return;
        arm.GetComponent<CapsuleCollider2D>().enabled = false;//アームの当たり判定を消去
        animator.SetBool("AniWork", false);
        if (_judgmentRecoveryTimer > JUDGMENT_RECOVERY_TIME) {//アームの当たり判定を表示させる条件
            arm.GetComponent<CapsuleCollider2D>().enabled = true;//アームの当たり判定を表示
            arm.GetComponent<Animator>().SetBool("AniWork", false);//アームを開くアニメーションにする
            _judgmentRecoveryTimer = 0;//タイマーの初期化
        } else {
            _judgmentRecoveryTimer += Time.deltaTime;
        }//if
    }//Arm

}//EnemyCrane
