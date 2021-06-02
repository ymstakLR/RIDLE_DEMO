using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ボタンUIの取得とボタンの情報更新処理
/// 更新処理:0603
/// </summary>
public class ButtonInfo : MonoBehaviour {

    private GameObject _buttonCanvas;

    private void Awake() {
        ButtonCanvasSearch(SceneManager.GetActiveScene().GetRootGameObjects());
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }//Awake


    /// <summary>
    /// シーン切り替えごとに更新する処理を記述する
    /// </summary>
    /// <param name="prevScene"></param>
    /// <param name="nextScene"></param>
    private void OnActiveSceneChanged(Scene prevScene,Scene nextScene) {
        ButtonCanvasSearch(SceneManager.GetActiveScene().GetRootGameObjects());
    }//OnActiveSceneChanged

    /// <summary>
    /// シーン上(Hierarchy直下)のButtonCanvasを探す処理
    /// </summary>
    /// <param name="searchField"></param>
    private void ButtonCanvasSearch(GameObject[] searchField) {
        foreach (GameObject child in searchField) {
            if(child.name == "ButtonCanvas") {
                _buttonCanvas = child.gameObject;
                return;
            } else {
                ButtonCanvasSearch(child.gameObject);
            }//if
        }//foreach
    }//ButtonCanvasSearch

    /// <summary>
    /// 子オブジェクト内からButtonCanvasを探す処理
    /// </summary>
    /// <param name="searchField"></param>
    private void ButtonCanvasSearch(GameObject searchField) {
        foreach (Transform child in searchField.transform) {
            if (child.name == "ButtonCanvas") {
                _buttonCanvas = child.gameObject;
                return;
            } else {
                ButtonCanvasSearch(child.gameObject);
            }//if
        }//foreach
    }//ButtonCanvasSearch


    /// <summary>
    /// 各種ボタンの有効か無効化変更処理
    /// </summary>
    /// <param name="isInteractable"></param>
    public void ButtonInteractable(bool isInteractable) {
        foreach(Transform button in _buttonCanvas.transform) {
            if (button.name.Contains("Button")) {
                button.GetComponent<Button>().interactable = isInteractable;
            }else if (button.name.Contains("Slider")) {
                button.GetComponent<Slider>().interactable = isInteractable;
            }//if
        }//foreach
    }//ButtonInteractable

}//ButtonInfo

