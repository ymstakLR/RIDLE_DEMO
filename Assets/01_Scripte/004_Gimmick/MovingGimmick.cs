using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動するギミック処理
/// 更新日時:0416
/// </summary>
public class MovingGimmick : MonoBehaviour {
    [SerializeField]
    private float X_POSITION_MAX;
    [SerializeField]
    private float X_POSITION_MIN;
    [SerializeField]
    private float Y_POSITION_MAX;
    [SerializeField]
    private float Y_POSITION_MIN;
    [SerializeField]
    private float MOVING_SPEED;

    [Tooltip("初期場所の設定(主に複数の移動床で移動ルートを作るときに使用する)")]
    [SerializeField]
    private bool isFromThePositionMIN;

    private float _xMovingSpeed;
    private float _yMovingSpeed;

    private bool _isMaxLateralMove;
    private bool _isMaxVerticalMove;

    protected GameObject _movingObject;

    protected float _xPositionNow;
    protected float _yPositionNow;

    public void Start() {
        if (isFromThePositionMIN) {
            _xPositionNow = X_POSITION_MIN;
            _yPositionNow = Y_POSITION_MIN;
        } else {
            _xPositionNow = X_POSITION_MAX;
            _yPositionNow = Y_POSITION_MAX;
        }//if

        if ((X_POSITION_MIN == 0 && X_POSITION_MAX == 0) || 
            (Y_POSITION_MIN == 0 && Y_POSITION_MAX == 0)) {
            _xMovingSpeed = MOVING_SPEED;
            _yMovingSpeed = MOVING_SPEED;
            if(X_POSITION_MIN == 0 && X_POSITION_MAX == 0) {
                _xPositionNow = _movingObject.transform.position.x;
                _movingObject.transform.position = new Vector2(_xPositionNow,_yPositionNow);
            } else {
                _yPositionNow = _movingObject.transform.position.y;
                _movingObject.transform.position = new Vector2(_xPositionNow,_yPositionNow);
            }//if
            return;
        }//if

        if (Mathf.Abs(X_POSITION_MAX - X_POSITION_MIN) <= Mathf.Abs(Y_POSITION_MAX - Y_POSITION_MIN)) {
            _xMovingSpeed = MOVING_SPEED;
            _yMovingSpeed = (Mathf.Abs(Y_POSITION_MAX - Y_POSITION_MIN) / Mathf.Abs(X_POSITION_MAX - X_POSITION_MIN)) * MOVING_SPEED;
        } else {//Yが小さい
            _xMovingSpeed = (Mathf.Abs(X_POSITION_MAX - X_POSITION_MIN) / Mathf.Abs(Y_POSITION_MAX - Y_POSITION_MIN)) * MOVING_SPEED;
            _yMovingSpeed = MOVING_SPEED;
        }//if
        _movingObject.transform.position = new Vector2(_xPositionNow,_yPositionNow);
    }//Start

    /// <summary>
    /// 指定オブジェクトを移動させる処理
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="xPositionNow"></param>
    /// <param name="yPositionNow"></param>
    /// <returns></returns>
    public (float,float) Move(GameObject gameObject,float xPositionNow,float yPositionNow) {
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -10));
        if (X_POSITION_MIN != 0 || X_POSITION_MAX != 0) {
            xPositionNow = AmountToMove(ref xPositionNow, X_POSITION_MIN, X_POSITION_MAX, _xMovingSpeed, ref _isMaxVerticalMove);
        }//if
        if (Y_POSITION_MIN != 0 || Y_POSITION_MAX != 0) {
            yPositionNow = AmountToMove(ref yPositionNow, Y_POSITION_MIN, Y_POSITION_MAX, _yMovingSpeed, ref _isMaxLateralMove);
        }//if
        gameObject.GetComponent<Transform>().position = new Vector2(xPositionNow, yPositionNow);
        return (xPositionNow, yPositionNow);
    }//MovingFloorMain

    /// <summary>
    /// 移動量を取得する
    /// </summary>
    /// <param name="positionNow"></param>
    /// <param name="positionMin"></param>
    /// <param name="positionMax"></param>
    /// <param name="movingSpeed"></param>
    /// <param name="isMaxMove"></param>
    /// <returns></returns>
    private float AmountToMove(
       ref float positionNow, float positionMin,
       float positionMax, float movingSpeed, ref bool isMaxMove) {
        if (positionMin < positionNow && isMaxMove) {
            positionNow -= movingSpeed;
        } else {
            isMaxMove = false;
        }//if   
        if (positionNow < positionMax && !isMaxMove) {
            positionNow += movingSpeed;
        } else {
            isMaxMove = true;
        }//if
        return positionNow;
    }//AmountToMove

}//MovingGimmick
