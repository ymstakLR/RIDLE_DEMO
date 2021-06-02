using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 初めのボタンカーソル選択処理
/// 更新日時:0603
/// </summary>
public class SelectFirstCursor : MonoBehaviour {

    private void Start() {//OnEnableだと反映されない(0408)
        GetComponent<Selectable>().Select();
    }//Start
}//SelectFirstCursor
