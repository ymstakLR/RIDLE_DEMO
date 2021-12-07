//�Q�lURL:https://qiita.com/Es_Program/items/fde067254cfc68035173
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
/// �R���t�B�O������舵��
/// �X�V����:20211025
/// </summary>
public class Config {
    private static Dictionary<string, List<KeyCode>> _config = new Dictionary<string, List<KeyCode>>();
    private string _configFilePath = "config.dat";

    /// <summary>
    /// �R���t�B�O�L�[�̃f�t�H���g�R�[�h
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
    /// Key���͂̓��̓^�C�v�̗񋓑�
    /// </summary>
    public enum KeyType {
        KeyBoard = 0,
        JoyStick = 1
    }//KeyType

    /// <summary>
    /// �����͂�+/-����񋓑�
    /// </summary>
    public enum AxisType {
        Negative = 0,
        Positive = 1
    }//AxisType

    /// <summary>
    /// ���͂��ꂽKeyCode���擾���鏈��
    /// </summary>
    /// <returns></returns>
    public KeyCode GetInputKeyCode() {
        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {//���̓L�[�擾
            if (Input.GetKeyDown(code))
                return code;
        }//foreach
        return KeyCode.None;
    }//GetInputKeyCode

    #region GetKey//����\�L�[�̓��͊m�F����
    /// <summary>
    /// �w�肵���L�[�̓��͏�Ԃ��`�F�b�N����
    /// </summary>
    /// <param name="keyName">�L�[���������O������</param>
    /// <param name="predicate">�L�[�����͂���Ă��邩�𔻒肷��q��</param>
    /// <returns>���͏��</returns>
    private bool InputKeyCheck(string keyName, Func<KeyCode, bool> predicate) {
        bool ret = false;
        foreach (var keyCode in _config[keyName])
            if (predicate(keyCode))
                return true;
        return ret;
    }//InputKeyCheck

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
    #endregion InputKey


    #region ConfigCodeChange //�R���t�B�O�f�[�^���̓��̓R�[�h��ύX����
    /// <summary>
    /// ���݂̃R���t�B�O��񂩂���̓R�[�h�ݒ��ύX����ۂɁA�ύX������̓R�[�h�����Əd�����Ă��邩���ׂ�
    /// </summary>
    /// <param name="targetInputName">�ύX������̓R�[�h��</param>
    /// <param name="changeCode">�ύX������̓R�[�h</param>
    /// <param name="nowCode">targetName�̓��̓^�C�v</param>
    /// <param name="targetInputType">���̓R�[�h���̓��̓^�C�v</param>
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
    /// �d�����Ă�����̓R�[�h���m�̌�������
    /// </summary>
    /// <param name="targetInputName">�ύX������͖�</param>
    /// <param name="checkInputName">���ׂ���͖�</param>
    /// <returns>���̓R�[�h���m�̌����� true:�������� false:�������Ȃ�</returns>
    public bool CodeExchangeCheck(string targetInputName,string checkInputName) {
        bool check1 = targetInputName ==  ConfigData.Submit.String|| targetInputName == ConfigData.Cancel.String;
        bool check2 = checkInputName == ConfigData.Submit.String || checkInputName == ConfigData.Cancel.String;
        if(check1 == check2)
            return true;
        return false;    
    }//DuplicationKeyChangeJudge

    /// <summary>
    /// �R�[�h�̕ύX���s��
    /// </summary>
    /// <param name="changeInputName">�ύX������͖�</param>
    /// <param name="changeCode">�ύX����R�[�h</param>
    /// <param name="changeInputType">changeInputName�̓��̓^�C�v</param>
    private void ChangeCode(string changeInputName,KeyCode changeCode,int changeInputType) {
        _config[changeInputName][changeInputType] = changeCode;
        SaveConfigFile();
    }//ChangeKeyCode
    #endregion ConfigCodeChange

