using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ステージデータの情報を表示させるための処理(ステージ選択画面で使用する)
/// 更新日時:0503
/// </summary>
public class StageDataDisp : MonoBehaviour {

    private SaveDataManager _saveDataManager;

    private void Start() {
        _saveDataManager = GameObject.Find("GameManager").GetComponent<SaveDataManager>();
        StageDataSearch();
    }//Start

    /// <summary>
    /// ステージデータ情報を探す処理
    /// </summary>
    private void StageDataSearch() {//ここの処理はネストが深いので浅くできる方法を検討する(0503)
        foreach (EnumSaveKey key in System.Enum.GetValues(typeof(EnumSaveKey))) {
            foreach (Transform childText in this.transform) {
                if (key.ToString() == childText.name.ToString()) {
                    StageDataUpdate(key,childText.GetComponent<Text>());
                }//if
            }//foreach
        }//foreach
    }//StageDataSearch

    /// <summary>
    /// ステージデータ情報を更新する
    /// </summary>
    /// <param name="key">データを保存するためのキー</param>
    /// <param name="updateText">更新するテキスト</param>
    private void StageDataUpdate(EnumSaveKey key,Text updateText) {
        if (updateText.name.ToString().Contains("Score")) {
            updateText.text = PlayerPrefs.GetString(key.ToString(), "Score:00000");
        } else if (updateText.name.Contains("Rank")) {
            updateText.text = PlayerPrefs.GetString(key.ToString(), "E");
        }//if
        _saveDataManager.DataSave(key.ToString(), updateText.text);
    }//StageDataUpdate

}//StageDataDisp
