using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[Serializable]
public struct StageDataStruct {
    public List<string> nameList;
    public List<string> rankList;
    public List<string> scoreList;
    public List<string> timeList;
}//StageData

[Serializable]
public struct OptionDataStruct {
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

[Serializable]
public struct InputData {
    public List<string> nameList;
    public List<string> negativeButtonList;
    public List<string> positiveButtonList;
    public List<string> altPositionButtonList;
    public List<string> invertList;
    public List<string> typeList;
    public List<string> axisList;
}

/// <summary>
/// ゲームで使用するデータの管理処理
/// 更新日時:0727
/// </summary>
public static class SaveManager {
    const string SAVE_DATA_PATH = "/Assets/05_SaveData/";
    const int STAGE_NUM = 3;

    const string STAGE_FILE ="stageData.json";
    public static StageDataStruct stageData;

    const string OPTION_FILE ="optionData.json";
    public static OptionDataStruct optionData;

    const string UNLOCK_FILE ="unlockData.json";
    public static UnlockData unlockData;

    const int AXES_SIZE_NUM = 10;
    const string INPUT_FILE = "inputData.json";
    public static InputData inputData;

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
        DataSave(jsonData, STAGE_FILE);
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
        DataSave(jsonData, OPTION_FILE);
    }//OptionDataUpdate

    /// <summary>
    /// アンロックデータの更新処理
    /// </summary>
    /// <param name="list">データ更新するリスト</param>
    public static void UnlockDataUpdate(List<bool> list) {
        unlockData.unlockList = list;
        string jsonData = JsonUtility.ToJson(unlockData, true);
        DataSave(jsonData, UNLOCK_FILE);
    }//UnlockDataUpdate

    public static void InputDataUpdate(List<List<string>> list) {
        inputData.nameList = list[0];
        inputData.negativeButtonList = list[1];
        inputData.positiveButtonList = list[2];
        inputData.altPositionButtonList = list[3];
        inputData.invertList = list[4];
        inputData.typeList = list[5];
        inputData.axisList = list[6];
        string jsonData = JsonUtility.ToJson(inputData, true);
        DataSave(jsonData, INPUT_FILE);
        Debug.Log("SaveManager.InputDataUpdate");
    }

    /// <summary>
    /// データの保存処理
    /// </summary>
    /// <param name="json">保存するjsonData</param>
    /// <param name="filePath">保存するファイルパス</param>
    private static void DataSave(string json, string filePath) {
        string path = SaveFilePathSetting();
        path = path + filePath;
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
    }//DataSave

    /// <summary>
    /// ステージデータを削除する処理
    /// </summary>
    public static void StageDataDelete() {
        File.Delete(SaveFilePathSetting() + STAGE_FILE);
        StageDataGenerate();
    }//StageDataDelete

    /// <summary>
    /// アンロックデータを削除する処理
    /// </summary>
    public static void UnlockDataDelete() {
        File.Delete(SaveFilePathSetting() + UNLOCK_FILE);
        UnlockDataGenerate();
    }//UnlockDataDelete


    /// <summary>
    /// 保存データの読み込み処理
    /// </summary>
    public static void DataInit() {
        string path = SaveFilePathSetting();
        if (File.Exists(path + STAGE_FILE)) {//指定のファイルが存在する場合
            DataLoad(STAGE_FILE);
        } else {
            StageDataGenerate();
        }//if
        if (File.Exists(path + OPTION_FILE)) {
            DataLoad(OPTION_FILE);
        } else {
            OptionDataGenerate();
        }//if
        if (File.Exists(path + UNLOCK_FILE)) {
            DataLoad(UNLOCK_FILE);
        } else {
            UnlockDataGenerate();
        }//if
        if (File.Exists(path + INPUT_FILE)) {
            DataLoad(INPUT_FILE);
        } else {
            InputDataGenerate();
        }//if
        
    }//DataInit

