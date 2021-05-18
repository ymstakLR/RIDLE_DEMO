using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// シーン遷移直後にBGMを再生する処理
/// 更新日時:0417
/// </summary>
public class InitialBGM: MonoBehaviour {

    [SerializeField, Tooltip("このシーンで始めに流すBGMを選択")]
    private string bgmName;

    private AudioManager _audioManager;

    private void Awake() {
        _audioManager = GameObject.Find("GameManager").GetComponent<AudioManager>();
        _audioManager.PlayBGM(bgmName);
    }//Awake
    private void Start() {
        _audioManager.BGMInit();
    }//Start

}//IntialBGM
