using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ端を生成する処理
/// 更新日時:0415
/// </summary>
public class StageEdgeGeneration : StageEdgeBase {

    private new void Awake() {
        _stageEdgeName = "StageEdge";
        base.Awake();
        EdgeGenerationCollect();
        StageEndPosUpdate(rightEndPos, leftEndPos, raiseEndPos, lowerEndPos, new Vector2(0,0), false);
    }//Awake

}//StageEdgeGeneration
