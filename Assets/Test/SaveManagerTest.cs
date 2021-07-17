using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManagerTest : MonoBehaviour {


    public List<List<string>> stageData = new List<List<string>>();

    public List<string> nameList = new List<string>();
    public List<string> rankList = new List<string>();
    public List<string> scoreList = new List<string>();
    public List<string> timeList = new List<string>();

    // Start is called before the first frame update
    void Start() {
        nameList.Insert(0, "ステージ1");
        nameList.Insert(1, "Stage2");

        rankList.Insert(0, "A");
        rankList.Insert(1, "E");

        scoreList.Insert(0, "123");
        scoreList.Insert(1, "456789");

        timeList.Insert(0, "0:25");
        timeList.Insert(1, "9:59");

        stageData.Insert(0, nameList);
        stageData.Insert(1, rankList);
        stageData.Insert(2, scoreList);
        stageData.Insert(3, timeList);

        SaveManager.Load();
        //SaveManager.SaveStageList(stageData);
        //SaveManager.LogOutput();
        Debug.Log(SaveManager.stageData.nameList[1]);
    }

    // Update is called once per frame
    void Update() {

    }
}
