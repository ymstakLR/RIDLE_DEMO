//参考URL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using MBLDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 入力を管理する
/// </summary>
internal class InputManager : SingletonMonoBehaviour<InputManager> {
    #region InnerClass //#region 指定範囲の折り畳み機能

    /// <summary>
    /// キーコンフィグ設定の変更や保存・ロードを管理する
    /// </summary>
    internal class KeyConfigSetting {
        private Array keyCodeValues;
        private static InputManager inputManager;
        private static KeyConfigSetting instance;

        public static KeyConfigSetting Instance {
            get {
                if (inputManager == null)
                    inputManager = InputManager.Instance;
                return instance = instance != null ? instance : new KeyConfigSetting();
            }
        }

        private KeyConfigSetting() {
        }

        /// <summary>
        /// 呼び出されたフレームで押下状態のKeyCodeリストを返す
        /// </summary>
        /// <returns>押下状態のKeyCodeリスト</returns>
        private List<KeyCode> GetCurrentInputKeyCode() {
            List<KeyCode> ret = new List<KeyCode>();
            if (keyCodeValues == null) {
                keyCodeValues = Enum.GetValues(typeof(KeyCode));
            } 
            foreach (var code in keyCodeValues) {
                if (Input.GetKey((KeyCode)(int)code)) {
                    //Debug.Log("code_" + code);//キー入力した値
                    ret.Add((KeyCode)(int)code);
                } 
            }
            return ret;
        }



        /// <summary>
        /// コンフィグから値を消去
        /// </summary>
        /// <param name="key">キーを表す識別子</param>
        /// <returns>値の消去が正常に終了したかどうか</returns>
        public bool RemoveKey(Key key) {
            return inputManager.keyConfig.RemoveKey(key.String);
        }

        /// <summary>
        /// デフォルトのキー設定を適用する
        /// </summary>
        public void SetDefaultKeyConfig() {
            foreach (var key in Key.AllKeyData) {
                SetKey(key, key.DefaultKeyCode);
            }
        }

        /// <summary>
        /// コンフィグにキーをセットする
        /// </summary>
        /// <param name="key">キーを表す識別子</param>
        /// <param name="keyCode">割り当てるキーコード</param>
        /// <returns>割り当てが正常に終了したかどうか</returns>
        public bool SetKey(Key key, List<KeyCode> keyCode) {
            return inputManager.keyConfig.SetKey(key.String, keyCode);
        }

    }

    #endregion InnerClass

    /// <summary>
    /// 使用するキーコンフィグ
    /// </summary>
    public KeyConfig keyConfig = new KeyConfig();

    public void Awake() {
        //最初はデフォルトの設定をコンフィグに格納
        KeyConfigSetting.Instance.SetDefaultKeyConfig();
        //コンフィグファイルがあれば読み出す
        try {
            InputManager.Instance.keyConfig.LoadKeyConfigFile();
        } catch (IOException e) {
            Debug.Log(e.Message);
            InputManager.Instance.keyConfig.SaveKeyConfigFile();
        }
    }

    public void Update() {
    }




    /// <summary>
    /// 指定したキーが押下状態かどうかを返す
    /// </summary>
    /// <returns>入力状態</returns>
    internal bool GetKey(Key key) {
        return keyConfig.GetKey(key.String);
    }

    /// <summary>
    /// 指定したキーが入力されたかどうかを返す
    /// </summary>
    /// <returns>入力状態</returns>
    internal bool GetKeyDown(Key key) {
        return keyConfig.GetKeyDown(key.String);
    }

    /// <summary>
    /// 指定したキーが離されたかどうかを返す
    /// </summary>
    /// <returns>入力状態</returns>
    internal bool GetKeyUp(Key key) {
        return keyConfig.GetKeyUp(key.String);
    }

}