using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManagerTest : MonoBehaviour {


    /// <summary>
    ///ステージデータ
    /// </summary>
    public List<List<string>> stageData = new List<List<string>>();
    public List<string> nameList = new List<string>();
    public List<string> rankList = new List<string>();
    public List<string> scoreList = new List<string>();
    public List<string> timeList = new List<string>();
    /// <summary>
    /// オプションデータ
    /// </summary>
    public ArrayList optionData = new ArrayList();

    // Start is called before the first frame update
    void Start() {
        SaveManager.DataInit();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {

            nameList.AddRange(SaveManager.stageData.nameList);
            rankList.AddRange(SaveManager.stageData.rankList);
            scoreList.AddRange(SaveManager.stageData.scoreList);
            timeList.AddRange(SaveManager.stageData.timeList);
            Debug.Log(nameList[0].ToString());
            //Debug.Log(SaveManager.optionData.resolutionH);
            nameList[1] = "すてーじ2";
            rankList[1] = "A";
            scoreList[1] = "1000";
            timeList[1] = "1:25";

            stageData.Insert(0, nameList);
            stageData.Insert(1, rankList);
            stageData.Insert(2, scoreList);
            stageData.Insert(3, timeList);

            Debug.Log(SaveManager.optionData.bgmVol);
            optionData.Insert(0,SaveManager.optionData.bgmVol);
            optionData.Insert(1, SaveManager.optionData.seVol);
            optionData.Insert(2, SaveManager.optionData.resolutionH);
            optionData.Insert(3, SaveManager.optionData.resolutionW);
            optionData.Insert(4, SaveManager.optionData.isFullscreen);

            optionData[0] = (int)8;
            SaveManager.StageDataUpdate(stageData);
            SaveManager.OptionDataUpdate(optionData);
        }//if
    }//Update
}//SaveManagerTest
