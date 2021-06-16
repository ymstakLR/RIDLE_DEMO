using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// シーンのフェードについての処理
/// 更新日時:0616
/// </summary>
public class SceneFade : MonoBehaviour {
    private Image _fadeOutImage;

    public float _fadeTimeMag;//フェードにかかる時間倍率

    public bool _isFade;//フェードの演出をするかどうか
    private bool _isFadeOut;//フェードアウトをするかどうか

    private void Start() {
        _fadeOutImage = this.transform.Find("Canvas").transform.Find("Image").GetComponent<Image>();
    }//Start

    private void FixedUpdate() {
        if (!_isFade)
            return;
        FadeJudge();
    }//Update

    /// <summary>
    /// フェードイン・フェードアウトの判定処理
    /// </summary>
    private void FadeJudge() {
        if (_isFadeOut) {
            FadeOut();
        } else {
            FadeIn();
        }//if
    }//FadeJudge

    /// <summary>
    /// フェードアウト処理
    /// </summary>
    private void FadeOut() {
        if (_fadeOutImage.color.a <= 1) {
            _fadeOutImage.color = new Color(0, 0, 0, _fadeOutImage.color.a + 1 / (60 * _fadeTimeMag));
            return;
        }//if
        _isFade = false;
        Time.timeScale = 1;
    }//FadeOut

    /// <summary>
    /// フェードイン処理
    /// </summary>
    private void FadeIn() {
        if (_fadeOutImage.color.a >= 0) {
            _fadeOutImage.color = new Color(0, 0, 0, _fadeOutImage.color.a - 1 / (60 * _fadeTimeMag));
            return;
        }//if
        _isFade = false;
    }//FadeIn

    /// <summary>
    /// フェードを始めるための処理
    /// </summary>
    /// <param name="fadeTimeMag">フェードにかける時間倍率</param>
    /// <param name="isFadeOut">フェードアウトを行うかの判定</param>
    public void StartFade(float fadeTimeMag, bool isFadeOut) {
        _fadeTimeMag = fadeTimeMag;
        _isFadeOut = isFadeOut ? true : false;
        if (_isFadeOut) {
            _fadeOutImage.color = new Color(0, 0, 0, 0);
        } else {
            _fadeOutImage.color = new Color(0, 0, 0, 1);
        }//if   
        _isFade = true;
    }//StartFade

}//SceneFade
