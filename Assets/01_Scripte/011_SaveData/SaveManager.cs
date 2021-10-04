using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
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
    private const string STAGE_FILE = "stageData.json";
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
    private const string OPTION_FILE ="optionData.json";
    //�A�����b�N�f�[�^�p���
    public struct UnlockDataStruct {
        public List<bool> unlockList;
    }//UnlockDataStruct
    public static UnlockDataStruct unlockDataStruct;
    private const string UNLOCK_FILE ="unlockData.json";
    //Input�f�[�^�p���
    public struct InputDataStruct {
        public List<string> nameList;
        public List<string> negativeButtonList;
        public List<string> positiveButtonList;
        public List<string> altPositionButtonList;
        public List<string> invertList;
        public List<string> typeList;
        public List<string> axisList;
    }//InputDataStruct
    public static InputDataStruct inputDataStruct;
    private const string INPUT_FILE = "inputData.json";
    private const int AXES_SIZE_NUM = 12;


    /// <summary>
    /// �t�@�C����ۑ�����p�X��ݒ肷�鏈��
    /// </summary>
    /// <returns>�t�@�C����ۑ�����p�X</returns>
    private static string SaveFilePathSetting() {
        string path = "";
        #if UNITY_EDITOR
            path = Directory.GetCurrentDirectory() + SAVE_DATA_PATH;
        #else
            path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');    
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
    /// �f�[�^�ǂݍ��ݏ���
    /// </summary>
    /// <param name="dataPath">�ǂݍ��ރf�[�^�t�@�C���̃p�X���</param>
    private static void DataLoad(string loadFilePath) {
        FileInfo info = new FileInfo(SaveFilePathSetting() + loadFilePath);
        StreamReader reader = new StreamReader(info.OpenRead());
        string json = reader.ReadToEnd();
        switch (loadFilePath) {
            case STAGE_FILE:
                stageDataStruct = JsonUtility.FromJson<StageDataStruct>(json);
                break;
            case OPTION_FILE:
                optionDataStruct = JsonUtility.FromJson<OptionDataStruct>(json);
                break;
            case UNLOCK_FILE:
                unlockDataStruct = JsonUtility.FromJson<UnlockDataStruct>(json);
                break;
            case INPUT_FILE:
                inputDataStruct = JsonUtility.FromJson<InputDataStruct>(json);
                break;
        }//switch
        reader.Close();
    }//DataLoad

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
    /// �X�e�[�W�f�[�^�̏���������
    /// </summary>
    /// 
    private static void StageDataGenerate() {
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
        string jsonData = JsonUtility.ToJson(stageDataStruct, true);
        DataSave(jsonData, STAGE_FILE);
    }//StageDataInit

    /// <summary>
    /// �I�v�V�����f�[�^�̏���������
    /// </summary>
    /// 
    private static void OptionDataGenerate() {
        optionDataStruct = new OptionDataStruct();
        optionDataStruct.bgmVol = 0;
        optionDataStruct.seVol = 0;
        optionDataStruct.resolutionH = 1980;
        optionDataStruct.resolutionW = 1080;
        optionDataStruct.isFullscreen = true;
        string jsonData = JsonUtility.ToJson(optionDataStruct, true);
        DataSave(jsonData, OPTION_FILE);
    }//OptionDataInit

    /// <summary>
    /// �A�����b�N�f�[�^�̏���������
    /// </summary>
    private static void UnlockDataGenerate() {
        unlockDataStruct = new UnlockDataStruct();
        List<bool> list = new List<bool>();
        for (int i = 0; i < STAGE_NUM * 4; i++) {
            list.Insert(i, false);
        }
        unlockDataStruct.unlockList = list;
        string jsonData = JsonUtility.ToJson(unlockDataStruct, true);
        DataSave(jsonData, UNLOCK_FILE);
    }//UnlockDataGenerate

    /// <summary>
    /// Input�f�[�^�̏���������
    /// </summary>
    private static void InputDataGenerate() {
        inputDataStruct = new InputDataStruct();
        List<string> nameList = new List<string>();
        List<string> nButtonList = new List<string>();
        List<string> pButtonList = new List<string>();
        List<string> apButtonList = new List<string>();
        List<string> invertList = new List<string>();
        List<string> typeList = new List<string>();
        List<string> axisList = new List<string>();
        for (int i = 0; i < AXES_SIZE_NUM; i++) {
            InputManagerInitialSetting.DefaultNameInsert(nameList, i);
            InputManagerInitialSetting.DefaultNegativeButtonInsert(nButtonList, i);
            InputManagerInitialSetting.DefaultPositiveButtonInsert(pButtonList, i);
            InputManagerInitialSetting.DefaultAltPositiveButtonInsert(apButtonList, i);
            InputManagerInitialSetting.DefaultInvert(invertList, i);
            InputManagerInitialSetting.DefaultType(typeList, i);
            InputManagerInitialSetting.DefaultAxis(axisList, i);
        }//for
        inputDataStruct.nameList = nameList;
        inputDataStruct.negativeButtonList = nButtonList;
        inputDataStruct.positiveButtonList = pButtonList;
        inputDataStruct.altPositionButtonList = apButtonList;
        inputDataStruct.invertList = invertList;
        inputDataStruct.typeList = typeList;
        inputDataStruct.axisList = axisList;
        string jsonData = JsonUtility.ToJson(inputDataStruct, true);
        DataSave(jsonData, INPUT_FILE);
    }//InputDataGenerate


    /// <summary>
    /// �X�e�[�W�f�[�^�X�V����
    /// </summary>
    /// <param name="list">�f�[�^�X�V���郊�X�g</param>
    public static void StageDataUpdate(List<List<string>> list) {
        stageDataStruct.nameList = list[0];
        stageDataStruct.rankList = list[1];
        stageDataStruct.scoreList = list[2];
        stageDataStruct.timeList = list[3];
        string jsonData = JsonUtility.ToJson(stageDataStruct, true);//SaveData���
        DataSave(jsonData, STAGE_FILE);
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
        string jsonData = JsonUtility.ToJson(optionDataStruct, true);
        DataSave(jsonData, OPTION_FILE);
    }//OptionDataUpdate

    /// <summary>
    /// �A�����b�N�f�[�^�̍X�V����
    /// </summary>
    /// <param name="list">�f�[�^�X�V���郊�X�g</param>
    public static void UnlockDataUpdate(List<bool> list) {
        unlockDataStruct.unlockList = list;
        string jsonData = JsonUtility.ToJson(unlockDataStruct, true);
        DataSave(jsonData, UNLOCK_FILE);
    }//UnlockDataUpdate

    /// <summary>
    /// InputData�̍X�V����
    /// </summary>
    /// <param name="list">�f�[�^�X�V���郊�X�g</param>
    public static void InputDataUpdate(List<List<string>> list) {
        inputDataStruct.nameList = list[0];
        inputDataStruct.negativeButtonList = list[1];
        inputDataStruct.positiveButtonList = list[2];
        inputDataStruct.altPositionButtonList = list[3];
        inputDataStruct.invertList = list[4];
        inputDataStruct.typeList = list[5];
        inputDataStruct.axisList = list[6];
        string jsonData = JsonUtility.ToJson(inputDataStruct, true);
        DataSave(jsonData, INPUT_FILE);
    }//InputDataUpdate


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
    /// InputData���폜���鏈��
    /// </summary>
    public static void InputDataDelete() {
        File.Delete(SaveFilePathSetting() + INPUT_FILE);
        InputDataGenerate();
    }//InputDataDelete

}//SaveManager
