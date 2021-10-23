//参考URL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using MBLDefine;
using LitJson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

/// <summary>
/// Keyコンフィグ情報を取り扱う
/// </summary>
public class KeyConfig {
    public static Dictionary<string, List<KeyCode>> keyConfig = new Dictionary<string, List<KeyCode>>();
    private readonly string keyConfigFilePath = "keyconf.dat";


    ///// <summary>
    ///// Keyコンフィグを管理するクラスを生成する
    ///// </summary>
    ///// <param name="keyConfigFilePath">Keyコンフィグファイルのパス</param>
    //public KeyConfig(string keyConfigFilePath) {
    //    this.keyConfigFilePath = keyConfigFilePath;
    //}//KeyCOnfig


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
    }//GetKey

    /// <summary>
    /// 指定したキーが入力されたかどうかを返す
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <returns>入力状態</returns>
    public bool GetKeyDown(string keyName) {
        return InputKeyCheck(keyName, Input.GetKeyDown);
    }//GetKeyDown

    /// <summary>
    /// 指定したキーが離されたかどうかを返す
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <returns>入力状態</returns>
    public bool GetKeyUp(string keyName) {
        return InputKeyCheck(keyName, Input.GetKeyUp);
    }//GetKeyUp

    /// <summary>
    /// 指定されたキーに割り付けられているキーコードを返す
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <returns>キーコード</returns>
    public List<KeyCode> GetKeyCode(string keyName) {
        if (keyConfig.ContainsKey(keyName))
            return new List<KeyCode>(keyConfig[keyName]);
        return new List<KeyCode>();
    }//GetKeyCode

    /// <summary>
    /// 名前文字列に対するキーコードを設定する
    /// </summary>
    /// <param name="keyName">キーに割り付ける名前</param>
    /// <param name="keyCode">キーコード</param>
    /// <returns>キーコードの設定が正常に完了したかどうか</returns>
    public bool SetKey(string keyName, List<KeyCode> keyCode) {
        if (string.IsNullOrEmpty(keyName) || keyCode.Count < 1)
            return false;
        keyConfig[keyName] = keyCode;
        return true;
    }//SetKey



    /// <summary>
    /// 入力されたキーコードをチェックする処理
    /// </summary>
    /// <param name="keyName">チェックするKey名</param>
    /// <returns>Key名に対応するキーコード</returns>
    public KeyCode GetInputKeyCodeCheck(string keyName,KeyType keyType) {
        return keyConfig[keyName][(int)keyType];
    }//KeyCodeCheck


    /// <summary>
    /// Key入力の入力タイプの列挙体
    /// </summary>
    public enum KeyType {
        KeyBoard = 0,
        JoyStick = 1
    }//KeyType

    /// <summary>
    /// コンフィグからキーコードを削除する
    /// </summary>
    /// <param name="keyName">キーに割り付けられている名前</param>
    /// <returns></returns>
    public bool RemoveKey(string keyName) {
        return keyConfig.Remove(keyName);
    }//RemoveKey

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

    public void DuplicationKeyCheck(string targetName,KeyCode changeCode,KeyCode nowCode,int inputType) {
        foreach(var keyInfo in keyConfig) {
            for(int i = 0; i < 2; i++) {
                if (changeCode == keyConfig[keyInfo.Key][i]) {
                    if (!DuplicationKeyChangeJudge(targetName, keyInfo.Key))
                        break;
                    ChangeKeyCode(targetName, changeCode, inputType);
                    ChangeKeyCode(keyInfo.Key, nowCode, i);
                }
            }
        }
        ChangeKeyCode(targetName, changeCode, inputType);
    }

    public bool DuplicationKeyChangeJudge(string targetName,string checkKeyName) {
        bool check1 = targetName ==  Key.Submit.String|| targetName == Key.Cancel.String;
        bool check2 = checkKeyName == Key.Submit.String || checkKeyName == Key.Cancel.String;
        if(check1 == check2)
            return true;
        return false;    
    }

    private void ChangeKeyCode(string changeName,KeyCode changeCode,int inputType) {
        keyConfig[changeName][inputType] = changeCode;
        SaveKeyConfigFile();
    }

    /// <summary>
    /// Axes入力の+/-判定列挙体
    /// </summary>
    public enum AxesType {
        Negative = 0,
        Positive = 1
    }//AxesType

    /// <summary>
    /// 入力されたAxesコードをチェックする処理
    /// </summary>
    /// <param name="axesName">チェックするaxes名</param>
    /// <param name="axesType">チェックするaxesタイプ</param>
    /// <returns>axes名とaxesタイプに対応するキーコード</returns>
    public KeyCode GetInputAxesCodeCheck(string axesName, AxesType axesType) {
        return keyConfig[axesName][(int)axesType];
    }//KeyCodeCheck

    /// <summary>
    /// 作成したAxes入力の移動量出力処理
    /// </summary>
    /// <param name="axesName"></param>
    /// <returns></returns>
    private int GetAxesRawValue(string axesName) {
        int i = 0;
        if (Input.GetKey(GetInputAxesCodeCheck(axesName, AxesType.Negative))) {
            i -= 1;
        }//if
        if (Input.GetKey(GetInputAxesCodeCheck(axesName, AxesType.Positive))) {
            i += 1;
        }//if
        return i;
    }//

    /// <summary>
    /// キー入力したときの移動量出力処理
    /// Input.GetAxisRawとInputManager.GetAxesRaw
    /// </summary>
    /// <param name="axesName"></param>
    /// <returns></returns>
    public float GetAxesRaw(string axesName) {
        float workMove = 0;
        workMove += GetAxesRawValue(axesName);
        workMove += Input.GetAxisRaw(axesName);
        return workMove;
    }//GetAxisRaw

    /// <summary>
    /// ファイルからキーコンフィグファイルをロードする
    /// </summary>
    public void LoadKeyConfigFile() {
        //TODO:復号処理
        using (TextReader tr = new StreamReader(keyConfigFilePath, Encoding.UTF8))
            keyConfig = JsonMapper.ToObject<Dictionary<string, List<KeyCode>>>(tr);
    }//LoadKeyConfigFile

    /// <summary>
    /// 現在のキーコンフィグをファイルにセーブする
    /// ファイルがない場合は新たにファイルを作成する
    /// </summary>
    public void SaveKeyConfigFile() {
        //TODO:暗号化処理
        var jsonText = JsonMapper.ToJson(keyConfig);
        using (TextWriter tw = new StreamWriter(keyConfigFilePath, false, Encoding.UTF8))
            tw.Write(jsonText);
    }//SaveKeyConfigFile


    public void SetDefaultConfig() {
        foreach(KeyValuePair<string,List<KeyCode>> keyInfo in _defaultKeyCode) {
            SetKey(keyInfo.Key, keyInfo.Value);
        }
        SaveKeyConfigFile();
    }

    Dictionary<string, List<KeyCode>> _defaultKeyCode = new Dictionary<string, List<KeyCode>>() {
        {"NormalJump", new List<KeyCode> { KeyCode.J, KeyCode.JoystickButton0 } },
        {"FlipJump", new List<KeyCode> { KeyCode.L, KeyCode.JoystickButton2 }},
        {"Attack", new List<KeyCode> { KeyCode.K, KeyCode.JoystickButton1 } },
        {"Pause", new List<KeyCode> { KeyCode.H, KeyCode.JoystickButton9 } },
        {"Horizontal", new List<KeyCode> { KeyCode.A, KeyCode.D } },
        {"Vertical", new List<KeyCode> { KeyCode.S, KeyCode.W } },
        {"Submit", new List<KeyCode> { KeyCode.K, KeyCode.JoystickButton1 } },
        {"Cancel", new List<KeyCode> { KeyCode.L, KeyCode.JoystickButton2 } }
    };


}