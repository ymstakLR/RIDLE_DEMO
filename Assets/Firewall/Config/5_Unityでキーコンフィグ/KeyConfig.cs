//参考URL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using LitJson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

/// <summary>
/// キーコンフィグ情報を取り扱う
/// </summary>
public class KeyConfig {
    public static Dictionary<string, List<KeyCode>> keyConfig = new Dictionary<string, List<KeyCode>>();
    private readonly string keyConfigFilePath;


    /// <summary>
    /// キーコンフィグを管理するクラスを生成する
    /// </summary>
    /// <param name="keyConfigFilePath">コンフィグファイルのパス</param>
    public KeyConfig(string keyConfigFilePath) {
        this.keyConfigFilePath = keyConfigFilePath;
    }


    /// <summary>
    /// 指定したキーの入力状態をチェックする
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <param name="predicate">キーが入力されているかを判定する述語</param>
    /// <returns>入力状態</returns>
    private bool InputKeyCheck(string keyName, Func<KeyCode, bool> predicate) {
        bool ret = false;
        foreach (var keyCode in keyConfig[keyName])
            if (predicate(keyCode))
                return true;
        return ret;
    }

    /// <summary>
    /// 指定したキーが押下状態かどうかを返す
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <returns>入力状態</returns>
    public bool GetKey(string keyName) {
        return InputKeyCheck(keyName, Input.GetKey);
    }

    /// <summary>
    /// 指定したキーが入力されたかどうかを返す
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <returns>入力状態</returns>
    public bool GetKeyDown(string keyName) {
        return InputKeyCheck(keyName, Input.GetKeyDown);
    }

    /// <summary>
    /// 指定したキーが離されたかどうかを返す
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <returns>入力状態</returns>
    public bool GetKeyUp(string keyName) {
        return InputKeyCheck(keyName, Input.GetKeyUp);
    }

    /// <summary>
    /// 指定されたキーに割り付けられているキーコードを返す
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <returns>キーコード</returns>
    public List<KeyCode> GetKeyCode(string keyName) {
        if (keyConfig.ContainsKey(keyName))
            return new List<KeyCode>(keyConfig[keyName]);
        return new List<KeyCode>();
    }

    /// <summary>
    /// 名前文字列に対するキーコードを設定する
    /// </summary>
    /// <param name="keyName">キーに割り付ける名前</param>
    /// <param name="keyCode">キーコード</param>
    /// <returns>キーコードの設定が正常に完了したかどうか</returns>
    public bool SetKey(string keyName, List<KeyCode> keyCode) {
        //Debug.LogError("KeyConfig.cs_SetKey");
        if (string.IsNullOrEmpty(keyName) || keyCode.Count < 1)
            return false;
        keyConfig[keyName] = keyCode;
        //Debug.Log(keyConfig[keyName] + "__" + keyName);
        //Debug.Log(keyConfig[keyName][0] + "__" + keyName);
        return true;
    }

    /// <summary>
    /// 入力されたキーコードをチェックする処理
    /// </summary>
    /// <param name="keyName">チェックする動作名</param>
    /// <returns>動作名に対応するキーコード</returns>
    public KeyCode GetInputKeyCodeCheck(string keyName) {
        int i = GetCheckKeyListCount(keyName);
        int j = 0;
        foreach (List<KeyCode> li in keyConfig.Values) {
            foreach (KeyCode kc in li) {
                if (i == j) {
                    return kc;
                }
                j++;
            }//foreach
        }//foreach
        return KeyCode.None;
    }//KeyCodeCheck


    /// <summary>
    /// 対象の動作名の格納リスト場所を取得する処理
    /// </summary>
    /// <param name="keyName">対象の動作名</param>
    /// <returns>対象の動作名の格納リスト場所</returns>
    private int GetCheckKeyListCount(string keyName) {
        int i = 0;
;        foreach (string str in keyConfig.Keys) {
            if (keyName == str) {
                return i;
            } else {
                i++;
            }//if
        }//foreach
        return i;
    }//


    /// <summary>
    /// コンフィグからキーコードを削除する
    /// </summary>
    /// <param name="keyName">キーに割り付けられている名前</param>
    /// <returns></returns>
    public bool RemoveKey(string keyName) {
        return keyConfig.Remove(keyName);
    }

    /// <summary>
    /// 設定されているキーコンフィグを確認用文字列として受け取る
    /// </summary>
    /// <returns>キーコンフィグを表す文字列</returns>
    public string CheckKeyConfig() {
        Debug.LogError("KeyConfig.cs__CheckConfig");
        StringBuilder sb = new StringBuilder();
        foreach (var keyValuePair in keyConfig) {
            sb.AppendLine("Key : " + keyValuePair.Key);
            foreach (var value in keyValuePair.Value) {
                sb.AppendLine("  |_ Value : " + value);
                Debug.Log(keyValuePair.Key+" : " + value);
            }    
        }
        Debug.LogWarning("KeyConfig.cs__CheckConfig");
        return sb.ToString();
    }

    /// <summary>
    /// ファイルからキーコンフィグファイルをロードする
    /// </summary>
    public void LoadKeyConfigFile() {
        //TODO:復号処理
        using (TextReader tr = new StreamReader(keyConfigFilePath, Encoding.UTF8))
            keyConfig = JsonMapper.ToObject<Dictionary<string, List<KeyCode>>>(tr);
    }

    /// <summary>
    /// 現在のキーコンフィグをファイルにセーブする
    /// ファイルがない場合は新たにファイルを作成する
    /// </summary>
    public void SaveKeyConfigFile() {
        //TODO:暗号化処理
        var jsonText = JsonMapper.ToJson(keyConfig);
        using (TextWriter tw = new StreamWriter(keyConfigFilePath, false, Encoding.UTF8))
            tw.Write(jsonText);
        Debug.LogWarning("KeyConfigデータの保存完了");
    }
}