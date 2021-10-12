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
    private Dictionary<string, List<KeyCode>> config = new Dictionary<string, List<KeyCode>>();
    private readonly string configFilePath;

    /// <summary>
    /// キーコンフィグを管理するクラスを生成する
    /// </summary>
    /// <param name="configFilePath">コンフィグファイルのパス</param>
    public KeyConfig(string configFilePath) {
        this.configFilePath = configFilePath;
        Debug.Log("KeyConfig.cs__this.configFilePath__" + this.configFilePath);
    }

    /// <summary>
    /// 指定したキーの入力状態をチェックする
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <param name="predicate">キーが入力されているかを判定する述語</param>
    /// <returns>入力状態</returns>
    private bool InputKeyCheck(string keyName, Func<KeyCode, bool> predicate) {
        bool ret = false;
        foreach (var keyCode in config[keyName])
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
        if (config.ContainsKey(keyName))
            return new List<KeyCode>(config[keyName]);
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
        config[keyName] = keyCode;
        Debug.Log("Name.Code__" + keyName + "." + keyCode[0]);
        return true;
    }

    /// <summary>
    /// コンフィグからキーコードを削除する
    /// </summary>
    /// <param name="keyName">キーに割り付けられている名前</param>
    /// <returns></returns>
    public bool RemoveKey(string keyName) {
        return config.Remove(keyName);
    }

    /// <summary>
    /// 設定されているキーコンフィグを確認用文字列として受け取る
    /// </summary>
    /// <returns>キーコンフィグを表す文字列</returns>
    public string CheckConfig() {
        Debug.LogError("KeyConfig.cs__CheckConfig");
        StringBuilder sb = new StringBuilder();
        foreach (var keyValuePair in config) {
            sb.AppendLine("Key : " + keyValuePair.Key);
            foreach (var value in keyValuePair.Value) {
                sb.AppendLine("  |_ Value : " + value);
                Debug.Log("KeyConfig.cs__CheckConfig__"+ keyValuePair.Key+" : " + value);
            }    
        }
        return sb.ToString();
    }

    /// <summary>
    /// ファイルからキーコンフィグファイルをロードする
    /// </summary>
    public void LoadConfigFile() {
        //TODO:復号処理
        using (TextReader tr = new StreamReader(configFilePath, Encoding.UTF8))
            config = JsonMapper.ToObject<Dictionary<string, List<KeyCode>>>(tr);
        Debug.LogWarning("KeyConfig.cs__LoadConfigFile__End");
    }

    /// <summary>
    /// 現在のキーコンフィグをファイルにセーブする
    /// ファイルがない場合は新たにファイルを作成する
    /// </summary>
    public void SaveConfigFile() {
        //TODO:暗号化処理
        var jsonText = JsonMapper.ToJson(config);
        using (TextWriter tw = new StreamWriter(configFilePath, false, Encoding.UTF8))
            tw.Write(jsonText);
        Debug.LogWarning("KeyConfig.cs__SaveConfigFile__End");
    }
}