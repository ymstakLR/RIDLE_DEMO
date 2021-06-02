using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アイテム情報の取得・反映処理
/// 更新日時:0602
/// </summary>
public class ItemManager : MonoBehaviour {

    private Score _uiScore;
    private ItemProperty _itemProperty;
    private RotatingArrow _rotatingArrow;
    private GameObject _canvasUI;
    private AudioManager _audioManager;

    private string _itemName;

    private void Start() {
        _uiScore = GameObject.Find("UI/UIText/ScoreNumText").GetComponent<Score>();
        _itemProperty = GameObject.Find("UI").GetComponent<ItemProperty>();
        _canvasUI = GameObject.Find("UI/UIItemReference").gameObject;
        _rotatingArrow = _canvasUI.transform.Find("ArrowToGoal").GetComponent<RotatingArrow>();
        _audioManager = GameObject.Find("GameManager").GetComponent<AudioManager>();
        _itemName = this.gameObject.name;
    }//Start

    /// <summary>
    /// 自機がアイテムを取得したときの判定
    /// </summary>
    public void PlayerGetItem() {
        Destroy(this.gameObject);
        //UIと取得オブジェクトを等しくさせる（ともに同じ名前にする必要あり）
        foreach (Transform childTransform in _canvasUI.transform) {
            if (childTransform.gameObject.name == _itemName) {
                childTransform.gameObject.GetComponent<Image>().enabled = true;
                break;
            }//if
        }//foreach
        ItemScore();
    }//GetItem

    /// <summary>
    /// アイテムのスコアを反映させる
    /// </summary>
    private void ItemScore() {
        string omissionItemName;
        omissionItemName = _itemName.Substring(0, 3);
        switch (omissionItemName) {
            case "Key":
                _audioManager.PlaySE("KeyGet");
                _rotatingArrow.IsTouchKey = true;
                break;
            case "Pla":
                _uiScore.AddScore(appScorePoint: 1000);
                break;
            case "Spe":
                _audioManager.PlaySE("SpecialItemGet");
                _uiScore.AddScore(appScorePoint: 500);
                _itemProperty.SpecialItem += 1;
                break;
        }//switch
    }//ItemScore

}//ItemManager
