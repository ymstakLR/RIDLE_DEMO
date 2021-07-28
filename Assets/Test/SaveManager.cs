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

/// <summary>
/// �Q�[���Ŏg�p����f�[�^�̊Ǘ�����
/// �X�V����:0727
/// </summary>
public static class SaveManager {
    const string SAVE_DATA_PATH = "/Assets/Test/";
    const int STAGE_NUM = 3;

    const string STAGE_FILE ="stageData.json";
    public static StageDataStruct stageData;

    const string OPTION_FILE ="optionData.json";
    public static OptionDataStruct optionData;

    const string UNLOCK_FILE ="unlockData.json";
    public static UnlockData unlockData;

    /// <summary>
    /// �X�e�[�W�f�[�^�X�V����
    /// </summary>
    /// <param name="list">�f�[�^�X�V���郊�X�g</param>
    public static void StageDataUpdate(List<List<string>> list) {
        stageData.nameList = list[0];
        stageData.rankList = list[1];
        stageData.scoreList = list[2];
        stageData.timeList = list[3];

        string jsonData = JsonUtility.ToJson(stageData, true);//SaveData���
        DataSave(jsonData, STAGE_FILE);
        Debug.Log("�X�e�[�W�f�[�^�̍X�V�I��");
    }//StageDataUpdate

    /// <summary>
    /// �I�v�V�����f�[�^�̍X�V����
    /// </summary>
    /// <param name="arrayList">�f�[�^�X�V���郊�X�g</param>
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
    /// �A�����b�N�f�[�^�̍X�V����
    /// </summary>
    /// <param name="list">�f�[�^�X�V���郊�X�g</param>
    public static void UnlockDataUpdate(List<bool> list) {
        unlockData.unlockList = list;
        string jsonData = JsonUtility.ToJson(unlockData, true);
        DataSave(jsonData, UNLOCK_FILE);
    }//UnlockDataUpdate

    /// <summary>
    /// �f�[�^�̕ۑ�����
    /// </summary>
    /// <param name="json">�ۑ�����jsonData</param>
    /// <param name="filePath">�ۑ�����t�@�C���p�X</param>
    private static void DataSave(string json, string filePath) {
        string path = SaveFilePathSetting();
        path = path + filePath;
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
    }//DataSave

    /// <summary>
    /// �X�e�[�W�f�[�^���폜���鏈��
    /// </summary>
    public static void StageDataDelete() {
        File.Delete(SaveFilePathSetting() + STAGE_FILE);
        StageDataGenerate();
    }//StageDataDelete

    /// <summary>
    /// �A�����b�N�f�[�^���폜���鏈��
    /// </summary>
    public static void UnlockDataDelete() {
        File.Delete(SaveFilePathSetting() + UNLOCK_FILE);
        UnlockDataGenerate();
    }//UnlockDataDelete

    /// <summary>
    /// �ۑ��f�[�^�̓ǂݍ��ݏ���
    /// </summary>
    public static void DataInit() {
        string path = SaveFilePathSetting();
        if (File.Exists(path + STAGE_FILE)) {//�w��̃t�@�C�������݂���ꍇ
            Debug.Log("�X�e�[�W�f�[�^�̓ǂݍ���");
            DataLoad(STAGE_FILE);
        } else {
            StageDataGenerate();
        }//if
        if (File.Exists(path + OPTION_FILE)) {
            DataLoad(OPTION_FILE);
        } else {
            OptionDataGenerate();
        }//if
        if (File.Exists(path + UNLOCK_FILE)) {//�w��̃t�@�C�������݂���ꍇ
            DataLoad(UNLOCK_FILE);
        } else {
            UnlockDataGenerate();
        }//if
    }//DataLoad

    /// <summary>
    /// �f�[�^�ǂݍ��ݏ���
    /// </summary>
    /// <param name="dataPath">�ǂݍ��ރf�[�^�t�@�C���̃p�X���</param>
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
        }//switch
        reader.Close();
    }//DataLoad

    /// <summary>
    /// �X�e�[�W�f�[�^�̏���������
    /// </summary>
    /// 
    private static void StageDataGenerate() {
        Debug.Log("�X�e�[�W�f�[�^�̏�����");
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
    /// �I�v�V�����f�[�^�̏���������
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
    /// �A�����b�N�f�[�^�̏���������
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

    /// <summary>
    /// �t�@�C����ۑ�����p�X��ݒ肷�鏈��
    /// </summary>
    /// <returns></returns>
    private static string SaveFilePathSetting() {
        string path="";
        #if UNITY_EDITOR
            path = Directory.GetCurrentDirectory()+"/Assets/Test/";
        #else
            path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');    
        #endif
        return path;
    }//SaveFilePathSetting

}//SaveManager
