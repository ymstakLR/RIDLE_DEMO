using ConfigDataDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// リザルト画面全般の処理
/// 更新日時:0728
/// </summary>
public class Result : MonoBehaviour {
    private enum RankValue { A, B, C, D, E }//ランク更新用の数値列挙体

    [SerializeField]
    private int optimalClearTime;

    private AudioManager _audioManager;
    private GameObject _goalForward;
    private GameObject _arrow;
    private ItemProperty _itemProperty;
    private Score _score;
    private StageStatusManagement _stageClearManagement;
    private Transform _resultTextTransform;
    /// <summary>
    /// Text
    /// </summary>
    private Text _timeText;
    private Text _timeRank;
    private Text _enemyText;
    private Text _enemyRank;
    private Text _specialItemText;
    private Text _specialItemRank;
    private Text _damegeText;
    private Text _damageRank;
    private Text _overallRank;

    private float _resultTextPositionX;

    private string _stageTime;

    private bool _isSupporterMove;
    public bool IsSupporterMove { get { return _isSupporterMove; } }

    private bool _isResultEnd;
    private bool _isResultInit;
    private bool _isResultBGM;
    private bool _isSceneBack;

    private void Start() {
        _arrow = GameObject.Find("UI/UIItemReference/ArrowToGoal");
        _audioManager = GameObject.Find("GameManager").GetComponent<AudioManager>();
        _goalForward = GameObject.Find("Goal/Goal_Forward");
        _itemProperty = GameObject.Find("UI").GetComponent<ItemProperty>();
        _score = GameObject.Find("UI/UIText/ScoreNumText").GetComponent<Score>();
        _stageClearManagement = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
        
        ///text
        _timeText = this.transform.Find("Time").GetComponent<Text>();
        _timeRank = this.transform.Find("TimeRank").GetComponent<Text>();
        _enemyText = this.transform.Find("EnemyCount").GetComponent<Text>();
        _enemyRank = this.transform.Find("EnemyCountRank").GetComponent<Text>();
        _specialItemText = this.transform.Find("SpecialItemCount").GetComponent<Text>();
        _specialItemRank = this.transform.Find("SpecialItemCountRank").GetComponent<Text>();
        _damegeText = this.transform.Find("DamageCount").GetComponent<Text>();
        _damageRank = this.transform.Find("DamageCountRank").GetComponent<Text>();
        _overallRank = this.transform.Find("OverallRank").GetComponent<Text>();

        _resultTextTransform = this.GetComponent<Transform>();
        _resultTextPositionX = _resultTextTransform.localPosition.x;
    }//Start

    private void FixedUpdate() {
        StageStatus();
    }//FixedUpdate

    private void Update() {
        StageEndStatusJudge();
    }//Update

    /// <summary>
    /// StageStatusによる切り替え処理
    /// </summary>
    private void StageStatus() {
        switch (_stageClearManagement.StageStatus) {
            case EnumStageStatus.Normal:
            case EnumStageStatus.GoalMove:
                _goalForward.GetComponent<Animator>().SetBool("AniLock", false);
                break;
            case EnumStageStatus.BeforeBossBattle:
                _goalForward.GetComponent<Animator>().SetBool("AniLock", true);
                break;
            case EnumStageStatus.Clear:
                if (!_isResultInit) {
                    ResultInit();
                }//if
                _goalForward.GetComponent<SpriteRenderer>().enabled = false;
                _isSupporterMove = true;
                StartCoroutine(ResultTextMoveCorutine());
                break;
        }//switch
    }//StageStatus

    /// <summary>
    /// リザルトテキストを動かす処理
    /// </summary>
    /// <returns></returns>
    IEnumerator ResultTextMoveCorutine() {
        yield return new WaitForSeconds(2.5f);
        if (_resultTextTransform.localPosition.x < -1200) {
            _audioManager.PlayBGM("Clear");
            _audioManager.LoopBGM(isLoop: false);
            _isResultBGM = true;
        }//if
        if (_resultTextTransform.localPosition.x < -800) {
            _resultTextPositionX += (float)8;
            _resultTextTransform.localPosition = new Vector2(
                _resultTextPositionX, _resultTextTransform.localPosition.y);
        } else {
            StartCoroutine(OverallRankDisplayCorutine());
        }//if
    }//ResultTextMove

