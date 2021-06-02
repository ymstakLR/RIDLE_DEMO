using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボス戦中のステージ移動範囲変更用処理
/// 更新日時:0602
/// </summary>
public class BossEnemyStageEdgeGeneration : StageEdgeBase {

    private BossEnemyManager _bigEnemyManager;
    private StageStatusManagement _stageClearManagement;

    private new void Awake() {
        _bigEnemyManager = this.GetComponent<BossEnemyManager>();
        _stageClearManagement = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
        _stageEdgeName = "BigEnemyStageEdge";
        base.Awake();
        EdgeGenerationCollect();
    }//Awake

    private void Update() {
        if (!_bigEnemyManager.IsAppearanceEnd)
            return;
        switch (_stageClearManagement.StageStatus) {
            case EnumStageStatus.BeforeBossBattle:
                StageEndPosUpdate(rightEndPos, leftEndPos, raiseEndPos, lowerEndPos, new Vector2(0, 5), true);
                break;
            case EnumStageStatus.AfterBossBattle:
                Destroy(_stageEdge);
                break;
        }//switch
    }//Update

}//BigEnemyStageEdgeGeneration
