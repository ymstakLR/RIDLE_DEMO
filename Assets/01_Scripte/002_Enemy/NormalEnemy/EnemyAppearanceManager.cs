using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定位置で敵を生成する処理
/// 更新日時:0409
/// </summary>
public class EnemyAppearanceManager : MonoBehaviour {
    [SerializeField,Tooltip("生成したい敵オブジェクト")]
    private GameObject _enemyObject;

    [SerializeField, Tooltip("敵オブジェクトで更新したい数値のリスト(使用する分だけ作成する)")]
    private List<float> _enemyFloatList;
    public List<float> EnemyFloatList { get { return _enemyFloatList; } }

    private GameObject _player;

    private int _hierarchCount;

    void Start() {
        _player = GameObject.Find("Ridle");
        GetHierarchyCount();
    }//Start

    void Update() {
        EnemyGenerateJudge();
    }//Update

    /// <summary>
    /// レイヤー数値の補正値を取得するための処理
    /// </summary>
    private void GetHierarchyCount() {
        foreach(Transform child in this.transform.parent.GetComponentsInChildren<Transform>()) {
            if(child.gameObject == this.gameObject) {
                _hierarchCount = child.GetSiblingIndex();
                return;
            }//if
        }//foreach
    }//GetHierarchyCount

    /// <summary>
    /// 敵を生成する判定処理
    /// </summary>
    private void EnemyGenerateJudge() {
        //カメラ範囲内の場合
        if (30 > Mathf.Abs(_player.transform.position.x - this.transform.position.x) &&
           20 > Mathf.Abs(_player.transform.position.y - this.transform.position.y))
            return;
        //生成範囲外の場合
        if (40 < Mathf.Abs(_player.transform.position.x - this.transform.position.x) ||
            30 < Mathf.Abs(_player.transform.position.y - this.transform.position.y))
            return;
        EnemyGenerate();
    }//EnemyGenarateJudge

    /// <summary>
    /// 敵を生成する処理
    /// </summary>
    private void EnemyGenerate() {
        GameObject instance = (GameObject)Instantiate(
            _enemyObject, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
        instance.transform.parent = this.transform;
        instance.GetComponent<SpriteRenderer>().sortingOrder = instance.GetComponent<SpriteRenderer>().sortingOrder - _hierarchCount;
        this.GetComponent<EnemyAppearanceManager>().enabled = false;
    }//EnemyGenerate

}//EnemyAppearanceManager