    /// <summary>
    /// 総合ランクの表示処理
    /// </summary>
    /// <returns></returns>
    IEnumerator OverallRankDisplayCorutine() {
        yield return new WaitForSeconds(1.5f);
        _overallRank.enabled = true;
        StartCoroutine(StageEscapeButtonCorutine());
    }//OverallRankDisplayCorutine

    /// <summary>
    /// ステージ終了ボタンの反映処理
    /// </summary>
    /// <returns></returns>
    IEnumerator StageEscapeButtonCorutine() {
        yield return new WaitForSeconds(2f);
        _isResultEnd = true;
    }//StageEscapeButtonCorutine

    /// <summary>
    /// ステージ終了状態判定処理
    /// </summary>
    private void StageEndStatusJudge() {
        if (!_isResultEnd)
            return;
        StageEndStatus();
    }//StageEndJudge

    /// <summary>
    /// ステージを終了できる状態にする処理
    /// </summary>
    private void StageEndStatus() {
        if (!ConfigManager.Instance.config.GetKeyDown(ConfigData.NormalJump.String) &&
            !ConfigManager.Instance.config.GetKeyDown(ConfigData.FlipJump.String) &&
            !ConfigManager.Instance.config.GetKeyDown(ConfigData.Attack.String))
            return;
        if (!_isSceneBack) {
            GameObject.Find("GameManager").GetComponent<SceneChange>().BackSceneChange(isBackSE: false);
            _isSceneBack = true;
        }//if
    }//StageEnd

    /// <summary>
    /// リザルト時の初期化処理
    /// </summary>
    private void ResultInit() {
        _arrow.GetComponent<RotatingArrow>().enabled = false;
        _arrow.GetComponent<SpriteRenderer>().enabled = false;
        TextUpdate();
        RankUpdate();
        _isResultInit = true;
    }//ResultInit

    /// <summary>
    /// リザルト画面のテキスト更新処理
    /// </summary>
    private void TextUpdate() {
        //タイム
        _stageTime = _itemProperty.StageTime.ToString();
        if (_itemProperty.StageTime.ToString().Length > 2) {
            _stageTime = _itemProperty.StageTime.ToString().Insert(_itemProperty.StageTime.ToString().Length - 2, ":");
        }//if
        _timeText.text = _stageTime;
        
        //救出エネミー
        _enemyText.text = _itemProperty.EnemyNum + "/" + _itemProperty.EnemyNumMax.ToString();
        
        //取得スペシャルアイテム
        _specialItemText.text = _itemProperty.SpecialItem.ToString() + "/4  ";

        //ダメージ数
        _damegeText.text = _itemProperty.PlayerMissCount.ToString();
    }//TextUpdate

    /// <summary>
    /// ステージのランクを更新する処理
    /// </summary>
    private void RankUpdate() {
        int overallRankNum = 0;

        //ステージタイム
        if(optimalClearTime > _itemProperty.StageTime) {
            _timeRank.text = "A";
            overallRankNum += 4;
        } else if(optimalClearTime *1.25 > _itemProperty.StageTime) {
            _timeRank.text = "B";
            overallRankNum += 3;
        } else if(optimalClearTime * 1.5 > _itemProperty.StageTime) {
            _timeRank.text = "C";
            overallRankNum += 2;
        } else {
            _timeRank.text = "D";
            overallRankNum += 1;
        }//if

        //救出エネミー
        float enemyRankNum = (float)_itemProperty.EnemyNum / (float)_itemProperty.EnemyNumMax;
        if (enemyRankNum > 0.9) {
            _enemyRank.text = "A";
            overallRankNum += 4;
        } else if(enemyRankNum > 0.75) {
            _enemyRank.text = "B";
            overallRankNum += 3;
        } else if(enemyRankNum > 0.45) {
            _enemyRank.text = "C";
            overallRankNum += 2;
        } else {
            _enemyRank.text = "D";
            overallRankNum += 1;
        }//if

        //スペシャルアイテム
        switch (_itemProperty.SpecialItem) {
            case 4:
                _specialItemRank.text = "A";
                overallRankNum += 4;
                break;
            case 3:
                _specialItemRank.text = "B";
                overallRankNum += 3;
                break;
            case 2:
            case 1:
                _specialItemRank.text = "C";
                overallRankNum += 2;
                break;
            default:
                _specialItemRank.text = "D";
                overallRankNum += 1;
                break;
        }//switch

        //ダメージ数
        switch (_itemProperty.PlayerMissCount) {
            case 0:
            case 1:
                _damageRank.text = "A";
                overallRankNum += 4;
                break;
            case 2:
            case 3:
                _damageRank.text = "B";
                overallRankNum += 3;
                break;
            case 4:
            case 5:
                _damageRank.text = "C";
                overallRankNum += 2;
                break;
            default:
                _damageRank.text = "D";
                overallRankNum += 1;
                break;
        }//switch

        //総合ランク
        if (overallRankNum > 13) {
            _overallRank.text = "A";
        }else if(overallRankNum > 10) {
            _overallRank.text = "B";
        } else if(overallRankNum > 5) {
            _overallRank.text = "C";
        } else {
            _overallRank.text = "D";
        }//if
        StageDataUpdate();
    }//RankUpdate

