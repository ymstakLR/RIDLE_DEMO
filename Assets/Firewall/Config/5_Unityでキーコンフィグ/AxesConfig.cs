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
    public static Dictionary<string, List<KeyCode>> axesConfig = new Dictionary<string, List<KeyCode>>();
    private readonly string axesConfigFilePath;

    /// <summary>
    /// Axes�R���t�B�O���Ǘ�����N���X�𐶐�����
    /// </summary>
    /// <param name="axesConfigFilePath">Axes�R���t�B�O�t�@�C���̃p�X</param>
    public AxesConfig(string axesConfigFilePath) {
        this.axesConfigFilePath = axesConfigFilePath;
    }//AxesConfig

    /// <summary>
    /// ���O������ɑ΂���Axes�R�[�h��ݒ肷��
    /// </summary>
    /// <param name="axesName">axesList�Ɋ��蓖�Ă閼�O</param>
    /// <param name="axesList">�ݒ肷��axesList</param>
    /// <returns></returns>
    public bool SetAxes(string axesName, List<KeyCode> axesList) {
        if (string.IsNullOrEmpty(axesName) || axesList.Count < 1)
            return false;
        axesConfig[axesName] = axesList;
        Debug.LogWarning(axesConfig.Count + "__" + axesName);
        Debug.LogWarning("Name.Code__" + axesName + "." + axesList[0]);//�Ή��L�[��:�Ή����̓L�[
        Debug.LogWarning("Name.Code__" + axesName + "." + axesList[1]);//�Ή��L�[��:�Ή����̓L�[
        return true;
    }//SetAxes

    /// <summary>
    /// ���͂��ꂽAxes�R�[�h���`�F�b�N���鏈��
    /// </summary>
    /// <param name="axesName">�`�F�b�N����axes��</param>
    /// <param name="axesType">�`�F�b�N����axes�^�C�v</param>
    /// <returns>axes����axes�^�C�v�ɑΉ�����L�[�R�[�h</returns>
    public KeyCode GetInputAxesCodeCheck(string axesName,AxesType axesType) {
        return axesConfig[axesName][(int)axesType];
    }//KeyCodeCheck

    /// <summary>
    /// Axes���͂�+/-����񋓑�
    /// </summary>
    public enum AxesType {
        Negative = 0,
        Positive = 1
    }//AxesType

    /// <summary>
    /// �쐬����Axes���͂̈ړ��ʏo�͏���
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
    /// �L�[���͂����Ƃ��̈ړ��ʏo�͏���
    /// Input.GetAxisRaw��InputManager.GetAxesRaw
    /// </summary>
    /// <param name="axesName"></param>
    /// <returns></returns>
    public float GetAxesRaw(string axesName) {
        float workMove = 0;
        workMove += GetAxesRawValue(axesName);
        workMove += Input.GetAxisRaw(axesName);
        return workMove;
    }//GetAxisRaw

    public bool GetAxesDown(string axesName) {
        return InputAxesCheck(axesName,Input.GetKeyDown);
    }

    private bool InputAxesCheck(string axesName,Func<KeyCode,bool>predicate) {
        bool ret = false;
        foreach(var keyCode in axesConfig[axesName]) {
            if (predicate(keyCode))
                return true;
        }//foreach
        return ret;
    }//InputAxesCheck

    /// <summary>
    /// �t�@�C������L�[�R���t�B�O�t�@�C�������[�h����
    /// </summary>
    public void LoadAxesConfigFile() {
        //TODO:��������
        using (TextReader tr = new StreamReader(axesConfigFilePath, Encoding.UTF8))
            axesConfig = JsonMapper.ToObject<Dictionary<string, List<KeyCode>>>(tr);
    }//LoadAxesConfigFile

    /// <summary>
    /// ���݂̃L�[�R���t�B�O���t�@�C���ɃZ�[�u����
    /// �t�@�C�����Ȃ��ꍇ�͐V���Ƀt�@�C�����쐬����
    /// </summary>
    public void SaveAxesConfigFile() {
        //TODO:�Í�������
        var jsonText = JsonMapper.ToJson(axesConfig);
        using (TextWriter tw = new StreamWriter(axesConfigFilePath, false, Encoding.UTF8))
            tw.Write(jsonText);
    }//SaveAxesConfigFile

}//AxesConfig