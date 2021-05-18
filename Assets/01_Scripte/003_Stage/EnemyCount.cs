using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 敵総数の初期化と更新を行う処理
/// 更新日時:0414
/// </summary>
public class EnemyCount : MonoBehaviour {
    private GameObject _enemyInfo;//敵をprefab化したときにこのオブジェクトの子オブジェクトにしないといけない(7/27)
    private ItemProperty _itemProperty;//処理が確定したら直接指定にする

    private void Start() {
        _enemyInfo = GameObject.Find("EnemyInfo");
        _itemProperty = GameObject.Find("UI").GetComponent<ItemProperty>();
        EnemyCountInit();
    }//Start

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void EnemyCountInit() {
        _itemProperty.EnemyNum = 0;//初期化用
        foreach (Transform childTransform in _enemyInfo.transform) {
            _itemProperty.EnemyNum += 1;
        }//foreach
        _itemProperty.EnemyNumMax = _itemProperty.EnemyNum;
        this.GetComponent<Text>().text = 0 + "/" + _itemProperty.EnemyNumMax;
        _itemProperty.EnemyNum = 0;//初期化用
    }//EnemyCountInit

    /// <summary>
    /// 敵がミスしたときに残りの敵の数を更新する
    /// </summary>
    public void EnemyCountDecrease() {
        _itemProperty.EnemyNum += 1;
        this.GetComponent<Text>().text = _itemProperty.EnemyNum + "/" + _itemProperty.EnemyNumMax;
    }//EnemyCountDecrease

}//EnemyCount
