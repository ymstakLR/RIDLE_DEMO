using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

[Serializable]
public struct StageData {
    public List<string> nameList;
    public List<string> rankList;
    public List<string> scoreList;
    public List<string> timeList;
}//StageData

[Serializable]
public struct OptionData {
    public int bgmVol;
    public int seVol;
    public int resolutionH;
    public int resolutionW;
    public bool isFullscreen;
}//OptionData

[Serializable]
public struct UnlockData {
    public List<bool> unlockList;
}

/// <summary>
/// ゲームで使用するデータの管理処理
/// 更新日時:0720
/// </summary>
public static class SaveManager {
    const string SAVE_DATA_PATH = "/Assets/Test/";
    const int STAGE_NUM = 3;

    const string STAGE_FILE_PATH = SAVE_DATA_PATH+"stageData.json";
    public static StageData stageData;

    const string OPTION_FILE_PATH = SAVE_DATA_PATH+"optionData.json";
    public static OptionData optionData;

    const string UNLOCK_FILE_PATH = SAVE_DATA_PATH + "unlockData.json";
    public static UnlockData unlockData;

    /// <summary>
    /// ステージデータ更新処理
    /// </summary>
    /// <param name="list">データ更新するリスト</param>
    public static void StageDataUpdate(List<List<string>> list) {
        stageData.nameList = list[0];
        stageData.rankList = list[1];
        stageData.scoreList = list[2];
        stageData.timeList = list[3];

        string jsonData = JsonUtility.ToJson(stageData, true);//SaveData情報
        DataSave(jsonData, STAGE_FILE_PATH);
        Debug.Log("ステージデータの更新終了");
    }//StageDataUpdate

    /// <summary>
    /// オプションデータの更新処理
    /// </summary>
    /// <param name="arrayList">データ更新するリスト</param>
    public static void OptionDataUpdate(ArrayList arrayList) {
        optionData.bgmVol = (int)arrayList[0];
        optionData.seVol = (int)arrayList[1];
        optionData.resolutionH = (int)arrayList[2];
        optionData.resolutionW = (int)arrayList[3];
        optionData.isFullscreen = (bool)arrayList[4];

        string jsonData = JsonUtility.ToJson(optionData, true);
        DataSave(jsonData, OPTION_FILE_PATH);
        Debug.Log("オプションデータの更新終了");
    }//OptionDataUpdate

    public static void UnlockDataUpdate(List<bool> list) {
        unlockData.unlockList = list;
        string jsonData = JsonUtility.ToJson(unlockData, true);
        DataSave(jsonData, UNLOCK_FILE_PATH);
        Debug.Log("アンロックデータの更新終了");
    }

    /// <summary>
    /// データの保存処理
    /// </summary>
    /// <param name="json">保存するjsonData</param>
    /// <param name="filePath">保存するファイルパス</param>
    public static void DataSave(string json, string filePath) {
        string path = Directory.GetCurrentDirectory();//プロジェクトフォルダまでのパス
        path = path + filePath;
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
    }//DataSave

    /// <summary>
    /// 保存データの読み込み処理
    /// </summary>
    public static void DataInit() {
        string path = Directory.GetCurrentDirectory();
        if (File.Exists(path + STAGE_FILE_PATH)) {//指定のファイルが存在する場合
            Debug.Log("ステージデータの読み込み");
            DataLoad(STAGE_FILE_PATH);
        } else {
            StageDataGenerate();
        }//if
        if (File.Exists(path + OPTION_FILE_PATH)) {
            Debug.Log("オプションデータの読み込み");
            DataLoad(OPTION_FILE_PATH);
        } else {
            OptionDataGenerate();
        }//if
        if (File.Exists(path + UNLOCK_FILE_PATH)) {//指定のファイルが存在する場合
            Debug.Log("アンロックデータの読み込み");
            DataLoad(UNLOCK_FILE_PATH);
        } else {
            UnlockDataGenerate();
        }//if
    }//DataLoad

    /// <summary>
    /// データ読み込み処理
    /// </summary>
    /// <param name="dataPath">読み込むデータファイルのパス情報</param>
    public static void DataLoad(string loadFilePath) {
        FileInfo info = new FileInfo(Directory.GetCurrentDirectory()+loadFilePath);
        StreamReader reader = new StreamReader(info.OpenRead());
        string json = reader.ReadToEnd();
        switch (loadFilePath) {
            case STAGE_FILE_PATH:
                stageData = JsonUtility.FromJson<StageData>(json);
                break;
            case OPTION_FILE_PATH:
                optionData = JsonUtility.FromJson<OptionData>(json);
                break;
            case UNLOCK_FILE_PATH:
                unlockData = JsonUtility.FromJson<UnlockData>(json);
                break;
        }//switch
        reader.Close();
    }//DataLoad

    /// <summary>
    /// ステージデータの初期化処理
    /// </summary>
    /// 
    public static void StageDataGenerate() {
        Debug.Log("ステージデータの初期化");
        stageData = new StageData();
        List<string> nList = new List<string>();
        List<string> rList = new List<string>();
        List<string> sList = new List<string>();
        List<string> tList = new List<string>();
        for (int i = 0; i <= STAGE_NUM; i++) {
            nList.Insert(i, "Stage" + i);
            rList.Insert(i, "E");
            sList.Insert(i, "0");
            tList.Insert(i, "9.59");
        }//for
        stageData.nameList = nList;
        stageData.rankList = rList;
        stageData.scoreList = sList;
        stageData.timeList = tList;
        string jsonData = JsonUtility.ToJson(stageData, true);
        DataSave(jsonData, STAGE_FILE_PATH);
    }//StageDataInit

    /// <summary>
    /// オプションデータの初期化処理
    /// </summary>
    /// 
    public static void OptionDataGenerate() {
        Debug.Log("オプションデータの初期化");
        optionData = new OptionData();
        optionData.bgmVol = 5;
        optionData.seVol = 5;
        optionData.resolutionH = 1980;
        optionData.resolutionW = 1080;
        optionData.isFullscreen = true;
        string jsonData = JsonUtility.ToJson(optionData, true);
        DataSave(jsonData, OPTION_FILE_PATH);
    }//OptionDataInit

    public static void UnlockDataGenerate() {
        Debug.Log("アンロックデータの初期化");
        unlockData = new UnlockData();
        List<bool> list = new List<bool>();
        for(int i = 0; i < STAGE_NUM * 4; i++) {
            list.Insert(i, false);
        }
        unlockData.unlockList = list;
        string jsonData = JsonUtility.ToJson(unlockData, true);
        DataSave(jsonData, UNLOCK_FILE_PATH);
    }
}//SaveManager
