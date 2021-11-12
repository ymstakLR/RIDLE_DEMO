using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// リソースオブジェクトの管理処理
/// 更新日時:20211007
/// </summary>
public class ResourcesObjectManager : MonoBehaviour {


    private Dictionary<string, GameObject> _rObjectDic;

    private const string R_OBJECT_PATH = "GameObject";

    private void Awake() {
        _rObjectDic = new Dictionary<string, GameObject>();
        object[] rObjectList = Resources.LoadAll(R_OBJECT_PATH);
        foreach (GameObject gameObj in rObjectList) {
            _rObjectDic[gameObj.name] = gameObj;
            GameObject instance = (GameObject)Instantiate((GameObject)Resources.Load(R_OBJECT_PATH+"/"+gameObj.name),this.transform);
        }
    }



    public string ResouresNameCheck(string rObjectName) {
        if (_rObjectDic.ContainsKey(rObjectName))
            return rObjectName;
        Debug.LogError(rObjectName + "が存在しません(代わりにerrorObjectを生成する)");
        return "ErrorObject";
    }//ResouresNameCheck

    private void ResouresStartUp() {
        //foreach()
    }

    private const float TIMER = 2f;
    private float timer;
    private void Update() {
        if (timer > TIMER) {
            SceneManager.LoadScene("Title_Demo");
        }
        timer += Time.deltaTime;
    }


}
