using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 重要となるアイテムの出現場所をランダムにする
/// 更新日時:0417
/// </summary>
public class KeyItemPositionChange : MonoBehaviour {

    private List<GameObject> _gameObjectList = new List<GameObject>();
    private List<Vector3> _positionList = new List<Vector3>();

    void Start() {
        //配列に要素取得
        foreach(Transform item in this.gameObject.transform) {
            _gameObjectList.Add(item.gameObject);
            _positionList.Add(item.transform.position);
        }//foreach

        for(int i = 0; i< 5; i++) {
            int randomNum = Random.Range(0, _gameObjectList.Count);//ランダム要素取得
            _gameObjectList[randomNum].transform.position = _positionList[0];//要素反映
            _gameObjectList.RemoveAt(randomNum);
            _positionList.RemoveAt(0);
        }//for

    }//Start

}//KeyItemPositionChange