    /// <summary>
    /// ステージクリア時の情報の更新処理
    /// </summary>
    private void StageDataUpdate() {
        string sceneName = SceneManager.GetActiveScene().name;
        int stageNum = StageDataEdit.StageDataIdentification(sceneName);
        RankDataUpdateCheck(sceneName,stageNum);
    }//StageDataUpdate

    /// <summary>
    /// ランクデータを更新判定処理
    /// </summary>
    /// <param name="sceneName">更新するステージ名</param>
    /// <param name="stageNum">更新するリスト配列番号</param>
    private void RankDataUpdateCheck(string sceneName,int stageNum) {
        string stageRankKey = StageDataEdit._rankList[stageNum];
        int maxRank = 0;
        int playRank = 0;
        foreach (RankValue key in System.Enum.GetValues(typeof(RankValue))) {
            if (key.ToString() == _overallRank.text) {
                playRank = (int)key;
            }//if
            if (key.ToString() == stageRankKey) {
                maxRank = (int)key;
            }//if
        }//foreach
        if (playRank < maxRank) {//ランクが更新した場合
            StageDataEdit.StageDataUpdate(sceneName, _overallRank.text.ToString(), _score.ScoreNum.ToString(),TimeTextEdit());
        } else if (playRank == maxRank) {
            ScoreDataUpdateCheck(sceneName, stageNum, _overallRank.text.ToString());
        }//if
    }//RankDataUpdate

    /// <summary>
    /// スコアデータの更新判定処理
    /// </summary>
    /// <param name="sceneName">更新するステージ名</param>
    /// <param name="stageNum">更新するリスト配列番号</param>
    /// <param name="overallRank">更新するランク</param>
    private void ScoreDataUpdateCheck(string sceneName,int stageNum,string overallRank) {
        if (_score.ScoreNum > int.Parse(StageDataEdit._scoreList[stageNum])) {//スコアが更新した場合
            StageDataEdit.StageDataUpdate(sceneName, overallRank, _score.ScoreNum.ToString(),TimeTextEdit());
        } else if (_score.ScoreNum == int.Parse(StageDataEdit._scoreList[stageNum])) {
            TimeDataUpdateCheck(sceneName, stageNum, overallRank);
        }//if
    }//ScoreUpdate

    /// <summary>
    /// タイムデータの更新判定処理
    /// </summary>
    /// <param name="sceneName">更新するステージ名</param>
    /// <param name="stageNum">更新するリスト配列番号</param>
    /// <param name="overallRank">更新するランク</param>
    private void TimeDataUpdateCheck(string sceneName,int stageNum,string overallRank) {
        string saveTimeM = StageDataEdit._timeList[stageNum].Substring(0, 2);
        string saveTimeS = StageDataEdit._timeList[stageNum].Substring(3, 2);
        int saveTime = int.Parse(saveTimeM + saveTimeS);
        if (_itemProperty.StageTime < saveTime) {//タイムが更新されたとき
            StageDataEdit.StageDataUpdate(sceneName, overallRank, _score.ScoreNum.ToString(),TimeTextEdit());
        }//if
    }//TimeDataUpdateCheck

    /// <summary>
    /// タイムテキストを編集する処理 例)0123 → 01:23
    /// </summary>
    /// <returns></returns>
    private string TimeTextEdit() {
        string timeM = (_itemProperty.StageTime / 60).ToString("D2");
        string timeS = (_itemProperty.StageTime % 60).ToString("D2");
        return timeM + ":" + timeS;
    }//TimeTextEdit

}//Result
