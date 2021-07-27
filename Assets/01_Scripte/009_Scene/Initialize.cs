using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム開始時の初期化処理
/// 更新日時:0603
/// </summary>
public class Initialize : MonoBehaviour {

    // 属性の設定
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad() {
        // スクリーンサイズの指定
        OptionDataEdit.SetResolution();
    }//OnRuntimeMethodLoad

}//Initialize
