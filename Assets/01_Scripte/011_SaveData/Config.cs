//参考URL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using LitJson;
using ConfigDataDefine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

/// <summary>
/// コンフィグ情報を取り扱う
/// 更新日時:20211025
/// </summary>
public class Config {
    private static Dictionary<string, List<KeyCode>> _config = new Dictionary<string, List<KeyCode>>();
    private string _configFilePath = "config.dat";

    /// <summary>
    /// コンフィグキーのデフォルトコード
    /// </summary>
    private readonly Dictionary<string, List<KeyCode>> _defaultKeyCode = new Dictionary<string, List<KeyCode>>() {
        {"NormalJump", new List<KeyCode> { KeyCode.Z, KeyCode.JoystickButton2 } },
        {"FlipJump", new List<KeyCode> { KeyCode.C, KeyCode.JoystickButton1 }},
        {"Attack", new List<KeyCode> { KeyCode.X, KeyCode.JoystickButton0 } },
        {"Pause", new List<KeyCode> { KeyCode.V, KeyCode.JoystickButton7 } },
        {"Horizontal", new List<KeyCode> { KeyCode.LeftArrow, KeyCode.RightArrow } },
        {"Vertical", new List<KeyCode> { KeyCode.DownArrow, KeyCode.UpArrow } },
        {"Submit", new List<KeyCode> { KeyCode.X, KeyCode.JoystickButton0 } },
        {"Cancel", new List<KeyCode> { KeyCode.Z, KeyCode.JoystickButton1 } }
    };//_defaultKeyCode

    /// <summary>
    /// Key入力の入力タイプの列挙体
    /// </summary>
    public enum KeyType {
        KeyBoard = 0,
        JoyStick = 1
    }//KeyType

    /// <summary>
    /// 軸入力の+/-判定列挙体
    /// </summary>
    public enum AxisType {
        Negative = 0,
        Positive = 1
    }//AxisType

    /// <summary>
    /// 入力されたKeyCodeを取得する処理
    /// </summary>
    /// <returns></returns>
    public KeyCode GetInputKeyCode() {
        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {//入力キー取得
            if (Input.GetKeyDown(code))
                return code;
        }//foreach
        return KeyCode.None;
    }//GetInputKeyCode

    #region GetKey//操作可能キーの入力確認処理
    /// <summary>
    /// 指定したキーの入力状態をチェックする
    /// </summary>
    /// <param name="keyName">キーを示す名前文字列</param>
    /// <param name="predicate">キーが入力されているかを判定する述語</param>
    /// <returns>入力状態</returns>
    private bool InputKeyCheck(string keyName, Func<KeyCode, bool> predicate) {
        bool ret = false;
        foreach (var keyCode in _config[keyName])
            if (predicate(keyCode))
                return true;
        return ret;
    }//InputKeyCheck

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
    #endregion InputKey


    #region ConfigCodeChange //コンフィグデータ内の入力コードを変更する
    /// <summary>
    /// 現在のコンフィグ情報から入力コード設定を変更する際に、変更する入力コードが他と重複しているか調べる
    /// </summary>
    /// <param name="targetInputName">変更する入力コード名</param>
    /// <param name="changeCode">変更する入力コード</param>
    /// <param name="nowCode">targetNameの入力タイプ</param>
    /// <param name="targetInputType">入力コード名の入力タイプ</param>
    public void DuplicationCodeCheck(string targetInputName,KeyCode changeCode,KeyCode nowCode,int targetInputType) {
        foreach(var keyInfo in _config) {
            for(int i = 0; i < 2; i++) {
                if (changeCode == _config[keyInfo.Key][i]) {
                    if (!CodeExchangeCheck(targetInputName, keyInfo.Key))
                        break;
                    ChangeCode(targetInputName, changeCode, targetInputType);
                    ChangeCode(keyInfo.Key, nowCode, i);
                }//if
            }//for
        }//foreache
        ChangeCode(targetInputName, changeCode, targetInputType);
    }//DuplicationKeyCheck

    /// <summary>
    /// 重複している入力コード同士の交換判定
    /// </summary>
    /// <param name="targetInputName">変更する入力名</param>
    /// <param name="checkInputName">調べる入力名</param>
    /// <returns>入力コード同士の交換可否 true:交換する false:交換しない</returns>
    public bool CodeExchangeCheck(string targetInputName,string checkInputName) {
        bool check1 = targetInputName ==  ConfigData.Submit.String|| targetInputName == ConfigData.Cancel.String;
        bool check2 = checkInputName == ConfigData.Submit.String || checkInputName == ConfigData.Cancel.String;
        if(check1 == check2)
            return true;
        return false;    
    }//DuplicationKeyChangeJudge

