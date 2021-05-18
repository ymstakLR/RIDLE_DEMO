using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム開始時の初期化処理
/// 更新日時:0417
/// </summary>
public class Initialize : MonoBehaviour {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod() {
        Debug.LogWarning("------------------------------------------------------------------------------------------------------------");
        Debug.LogWarning("呼び出し順の確認");
        Debug.LogWarning("RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)");
        Debug.LogWarning("Awake[アタッチ時]");
        Debug.LogWarning("OnEnable[アタッチ時])");
    }//OnBeforeSceneLoadRuntimeMethod

    private void Awake() {
    }

    private void OnEnable() {
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnAfterSceneLoadRuntimeMethod() {
        Debug.LogWarning("RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)");
    }//OnAfterSceneLoadRuntimeMethod

    // 属性の設定
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad() {
        Debug.LogWarning("RuntimeInitializeOnLoadMethod");
        Debug.LogWarning("Start[アタッチ時]");
        Debug.LogWarning("------------------------------------------------------------------------------------------------------------");
        // スクリーンサイズの指定
        Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
    }//OnRuntimeMethodLoad

    private void Start() {
    }//Start1

}//Initialize
