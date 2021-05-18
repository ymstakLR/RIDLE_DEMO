using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 時間経過で起動するトゲギミックの処理
/// 更新日時:0416
/// </summary>
public class Thorn : MonoBehaviour {
    [SerializeField,Tooltip("起動するまでの時間(主に複数を使用して移動ルートを作る際に使用する)")]
    private float mStartUpTime = 0;
    [SerializeField, Tooltip("棘が動く場合の判定")]
    private bool mIsMove;

    private Transform _thornUnDamage;//子オブジェクトを取得する
    private Transform _thornDamege;//子オブジェクトを取得する

    private float _gimmickTime;//ギミック処理切り替えタイム

    private bool _isDamageDecision;//ダメージ発生判定

    private void Start() {
        _thornUnDamage = this.transform.Find("ThornUnDamage");
        _thornDamege = this.transform.Find("ThornDamage");
    }//Start

    private void Update() {
        if (mIsMove) {
            StartCoroutine(ThornCoroutine());
        }//if
    }//Update

    /// <summary>
    /// 棘起動までのコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator ThornCoroutine() {
        yield return new WaitForSeconds(mStartUpTime);
        GimmickDecision();
        GimmickWork();
    }//ThornCoroutine

    /// <summary>
    /// 棘の出現・消滅判定処理
    /// </summary>
    private void GimmickDecision() {
        if (_gimmickTime < 1) {//試用で1秒ごとにON、OFF切り替えるようにしている(0827)
            _gimmickTime += Time.deltaTime;
        } else {
            _isDamageDecision = !_isDamageDecision;
            _gimmickTime = 0;
        }//if
    }//GimmickDecision

    /// <summary>
    /// 棘を動かすための処理
    /// </summary>
    private void GimmickWork() {
        if (_thornDamege.localPosition.y < 2.55 &&_isDamageDecision) {
            _thornDamege.localPosition = new Vector2(
                _thornDamege.localPosition.x, _thornDamege.localPosition.y + (float)0.64);//0.64はトゲが出現したのがわかる程度の時間を設定した(0827)
            this.GetComponent<BoxCollider2D>().enabled = true;
            _thornDamege.GetComponent<BoxCollider2D>().enabled = true;
        }//if
        if (0 < _thornDamege.localPosition.y && !_isDamageDecision) {
            _thornDamege.localPosition = new Vector2(
                _thornDamege.localPosition.x, _thornDamege.localPosition.y - (float)0.64);
            _thornDamege.GetComponent<BoxCollider2D>().enabled = false;
        }//if
    }//GimmicWork

}//Thorn
