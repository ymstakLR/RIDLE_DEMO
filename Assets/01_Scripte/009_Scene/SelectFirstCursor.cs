using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 初めのボタンカーソル選択処理
/// 更新日時:0419
/// </summary>
public class SelectFirstCursor : MonoBehaviour {

    private void Start() {//OnEnableだと反映されない(0408)
        GetComponent<Selectable>().Select();
        Debug.LogWarning("Disp__GameObject_" + this.gameObject.name);
    }//Start
}//SelectFirstCursor
