using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ステージデータの情報を表示させるための処理(ステージ選択画面で使用する)
/// 更新日時:20210819
/// </summary>
public class StageDataDisp : MonoBehaviour {

    private void Start() {
        StageDataSearch();
    }//Start

    /// <summary>
    /// ステージデータ情報を探す処理
    /// </summary>
    private void StageDataSearch() {
        int stageCount = 0;//ステージ番号 1ずつ増やしていき該当するデータ情報を更新する
        foreach(Transform stageNum in this.transform) {
            stageCount++;
            foreach(Transform stageData in stageNum.transform) {
                StageDataUpdate(stageData, stageCount);
            }//foreach
        }//foreach
    }//StageDataSearch

    /// <summary>
    /// ステージデータを更新する
    /// </summary>
    /// <param name="stageData">更新するステージデータ</param>
    /// <param name="stageCount">更新するステージの</param>
    private void StageDataUpdate(Transform stageData,int stageCount) {
        Text updateText = stageData.GetComponent<Text>();
        switch (stageData.ToString()) {
            case string str when str.Contains("Score"):
                updateText.text = "Score: " + int.Parse(SaveManager.stageDataStruct.scoreList[stageCount]).ToString("D5");
                break;
            case string str when str.Contains("Rank"):
                updateText.text = SaveManager.stageDataStruct.rankList[stageCount].ToString();
                break;
            case string str when str.Contains("Time"):
                updateText.text = " Time: " + SaveManager.stageDataStruct.timeList[stageCount].ToString();
                break;
        }//switch
    }//StageDataUpdate

}//StageDataDisp