    /// <summary>
    /// コードの変更を行う
    /// </summary>
    /// <param name="changeInputName">変更する入力名</param>
    /// <param name="changeCode">変更するコード</param>
    /// <param name="changeInputType">changeInputNameの入力タイプ</param>
    private void ChangeCode(string changeInputName,KeyCode changeCode,int changeInputType) {
        _config[changeInputName][changeInputType] = changeCode;
        SaveConfigFile();
    }//ChangeKeyCode
    #endregion ConfigCodeChange

    /// <summary>
    /// 入力されたキーコードをチェックする処理
    /// </summary>
    /// <param name="keyName">チェックするKey名</param>
    /// <returns>Key名に対応するキーコード</returns>
    public KeyCode GetInputKeyCodeCheck(string keyName, KeyType keyType) {
        return _config[keyName][(int)keyType];
    }//GetInputKeyCodeCheck

    #region AxisRaw //軸入力したときの軸値
    /// <summary>
    /// 軸入力したときの移動量出力処理
    /// Input.GetAxisRawとInputManager.GetAxesRaw
    /// </summary>
    /// <param name="axisName">対象の軸名</param>
    /// <returns>移動させる値</returns>
    public float GetAxisRaw(string axisName) {
        float workMove = 0;
        workMove += GetAxisRawValue(axisName);//キーボード入力
        workMove += Input.GetAxisRaw(axisName);//コントローラ入力
        return workMove;
    }//GetAxisRaw

    /// <summary>
    /// 作成した軸入力の移動量出力処理
    /// </summary>
    /// <param name="axisName">対象の軸名</param>
    /// <returns>入力された軸向きの値</returns>
    private int GetAxisRawValue(string axisName) {
        int i = 0;
        if (Input.GetKey(GetInputAxisCodeCheck(axisName, AxisType.Negative))) {
            i -= 1;
        }//if
        if (Input.GetKey(GetInputAxisCodeCheck(axisName, AxisType.Positive))) {
            i += 1;
        }//if
        return i;
    }//GetAxisRawValue

    /// <summary>
    /// 対象の軸コードの向き出力
    /// </summary>
    /// <param name="axisName">チェックする軸名</param>
    /// <param name="axisType">チェックするaxisタイプ</param>
    /// <returns>axis名・axisタイプに対応するキーコード</returns>
    public KeyCode GetInputAxisCodeCheck(string axisName, AxisType axisType) {
        return _config[axisName][(int)axisType];
    }//KeyCodeCheck
    #endregion AxisRaw

    #region DefaultConfigChange
    /// <summary>
    /// デフォルトコンフィグのキーコードを代入する
    /// </summary>
    public void SetDefaultConfig() {
        foreach (KeyValuePair<string, List<KeyCode>> keyInfo in _defaultKeyCode) {
            SetKey(keyInfo.Key, keyInfo.Value);
        }//foreach
    }//SetDefaultConfig

    /// <summary>
    /// 名前文字列に対するキーコードを設定する
    /// </summary>
    /// <param name="keyName">キーに割り付ける名前</param>
    /// <param name="keyCode">キーコード</param>
    /// <returns>キーコードの設定が正常に完了したかどうか</returns>
    public bool SetKey(string keyName, List<KeyCode> keyCode) {
        if (string.IsNullOrEmpty(keyName) || keyCode.Count < 1)
            return false;
        _config[keyName] = keyCode;
        return true;
    }//SetKey
    #endregion DefaultConfigChange

    #region Save・Load

    /// <summary>
    /// ファイルを保存するパスを設定する処理
    /// </summary>
    /// <returns>ファイルを保存するパス</returns>
    private string SaveFilePathSetting() {
        string path = "";
        #if UNITY_EDITOR
             path = Directory.GetCurrentDirectory() + "/Assets/05_SaveData/";
        #else
            path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')+"/"+Application.productName+"_Data/";    
        #endif
        return path;
    }//SaveFilePathSetting


    /// <summary>
    /// ファイルからキーコンフィグファイルをロードする
    /// </summary>
    public void LoadConfigFile() {
        using (TextReader tr = new StreamReader(SaveFilePathSetting()+_configFilePath, Encoding.UTF8))
            _config = JsonMapper.ToObject<Dictionary<string, List<KeyCode>>>(tr);
    }//LoadConfigFile

    /// <summary>
    /// 現在のキーコンフィグをファイルにセーブする
    /// ファイルがない場合は新たにファイルを作成する
    /// </summary>
    public void SaveConfigFile() {
        var jsonText = JsonMapper.ToJson(_config);
        using (TextWriter tw = new StreamWriter(SaveFilePathSetting()+_configFilePath, false, Encoding.UTF8))
            tw.Write(jsonText);
    }//SaveConfigFile
    #endregion Save・Load

}//Config