using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumStageStatus {//現在のステージ状態
    Pause,
    Normal,
    GoalMove,
    ClearCriteria,
    BeforeBossBattle,
    BossBattle,
    AfterBossBattle,
    Clear
}//EnumStageaStatus

/// <summary>
/// ステージ状態の管理処理
/// 更新日時:0616
/// </summary>
public class StageStatusManagement : MonoBehaviour {
    public EnumStageStatus StageStatus { get; set; }

    private List<GameObject> bossEnemyArray = new List<GameObject>();
    public List<GameObject> BossEnemyArray {
        get { return bossEnemyArray; }
        set { bossEnemyArray = value; }
    }//BossEnemyArray

    void Start() {
        Object[] allGameObject = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        // 取得したオブジェクトの名前を表示
        foreach (GameObject obj in allGameObject) {
            if(obj.name != "BossEnemyInfo")
                continue;
            foreach (Transform bossObject in obj.transform) {
                bossEnemyArray.Add(GameObject.Find(obj.name).transform.Find(bossObject.name).gameObject);
            }//foreach
        }//foreach
        //Debug.Log("ボス敵の残り数="+bossEnemyArray.Count);
    }//Start

    void Update() {
        if (StageStatus == EnumStageStatus.Normal)//ステージ状態が通常だったら
            return;
        if (StageStatus == EnumStageStatus.ClearCriteria){
            StatusBossBattleJudge();
        }//if
    }//Update

    /// <summary>
    /// ステージ状態をボス戦前に変更するかの判定処理
    /// </summary>
    private void StatusBossBattleJudge() {
        for(int i = 0; i < bossEnemyArray.Count; i++) {//ボス敵現在のボス敵数確認
            if (!bossEnemyArray[i].activeSelf) {//一番上のボス敵を表示させる
                bossEnemyArray[i].SetActive(true);
                bossEnemyArray[i].GetComponent<SpriteRenderer>().enabled = false;
                StageStatus = EnumStageStatus.BeforeBossBattle;
                GameObject.Find("GameManager").GetComponent<AudioManager>().PlaySE("GoalLock");
                return;
            }//if
        }//for
        //ボス敵数が0の場合
        StatusClearChange();
    }//StatusBossBattleJudge

    /// <summary>
    /// ステージ状態をクリアに変更する
    /// </summary>
    private void StatusClearChange() {
        StageStatus = EnumStageStatus.Clear;
        GameObject.Find("GameManager").GetComponent<AudioManager>().PlaySE("GoalRelease");
    }//StatusClearChange

}//StageStatusManagement
