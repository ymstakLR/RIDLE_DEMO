//�Q�lURL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using LitJson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

/// <summary>
/// Axes�R���t�B�O������舵��
/// </summary>
public class AxesConfig {
    public static Dictionary<string, List<List<KeyCode>>> axesConfig = new Dictionary<string, List<List<KeyCode>>>();
    private readonly string axesConfigFilePath;

    private int axesListCount;

    /// <summary>
    /// �L�[�R���t�B�O���Ǘ�����N���X�𐶐�����
    /// </summary>
    /// <param name="axesConfigFilePath">�R���t�B�O�t�@�C���̃p�X</param>
    public AxesConfig(string axesConfigFilePath) {
        this.axesConfigFilePath = axesConfigFilePath;
    }


    public bool SetAxes(string keyName, List<List<KeyCode>> keyCode) {
        if (string.IsNullOrEmpty(keyName) || keyCode.Count < 1)
            return false;
        axesConfig[keyName] = keyCode;
        Debug.LogWarning(axesConfig.Count + "__" + keyName);
        Debug.LogWarning("Name.Code__" + keyName + "." + keyCode[0][0]);//�Ή��L�[��:�Ή����̓L�[
        Debug.LogWarning("Name.Code__" + keyName + "." + keyCode[0][1]);//�Ή��L�[��:�Ή����̓L�[
        return true;
    }


    public KeyCode GetInputAxesCodeCheck(string keyName,AxesType axesType) {
        int i = GetCheckAxesListCount(keyName);
        int j = 0;

        foreach (KeyCode kc in axesConfig[keyName][0]) {
            
        }//foreach
        return axesConfig[keyName][0][(int)axesType];
    }//KeyCodeCheck

    public enum AxesType {
        Negative = 0,
        Positive = 1
    }

    private int GetCheckAxesListCount(string keyName) {
        int i = 0;
        foreach (string str in axesConfig.Keys) {
            if (keyName == str) {
                Debug.Log(keyName+"___"+str);
                Debug.Log(keyName + "___" + str);
                return i;
            } else {
                i++;
            }//if
        }//foreach
        return i;
    }//

    public void LoadAxesConfigFile() {
        //TODO:��������
        using (TextReader tr = new StreamReader(axesConfigFilePath, Encoding.UTF8))
            axesConfig = JsonMapper.ToObject<Dictionary<string, List<List<KeyCode>>>>(tr);
    }

    public void SaveAxesConfigFile() {
        //TODO:�Í�������
        var jsonText = JsonMapper.ToJson(axesConfig);
        using (TextWriter tw = new StreamWriter(axesConfigFilePath, false, Encoding.UTF8))
            tw.Write(jsonText);
    }


}