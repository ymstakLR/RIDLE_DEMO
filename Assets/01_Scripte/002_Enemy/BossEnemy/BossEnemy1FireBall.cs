using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ボス敵1の生成したダメージ玉の処理
/// 更新日時:0413
/// </summary>
public class BossEnemy1FireBall : GenerateDamageObjBase {
    private GameObject _player;

    private readonly float BALL_SPEED = (float)22.5;

    private float _ballMoveX;
    private float _ballMoveY;

    void Start() {
        _player = _player = GameObject.Find("Ridle"); 

        Vector2 arrowPosition = this.transform.position;
        Vector2 dt = (Vector2)_player.transform.position - arrowPosition;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float angle = (270 - (rad * Mathf.Rad2Deg)) % 360;//角度取得

        float directionPosX = Mathf.Abs(_player.transform.localPosition.x - this.transform.position.x);
        float directionPosY = Mathf.Abs(_player.transform.localPosition.y - this.transform.position.y);

        if (directionPosX > directionPosY) {
            _ballMoveY = (Mathf.Floor(((directionPosY / directionPosX) * 10)) / 10) * BALL_SPEED;
            _ballMoveX = BALL_SPEED;
        } else {
            _ballMoveX = (Mathf.Floor(((directionPosX / directionPosY) * 10)) / 10 * BALL_SPEED);
            _ballMoveY = BALL_SPEED;
        }//if

        (_ballMoveX,_ballMoveY) = AngleCorrection(angle, _ballMoveX,_ballMoveY);

        if (this.transform.localScale.x > 0) {
            _ballMoveX = -_ballMoveX;
        }//if
        if(this.transform.position.y > _player.transform.position.y) {
            _ballMoveY = -_ballMoveY;
        }//if
    }//Start

    /// <summary>
    /// angleの値補正の処理
    /// 上下方向の移動範囲
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private (float,float) AngleCorrection(float angle,float ballMoveX,float ballMoveY) {//if文を一つにまとめれるが今後変更する可能性があるので処理を分割しておく(1124)
        if (this.transform.localScale.x > 0) {//左に移動する場合
            if (angle > 45 && angle <180 ) {
                return ((float)22.5,15);
            } else if(angle < 315 && angle > 180){
                return ((float)22.5, 15);
            }//if
        } else {//右向き
            if(angle < 135 && angle > 0) {//下向きの補正
                return ((float)22.5, 15);
            }else if(angle > 225 && angle < 360) {
                return ((float)22.5, 15);
            }//if
        }//if
        return (ballMoveX,ballMoveY);
    }//AngleCorrection

    private void FixedUpdate() {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(_ballMoveX, _ballMoveY);//
    }//FixedUpdate

    void Update() {
        GenerateDamageObjDestroy();
    }//Update

}//BossEnemy1FireBall
