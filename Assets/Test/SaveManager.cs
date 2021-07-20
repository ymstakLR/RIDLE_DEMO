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
/// �Q�[���Ŏg�p����f�[�^�̊Ǘ�����
/// �X�V����:0720
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
    /// �X�e�[�W�f�[�^�X�V����
    /// </summary>
    /// <param name="list">�f�[�^�X�V���郊�X�g</param>
    public static void StageDataUpdate(List<List<string>> list) {
        stageData.nameList = list[0];
        stageData.rankList = list[1];
        stageData.scoreList = list[2];
        stageData.timeList = list[3];

        string jsonData = JsonUtility.ToJson(stageData, true);//SaveData���
        DataSave(jsonData, STAGE_FILE_PATH);
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
        DataSave(jsonData, OPTION_FILE_PATH);
        Debug.Log("�I�v�V�����f�[�^�̍X�V�I��");
    }//OptionDataUpdate

    public static void UnlockDataUpdate(List<bool> list) {
        unlockData.unlockList = list;
        string jsonData = JsonUtility.ToJson(unlockData, true);
        DataSave(jsonData, UNLOCK_FILE_PATH);
        Debug.Log("�A�����b�N�f�[�^�̍X�V�I��");
    }

    /// <summary>
    /// �f�[�^�̕ۑ�����
    /// </summary>
    /// <param name="json">�ۑ�����jsonData</param>
    /// <param name="filePath">�ۑ�����t�@�C���p�X</param>
    public static void DataSave(string json, string filePath) {
        string path = Directory.GetCurrentDirectory();//�v���W�F�N�g�t�H���_�܂ł̃p�X
        path = path + filePath;
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
    }//DataSave

    /// <summary>
    /// �ۑ��f�[�^�̓ǂݍ��ݏ���
    /// </summary>
    public static void DataInit() {
        string path = Directory.GetCurrentDirectory();
        if (File.Exists(path + STAGE_FILE_PATH)) {//�w��̃t�@�C�������݂���ꍇ
            Debug.Log("�X�e�[�W�f�[�^�̓ǂݍ���");
            DataLoad(STAGE_FILE_PATH);
        } else {
            StageDataGenerate();
        }//if
        if (File.Exists(path + OPTION_FILE_PATH)) {
            Debug.Log("�I�v�V�����f�[�^�̓ǂݍ���");
            DataLoad(OPTION_FILE_PATH);
        } else {
            OptionDataGenerate();
        }//if
        if (File.Exists(path + UNLOCK_FILE_PATH)) {//�w��̃t�@�C�������݂���ꍇ
            Debug.Log("�A�����b�N�f�[�^�̓ǂݍ���");
            DataLoad(UNLOCK_FILE_PATH);
        } else {
            UnlockDataGenerate();
        }//if
    }//DataLoad

    /// <summary>
    /// �f�[�^�ǂݍ��ݏ���
    /// </summary>
    /// <param name="dataPath">�ǂݍ��ރf�[�^�t�@�C���̃p�X���</param>
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
    /// �X�e�[�W�f�[�^�̏���������
    /// </summary>
    /// 
    public static void StageDataGenerate() {
        Debug.Log("�X�e�[�W�f�[�^�̏�����");
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
    /// �I�v�V�����f�[�^�̏���������
    /// </summary>
    /// 
    public static void OptionDataGenerate() {
        Debug.Log("�I�v�V�����f�[�^�̏�����");
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
        Debug.Log("�A�����b�N�f�[�^�̏�����");
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
