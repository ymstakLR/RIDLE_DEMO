using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;


[Serializable]
public  class StageData {
    public List<string> nameList;
    public List<string> rankList;
    public List<string> scoreList;
    public List<string> timeList;
}//StageData


public static class SaveManager {

    const string SAVE_FILE_PATH = "saveData.json";

    public static StageData stageData = new StageData();


    public static void SaveStageList(List<List<string>> list) {
        stageData.nameList = list[0];
        stageData.rankList = list[1];
        stageData.scoreList = list[2];
        stageData.timeList = list[3];
        Save();
    }

    public static void LogOutput() {
        Debug.Log("LogOutputの開始");
        for (int i = 0; i < stageData.nameList.Count; i++) {
            Debug.Log((i+1).ToString()+"リスト目の表示");
            Debug.Log(stageData.nameList[i]);
            Debug.Log(stageData.rankList[i]);
            Debug.Log(stageData.scoreList[i]);
            Debug.Log(stageData.timeList[i]);
        }//for
    }//LogOutput


    public static void Save() {
        string json = JsonUtility.ToJson(stageData, true);//SaveData情報

        Debug.Log(json.ToString());
        string path = Directory.GetCurrentDirectory();//プロジェクトフォルダまでのパス
        path += ("/Assets/Test/" + SAVE_FILE_PATH);
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
    }

    public static void Load() {
        try {
            string path = Directory.GetCurrentDirectory();
            FileInfo info = new FileInfo(path + "/Assets/Test/" + SAVE_FILE_PATH);
            StreamReader reader = new StreamReader(info.OpenRead());
            string json = reader.ReadToEnd();
            //sd = JsonUtility.FromJson<StageData>(json);
            stageData = JsonUtility.FromJson<StageData>(json);
            LogOutput();
        } catch (Exception e) {
            //sd = new StageData();
            stageData = new StageData();
        }
        
    }

}
