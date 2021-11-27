using LitJson;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �Q�[���Ŏg�p����f�[�^�̊Ǘ�����
/// �X�V����:20211004
/// </summary>
public static class SaveManager {
    //�ۑ��f�[�^�p�X���
    const string SAVE_DATA_PATH = "/Assets/05_SaveData/";
    //�X�e�[�W�f�[�^�p���
    public struct StageDataStruct {
        public List<string> nameList;
        public List<string> rankList;
        public List<string> scoreList;
        public List<string> timeList;
    }//StageDataStruct
    public static StageDataStruct stageDataStruct;
    private const string STAGE_FILE = "stageData.dat";
    private const int STAGE_NUM = 3;
    //�I�v�V�����f�[�^�p���
    public struct OptionDataStruct {
        public int bgmVol;
        public int seVol;
        public int resolutionH;
        public int resolutionW;
        public bool isFullscreen;
    }//OptionDataStruct
    public static OptionDataStruct optionDataStruct;
    private const string OPTION_FILE = "optionData.dat";
    //�A�����b�N�f�[�^�p���
    public struct UnlockDataStruct {
        public List<bool> unlockList;
    }//UnlockDataStruct
    public static UnlockDataStruct unlockDataStruct;
    private const string UNLOCK_FILE = "unlockData.dat";


    /// <summary>
    /// �t�@�C����ۑ�����p�X��ݒ肷�鏈��
    /// </summary>
    /// <returns>�t�@�C����ۑ�����p�X</returns>
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
    /// �ۑ��f�[�^�̗L���m�F����
    /// </summary>
    public static void DataInit() {
        string path = SaveFilePathSetting();
        if (File.Exists(path + STAGE_FILE)) {//�w��̃t�@�C�������݂���ꍇ
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
    /// �f�[�^�ǂݍ��ݏ���
    /// </summary>
    /// <param name="dataPath">�ǂݍ��ރf�[�^�t�@�C���̃p�X���</param>
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
    /// ���݂̃L�[�R���t�B�O���t�@�C���ɃZ�[�u����
    /// �t�@�C�����Ȃ��ꍇ�͐V���Ƀt�@�C�����쐬����
    /// </summary>
    public static void SaveDataFile(string jsonText,string filePath) {
        string path = SaveFilePathSetting();
        path = path + filePath;
        using (TextWriter tw = new StreamWriter(path, false, Encoding.UTF8))
            tw.Write(jsonText);
    }//SaveConfigFile


    /// <summary>
    /// �X�e�[�W�f�[�^�̏���������
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
    /// �I�v�V�����f�[�^�̏���������
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
    /// �A�����b�N�f�[�^�̏���������
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
    /// �X�e�[�W�f�[�^�X�V����
    /// </summary>
    /// <param name="list">�f�[�^�X�V���郊�X�g</param>
    public static void StageDataUpdate(List<List<string>> list) {
        stageDataStruct.nameList = list[0];
        stageDataStruct.rankList = list[1];
        stageDataStruct.scoreList = list[2];
        stageDataStruct.timeList = list[3];
        //string jsonData = JsonUtility.ToJson(stageDataStruct, true);//SaveData���
        //DataSave(jsonData, STAGE_FILE);
        string jsonText = JsonMapper.ToJson(stageDataStruct);
        SaveDataFile(jsonText, STAGE_FILE);
    }//StageDataUpdate

    /// <summary>
    /// �I�v�V�����f�[�^�̍X�V����
    /// </summary>
    /// <param name="arrayList">�f�[�^�X�V���郊�X�g</param>
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
    /// �A�����b�N�f�[�^�̍X�V����
    /// </summary>
    /// <param name="list">�f�[�^�X�V���郊�X�g</param>
    public static void UnlockDataUpdate(List<bool> list) {
        unlockDataStruct.unlockList = list;
        //string jsonData = JsonUtility.ToJson(unlockDataStruct, true);
        //DataSave(jsonData, UNLOCK_FILE);
        string jsonText = JsonMapper.ToJson(unlockDataStruct);
        SaveDataFile(jsonText, UNLOCK_FILE);
    }//UnlockDataUpdate


    /// <summary>
    /// �X�e�[�W�f�[�^���폜���鏈��
    /// </summary>
    public static void StageDataDelete() {
        File.Delete(SaveFilePathSetting() + STAGE_FILE);
        StageDataCreate();
    }//StageDataDelete

    /// <summary>
    /// �A�����b�N�f�[�^���폜���鏈��
    /// </summary>
    public static void UnlockDataDelete() {
        File.Delete(SaveFilePathSetting() + UNLOCK_FILE);
        UnlockDataCreate();
    }//UnlockDataDelete





}//SaveManager
