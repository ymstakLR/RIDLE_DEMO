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
/// Axesコンフィグ情報を取り扱う
/// </summary>
public class AxesConfig {
    public static Dictionary<string, List<KeyCode>> axesConfig = new Dictionary<string, List<KeyCode>>();
    private readonly string axesConfigFilePath;

    /// <summary>
    /// Axesコンフィグを管理するクラスを生成する
    /// </summary>
    /// <param name="axesConfigFilePath">Axesコンフィグファイルのパス</param>
    public AxesConfig(string axesConfigFilePath) {
        this.axesConfigFilePath = axesConfigFilePath;
    }//AxesConfig

    /// <summary>
    /// 名前文字列に対するAxesコードを設定する
    /// </summary>
    /// <param name="axesName">axesListに割り当てる名前</param>
    /// <param name="axesList">設定するaxesList</param>
    /// <returns></returns>
    public bool SetAxes(string axesName, List<KeyCode> axesList) {
        if (string.IsNullOrEmpty(axesName) || axesList.Count < 1)
            return false;
        axesConfig[axesName] = axesList;
        Debug.LogWarning(axesConfig.Count + "__" + axesName);
        Debug.LogWarning("Name.Code__" + axesName + "." + axesList[0]);//対応キー名:対応入力キー
        Debug.LogWarning("Name.Code__" + axesName + "." + axesList[1]);//対応キー名:対応入力キー
        return true;
    }//SetAxes

    /// <summary>
    /// 入力されたAxesコードをチェックする処理
    /// </summary>
    /// <param name="axesName">チェックするaxes名</param>
    /// <param name="axesType">チェックするaxesタイプ</param>
    /// <returns>axes名とaxesタイプに対応するキーコード</returns>
    public KeyCode GetInputAxesCodeCheck(string axesName,AxesType axesType) {
        return axesConfig[axesName][(int)axesType];
    }//KeyCodeCheck

    /// <summary>
    /// Axes入力の+/-判定列挙体
    /// </summary>
    public enum AxesType {
        Negative = 0,
        Positive = 1
    }//AxesType

    public int GetAxesRaw(string axesName) {
        int i = 0;
        if (Input.GetKey(GetInputAxesCodeCheck(axesName, AxesType.Negative))) {
            i -= 1;
        }
        if (Input.GetKey(GetInputAxesCodeCheck(axesName, AxesType.Positive))) {
            i += 1;
        }
        return i;
    }

    /// <summary>
    /// ファイルからキーコンフィグファイルをロードする
    /// </summary>
    public void LoadAxesConfigFile() {
        //TODO:復号処理
        using (TextReader tr = new StreamReader(axesConfigFilePath, Encoding.UTF8))
            axesConfig = JsonMapper.ToObject<Dictionary<string, List<KeyCode>>>(tr);
    }//LoadAxesConfigFile

    /// <summary>
    /// 現在のキーコンフィグをファイルにセーブする
    /// ファイルがない場合は新たにファイルを作成する
    /// </summary>
    public void SaveAxesConfigFile() {
        //TODO:暗号化処理
        var jsonText = JsonMapper.ToJson(axesConfig);
        using (TextWriter tw = new StreamWriter(axesConfigFilePath, false, Encoding.UTF8))
            tw.Write(jsonText);
    }//SaveAxesConfigFile

}//AxesConfig