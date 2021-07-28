using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 保存データを管理するための処理
/// 更新日時:0728
/// </summary>
public class GameStartInit : MonoBehaviour {

    private void Awake() {
        SaveManager.DataInit();
        AudioSet(this.GetComponent<AudioManager>());
        OptionDataEdit.SetResolution();
    }//Awake

    /// <summary>
    /// BGM,SEの音量を設定する
    /// </summary>
    /// <param name="audioManager"></param>
    private void AudioSet(AudioManager audioManager) {
        audioManager.BGMAudio.volume = SaveManager.optionData.bgmVol/10f;//Vol/10と記述すると整数(0)になる
        audioManager.SEAudio.volume = SaveManager.optionData.seVol/10f;
    }//AudioSet

}//SaveData
