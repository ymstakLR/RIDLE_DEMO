using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManagerTest : MonoBehaviour {


    /// <summary>
    ///�X�e�[�W�f�[�^
    /// </summary>
    public List<List<string>> stageData = new List<List<string>>();
    public List<string> nameList = new List<string>();
    public List<string> rankList = new List<string>();
    public List<string> scoreList = new List<string>();
    public List<string> timeList = new List<string>();
    /// <summary>
    /// �I�v�V�����f�[�^
    /// </summary>
    public ArrayList optionData = new ArrayList();
    /// <summary>
    /// �A�����b�N�f�[�^
    /// </summary>
    public List<bool> unlockData = new List<bool>();

    // Start is called before the first frame update
    void Awake() {
        //SaveManager.DataInit();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {//�X�e�[�W�f�[�^�̍X�V����
            nameList.AddRange(SaveManager.stageData.nameList);
            rankList.AddRange(SaveManager.stageData.rankList);
            scoreList.AddRange(SaveManager.stageData.scoreList);
            timeList.AddRange(SaveManager.stageData.timeList);
            nameList[1] = "���ā[��2";
            rankList[1] = "A";
            scoreList[1] = "1000";
            timeList[1] = "1:25";
            stageData.Insert(0, nameList);
            stageData.Insert(1, rankList);
            stageData.Insert(2, scoreList);
            stageData.Insert(3, timeList);
            SaveManager.StageDataUpdate(stageData);
        }//if
        if (Input.GetKeyDown(KeyCode.F2)) {//�I�v�V�����f�[�^�̍X�V����
            optionData.Insert(0, SaveManager.optionData.bgmVol);
            optionData.Insert(1, SaveManager.optionData.seVol);
            optionData.Insert(2, SaveManager.optionData.resolutionH);
            optionData.Insert(3, SaveManager.optionData.resolutionW);
            optionData.Insert(4, SaveManager.optionData.isFullscreen);
            optionData[0] = (int)8;
            SaveManager.OptionDataUpdate(optionData);
        }//if
        if (Input.GetKeyDown(KeyCode.F3)) {//�A�����b�N�f�[�^�̍X�V����
            unlockData.AddRange(SaveManager.unlockData.unlockList);
            unlockData[1] = true;
            SaveManager.UnlockDataUpdate(unlockData);
        }//if
        if (Input.GetKeyDown(KeyCode.F4)) {
            SaveDataUpdate.BGMVolumeUpadate(9);
        }
    }//Update
}//SaveManagerTest
