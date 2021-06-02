using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

/// <summary>
/// シーンの遷移状態を保存する処理
/// 更新日時:0603
/// </summary>
public class SceneSingleton :SingletonMonoBehaviour<SceneSingleton> {
    private static AudioManager _audioManager;

    private List<string> _sceneHistoryList = new List<string>();
    public List<string> SceneHistoryList {
        get { return _sceneHistoryList; }
        set { _sceneHistoryList = value; }
    }


    public new void Awake() {
        if (_sceneHistoryList.Count == 0) {
            _sceneHistoryList.Add("Title_Demo");//デモ版の段階のみTitle_Demoにする
        }//if
        if (this != Instance) {
            Destroy(this);
            Destroy(GetComponent<AudioSource>());
            return;
        }//if
        DontDestroyOnLoad(this.gameObject);
        _audioManager = this.GetComponent<AudioManager>();
    }//Awake

}//SceneSingleton