    /// <summary>
    /// ���͂��ꂽ�L�[�R�[�h���`�F�b�N���鏈��
    /// </summary>
    /// <param name="keyName">�`�F�b�N����Key��</param>
    /// <returns>Key���ɑΉ�����L�[�R�[�h</returns>
    public KeyCode GetInputKeyCodeCheck(string keyName, KeyType keyType) {
        return _config[keyName][(int)keyType];
    }//GetInputKeyCodeCheck

    #region AxisRaw //�����͂����Ƃ��̎��l
    /// <summary>
    /// �����͂����Ƃ��̈ړ��ʏo�͏���
    /// Input.GetAxisRaw��InputManager.GetAxesRaw
    /// </summary>
    /// <param name="axisName">�Ώۂ̎���</param>
    /// <returns>�ړ�������l</returns>
    public float GetAxisRaw(string axisName) {
        float workMove = 0;
        workMove += GetAxisRawValue(axisName);//�L�[�{�[�h����
        workMove += Input.GetAxisRaw(axisName);//�R���g���[������
        return workMove;
    }//GetAxisRaw

    /// <summary>
    /// �쐬���������͂̈ړ��ʏo�͏���
    /// </summary>
    /// <param name="axisName">�Ώۂ̎���</param>
    /// <returns>���͂��ꂽ�������̒l</returns>
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
    /// �Ώۂ̎��R�[�h�̌����o��
    /// </summary>
    /// <param name="axisName">�`�F�b�N���鎲��</param>
    /// <param name="axisType">�`�F�b�N����axis�^�C�v</param>
    /// <returns>axis���Eaxis�^�C�v�ɑΉ�����L�[�R�[�h</returns>
    public KeyCode GetInputAxisCodeCheck(string axisName, AxisType axisType) {
        return _config[axisName][(int)axisType];
    }//KeyCodeCheck
    #endregion AxisRaw

    #region DefaultConfigChange
    /// <summary>
    /// �f�t�H���g�R���t�B�O�̃L�[�R�[�h��������
    /// </summary>
    public void SetDefaultConfig() {
        foreach (KeyValuePair<string, List<KeyCode>> keyInfo in _defaultKeyCode) {
            SetKey(keyInfo.Key, keyInfo.Value);
        }//foreach
    }//SetDefaultConfig

    /// <summary>
    /// ���O������ɑ΂���L�[�R�[�h��ݒ肷��
    /// </summary>
    /// <param name="keyName">�L�[�Ɋ���t���閼�O</param>
    /// <param name="keyCode">�L�[�R�[�h</param>
    /// <returns>�L�[�R�[�h�̐ݒ肪����Ɋ����������ǂ���</returns>
    public bool SetKey(string keyName, List<KeyCode> keyCode) {
        if (string.IsNullOrEmpty(keyName) || keyCode.Count < 1)
            return false;
        _config[keyName] = keyCode;
        return true;
    }//SetKey
    #endregion DefaultConfigChange

    #region Save�ELoad

    /// <summary>
    /// �t�@�C����ۑ�����p�X��ݒ肷�鏈��
    /// </summary>
    /// <returns>�t�@�C����ۑ�����p�X</returns>
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
    /// �t�@�C������L�[�R���t�B�O�t�@�C�������[�h����
    /// </summary>
    public void LoadConfigFile() {
        using (TextReader tr = new StreamReader(SaveFilePathSetting()+_configFilePath, Encoding.UTF8))
            _config = JsonMapper.ToObject<Dictionary<string, List<KeyCode>>>(tr);
    }//LoadConfigFile

    /// <summary>
    /// ���݂̃L�[�R���t�B�O���t�@�C���ɃZ�[�u����
    /// �t�@�C�����Ȃ��ꍇ�͐V���Ƀt�@�C�����쐬����
    /// </summary>
    public void SaveConfigFile() {
        var jsonText = JsonMapper.ToJson(_config);
        using (TextWriter tw = new StreamWriter(SaveFilePathSetting()+_configFilePath, false, Encoding.UTF8))
            tw.Write(jsonText);
    }//SaveConfigFile
    #endregion Save�ELoad

}//Config