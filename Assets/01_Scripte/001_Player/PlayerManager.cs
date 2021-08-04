using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機の移動情報の取得更新を行う
/// 更新日時:0616
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
        InputJudge();
        MovePowerUpdate();
    }//FixedUpdate

    private void Update() {
        _pAnimator.AnimatorMove(WorkPower);//アニメーション更新
        if (_stageClearMgmt.StageStatus == EnumStageStatus.Pause)
            return;
        _pJump.JumpButtonInput();
    }//Update

    /// <summary>
    /// 自機の移動量の更新
    /// </summary>
    private void MovePowerUpdate() {
        //workPower/10は情報落ちする
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
        switch (_stageClearMgmt.StageStatus) {
            case EnumStageStatus.Normal:
            case EnumStageStatus.BossBattle:
                //操作可能
                JumpPower = _pJump.MoveJump(JumpPower);
                WorkPower = _pWork.MoveWork(WorkPower);//ジャンプ後の変変数取得が必要になるのでJumpSpeed後に記述する
                _pWork.RightAngleWork(WorkPower);//角度変更移動
                break;
            case EnumStageStatus.GoalMove:
            case EnumStageStatus.ClearCriteria:
                //自動移動
                JumpPower = _pJump.MoveJump(JumpPower);
                (WorkPower, _stageClearMgmt.StageStatus) =
                    _pWork.GoalMoveWork(this.GetComponent<Transform>(), _goalPos, _stageClearMgmt.StageStatus);
                break;
            default:
                //停止
                JumpPower = _pJump.JumpStop(JumpPower);
                WorkPower = 0;
                break;
        }//switch
    }//PlayerInput

}//PlayerManager
