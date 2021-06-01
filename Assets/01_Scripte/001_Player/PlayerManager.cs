using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機の移動情報の取得更新を行う
/// 更新日時:0405
/// </summary>
public class PlayerManager : MonoBehaviour {
    private PlayerAnimator _pAnimator;
    private PlayerJump _pJump;
    private PlayerWork _pWork;
    private StageStatusManagement _stageClearMgmt;

    private Vector3 _goalPos;

    public int WorkPower { get; set; }//移動量
    public float JumpPower { get; set; }//ジャンプ量

    void Start() {
        _pAnimator = GetComponent<PlayerAnimator>();
        _pJump = GetComponent<PlayerJump>();
        _pWork = GetComponent<PlayerWork>();
        _stageClearMgmt = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
        _goalPos = GameObject.Find("Goal").transform.position;
        _goalPos = new Vector3(_goalPos.x - 8.25f, _goalPos.y-1 , _goalPos.z);
    }//Start

    private void FixedUpdate() {//ゲーム時間で一定時間ごとに呼ばれる
        MovePowerUpdate();
    }//FixedUpdate

    /// <summary>
    /// 自機の移動量の更新
    /// </summary>
    private void MovePowerUpdate() {
        //workSpeed/10は情報落ちするためこのように記述している(0914)ほかでこの値を使用する場合変数にする案も出てくる
        if (_pJump.IsFlipJumpFall) {//FlipJump中に落下するときの重力
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(WorkPower / 10, JumpPower / 10);
            return;
        }//if
        switch (this.transform.localEulerAngles.z) {//重力のかかっている向きで増加箇所を変更
            case 0:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(WorkPower / 10, JumpPower / 10);
                break;
            case 90:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(-JumpPower / 10, WorkPower / 10);
                break;
            case 180:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(-WorkPower / 10, -JumpPower / 10);
                break;
            case 270:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(JumpPower / 10, -WorkPower / 10);
                break;
        }//switch
    }//MovePowerUpdate

    void Update() {//毎フレーム呼ばれる
        _pAnimator.AnimatorMove(WorkPower);//アニメーション更新
        InputJudge();
    }//Update

    /// <summary>
    /// 入力可能か判定する処理
    /// </summary>
    private void InputJudge() {
        if (_pAnimator.AniMiss) {
            JumpPower = 0;
            WorkPower = 0;
            return;
        }//if
        if (Time.timeScale == 0) 
            return;
        PlayerInput();
    }//InputJudge

    /// <summary>
    /// 自機の入力情報反映処理
    /// </summary>
    private void PlayerInput() {
        if (_stageClearMgmt.StageStatus == EnumStageStatus.Normal ||
            _stageClearMgmt.StageStatus == EnumStageStatus.BossBattle) {//移動可能な場合
            JumpPower = _pJump.MoveJump(JumpPower);//キー入力処理があるのでUpdateに記述する(0914)
            WorkPower = _pWork.MoveWork(WorkPower);//ジャンプ後の変変数取得が必要になるのでJumpSpeed後に記述する（確認中）0502
            _pWork.RightAngleWork(WorkPower);//角度変更移動
        } else if (_stageClearMgmt.StageStatus == EnumStageStatus.GoalMove ||
            _stageClearMgmt.StageStatus == EnumStageStatus.ClearCriteria) {
            JumpPower = _pJump.MoveJump(JumpPower);
            (WorkPower, _stageClearMgmt.StageStatus) =
                _pWork.GoalMoveWork(this.GetComponent<Transform>(), _goalPos, _stageClearMgmt.StageStatus);
        } else {
            //停止処理
            JumpPower = _pJump.JumpStop(JumpPower);
            //JumpSpeed = jump.MoveJump(JumpSpeed);
            WorkPower = 0;
        }//if
    }//PlayerInput

}//PlayerManager
