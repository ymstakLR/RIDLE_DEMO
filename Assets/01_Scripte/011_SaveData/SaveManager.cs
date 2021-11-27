using LitJson;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ゲームで使用するデータの管理処理
/// 更新日時:20211004
/// </summary>
public static class SaveManager {
    //保存データパス情報
    const string SAVE_DATA_PATH = "/Assets/05_SaveData/";
    //ステージデータ用情報
    public struct StageDataStruct {
        public List<string> nameList;
        public List<string> rankList;
        public List<string> scoreList;
        public List<string> timeList;
    }//StageDataStruct
    public static StageDataStruct stageDataStruct;
    private const string STAGE_FILE = "stageData.dat";
    private const int STAGE_NUM = 3;
    //オプションデータ用情報
    public struct OptionDataStruct {
        public int bgmVol;
        public int seVol;
        public int resolutionH;
        public int resolutionW;
        public bool isFullscreen;
    }//OptionDataStruct
    public static OptionDataStruct optionDataStruct;
    private const string OPTION_FILE = "optionData.dat";
    //アンロックデータ用情報
    public struct UnlockDataStruct {
        public List<bool> unlockList;
    }//UnlockDataStruct
    public static UnlockDataStruct unlockDataStruct;
    private const string UNLOCK_FILE = "unlockData.dat";


    /// <summary>
    /// ファイルを保存するパスを設定する処理
    /// </summary>
    /// <returns>ファイルを保存するパス</returns>
    private static string SaveFilePathSetting() {
        string path = "";
        #if UNITY_EDITOR
            path = Directory.GetCurrentDirectory() + SAVE_DATA_PATH;
        #else
            path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')+"/"+Application.productName+"_Data/";  
        #endif
        return path;
    }//SaveFilePathSetting

    /// <summary>
    /// 保存データの有無確認処理
    /// </summary>
    public static void DataInit() {
        string path = SaveFilePathSetting();
        if (File.Exists(path + STAGE_FILE)) {//指定のファイルが存在する場合
            DataLoad(STAGE_FILE);
        } else {
            StageDataCreate();
        }//if
        if (File.Exists(path + OPTION_FILE)) {
            DataLoad(OPTION_FILE);
        } else {
            OptionDataCreate();
        }//if
        if (File.Exists(path + UNLOCK_FILE)) {
            DataLoad(UNLOCK_FILE);
        } else {
            UnlockDataCreate();
        }//if
    }//DataInit

    /// <summary>
    /// データ読み込み処理
    /// </summary>
    /// <param name="dataPath">読み込むデータファイルのパス情報</param>
    private static void DataLoad(string loadFilePath) {
        FileInfo info = new FileInfo(SaveFilePathSetting() + loadFilePath);
        StreamReader reader = new StreamReader(info.OpenRead());
        string json = reader.ReadToEnd();
        switch (loadFilePath) {
            case STAGE_FILE:
                using (TextReader tr = new StreamReader(SaveFilePathSetting() + loadFilePath, Encoding.UTF8))
                    stageDataStruct = JsonMapper.ToObject<StageDataStruct>(tr);
                break;
            case OPTION_FILE:
                using (TextReader tr = new StreamReader(SaveFilePathSetting() + loadFilePath, Encoding.UTF8))
                    optionDataStruct = JsonMapper.ToObject<OptionDataStruct>(tr);
                break;
            case UNLOCK_FILE:
                using (TextReader tr = new StreamReader(SaveFilePathSetting() + loadFilePath, Encoding.UTF8))
                    unlockDataStruct = JsonMapper.ToObject<UnlockDataStruct>(tr);
                break;
        }//switch
        reader.Close();
    }//DataLoad


    /// <summary>
    /// 現在のキーコンフィグをファイルにセーブする
    /// ファイルがない場合は新たにファイルを作成する
    /// </summary>
    public static void SaveDataFile(string jsonText,string filePath) {
        string path = SaveFilePathSetting();
        path = path + filePath;
        using (TextWriter tw = new StreamWriter(path, false, Encoding.UTF8))
            tw.Write(jsonText);
    }//SaveConfigFile


