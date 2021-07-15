using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

[Serializable]
public struct StageData {
    public List<string> deck;
    public int money;
}
public struct InfoData {
    public string[,] deck;
}

public static class SaveManager {

    public static StageData sd;
    public static InfoData id;
    const string SAVE_FILE_PATH = "saveData.json";

    public static void saveDeck(List<string> _deck) {
        sd.deck = _deck;
        save();
    }
    public static void saveStageInfo(string[,] _info) {
        id.deck = _info;
        save();
    }

    public static void saveMoney(int _money) {
        sd.money = _money;
        save();
    }

    public static void save() {
        //string json = JsonUtility.ToJson(sd);//SaveData情報
        string json = JsonUtility.ToJson(id);//SaveData情報
        string path = Directory.GetCurrentDirectory();//プロジェクトフォルダまでのパス
        path += ("/Assets/Test/" + SAVE_FILE_PATH);
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
    }

    public static void load() {
        try {
            string path = Directory.GetCurrentDirectory();
            FileInfo info = new FileInfo(path + "/" + SAVE_FILE_PATH);
            StreamReader reader = new StreamReader(info.OpenRead());
            string json = reader.ReadToEnd();
            //sd = JsonUtility.FromJson<StageData>(json);
            id = JsonUtility.FromJson<InfoData>(json);
        } catch (Exception e) {
            //sd = new StageData();
            id = new InfoData();
        }
    }
}
