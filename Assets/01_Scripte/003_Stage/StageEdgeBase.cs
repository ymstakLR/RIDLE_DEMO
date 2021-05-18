using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ端に関するベースクラス
/// 更新日時:0415
/// </summary>
public class StageEdgeBase : MonoBehaviour {

    private enum EdgeType {//boolでも賄えそうだが見やすさ優先でenum型にした(1102)
        height,
        width
    }//EdgeType

    protected GameObject _stageEdge;
    protected CameraChase _cameraChase;

    protected string _stageEdgeName;

    [Tooltip("ステージ右端の値を入力")]
    [SerializeField]
    protected float rightEndPos;
    [Tooltip("ステージ左端の値を入力")]
    [SerializeField]
    protected float leftEndPos;
    [Tooltip("ステージ上端の値を入力")]
    [SerializeField]
    protected float raiseEndPos;
    [Tooltip("ステージ下端の値を入力")]
    [SerializeField]
    protected float lowerEndPos;
    public float LowerEndPos { get { return lowerEndPos; } }

    protected void Awake() {
        _stageEdge = new GameObject(_stageEdgeName);
        _cameraChase = GameObject.Find("Main Camera").GetComponent<CameraChase>();
    }//Awake

    /// <summary>
    /// ステージ端を新たに生成する処理
    /// </summary>
    protected void EdgeGenerationCollect() {
        EdgeGeneration("RightEdge", EdgeType.height, rightEndPos);
        EdgeGeneration("LeftEdge", EdgeType.height, leftEndPos);
        EdgeGeneration("RaiseEdge", EdgeType.width, raiseEndPos);
        EdgeGeneration("LowerEdge", EdgeType.width, lowerEndPos);
    }//EdgeGeneration

    /// <summary>
    /// 指定のステージ端を生成する
    /// </summary>
    /// <param name="gameObjectName">生成するオブジェクトの名前</param>
    /// <param name="edgeType">生成するステージ端のタイプ</param>
    /// <param name="generatePos">生成する場所</param>
    private void EdgeGeneration(string gameObjectName, EdgeType edgeType, float generatePos) {
        GameObject edgeObject = new GameObject(gameObjectName);
        if (edgeType == EdgeType.height) {
            edgeObject.transform.position = new Vector2(generatePos, 0);
            edgeObject.transform.localScale = new Vector2(1, 10000);
        } else {
            edgeObject.transform.position = new Vector2(0, generatePos);
            edgeObject.transform.localScale = new Vector2(10000, 1);
        }//if
        edgeObject.tag = "StageEdge";
        edgeObject.AddComponent<BoxCollider2D>();
        edgeObject.transform.parent = _stageEdge.transform;
    }//EdgeGeneration

    /// <summary>
    /// 反映するステージ端の更新
    /// </summary>
    /// <param name="right">右端の値</param>
    /// <param name="left">左端の値</param>
    /// <param name="raise">上端の値</param>
    /// <param name="lower">下端の値</param>
    /// <param name="cameraPosCorrection">カメラ位置の補正値</param>
    /// <param name="bossEdge">ボス戦のステージ範囲化の判定</param>
    protected void StageEndPosUpdate(float right, float left, float raise, float lower,Vector2 cameraPosCorrection,bool bossEdge) {
        _cameraChase.StageCorrectionPos = cameraPosCorrection;
        if (bossEdge) {
            _cameraChase.BossStageRightEndPos = right - 27;
            _cameraChase.BossStageLeftEndPos = left + 27;
            _cameraChase.BossStageRaiseEndPos = raise + 16;
            _cameraChase.BossStageLowerEndPos = lower - 16;
            return;
        }//if
        _cameraChase.StageRightEndPos = right - 27;
        _cameraChase.StageLeftEndPos = left + 27;
        _cameraChase.StageRaiseEndPos = raise -16;
        _cameraChase.StageLowerEndPos = lower +24;
    }//StageEndPosUpdate

}//StageEdgeBase
