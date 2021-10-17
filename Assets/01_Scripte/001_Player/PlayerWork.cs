using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機の移動処理
/// 更新日時:0603
/// </summary>
public class PlayerWork : MonoBehaviour {
    private PlayerAnimator _pAnimator;
    private PlayerBody _pBody;
    private PlayerBodyForward _pBodyForward;
    private PlayerJump _pJump;
    private PlayerUnderTrigger _pUnderTrigger;

    private readonly int MAX_MOVING_VALUE = 250;//移動値の最大
    private readonly int MOVING_VALUE = 7;//移動量
    private readonly int STOP_VALUE = 10;//キー入力してないときの移動量
    private readonly int TURN_AROUND_MOVING_VALUE = 100;//振り向き時の移動量

    private Transform _insideTF;
    private Transform _outsideTF;

    private bool _isMovingSpeedInversion;//重力を変更するためのフラグ　trueで変更可能となる

    private void Start() {
        _pAnimator = this.GetComponent<PlayerAnimator>();
        _pBody = transform.Find("Body").GetComponent<PlayerBody>();
        _pBodyForward = transform.Find("BodyForward").GetComponent<PlayerBodyForward>();
        _pJump = this.GetComponent<PlayerJump>();
        _pUnderTrigger = transform.Find("UnderTrigger").GetComponent<PlayerUnderTrigger>();
        _insideTF = this.transform.Find("Point/InsideWorkPoint");
        _outsideTF = this.transform.Find("Point/OutsideWorkPoint");
        _isMovingSpeedInversion = true;//宣言時に初期化できるがコードを見やすくするために分割した
    }//Start

    /// <summary>
    /// ・プレイヤーの移動量を変更する処理
    /// ・PlayerManagerクラスで使用する
    /// </summary>
    /// <param name="movingSpeed">プレイヤーの現在の移動量</param>
    /// <returns>プレイヤーの変更後の移動量</returns>
    public int MoveWork(int movingSpeed) {//PlayerManagerで使用する
        movingSpeed = GravityChange(movingSpeed);
        float movingDirection = OperationKeyChange();

        movingSpeed = FlipValueUpdate(movingSpeed);

        //FlipJump状態が解除される場合
        if (_pJump.SideGravityFlipTimer < _pJump.SIDE_GRAVITY_FLIP_TIME &&
            _pJump.SideGravityFlipTimer < _pJump.SIDE_GRAVITY_FLIP_TIME * 2 &&
            (this.transform.localEulerAngles.z == 90 ||this.transform.localEulerAngles.z == 270)){
            if (this.transform.localScale.x > 0)
                return MAX_MOVING_VALUE;
            else
                return -MAX_MOVING_VALUE;
        }//if
        if (!_pJump.IsJump) {
            DirectionChange(movingSpeed);
        }//if
        if (movingDirection > 0 || movingDirection < 0) {//移動する場合
            return MoveUpdate(movingSpeed, movingDirection);
        }//if
        return Stopping(movingSpeed);
    }//MoveWork

    /// <summary>
    /// FlipJump中の移動量の更新処理
    /// </summary>
    /// <param name="movingSpeed">自機の現在の移動量</param>
    public int FlipValueUpdate(int movingSpeed) {
        ///下に落下している状態でFlipJumpが解除されたとき
        if (!_pJump.IsWorkSpeedFlip)
            return movingSpeed;
        _pJump.IsWorkSpeedFlip = false;
        if (_pUnderTrigger.IsUnderTrigger)
            return movingSpeed;
        return -movingSpeed;
    }//FlipValueUpdate

    /// <summary>
    /// ・プレイヤーにかかる重力を変更する処理
    /// ・MoveWorkメソッドで使用する
    /// ・この処理は一回しか使用しないがコードの見やすさを優先してメソッド化した(0918)
    /// </summary>
    /// <param name="movingSpeed">プレイヤーの現在の移動量</param>
    /// <returns>プレイヤーの移動量</returns>
    private int GravityChange(int movingSpeed) {
        //下重力以外の時にジャンプボタンを押したとき
        if (_isMovingSpeedInversion) {
            if(_pJump.JumpTypeFlag == EnumJumpTypeFlag.wallFlipFall) {
                movingSpeed = -movingSpeed;
            }//if
            if (_pJump.JumpTypeFlag == EnumJumpTypeFlag.wallFall && !_pAnimator.AniFall) {
                movingSpeed = -movingSpeed;
            }//if
            _isMovingSpeedInversion = false;
        }//if            

        //床に触れたとき
        if (_pUnderTrigger.IsUnderTrigger) {
            _isMovingSpeedInversion = true;
        }//if
        return movingSpeed;
    }//GravityChange

