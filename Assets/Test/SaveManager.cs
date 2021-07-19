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

public static class SaveManager {
    const string SAVE_FILE_PATH = "saveData.json";
    public static StageData stageData;

    public static void SaveStageList(List<List<string>> list) {
        stageData.nameList = list[0];
        stageData.rankList = list[1];
        stageData.scoreList = list[2];
        stageData.timeList = list[3];
        Save();
        Debug.Log("ステージデータの更新");
    }

    public static void LogOutput() {
        Debug.Log("LogOutputの開始");
        for (int i = 0; i < stageData.nameList.Count; i++) {
            Debug.Log((i + 1).ToString() + "リスト目の表示");
            Debug.Log(stageData.nameList[i]);
            Debug.Log(stageData.rankList[i]);
            Debug.Log(stageData.scoreList[i]);
            Debug.Log(stageData.timeList[i]);
        }//for
    }//LogOutput

    public static void Save() {
        
        string json = JsonUtility.ToJson(stageData, true);//SaveData情報
        string path = Directory.GetCurrentDirectory();//プロジェクトフォルダまでのパス
        path = path + "/Assets/Test/" + SAVE_FILE_PATH;
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
        //LogOutput();
    }

    public static void Load() {
        if(File.Exists(Directory.GetCurrentDirectory() + "/Assets/Test/" + SAVE_FILE_PATH)) {
            Debug.Log("ステージデータの読み込み");
            string path = Directory.GetCurrentDirectory();
            FileInfo info = new FileInfo(path + "/Assets/Test/" + SAVE_FILE_PATH);
            StreamReader reader = new StreamReader(info.OpenRead());
            string json = reader.ReadToEnd();
            stageData = JsonUtility.FromJson<StageData>(json);
            reader.Close();
        } else {
            stageData = new StageData();
            StageDataInit();
        }//if
    }

    public static void StageDataInit() {
        Debug.Log("ステージデータの初期化");
        List<string> nList = new List<string>();
        List<string> rList = new List<string>();
        List<string> sList = new List<string>();
        List<string> tList = new List<string>();
        for (int i = 0; i < 2; i++) {
            nList.Insert(i, "Stage" + (i+1));
            rList.Insert(i, "E");
            sList.Insert(i, "0");
            tList.Insert(i, "9.59");
        }
        stageData.nameList = nList;
        stageData.rankList = rList;
        stageData.scoreList = sList;
        stageData.timeList = tList;
        Save();

    }

}
