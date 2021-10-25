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
        private static InputManager inputManager;
        private static KeyConfigSetting instance;

        public static KeyConfigSetting Instance {
            get {
                if (inputManager == null)
                    inputManager = InputManager.Instance;
                return instance = instance != null ? instance : new KeyConfigSetting();
            }
        }

        /// <summary>
        /// デフォルトのキー設定を適用する
        /// </summary>
        public void SetDefaultKeyConfig() {
            foreach (var key in Key.AllKeyData) {
                inputManager.keyConfig.SetKey(key.String, key.DefaultKeyCode);
            }
        }

    }
    #endregion InnerClass

    /// <summary>
    /// 使用するキーコンフィグ
    /// </summary>
    public Config keyConfig = new Config();

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

}