using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定位置でオブジェクトを生成する処理
/// 更新日時:20210914
/// </summary>
public class ObjectAppearanceManager : MonoBehaviour {
    [SerializeField, Tooltip("生成したいオブジェクト")]
    private GameObject _object;

    [SerializeField, Tooltip("オブジェクトで更新したい数値のリスト(使用する分だけ作成する)")]
    private List<float> _objectFloatList;
    public List<float> ObjectFloatList { get { return _objectFloatList; } }

    private GameObject _player;
    private StageStatusManagement _stageClearManagement;

    private int _hierarchCount;

    void Start() {
        _player = GameObject.Find("Ridle");
        _stageClearManagement = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
        GetHierarchyCount();
    }//Start

    void Update() {
        ObjectGenerateJudge();
    }//Update

    /// <summary>
    /// レイヤー数値の補正値を取得するための処理
    /// </summary>
    private void GetHierarchyCount() {
        foreach (Transform child in this.transform.parent.GetComponentsInChildren<Transform>()) {
            if (child.gameObject == this.gameObject) {
                _hierarchCount = child.GetSiblingIndex();
                return;
            }//if
        }//foreach
    }//GetHierarchyCount

    /// <summary>
    /// 対象のオブジェクトを生成する判定処理
    /// </summary>
    private void ObjectGenerateJudge() {
        //指定のステージステータスでない場合
        if ((_stageClearManagement.StageStatus != EnumStageStatus.Normal &&
            _stageClearManagement.StageStatus != EnumStageStatus.Pause))
            return;
        //カメラ範囲内の場合
        if (30 > Mathf.Abs(_player.transform.position.x - this.transform.position.x) &&
           20 > Mathf.Abs(_player.transform.position.y - this.transform.position.y))
            return;
        //生成範囲外の場合
        if (40 < Mathf.Abs(_player.transform.position.x - this.transform.position.x) ||
            30 < Mathf.Abs(_player.transform.position.y - this.transform.position.y))
            return;
        ObjectGenerate();
    }//EnemyGenarateJudge

    /// <summary>
    /// オブジェクトを生成する処理
    /// </summary>
    private void ObjectGenerate() {
        GameObject instance = (GameObject)Instantiate(_object,this.transform);
        instance.GetComponent<SpriteRenderer>().sortingOrder = instance.GetComponent<SpriteRenderer>().sortingOrder - _hierarchCount;
        this.GetComponent<ObjectAppearanceManager>().enabled = false;
    }//EnemyGenerate
}
