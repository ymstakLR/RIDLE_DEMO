using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 初めのボタンカーソル選択処理
/// 更新日時:0603
/// </summary>
public class SelectFirstCursor : MonoBehaviour {

    private readonly float CURSOR_INPUT_COUNTER = 2;//

    private float _cursorInputCounter;

    private void Update() {
        CorsorInputCheck();
    }//Update

    /// <summary>
    /// カーソルのボタン入力の有効化をチェックする
    /// </summary>
    private void CorsorInputCheck() {
        ///Updateで2回以上回す必要がある
        ///2回以下だとボタンを押しっぱなしで次シーンに遷移してしまうから。(20210915)
        if (_cursorInputCounter == CURSOR_INPUT_COUNTER) {
            GetComponent<Selectable>().Select();
        }//if
        _cursorInputCounter++;
    }//CorsorInputCheck

}//SelectFirstCursor
