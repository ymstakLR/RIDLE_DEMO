using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 触れたら自機をジャンプさせる処理
/// 更新日時:0616
/// </summary>
public class Spring : MonoBehaviour {

    private PlayerUnderTrigger _pUnderTrigger;

    private GameObject _springUpper;//バネの上部オブジェクトを取得
    private GameObject _springUnder;//バネの下部オブジェクトを取得

    private float _underScaleY;//SpringUnderのYスケール
    private float _underPositionY;//SpringUnderのYポジション
    private float _upperPositionY;//SpringUpperのYポジション

    private bool _isElasticity;//自機が接触しているかの判定

    private void Start() {
        _pUnderTrigger = GameObject.Find("Ridle/UnderTrigger").GetComponent<PlayerUnderTrigger>();
        _springUpper = this.transform.Find("SpringUpper").gameObject;
        _springUnder = this.transform.Find("SpringUnder").gameObject;
        _underPositionY = (float)-1.5;
    }//Start

    private void FixedUpdate() {
        SpringMove();
        SpringWork();
        SpringReturn();
    }//Update


    /// <summary>
    /// バネの動きで使用する値の更新
    /// </summary>
    private void SpringMove() {
        if (_underPositionY < 0 && _isElasticity) {
            _underScaleY += (float)0.25;
            _underPositionY += (float)0.375;
            _upperPositionY += (float)0.625;
        }//if
        if (_underPositionY > -(float)1.5 && !_isElasticity) {
            _underScaleY -= (float)0.25;
            _underPositionY -= (float)0.375;
            _upperPositionY -= (float)0.625;
        }//if
    }//SpringCount

    /// <summary>
    /// バネの動きの反映
    /// </summary>
    private void SpringWork() {
        _springUnder.GetComponent<Transform>().localScale = new Vector2(_springUnder.GetComponent<Transform>().localScale.x, _underScaleY);
        _springUnder.GetComponent<Transform>().localPosition = new Vector2(_springUnder.GetComponent<Transform>().localPosition.x, _underPositionY);
        _springUpper.GetComponent<Transform>().localPosition = new Vector2(_springUpper.GetComponent<Transform>().localPosition.x, _upperPositionY);
        this.GetComponent<BoxCollider2D>().offset = new Vector2(this.GetComponent<BoxCollider2D>().offset.x, _upperPositionY);
    }//SpringWork

    /// <summary>
    /// バネを縮ませる判定処理
    /// </summary>
    private void SpringReturn() {
        if (_underPositionY >= 0 && _isElasticity) {
            _isElasticity = false;
        }//if
    }//SpringReturn


    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag != "Player")
            return;
        if (!_isElasticity) {
            GameObject.Find("GameManager").GetComponent<AudioManager>().PlaySE("Spring");
        }//if
        
        col.gameObject.GetComponent<PlayerJump>().RotationChange(0);
        col.gameObject.GetComponent<PlayerJump>().PastTPY = this.transform.position.y-1;
        //col.gameObject.GetComponent<PlayerJump>().IsFlipJumpFall = false;
        col.gameObject.GetComponent<PlayerJump>().JumpTypeFlag = EnumJumpTypeFlag.normal;
        col.gameObject.GetComponent<PlayerAnimator>().AniJump = true;
        col.gameObject.GetComponent<PlayerAnimator>().AniFall = false;
        
        _pUnderTrigger.IsGimmickJump = true;
        _isElasticity = true;
        SpringJumpUpdate(col);

    }//OnCollisionEnter2D

    /// <summary>
    /// ジャンプ量の更新処理
    /// </summary>
    /// <param name="col"></param>
    private void SpringJumpUpdate(Collision2D col) {
        switch (this.gameObject.transform.localEulerAngles.z) {//バネの角度 //左右のバネは開発に余裕があったら作成する
            case 0://上移動
                col.gameObject.GetComponent<Transform>().position =
                    new Vector2(col.gameObject.transform.position.x, this.GetComponent<Transform>().position.y + (float)7);
                col.gameObject.GetComponent<PlayerManager>().JumpPower = 450;
                break;
            case 180://下移動
                col.gameObject.GetComponent<Transform>().position =
                    new Vector2(col.gameObject.transform.position.x, this.GetComponent<Transform>().position.y - (float)7);
                col.gameObject.GetComponent<PlayerManager>().JumpPower = -450;
                break;
        }//switch
        col.gameObject.GetComponent<PlayerJump>().JumpInputLimit();
    }//SpringJumpUpdate

}//Spring
