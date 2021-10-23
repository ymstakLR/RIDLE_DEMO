using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//[HideInInspector]
public enum SceneChoice {
    Select,
    Select_Demo,
    Arcade,
    StageSelect,
    Gallery,
    Option,
    Options_Demo,
    Stage1,
    Stage2,
    Stage3,
    Exit,
    Null,
    Title,
    Title_Demo,
    Config_Demo
}//MainScene

/// <summary>
/// シーンを遷移するための処理
/// 更新日時:0616
/// </summary>
public class SceneChange : MonoBehaviour {

    private SceneSingleton _gameManager;


    private void Start() {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        _gameManager = GameObject.Find("GameManager").GetComponent<SceneSingleton>();
    }//Start

    /// <summary>
    /// 次の遷移に移動する処理
    /// </summary>
    public void NextSceneChange(string sceneChoice) {
        this.GetComponent<ButtonInfo>().ButtonInteractable(false);
        //gameManager.GetComponent<AudioManager>().PlaySE("ButtonClick");
        _gameManager.GetComponent<AudioManager>().PlaySE("Click");
        SceneFade(0.5f, true);//SceneFade(1, true);0616
        StartCoroutine(NextSceneCoroutine(sceneChoice));
    }//MainChange

    /// <summary>
    /// 一定時間停止を処理してからシーン移動を行う
    /// </summary>
    /// <param name="sceneChoice"></param>
    /// <returns></returns>
    IEnumerator NextSceneCoroutine(string sceneChoice) {
        string nextSceneStr = sceneChoice.ToString();
        yield return new WaitForSeconds(1);
        _gameManager.SceneHistoryList.Add(nextSceneStr);
        if (nextSceneStr == SceneChoice.Exit.ToString()) {//ゲーム終了する場合
            Application.Quit();
            yield break;
        }//if
        SceneManager.LoadScene(nextSceneStr);
    }//NextSceneEnumerator

    /// <summary>
    /// 一つ前の遷移に戻る処理
    /// </summary>
    public void BackSceneChange(bool isBackSE) {
        BackSceneJudge(isBackSE);
    }//MainChange

    /// <summary>
    /// シーンを戻るときの判定処理
    /// </summary>
    /// <param name="isBackSE"></param>
    private void BackSceneJudge(bool isBackSE) {
        if (_gameManager.SceneHistoryList.Count == 1 ||
            _gameManager.SceneHistoryList[_gameManager.SceneHistoryList.Count - 1].ToString() == SceneChoice.Title_Demo.ToString()) 
            return;
        if (isBackSE) {
            _gameManager.GetComponent<AudioManager>().PlaySE("Cancel");
        } else {
            _gameManager.GetComponent<AudioManager>().PlaySE("Click");
        }//if
        SceneFade(0.5f, true);//SceneFade(1, true);0616
        StartCoroutine(BackSceneCoroutine());
    }//StageClearChange

    /// <summary>
    /// シーンを戻すときの一時停止処理
    /// </summary>
    /// <returns></returns>
    IEnumerator BackSceneCoroutine() {
        string backSceneStr = _gameManager.SceneHistoryList[_gameManager.SceneHistoryList.Count - 2];
        yield return new WaitForSeconds(1);
        _gameManager.SceneHistoryList.RemoveAt(_gameManager.SceneHistoryList.Count - 1);
        SceneManager.LoadScene(backSceneStr);
    }//SceneBackChange

    /// <summary>
    /// フェードインを行う処理
    /// </summary>
    /// <param name="prevScene"></param>
    /// <param name="nextScene"></param>
    private void OnActiveSceneChanged(Scene prevScene,Scene nextScene) {
        SceneFade(0.5f,false);
    }//OnActiveSceneChange

    /// <summary>
    /// フェードアウトを行う処理
    /// </summary>
    /// <param name="time"></param>
    /// <param name="fadeout"></param>
    private void SceneFade(float time, bool fadeout) {
        _gameManager.GetComponent<SceneFade>().StartFade(time, fadeout);
    }//SceneFade

}//MainChangeScene
