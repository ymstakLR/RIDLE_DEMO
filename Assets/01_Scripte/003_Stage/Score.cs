using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// メインゲーム中のスコア表示処理
/// 更新日時:0602
/// </summary>
public class Score : MonoBehaviour {
    private int _scoreNum;
    public int ScoreNum { get { return _scoreNum; } }

    private void Start() {
        this.GetComponent<Text>().text = _scoreNum.ToString();
    }//Start

    /// <summary>
    /// スコアの更新を行う処理
    /// </summary>
    /// <param name="appScorePoint"></param>
    public void AddScore(int appScorePoint) {//ポイントの追加
        _scoreNum += appScorePoint;//スコアを増加させる
        this.GetComponent<Text>().text = _scoreNum.ToString();
    }//AddPoint

}//Score
