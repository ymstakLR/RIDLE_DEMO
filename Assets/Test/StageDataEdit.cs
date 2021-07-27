using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageDataEdit{
    public static List<List<string>> _stageData = new List<List<string>>();
    public static List<string> _nameList = new List<string>();
    public static List<string> _rankList = new List<string>();
    public static List<string> _scoreList = new List<string>();
    public static List<string> _timeList = new List<string>();


    /// <summary>
    /// �X�e�[�W�f�[�^�ǂݍ��ݏ���
    /// </summary>
    private static void StageDataLoad() {
        _nameList.AddRange(SaveManager.stageData.nameList);
        _rankList.AddRange(SaveManager.stageData.rankList);
        _scoreList.AddRange(SaveManager.stageData.scoreList);
        _timeList.AddRange(SaveManager.stageData.timeList);
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
    private static void StageDataUpdate(int stageNum,
        string stageName,string stageRank,
        string stageScore,string stageTime) {
        StageDataLoad();
        _nameList[stageNum] = stageName;
        _rankList[stageNum] = stageRank;
        _scoreList[stageNum] = stageScore;
        _timeList[stageNum] = stageTime;
        StageDataSave();
    }//StageDataUpdate

}
