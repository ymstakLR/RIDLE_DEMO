using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageDataEdit{
    public static List<List<string>> _stageData = new List<List<string>>();
    public static List<string> _nameList = new List<string>();
    public static List<string> _rankList = new List<string>();
    public static List<string> _scoreList = new List<string>();
    public static List<string> _timeList = new List<string>();


    /// <summary>
    /// ステージデータ読み込み処理
    /// </summary>
    private static void StageDataLoad() {
        _nameList.AddRange(SaveManager.stageData.nameList);
        _rankList.AddRange(SaveManager.stageData.rankList);
        _scoreList.AddRange(SaveManager.stageData.scoreList);
        _timeList.AddRange(SaveManager.stageData.timeList);
    }//StageDataLoad

    /// <summary>
    /// ステージデータ保存処理
    /// </summary>
    private static void StageDataSave() {
        _stageData.Insert(0, _nameList);
        _stageData.Insert(1, _rankList);
        _stageData.Insert(2, _scoreList);
        _stageData.Insert(3, _timeList);
        SaveManager.StageDataUpdate(_stageData);
    }//StageDataSave

    /// <summary>
    /// ステージデータの更新処理
    /// </summary>
    /// <param name="stageNum">更新するステージ番号</param>
    /// <param name="stageName">更新するステージ名</param>
    /// <param name="stageRank">更新するステージランク</param>
    /// <param name="stageScore">更新するステージスコア</param>
    /// <param name="stageTime">更新するステージタイム</param>
    private static void StageDataUpdate(int stageNum,
        string stageName,string stageRank,
        string stageScore,string stageTime) {
        StageDataLoad();
        _nameList[stageNum] = stageName;
        _rankList[stageNum] = stageRank;
        _scoreList[stageNum] = stageScore;
        _timeList[stageNum] = stageTime;
        StageDataSave();
    }//StageDataUpdate

}
