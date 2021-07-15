using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManagerTest : MonoBehaviour
{
    public List<string> stageData;
    string[,] stageDataInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        stageDataInfo = new string[2, 4];
        stageDataInfo[0, 0] = "ステージ";
        stageDataInfo[0, 1] = "ランク";
        stageDataInfo[0, 2] = "スコア";
        stageDataInfo[0, 3] = "タイム";

        stageDataInfo[0, 0] = "stage";
        stageDataInfo[0, 1] = "lank";
        stageDataInfo[0, 2] = "score";
        stageDataInfo[0, 3] = "time";

        stageData.Insert(0,"ランク");
        stageData.Insert(1, "スコア");
        stageData.Insert(2, "タイム");
        SaveManager.load();
        //SaveManager.saveDeck(stageData);
        SaveManager.saveStageInfo(stageDataInfo);
        //SaveManager.saveMoney(1);
        //SaveManager.saveMoney(100);
        //SaveManager.saveMoney(10000);
        //Debug.Log(SaveManager.sd.money);
        //SaveManager.save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
