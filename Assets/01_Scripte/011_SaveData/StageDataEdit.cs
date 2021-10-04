using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�W�f�[�^��ҏW���鏈��
/// �X�V����:0728
/// </summary>
public static class StageDataEdit{
    public static List<List<string>> _stageData = new List<List<string>>();
    public static List<string> _nameList;
    public static List<string> _rankList;
    public static List<string> _scoreList;
    public static List<string> _timeList;

    /// <summary>
    /// �X�e�[�W�f�[�^�ǂݍ��ݏ���
    /// </summary>
    private static void StageDataLoad() {
        _nameList = new List<string>();
        _nameList.AddRange(SaveManager.stageDataStruct.nameList);
        _rankList = new List<string>();
        _rankList.AddRange(SaveManager.stageDataStruct.rankList);
        _scoreList = new List<string>();
        _scoreList.AddRange(SaveManager.stageDataStruct.scoreList);
        _timeList = new List<string>();
        _timeList.AddRange(SaveManager.stageDataStruct.timeList);
    }//StageDataLoad

    /// <summary>
    /// �X�e�[�W�f�[�^�ۑ�����
    /// </summary>
    private static void StageDataSave() {
        _stageData.Insert(0, _nameList);
        _stageData.Insert(1, _rankList);
        _stageData.Insert(2, _scoreList);
        _stageData.Insert(3, _timeList);
        SaveManager.StageDataUpdate(_stageData);
    }//StageDataSave

    /// <summary>
    /// �X�e�[�W�f�[�^�̍X�V����
    /// </summary>
    /// <param name="stageNum">�X�V����X�e�[�W�ԍ�</param>
    /// <param name="stageName">�X�V����X�e�[�W��</param>
    /// <param name="stageRank">�X�V����X�e�[�W�����N</param>
    /// <param name="stageScore">�X�V����X�e�[�W�X�R�A</param>
    /// <param name="stageTime">�X�V����X�e�[�W�^�C��</param>
    public static void StageDataUpdate(
        string stageName,string stageRank,
        string stageScore,string stageTime) {
        StageDataLoad();
        int stageNum = StageDataIdentification(stageName);
        _nameList[stageNum] = stageName;
        _rankList[stageNum] = stageRank;
        _scoreList[stageNum] = stageScore;
        _timeList[stageNum] = stageTime;
        StageDataSave();
    }//StageDataUpdate

    /// <summary>
    /// �X�e�[�W�f�[�^���w�肵���X�e�[�W���ƕR�Â��鏈��
    /// </summary>
    /// <param name="stageName">�R�Â������X�e�[�W��</param>
    /// <returns></returns>
    public static int StageDataIdentification(string stageName) {
        StageDataLoad();
        int stageNum = 0;
        while (_nameList[stageNum].ToString() != stageName) {
            stageNum++;
        }//While
        return stageNum;
    }//StageDataIdentification

}//StageDataEdit