    /// <summary>
    /// プレイヤーの向きを変更する処理
    /// </summary>
    /// <param name="movingDirection">変更後のプレイヤーの向き</param>
    private void DirectionChange(float movingDirection) {//向き変更用処理
        if (movingDirection == 0)
            return;
        if (movingDirection > 0) {
            this.transform.localScale = new Vector2(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y);
        } else {
            this.transform.localScale = new Vector2(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y);
        }//if
    }//DirectionChange

    /// <summary>
    /// プレイヤーの移動量の更新
    /// MoveWorkメソッドで使用する
    /// </summary>
    /// <param name="movingSpeed">現在のプレイヤーの移動量</param>
    /// <param name="movingDirection">現在のプレイヤーの向き</param>
    /// <returns></returns>
    private int MoveUpdate(int movingSpeed,float movingDirection) {//移動値反映を行う
        float changeMovingValue = MAX_MOVING_VALUE * movingDirection;
        if (movingDirection > 0 && movingSpeed < changeMovingValue) {//右移動の場合
            if (movingSpeed < 0) {
                movingSpeed = TURN_AROUND_MOVING_VALUE;
            } else {
                movingSpeed += MOVING_VALUE;
            }//if
            if (movingSpeed > changeMovingValue) {
                movingSpeed = (int)changeMovingValue;
            }//if
            return movingSpeed;
        }//if
        if (movingDirection < 0 && movingSpeed > changeMovingValue) {//左移動の場合
            if(movingSpeed > 0) {
                movingSpeed = -TURN_AROUND_MOVING_VALUE;
            } else {
                movingSpeed -= MOVING_VALUE;
            }//if
            if (movingSpeed < changeMovingValue) {
                movingSpeed = (int)changeMovingValue;
            }//if
            return movingSpeed;
        }//if
        return movingSpeed;//値変更なし
    }//MoveUpdate

    /// <summary>
    /// プレイヤーの移動量を減少させる処理
    /// MoveWorkメソッドで使用する
    /// </summary>
    /// <param name="movingSpeed">現在のプレイヤーの移動量</param>
    /// <returns>変更後のプレイヤーの移動量</returns>
    private int Stopping(int movingSpeed) {//停止値反映を行う
        //自機が移動ボタンで動いていないとき
        if (-MOVING_VALUE < movingSpeed && movingSpeed < MOVING_VALUE )
            return 0;
        if (movingSpeed > 0) {
            if(movingSpeed - STOP_VALUE < 0)
                return 0;
            return movingSpeed - STOP_VALUE;
        }//if
        if (movingSpeed < 0) {
            if (movingSpeed + STOP_VALUE > 0)
                return 0;
            return movingSpeed + STOP_VALUE;
        }//if
        return 0;//到達不可能コード
    }//Stopping

    /// <summary>
    /// ・プレイヤーの移動するための入力キーを変更
    /// ・MoveWork,RightAngleWorkメソッドで使用する
    /// </summary>
    /// <returns>対応する入力キー</returns>
    private float OperationKeyChange() {
        if(_pJump.IsFlipJumpFall)//左右重力でFlipJumpをして落下する場合
            return Input.GetAxisRaw("Horizontal");
        switch (this.transform.localEulerAngles.z) {
            case 0:
                return Input.GetAxisRaw("Horizontal");

            case 90:
                return Input.GetAxisRaw("Vertical");
            case 180:
                return -Input.GetAxisRaw("Horizontal");
            case 270:
                return -Input.GetAxisRaw("Vertical");
        }//switch
        return 0;//到達不可コード
    }//OperationKeyChange


