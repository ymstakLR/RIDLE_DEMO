using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機がミスしたときの処理
/// 更新日時:0616
/// </summary>
public class PlayerMiss : MonoBehaviour {
    private PlayerAnimator _pAnimator;
    private Retray _retray;

    private float _missPos;
    private bool _isMiss;

    private void Start() {
        _pAnimator = this.GetComponent<PlayerAnimator>();
        _retray = GameObject.Find("EventSystem").GetComponent<Retray>();
        _missPos = Camera.main.GetComponent<StageEdgeGeneration>().LowerEndPos ;
    }//Start
    

    private void Update() {
        if(_isMiss)
            return;
        FallOffScreen();
        NoLife();
    }//Update

    /// <summary>
    /// 画面外に落下したときの処理
    /// </summary>
    private void FallOffScreen() {
        float angleZ = this.transform.localEulerAngles.z;
        float posY = this.transform.position.y;
        if (((angleZ == 0 || angleZ == 180) && posY < _missPos + 5f) ||
            ((angleZ == 90 || angleZ == 270) && posY < _missPos + 6.5f)) {
            CommonMiss(1.0f);//(2.0f)
        }//if
    }//FallOffScreen

    /// <summary>
    /// 体力がなくなったときの処理
    /// </summary>
    private void NoLife() {
        if (_pAnimator.AniMiss) {
            CommonMiss(0.5f);
        }//if
    }//NoLife

    /// <summary>
    /// ミスしたときの共通の処理
    /// </summary>
    /// <param name="retrayTime"></param>
    private void CommonMiss(float retrayTime) {
        _retray.SceneRetray(retrayTime,Time.deltaTime);
        _pAnimator.AudioManager.FadeOutStart(1f);
        TagChange(this.gameObject, "PlayerMiss");
        _isMiss = true;
    }//CommonMiss

    /// <summary>
    /// タグを変更する処理(子要素も含む)
    /// </summary>
    /// <param name="thisGameObject">タグを変更するオブジェクト</param>
    /// <param name="tagName">変更したいタグ名</param>
    private void TagChange(GameObject thisGameObject,string tagName) {
        thisGameObject.tag = tagName;
        foreach(Transform child in thisGameObject.transform) {
            child.tag = tagName;
        }//foreach
    }//TagChange

}//PlayerMiss
