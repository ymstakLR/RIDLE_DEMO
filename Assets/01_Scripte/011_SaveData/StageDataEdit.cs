using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージデータを編集する処理
/// 更新日時:0728
/// </summary>
public static class StageDataEdit{
    public static List<List<string>> _stageData = new List<List<string>>();
    public static List<string> _nameList;
    public static List<string> _rankList;
    public static List<string> _scoreList;
    public static List<string> _timeList;

    /// <summary>
    /// ステージデータ読み込み処理
    /// </summary>
    private static void StageDataLoad() {
        _nameList = new List<string>();
        _nameList.AddRange(SaveManager.stageDataStruct.nameList);
        _rankList = new List<string>();
        _rankList.AddRange(SaveManager.stageDataStruct.rankList);
        _scoreList = new List<string>();
        _scoreList.AddRange(SaveManager.stageDataStruct.scoreList);
        _timeList = new List<string>();
        _timeList.AddRange(SaveManager.stageDataStruct.timeList);
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
    public static void StageDataUpdate(
        string stageName,string stageRank,
        string stageScore,string stageTime) {
        StageDataLoad();
        int stageNum = StageDataIdentification(stageName);
        _nameList[stageNum] = stageName;
        _rankList[stageNum] = stageRank;
        _scoreList[stageNum] = stageScore;
        _timeList[stageNum] = stageTime;
        StageDataSave();
    }//StageDataUpdate

    /// <summary>
    /// ステージデータを指定したステージ名と紐づける処理
    /// </summary>
    /// <param name="stageName">紐づけたいステージ名</param>
    /// <returns></returns>
    public static int StageDataIdentification(string stageName) {
        StageDataLoad();
        int stageNum = 0;
        while (_nameList[stageNum].ToString() != stageName) {
            stageNum++;
        }//While
        return stageNum;
    }//StageDataIdentification

}//StageDataEdit
