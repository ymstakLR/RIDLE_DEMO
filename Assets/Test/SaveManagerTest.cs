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
        SaveManager.Load();
        nameList.AddRange(SaveManager.stageData.nameList);
        rankList.AddRange(SaveManager.stageData.rankList);
        scoreList.AddRange(SaveManager.stageData.scoreList);
        timeList.AddRange(SaveManager.stageData.timeList);
        
       
        nameList[1] = "Ç∑ÇƒÅ[Ç∂2";
        rankList[1] = "A";
        scoreList[1] = "1000";
        timeList[1] = "1:25";


        stageData.Insert(0, nameList);
        stageData.Insert(1, rankList);
        stageData.Insert(2, scoreList);
        stageData.Insert(3, timeList);

        SaveManager.SaveStageList(stageData);
    }

    // Update is called once per frame
    void Update() {

    }
}
