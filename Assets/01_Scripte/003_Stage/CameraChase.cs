using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// カメラを追跡させる処理
/// 更新日時:0414
/// </summary>
public class CameraChase : MonoBehaviour {

    [SerializeField,Tooltip("移動させたい背景数")]
    public BackGroundArray[] backGroundArray;

    private StageStatusManagement _stageClearManagement;
    private Transform _playerTF;

    public Vector2 StageCorrectionPos { get; set; }//カメラの描画範囲の補正値

    private float _stageRightEndPos;
    public float StageRightEndPos { set { _stageRightEndPos = value; } }
    private float _stageLeftEndPos;
    public float StageLeftEndPos { set { _stageLeftEndPos = value; } }
    private float _stageRaiseEndPos;
    public float StageRaiseEndPos { set { _stageRaiseEndPos = value; } }
    private float _stageLowerEndPos;
    public float StageLowerEndPos { set { _stageLowerEndPos = value; } }

    private float _bossStageRightEndPos;
    public float BossStageRightEndPos { set { _bossStageRightEndPos = value; } }
    private float _bossStageLeftEndPos;
    public float BossStageLeftEndPos { set { _bossStageLeftEndPos = value; } }
    private float _bossStageRaiseEndPos;
    public float BossStageRaiseEndPos { set { _bossStageRaiseEndPos = value; } }
    private float _bossStageLowerEndPos;
    public float BossStageLowerEndPos { set { _bossStageLowerEndPos = value; } }

    private float _playerPositionX;
    private float _playerPositionY;
    private float _chaseCorrection;//カメラを追跡させるためのスピード補正

    public bool IsBossBattle { get; set; }

    private void Awake() {
        _stageClearManagement = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
        _playerTF = GameObject.Find("Ridle").GetComponent<Transform>();
        _playerPositionX = _playerTF.transform.position.x;
        _playerPositionY = _playerTF.transform.position.y;
        this.transform.position = new Vector3(_playerPositionX, _playerPositionY, this.transform.position.z);
        BackGroundMove();
    }//Start

    private void FixedUpdate() {
        if (IsBossBattle) {
            StageEdgeSpecified(_bossStageLeftEndPos, _bossStageRightEndPos, _bossStageLowerEndPos, _bossStageRaiseEndPos, true);
        } else {
            StageEdgeSpecified(_stageLeftEndPos, _stageRightEndPos, _stageLowerEndPos, _stageRaiseEndPos, false);
        }//if
        BackGroundMove();
    }//FixedUpdate

    void Update() {
        StageStatusReflect();
    }//Update

    /// <summary>
    /// ステージ状態ごとの処理
    /// </summary>
    private void StageStatusReflect() {
        switch (_stageClearManagement.StageStatus) {
            case EnumStageStatus.BeforeBossBattle:
            case EnumStageStatus.BossBattle:
            case EnumStageStatus.AfterBossBattle:
                _chaseCorrection = (float)0.1;//10　元の値
                bool posX =
                    this.transform.position.x < (_playerTF.position.x + StageCorrectionPos.x + 0.1) &&
                    this.transform.position.x > (_playerTF.position.x + StageCorrectionPos.x - 0.1);
                bool
                    posY =
                    this.transform.position.y < (_playerTF.position.y + StageCorrectionPos.y + 0.1) &&
                    this.transform.position.y > (_playerTF.position.y + StageCorrectionPos.y - 0.1);
                if (posX && posY && IsBossBattle) {
                    _stageClearManagement.StageStatus = EnumStageStatus.BossBattle;
                }//if
                break;
            default:
                IsBossBattle = false;
                _chaseCorrection = 1;
                break;
        }//switch
    }//StageStatusReflect

    /// <summary>
    /// ステージ端まで移動したときの処理
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="lower"></param>
    /// <param name="raise"></param>
    /// <param name="isCorrectionPos"></param>
    private void StageEdgeSpecified(float left,float right,float lower,float raise,bool isCorrectionPos) {
        if (_playerTF.transform.position.x > left &&
            _playerTF.transform.position.x < right) {
            if (isCorrectionPos) {
                _playerPositionX = PositionAssignment(this.transform.position.x, _playerTF.position.x + StageCorrectionPos.x);
            } else { 
                _playerPositionX = PositionAssignment(this.transform.position.x, _playerTF.position.x);
            }//if
        }//if
        if (_playerTF.transform.position.y > lower &&
            _playerTF.transform.position.y < raise) {
            if (isCorrectionPos) {
                _playerPositionY = PositionAssignment(this.transform.position.y, _playerTF.position.y + StageCorrectionPos.y);
            } else {
                _playerPositionY = PositionAssignment(this.transform.position.y, _playerTF.position.y);
            }//if   
        }//if
        this.transform.position = new Vector3(_playerPositionX, _playerPositionY, this.transform.position.z);
    }//StageEdgeSpecified


    /// <summary>
    /// 各座標値の代入処理
    /// </summary>
    /// <param name="thisTFP"></param>
    /// <param name="playerTFP"></param>
    /// <returns></returns>
    private float PositionAssignment(float thisTFP,float playerTFP) {
        if (thisTFP < (playerTFP + 1) && thisTFP > (playerTFP - 1)) {
            return playerTFP;
        }//if
        if (thisTFP < playerTFP) {
            return thisTFP + _chaseCorrection;
        } else {
            return thisTFP - _chaseCorrection;
        }//if
    }//PositionAssignment

    /// <summary>
    /// 背景の移動値設定と反映
    /// </summary>
    private void BackGroundMove() {
        foreach (BackGroundArray array in backGroundArray) {
            array.RectTransform.offsetMax = new Vector2(
                50 - (_playerPositionX / array.MovingX),
                50 - (_playerPositionY / array.MovingY));
            array.RectTransform.offsetMin = new Vector2(
                -50 - (_playerPositionX / array.MovingX),
                -50 - (_playerPositionY / array.MovingY));
        }//foreace
    }//BackGroundMove

}//CameraChase


/// <summary>
/// 多次元配列用のクラス(CamearChaseでしか使わない予定なので同じクラスに記述する(0227))
/// </summary>
[System.Serializable]
public class BackGroundArray {
    [SerializeField,Tooltip("移動させる背景")]
    private RectTransform rectTransform;
    public RectTransform RectTransform { get { return rectTransform; } }
    [SerializeField, Tooltip("X軸に移動させる補正値")]
    private float movingX;
    public float MovingX { get { return movingX; } }
    [SerializeField,Tooltip("Y軸に移動させる補正値")]
    private float movingY;
    public float MovingY { get { return movingY; } }
}//BackGroundArray
