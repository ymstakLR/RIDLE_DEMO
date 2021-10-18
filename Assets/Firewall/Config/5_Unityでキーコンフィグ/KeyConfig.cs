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
/// Key�R���t�B�O������舵��
/// </summary>
public class KeyConfig {
    public static Dictionary<string, List<KeyCode>> keyConfig = new Dictionary<string, List<KeyCode>>();
    private readonly string keyConfigFilePath;


    /// <summary>
    /// Key�R���t�B�O���Ǘ�����N���X�𐶐�����
    /// </summary>
    /// <param name="keyConfigFilePath">Key�R���t�B�O�t�@�C���̃p�X</param>
    public KeyConfig(string keyConfigFilePath) {
        this.keyConfigFilePath = keyConfigFilePath;
    }//KeyCOnfig


    /// <summary>
    /// �w�肵���L�[�̓��͏�Ԃ��`�F�b�N����
    /// </summary>
    /// <param name="keyName">�L�[���������O������</param>
    /// <param name="predicate">�L�[�����͂���Ă��邩�𔻒肷��q��</param>
    /// <returns>���͏��</returns>
    private bool InputKeyCheck(string keyName, Func<KeyCode, bool> predicate) {
        bool ret = false;
        foreach (var keyCode in keyConfig[keyName])
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
    }//GetKey

    /// <summary>
    /// �w�肵���L�[�����͂��ꂽ���ǂ�����Ԃ�
    /// </summary>
    /// <param name="keyName">�L�[���������O������</param>
    /// <returns>���͏��</returns>
    public bool GetKeyDown(string keyName) {
        return InputKeyCheck(keyName, Input.GetKeyDown);
    }//GetKeyDown

    /// <summary>
    /// �w�肵���L�[�������ꂽ���ǂ�����Ԃ�
    /// </summary>
    /// <param name="keyName">�L�[���������O������</param>
    /// <returns>���͏��</returns>
    public bool GetKeyUp(string keyName) {
        return InputKeyCheck(keyName, Input.GetKeyUp);
    }//GetKeyUp

    /// <summary>
    /// �w�肳�ꂽ�L�[�Ɋ���t�����Ă���L�[�R�[�h��Ԃ�
    /// </summary>
    /// <param name="keyName">�L�[���������O������</param>
    /// <returns>�L�[�R�[�h</returns>
    public List<KeyCode> GetKeyCode(string keyName) {
        if (keyConfig.ContainsKey(keyName))
            return new List<KeyCode>(keyConfig[keyName]);
        return new List<KeyCode>();
    }//GetKeyCode

    /// <summary>
    /// ���O������ɑ΂���L�[�R�[�h��ݒ肷��
    /// </summary>
    /// <param name="keyName">�L�[�Ɋ���t���閼�O</param>
    /// <param name="keyCode">�L�[�R�[�h</param>
    /// <returns>�L�[�R�[�h�̐ݒ肪����Ɋ����������ǂ���</returns>
    public bool SetKey(string keyName, List<KeyCode> keyCode) {
        if (string.IsNullOrEmpty(keyName) || keyCode.Count < 1)
            return false;
        keyConfig[keyName] = keyCode;
        return true;
    }//SetKey

    /// <summary>
    /// ���͂��ꂽ�L�[�R�[�h���`�F�b�N���鏈��
    /// </summary>
    /// <param name="keyName">�`�F�b�N����Key��</param>
    /// <returns>Key���ɑΉ�����L�[�R�[�h</returns>
    public KeyCode GetInputKeyCodeCheck(string keyName,KeyType keyType) {
        return keyConfig[keyName][(int)keyType];
    }//KeyCodeCheck

    /// <summary>
    /// Key���͂̓��̓^�C�v�̗񋓑�
    /// </summary>
    public enum KeyType {
        KeyBoard = 0,
        JoyStick = 1
    }//KeyType

    /// <summary>
    /// �R���t�B�O����L�[�R�[�h���폜����
    /// </summary>
    /// <param name="keyName">�L�[�Ɋ���t�����Ă��閼�O</param>
    /// <returns></returns>
    public bool RemoveKey(string keyName) {
        return keyConfig.Remove(keyName);
    }//RemoveKey

    /// <summary>
    /// �ݒ肳��Ă���L�[�R���t�B�O���m�F�p������Ƃ��Ď󂯎��
    /// </summary>
    /// <returns>�L�[�R���t�B�O��\��������</returns>
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
    /// �t�@�C������L�[�R���t�B�O�t�@�C�������[�h����
    /// </summary>
    public void LoadKeyConfigFile() {
        //TODO:��������
        using (TextReader tr = new StreamReader(keyConfigFilePath, Encoding.UTF8))
            keyConfig = JsonMapper.ToObject<Dictionary<string, List<KeyCode>>>(tr);
    }//LoadKeyConfigFile

    /// <summary>
    /// ���݂̃L�[�R���t�B�O���t�@�C���ɃZ�[�u����
    /// �t�@�C�����Ȃ��ꍇ�͐V���Ƀt�@�C�����쐬����
    /// </summary>
    public void SaveKeyConfigFile() {
        //TODO:�Í�������
        var jsonText = JsonMapper.ToJson(keyConfig);
        using (TextWriter tw = new StreamWriter(keyConfigFilePath, false, Encoding.UTF8))
            tw.Write(jsonText);
    }//SaveKeyConfigFile
}