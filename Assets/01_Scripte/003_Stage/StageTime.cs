using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステージ記載のタイムを更新する処理
/// 更新日時:0415
/// </summary>
public class StageTime : MonoBehaviour {
    private Text _timeTextS;
    private Text _timeTextM;
    private ItemProperty _itemProperty;
    private StageStatusManagement _stageClearManagement;

    private int _calculationTimeM;

    private float _calculationTimeS;

    private string _beforeTimeS;
    private string _displeyTime;

    void Start() {
        _timeTextS = this.transform.Find("TimeNumS").GetComponent<Text>();
        _timeTextM = this.transform.Find("TimeNumM").GetComponent<Text>();
        _itemProperty = GameObject.Find("UI").GetComponent<ItemProperty>();
        _stageClearManagement = GameObject.Find("Stage").GetComponent<StageStatusManagement>();

        _displeyTime = "00";
        _timeTextS.text = _displeyTime.ToString();
    }//Start


    void Update() {
        if (_stageClearManagement.StageStatus == EnumStageStatus.Normal ||
            _stageClearManagement.StageStatus == EnumStageStatus.BossBattle) {
            _calculationTimeS += Time.deltaTime;
        }//if
        DisplayTimeUpdateJudge();
    }//Update

    /// <summary>
    /// 画面のタイム更新処理の実施判定処理
    /// </summary>
    private void DisplayTimeUpdateJudge() {
        _displeyTime = ((int)_calculationTimeS).ToString();//小数点の削除
        if (_displeyTime == _beforeTimeS)
            return;
        DisplayTimeUpdate();
    }//DisplayTimeUpdateJudge

    /// <summary>
    /// 画面のタイム更新処理
    /// </summary>
    private void DisplayTimeUpdate() {
        TimeMinutesUpdate();
        TimeSecondUpdate();
        _itemProperty.StageTime = int.Parse(_timeTextM.text + _timeTextS.text);
    }//DisplayTimeUpdate

    /// <summary>
    /// 分単位の数値を更新する処理
    /// </summary>
    private void TimeMinutesUpdate() {
        if (_calculationTimeM == 59 && _displeyTime == "59")
            return;
        if (_displeyTime != "60")
            return;
        _displeyTime = "00";
        _calculationTimeS = 0;
        _calculationTimeM += 1;
        if (_calculationTimeM.ToString().Length == 1) {
            _timeTextM.text = "0" + _calculationTimeM;
        } else {
            _timeTextM.text = _calculationTimeM.ToString();
        }
    }//TimeMinutesUpdate

    /// <summary>
    /// 秒単位の数値を更新する処理
    /// </summary>
    private void TimeSecondUpdate() {
        _beforeTimeS = _displeyTime;
        if (_displeyTime.Length == 1) {
            _displeyTime = "0" + _displeyTime;
        }//if
        _timeTextS.text = _displeyTime.ToString();
    }//TimeSecondUpdate

}//StageTime