    /// <summary>
    /// ステージデータの初期化処理
    /// </summary>
    /// 
    private static void StageDataCreate() {
        stageDataStruct = new StageDataStruct();
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
        stageDataStruct.nameList = nList;
        stageDataStruct.rankList = rList;
        stageDataStruct.scoreList = sList;
        stageDataStruct.timeList = tList;
        //string jsonData = JsonUtility.ToJson(stageDataStruct, true);
        //DataSave(jsonData, STAGE_FILE);
        string jsonText = JsonMapper.ToJson(stageDataStruct);
        SaveDataFile(jsonText, STAGE_FILE);
    }//StageDataCreate

    /// <summary>
    /// オプションデータの初期化処理
    /// </summary>
    /// 
    private static void OptionDataCreate() {
        optionDataStruct = new OptionDataStruct();
        optionDataStruct.bgmVol = 2;
        optionDataStruct.seVol = 2;
        optionDataStruct.resolutionH = 1280;
        optionDataStruct.resolutionW = 720;
        optionDataStruct.isFullscreen = false;
        string jsonText = JsonMapper.ToJson(optionDataStruct);
        SaveDataFile(jsonText, OPTION_FILE);
    }//OptionDataCreate

    /// <summary>
    /// アンロックデータの初期化処理
    /// </summary>
    private static void UnlockDataCreate() {
        unlockDataStruct = new UnlockDataStruct();
        List<bool> list = new List<bool>();
        for (int i = 0; i < STAGE_NUM * 4; i++) {
            list.Insert(i, false);
        }
        unlockDataStruct.unlockList = list;
        //string jsonData = JsonUtility.ToJson(unlockDataStruct, true);
        //DataSave(jsonData, UNLOCK_FILE);
        string jsonText = JsonMapper.ToJson(unlockDataStruct);
        SaveDataFile(jsonText, UNLOCK_FILE);
    }//UnlockDataCreate

    /// <summary>
    /// ステージデータ更新処理
    /// </summary>
    /// <param name="list">データ更新するリスト</param>
    public static void StageDataUpdate(List<List<string>> list) {
        stageDataStruct.nameList = list[0];
        stageDataStruct.rankList = list[1];
        stageDataStruct.scoreList = list[2];
        stageDataStruct.timeList = list[3];
        //string jsonData = JsonUtility.ToJson(stageDataStruct, true);//SaveData情報
        //DataSave(jsonData, STAGE_FILE);
        string jsonText = JsonMapper.ToJson(stageDataStruct);
        SaveDataFile(jsonText, STAGE_FILE);
    }//StageDataUpdate

    /// <summary>
    /// オプションデータの更新処理
    /// </summary>
    /// <param name="arrayList">データ更新するリスト</param>
    public static void OptionDataUpdate(ArrayList arrayList) {
        optionDataStruct.bgmVol = (int)arrayList[0];
        optionDataStruct.seVol = (int)arrayList[1];
        optionDataStruct.resolutionH = (int)arrayList[2];
        optionDataStruct.resolutionW = (int)arrayList[3];
        optionDataStruct.isFullscreen = (bool)arrayList[4];
        //string jsonData = JsonUtility.ToJson(optionDataStruct, true);
        //DataSave(jsonData, OPTION_FILE);
        string jsonText = JsonMapper.ToJson(optionDataStruct);
        SaveDataFile(jsonText, OPTION_FILE);
    }//OptionDataUpdate

    /// <summary>
    /// アンロックデータの更新処理
    /// </summary>
    /// <param name="list">データ更新するリスト</param>
    public static void UnlockDataUpdate(List<bool> list) {
        unlockDataStruct.unlockList = list;
        //string jsonData = JsonUtility.ToJson(unlockDataStruct, true);
        //DataSave(jsonData, UNLOCK_FILE);
        string jsonText = JsonMapper.ToJson(unlockDataStruct);
        SaveDataFile(jsonText, UNLOCK_FILE);
    }//UnlockDataUpdate


    /// <summary>
    /// ステージデータを削除する処理
    /// </summary>
    public static void StageDataDelete() {
        File.Delete(SaveFilePathSetting() + STAGE_FILE);
        StageDataCreate();
    }//StageDataDelete

    /// <summary>
    /// アンロックデータを削除する処理
    /// </summary>
    public static void UnlockDataDelete() {
        File.Delete(SaveFilePathSetting() + UNLOCK_FILE);
        UnlockDataCreate();
    }//UnlockDataDelete





}//SaveManager
