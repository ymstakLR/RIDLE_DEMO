using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// オプション画面の各種ボタン処理
/// 更新日時:0728
/// </summary>
public class OptionsButtonMove : MonoBehaviour {

    private AudioManager _audioManager;

    
    private Slider _bgmSlider;
    private Slider _seSlider;

    //画面サイズ変更関連
    private GameObject _numericalValueButton;
    private Text _numericalValueText;
    private GameObject _resolutionChange;
    private GameObject _resolutionChangeFirstCursor;

    //全画面変更関連
    private Text _fullScreenText;
    private bool _isFullScreen;

    private bool _isInit;//この変数はなく世相

    //データ削除関連
    private GameObject _dataDeleteButton;
    private GameObject _dataDeleteCheack;
    private GameObject _dataDeleteCheackFirstCursor;

    private void Start() {
        //変数宣言
        _audioManager = GameObject.Find("GameManager").GetComponent<AudioManager>();

        _bgmSlider = this.transform.Find("BGMSlider").GetComponent<Slider>();
        _seSlider = this.transform.Find("SESlider").GetComponent<Slider>();
        _numericalValueButton = this.transform.Find("NumericalValueButton").gameObject;
        _numericalValueText = _numericalValueButton.transform.Find("Text").GetComponent<Text>();
        _resolutionChange = this.transform.Find("ResolutionChange").gameObject;
        _resolutionChangeFirstCursor = _resolutionChange.transform.Find("1280720Button").gameObject;
        _fullScreenText = this.transform.Find("FULLSCREENButton/Text").GetComponent<Text>();

        _dataDeleteButton = this.transform.Find("DATADELETEButton").gameObject;
        _dataDeleteCheack = this.transform.Find("DataDeleteCheack").gameObject;
        _dataDeleteCheackFirstCursor = _dataDeleteCheack.transform.Find("NOButton").gameObject;

        OptionInit();
    }//Start

    /// <summary>
    /// オプション画面起動時の情報更新処理
    /// </summary>
    private void OptionInit() {
        _resolutionChange.SetActive(false);
        _dataDeleteCheack.SetActive(false);
        _bgmSlider.value = _audioManager.BGMAudio.volume;
        BarActive(_audioManager.BGMAudio, _bgmSlider);
        _seSlider.value = _audioManager.SEAudio.volume;
        BarActive(_audioManager.SEAudio, _seSlider);
        _numericalValueText.text = Screen.width + "×" + Screen.height;
        if (Screen.fullScreen) {
            _fullScreenText.text = "ON";
        } else {
            _fullScreenText.text = "OFF";
        }//if
        _isInit = true;
    }//OptionInit


    /// <summary>
    /// BGM,SEの音量変更処理
    /// </summary>
    /// <param name="slider"></param>
    public void VolumeChange(Slider slider) {
        switch (slider.gameObject.name) {
            case "BGMSlider":
                _audioManager.BGMAudio.volume = slider.value;
                _audioManager.BGMSettingVolume = _audioManager.BGMAudio.volume;
                OptionDataEdit.BGMVolumeUpadate(_audioManager.BGMAudio.volume);
                BarActive(_audioManager.BGMAudio, slider);
                break;
            case "SESlider":
                _audioManager.SEAudio.volume = slider.value;
                OptionDataEdit.SEVolumeUpdate(_audioManager.SEAudio.volume);
                BarActive(_audioManager.SEAudio, slider);
                if (_isInit) {
                    _audioManager.PlaySE("ButtonClick");
                }//if                   
                break;
        }//switch 
    }//VolumeChange

    /// <summary>
    /// スライダーの表示・非表示判定処理
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="slider"></param>
    private void BarActive(AudioSource audio,Slider slider) {
        if(audio.volume < 0.1) {
            slider.transform.GetChild(1).gameObject.SetActive(false);
            audio.volume = 0;
            slider.value = 0;
        } else {
            slider.transform.GetChild(1).gameObject.SetActive(true);
        }//if
    }//BarActive

    /// <summary>
    /// 画面比率変更画面の起動時処理
    /// </summary>
    public void ResolutionButton() {
        _audioManager.PlaySE("ButtonClick");
        _resolutionChange.SetActive(true);
        _resolutionChangeFirstCursor.GetComponent<Selectable>().Select();
    }//ResolutionButton

    /// <summary>
    /// 画面比率の変更処理
    /// </summary>
    /// <param name="clickButton"></param>
    public void ResolutionChangeButton(GameObject clickButton) {
        _audioManager.PlaySE("ButtonClick");
        //表示テキストの更新
        Text changeNumText = clickButton.transform.GetChild(0).GetComponent<Text>();
        int xSize = int.Parse(changeNumText.text.ToString().Substring(0, 4));
        int ySize = int.Parse(changeNumText.text.ToString().Substring(5, changeNumText.text.Length - 5));
        _numericalValueText.text = xSize + "×" + ySize;
        //データ保存と反映
        OptionDataEdit.ResolutionUpdate(xSize, ySize);
        ResolutionChangeCancel(clickButton);
    }//ResolutionChangeButton

    /// <summary>
    /// 画面比率変更画面から戻る処理
    /// </summary>
    /// <param name="clickButton"></param>
    public void ResolutionChangeCancel(GameObject clickButton) {
        _resolutionChange.SetActive(false);
        clickButton.transform.localScale = new Vector3(1, 1, 1);
        _numericalValueButton.GetComponent<Selectable>().Select();
    }//ResolutionChangeCancel

    /// <summary>
    /// フルスクリーンへの反映処理
    /// </summary>
    public void FullScreenButton() {
        _audioManager.PlaySE("ButtonClick");
        if (_isFullScreen) {
            Debug.Log("2");
            _isFullScreen = false;
            _fullScreenText.text = "OFF";
        } else {
            Debug.Log("1");
            _isFullScreen = true;
            _fullScreenText.text = "ON";
        }//if
        OptionDataEdit.FullScreenCheackUpdate(_isFullScreen);
    }//FullScreenButton

    /// <summary>
    /// データ削除ボタンをクリックしたときの処理
    /// </summary>
    public void DataDeleteButton() {
        _audioManager.PlaySE("ButtonClick");
        _dataDeleteCheack.SetActive(true);
        _dataDeleteCheackFirstCursor.GetComponent<Selectable>().Select();
    }//DataDeleteButton

    /// <summary>
    /// データ削除処理
    /// </summary>
    /// <param name="clickButton">クリックしたボタン</param>
    public void DataDelete(GameObject clickButton) {
        _audioManager.PlaySE("ButtonClick");
        SaveManager.StageDataDelete();
        DataDeleteCancel(clickButton);
    }//DataDelete

    /// <summary>
    /// データ削除のキャンセル処理
    /// </summary>
    /// <param name="selectButton">選択しているボタン</param>
    public void DataDeleteCancel(GameObject selectButton) {
        _dataDeleteCheack.SetActive(false);
        selectButton.transform.localScale = new Vector3(1, 1, 1);
        _dataDeleteButton.GetComponent<Selectable>().Select();
    }//DataDeleteCancel

}//OptionsButtonMove
