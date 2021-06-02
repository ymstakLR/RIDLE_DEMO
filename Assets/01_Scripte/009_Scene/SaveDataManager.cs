using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// データを保存するためのキーの列挙体(基本的にこのキーを経由してデータを保存する)
/// この列挙体は使用用途によって分割したほうが、今後利用しやすいと考える(0504)
/// 更新日時:0603
/// </summary>
public enum EnumSaveKey {
    BGMVol,
    SEVol,
    Stage1MaxScore,
    Stage1MaxRank,
    Stage2MaxScore,
    Stage2MaxRank,
    Stage3MaxScore,
    Stage3MaxRank
}//saveKeyName

/// <summary>
/// 保存データを管理するための処理
/// 更新日時:0503
/// </summary>
public class SaveDataManager : MonoBehaviour {


    private void Awake() {
        AudioSet(this.GetComponent<AudioManager>());
    }//Awake

    /// <summary>
    /// BGM,SEの音量を設定する
    /// </summary>
    /// <param name="audioManager"></param>
    private void AudioSet(AudioManager audioManager) {
        audioManager.BGMAudio.volume = float.Parse(PlayerPrefs.GetString(EnumSaveKey.BGMVol.ToString(), 0.5.ToString()));
        DataSave(EnumSaveKey.BGMVol.ToString(), audioManager.BGMAudio.volume.ToString());
        audioManager.SEAudio.volume = float.Parse(PlayerPrefs.GetString(EnumSaveKey.SEVol.ToString(), 0.5.ToString()));
        DataSave(EnumSaveKey.SEVol.ToString(), audioManager.SEAudio.volume.ToString());
    }

    /// <summary>
    /// データを保存するための処理
    /// </summary>
    /// <param name="saveKeyName">データを保存するためのキー名</param>
    /// <param name="saveValue">保存する値</param>
    public void DataSave(string saveKeyName,string saveValue) {//int,float型は、string型のものを型変換して使用する
        PlayerPrefs.SetString(saveKeyName, saveValue);
        PlayerPrefs.Save();
    }//DataSave

    public void StageDataDelete() {
        for(int i = 2; i < Enum.GetValues(typeof(EnumSaveKey)).Length; i++) {
            string dataDeleteKey = Enum.ToObject(typeof(EnumSaveKey),i).ToString();
            DataDelete(dataDeleteKey);
        }//for
    }//StageDataDelete

    /// <summary>
    /// 保存しているデータを削除するための処理
    /// </summary>
    /// <param name="saveKeyName">削除するキー名</param>
    public void DataDelete(string saveKeyName) {
        PlayerPrefs.DeleteKey(saveKeyName);
        PlayerPrefs.Save();
    }//DataDelete

}//SaveData
