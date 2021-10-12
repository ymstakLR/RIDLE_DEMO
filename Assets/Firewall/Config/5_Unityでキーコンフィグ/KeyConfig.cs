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
/// �L�[�R���t�B�O������舵��
/// </summary>
public class KeyConfig {
    private Dictionary<string, List<KeyCode>> config = new Dictionary<string, List<KeyCode>>();
    private readonly string configFilePath;

    /// <summary>
    /// �L�[�R���t�B�O���Ǘ�����N���X�𐶐�����
    /// </summary>
    /// <param name="configFilePath">�R���t�B�O�t�@�C���̃p�X</param>
    public KeyConfig(string configFilePath) {
        this.configFilePath = configFilePath;
        Debug.Log("KeyConfig.cs__this.configFilePath__" + this.configFilePath);
    }

    /// <summary>
    /// �w�肵���L�[�̓��͏�Ԃ��`�F�b�N����
    /// </summary>
    /// <param name="keyName">�L�[���������O������</param>
    /// <param name="predicate">�L�[�����͂���Ă��邩�𔻒肷��q��</param>
    /// <returns>���͏��</returns>
    private bool InputKeyCheck(string keyName, Func<KeyCode, bool> predicate) {
        bool ret = false;
        foreach (var keyCode in config[keyName])
            if (predicate(keyCode))
                return true;
        return ret;
    }

    /// <summary>
    /// �w�肵���L�[��������Ԃ��ǂ�����Ԃ�
    /// </summary>
    /// <param name="keyName">�L�[���������O������</param>
    /// <returns>���͏��</returns>
    public bool GetKey(string keyName) {
        return InputKeyCheck(keyName, Input.GetKey);
    }

    /// <summary>
    /// �w�肵���L�[�����͂��ꂽ���ǂ�����Ԃ�
    /// </summary>
    /// <param name="keyName">�L�[���������O������</param>
    /// <returns>���͏��</returns>
    public bool GetKeyDown(string keyName) {
        return InputKeyCheck(keyName, Input.GetKeyDown);
    }

    /// <summary>
    /// �w�肵���L�[�������ꂽ���ǂ�����Ԃ�
    /// </summary>
    /// <param name="keyName">�L�[���������O������</param>
    /// <returns>���͏��</returns>
    public bool GetKeyUp(string keyName) {
        return InputKeyCheck(keyName, Input.GetKeyUp);
    }

    /// <summary>
    /// �w�肳�ꂽ�L�[�Ɋ���t�����Ă���L�[�R�[�h��Ԃ�
    /// </summary>
    /// <param name="keyName">�L�[���������O������</param>
    /// <returns>�L�[�R�[�h</returns>
    public List<KeyCode> GetKeyCode(string keyName) {
        if (config.ContainsKey(keyName))
            return new List<KeyCode>(config[keyName]);
        return new List<KeyCode>();
    }

    /// <summary>
    /// ���O������ɑ΂���L�[�R�[�h��ݒ肷��
    /// </summary>
    /// <param name="keyName">�L�[�Ɋ���t���閼�O</param>
    /// <param name="keyCode">�L�[�R�[�h</param>
    /// <returns>�L�[�R�[�h�̐ݒ肪����Ɋ����������ǂ���</returns>
    public bool SetKey(string keyName, List<KeyCode> keyCode) {
        //Debug.LogError("KeyConfig.cs_SetKey");
        if (string.IsNullOrEmpty(keyName) || keyCode.Count < 1)
            return false;
        config[keyName] = keyCode;
        Debug.Log("Name.Code__" + keyName + "." + keyCode[0]);
        return true;
    }

    /// <summary>
    /// �R���t�B�O����L�[�R�[�h���폜����
    /// </summary>
    /// <param name="keyName">�L�[�Ɋ���t�����Ă��閼�O</param>
    /// <returns></returns>
    public bool RemoveKey(string keyName) {
        return config.Remove(keyName);
    }

    /// <summary>
    /// �ݒ肳��Ă���L�[�R���t�B�O���m�F�p������Ƃ��Ď󂯎��
    /// </summary>
    /// <returns>�L�[�R���t�B�O��\��������</returns>
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
    /// �t�@�C������L�[�R���t�B�O�t�@�C�������[�h����
    /// </summary>
    public void LoadConfigFile() {
        //TODO:��������
        using (TextReader tr = new StreamReader(configFilePath, Encoding.UTF8))
            config = JsonMapper.ToObject<Dictionary<string, List<KeyCode>>>(tr);
        Debug.LogWarning("KeyConfig.cs__LoadConfigFile__End");
    }

    /// <summary>
    /// ���݂̃L�[�R���t�B�O���t�@�C���ɃZ�[�u����
    /// �t�@�C�����Ȃ��ꍇ�͐V���Ƀt�@�C�����쐬����
    /// </summary>
    public void SaveConfigFile() {
        //TODO:�Í�������
        var jsonText = JsonMapper.ToJson(config);
        using (TextWriter tw = new StreamWriter(configFilePath, false, Encoding.UTF8))
            tw.Write(jsonText);
        Debug.LogWarning("KeyConfig.cs__SaveConfigFile__End");
    }
}