using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ開始時の演出処理
/// 更新日時:0616
/// </summary>
public class StageStartInitialize : MonoBehaviour {
    private Transform _titleText;
    private StageStatusManagement _stageClearMgmt;

    private float _waitTimer;

    private void Awake() {

        _titleText = this.transform.Find("Text");
        _stageClearMgmt = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
        //Time.timeScale = 0f;
        _titleText.localPosition = new Vector3(800, 0,0);
    }//Awake

    private void FixedUpdate() {
        //テキストのみスクロール
        if (_titleText.localPosition.x > 0) {
            _titleText.localPosition = new Vector3(_titleText.localPosition.x - 15f, 0, 0);
            return;
        }//if
        //一定時間停止
        if (_waitTimer < 2) {
            _waitTimer += Time.deltaTime;
            return;
        }//if
        //テキスト・画像のスクロール
        if (this.transform.localPosition.x > -800) {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x - 15f, 0, 0);
        } else {
            Time.timeScale = 1f;
            _stageClearMgmt.StageStatus = EnumStageStatus.Normal;
            Destroy(this);
        }//if
    }

    private void Update() {

        
    }//Update

}//StageStartInitialize