    /// <summary>
    /// データ読み込み処理
    /// </summary>
    /// <param name="dataPath">読み込むデータファイルのパス情報</param>
    private static void DataLoad(string loadFilePath) {
        FileInfo info = new FileInfo(SaveFilePathSetting()+loadFilePath);
        StreamReader reader = new StreamReader(info.OpenRead());
        string json = reader.ReadToEnd();
        switch (loadFilePath) {
            case STAGE_FILE:
                stageData = JsonUtility.FromJson<StageDataStruct>(json);
                break;
            case OPTION_FILE:
                optionData = JsonUtility.FromJson<OptionDataStruct>(json);
                break;
            case UNLOCK_FILE:
                unlockData = JsonUtility.FromJson<UnlockData>(json);
                break;
            case INPUT_FILE:
                inputData = JsonUtility.FromJson<InputData>(json);
                break;
        }//switch
        reader.Close();
    }//DataLoad

    /// <summary>
    /// ステージデータの初期化処理
    /// </summary>
    /// 
    private static void StageDataGenerate() {
        stageData = new StageDataStruct();
        List<string> nList = new List<string>();
        List<string> rList = new List<string>();
        List<string> sList = new List<string>();
        List<string> tList = new List<string>();
        for (int i = 0; i <= STAGE_NUM; i++) {
            nList.Insert(i, "Stage" + i);
            rList.Insert(i, "E");
            sList.Insert(i, "0");
            tList.Insert(i, "09:59");
        }//for
        stageData.nameList = nList;
        stageData.rankList = rList;
        stageData.scoreList = sList;
        stageData.timeList = tList;
        string jsonData = JsonUtility.ToJson(stageData, true);
        DataSave(jsonData, STAGE_FILE);
    }//StageDataInit

    /// <summary>
    /// オプションデータの初期化処理
    /// </summary>
    /// 
    private static void OptionDataGenerate() {
        optionData = new OptionDataStruct();
        optionData.bgmVol = 0;
        optionData.seVol = 0;
        optionData.resolutionH = 1980;
        optionData.resolutionW = 1080;
        optionData.isFullscreen = true;
        string jsonData = JsonUtility.ToJson(optionData, true);
        DataSave(jsonData, OPTION_FILE);
    }//OptionDataInit

    /// <summary>
    /// アンロックデータの初期化処理
    /// </summary>
    private static void UnlockDataGenerate() {
        unlockData = new UnlockData();
        List<bool> list = new List<bool>();
        for(int i = 0; i < STAGE_NUM * 4; i++) {
            list.Insert(i, false);
        }
        unlockData.unlockList = list;
        string jsonData = JsonUtility.ToJson(unlockData, true);
        DataSave(jsonData, UNLOCK_FILE);
    }//UnlockDataGenerate


    private static void InputDataGenerate() {
        inputData = new InputData();
        List<string> nameList = new List<string>();
        List<string> nButtonList = new List<string>();
        List<string> pButtonList = new List<string>();
        List<string> apButtonList = new List<string>();
        List<string> invertList = new List<string>();
        List<string> typeList = new List<string>();
        List<string> axisList = new List<string>();
        for(int i = 0; i < AXES_SIZE_NUM; i++) {
            InputManagerInfo.DefaultNameInsert(nameList, i);
            InputManagerInfo.DefaultNegativeButtonInsert(nButtonList, i);
            InputManagerInfo.DefaultPositiveButtonInsert(pButtonList, i);
            InputManagerInfo.DefaultAltPositiveButtonInsert(apButtonList, i);
            InputManagerInfo.DefaultInvert(invertList, i);
            InputManagerInfo.DefaultType(typeList, i);
            InputManagerInfo.DefaultAxis(axisList, i);
        }//for
        inputData.nameList = nameList;
        inputData.negativeButtonList = nButtonList;
        inputData.positiveButtonList = pButtonList;
        inputData.altPositionButtonList = apButtonList;
        inputData.invertList = invertList;
        inputData.typeList = typeList;
        inputData.axisList = axisList;
        string jsonData = JsonUtility.ToJson(inputData, true);
        DataSave(jsonData, INPUT_FILE);
    }//InputDataGenerate


    /// <summary>
    /// ファイルを保存するパスを設定する処理
    /// </summary>
    /// <returns></returns>
    private static string SaveFilePathSetting() {
        string path="";
        #if UNITY_EDITOR
            path = Directory.GetCurrentDirectory()+ SAVE_DATA_PATH;
        #else
            path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');    
        #endif
        return path;
    }//SaveFilePathSetting

}//SaveManager