    /// <summary>
    /// 直角移動(内側移動と外側移動)の処理
    /// PlayerManagerで使用する
    /// </summary>
    /// <param name="movingSpeed">プレイヤーの現在の移動スピードを取得</param>
    public void RightAngleWork(float movingSpeed) {
        if (_pJump.IsJump || _pUnderTrigger.IsGimmickJump) {
            return;
        }//if
        float movingDirection = OperationKeyChange();
        InsideWork(movingSpeed,movingDirection);
        OutsideWork();
        _pAnimator.AniFall = false;
    }//RightAngleWork

    /// <summary>
    /// 内側移動の処理
    /// RightAngleWorkメソッドで使用
    /// </summary>
    /// <param name="movingSpeed">プレイヤーの現在の移動スピードを取得している</param>
    /// <param name="movingDirection">プレイヤーの向き(右向きか左向きか)取得</param>
    private void InsideWork(float movingSpeed,float movingDirection) {
        //以下の条件式3つは一つにまとめれるが条件式の内容がわかりにくくなるので分割した
        if (!_pBodyForward.IsBodyForward || !_pUnderTrigger.IsUnderTrigger)
            return;
        if ((movingSpeed < 0 && this.transform.localScale.x > 0) ||
            (movingSpeed > 0 && this.transform.localScale.x < 0)) 
            return;
        if (Mathf.Abs(movingSpeed) < MOVING_VALUE * 5) 
            return;
        this.transform.position = new Vector2(_insideTF.position.x, _insideTF.position.y);
        if (movingSpeed > 0) {//右向きの場合
            _pJump.RotationChange(this.transform.localEulerAngles.z + 90);
        } else {//左向きの場合
            _pJump.RotationChange(this.transform.localEulerAngles.z + 270);
        }//if
        _pBodyForward.IsBodyForward = false;
        _pBody.IsBody = BodyType.stage;
    }//InsideWork

    /// <summary>
    /// 外側移動の処理
    /// RightAngleWorkメソッドで使用
    /// </summary>
    private void OutsideWork() {
        if (_pUnderTrigger.IsUnderTrigger || 
            _pBody.IsBody != BodyType.stage)
            return;
        _pUnderTrigger.IsUnderTrigger = true;
        this.transform.position = new Vector2(_outsideTF.position.x, _outsideTF.position.y);
        if (this.transform.localScale.x > 0) {//右向きの場合
            _pJump.RotationChange(this.transform.localEulerAngles.z + 270);
        } else {//左向きの場合
            _pJump.RotationChange(this.transform.localEulerAngles.z + 90);
        }//if
    }//OutsideWork

    /// <summary>
    /// ステージクリア状態でゴールに触れたときの処理
    /// </summary>
    /// <param name="playerTransform">自機のTransform</param>
    /// <param name="playerGoalPosition">ゴールオブジェクトの扉前のTransform</param>
    /// <returns></returns>
    public (int,EnumStageStatus) GoalMoveWork(Transform playerTransform,Vector3 playerGoalPosition,EnumStageStatus enumStageStatus) {
        float intervalX = Mathf.Abs(playerTransform.position.x - playerGoalPosition.x);
        float intervalY = Mathf.Abs(playerTransform.position.y - playerGoalPosition.y);

        if (intervalX < 1 && intervalX > -1) {//移動する必要がない時
            if (playerTransform.localScale.x < 0) { 
                playerTransform.localScale = new Vector2(-playerTransform.localScale.x, playerTransform.localScale.y);
            }//if
            if (intervalY < 1 && intervalY > -1) {
                return (0, EnumStageStatus.ClearCriteria);
            }//if
            return (0, enumStageStatus);
        }//if

        //ここのif文は一つで十分だが視覚的に見やすくなるので二つにした
        if(playerTransform.position.x < playerGoalPosition.x) {//右移動
            this.transform.localScale = new Vector2(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y);
            return (250,enumStageStatus);
        }//if
        if(playerTransform.position.x > playerGoalPosition.x) {//左移動
            this.transform.localScale = new Vector2(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y);
            return (-250, enumStageStatus);
        }//if
        return (0, enumStageStatus);//到達不可能コード
    }//GoalMoveWork

}//Work
