///�ꕔ�Q�l�T�C�g�Q�l http://wordpress.notargs.com/blog/blog/2015/01/23/92/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// InputManager�̕ҏW
/// �X�V����:20211004
/// </summary>
public static class InputManagerEdit {
    public static List<List<string>> _inputData = new List<List<string>>();

    public static List<string> _nameList;
    public static List<string> _negativeButtonList;
    public static List<string> _positiveButtonList;
    public static List<string> _altPositionButtonList;
    public static List<string> _invertList;
    public static List<string> _typeList;
    public static List<string> _axisList;

///�ۑ��f�[�^�֘A�̏����ɂ��Ă̏���

    /// <summary>
    /// InputData�̓ǂݍ��ݏ���
    /// </summary>
    public static void InputDataLoad() {
        _nameList = new List<string>();
        _nameList.AddRange(SaveManager.inputDataStruct.nameList);
        _negativeButtonList = new List<string>();
        _negativeButtonList.AddRange(SaveManager.inputDataStruct.negativeButtonList);
        _positiveButtonList = new List<string>();
        _positiveButtonList.AddRange(SaveManager.inputDataStruct.positiveButtonList);
        _altPositionButtonList = new List<string>();
        _altPositionButtonList.AddRange(SaveManager.inputDataStruct.altPositionButtonList);
        _invertList = new List<string>();
        _invertList.AddRange(SaveManager.inputDataStruct.invertList);
        _typeList = new List<string>();
        _typeList.AddRange(SaveManager.inputDataStruct.typeList);
        _axisList = new List<string>();
        _axisList.AddRange(SaveManager.inputDataStruct.axisList);
    }//InputDataLoad

    /// <summary>
    /// InputData�̕ۑ�����
    /// </summary>
    private static void InputDataSave() {
        _inputData.Insert(0, _nameList);
        _inputData.Insert(1, _negativeButtonList);
        _inputData.Insert(2, _positiveButtonList);
        _inputData.Insert(3, _altPositionButtonList);
        _inputData.Insert(4, _invertList);
        _inputData.Insert(5, _typeList);
        _inputData.Insert(6, _axisList);
        SaveManager.InputDataUpdate(_inputData);
    }//InputDataSave

    /// <summary>
    /// InputManager�̍X�V����
    /// </summary>
    public static void InputManagerUpdate() {
        InputDataLoad();
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        axesProperty.ClearArray();
        for (int i = 0; i < 12; i++) {
            AddAxis(CreateInputAxis(i), serializedObject, axesProperty);
        }//for
    }//InputManagerUpdate

    /// <summary>
    /// Axes�L�[���쐬���鏈��
    /// </summary>
    private static InputAxis CreateInputAxis(int i) {
        var axis = new InputAxis();
        axis.name = _nameList[i];
        axis.negativeButton = _negativeButtonList[i];
        axis.positiveButton = _positiveButtonList[i];
        axis.altPositiveButton = _altPositionButtonList[i];
        axis.gravity = 1000;
        axis.dead = 0.2f;
        axis.sensitivity = 1;
        if (_invertList[i].ToLower() == "true") {
            axis.invert = true;
        }//if
        if (_typeList[i] == "JoystickAxis") {
            axis.type = AxisType.JoystickAxis;
        }//if
        axis.axis = int.Parse(_axisList[i]);
        return axis;
    }//CreateInputAxis

    /// <summary>
    /// InputManager�ɃL�[��ǉ����鏈��
    /// </summary>
    /// <param name="axis">�ǉ�����Axis��</param>
    /// <param name="serializedObject"></param>
    /// <param name="axesProperty"></param>
    private static void AddAxis(InputAxis axis, SerializedObject serializedObject, SerializedProperty axesProperty) {
        axesProperty.arraySize++;
        serializedObject.ApplyModifiedProperties();
        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

        GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
        GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
        GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
        GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
        GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
        GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

        serializedObject.ApplyModifiedProperties();
    }//AddAxis

    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name) {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do {
            if (child.name == name) {
                return child;
            }
        } while (child.Next(false));
        return null;
    }//SerializedProperty


///�ʃX�N���v�g�Ɉړ�

    /// <summary>
    /// �R���t�B�O�{�^���̃e�L�X�g���擾���鏈��
    /// </summary>
    /// <param name="targetAxesName">�Ώۂ�Axes��</param>
    /// <param name="targetInputDataType">�Ώۂ�InputDataType</param>
    /// <returns>�ΏۃR���t�B�O�{�^���̃L�[�e�L�X�g</returns>
    public static string GetConficButtonText(string targetAxesName,InputDataType targetInputDataType) {
        InputDataLoad();
        string targetKeyText = "";
        int targetAxesListCount = GetInputDataID(targetAxesName);
        switch (targetInputDataType) {
            case InputDataType.KeyNegative:
                targetKeyText = _negativeButtonList[targetAxesListCount];
                break;
            case InputDataType.KeyPositive:
                targetKeyText = _positiveButtonList[targetAxesListCount];
                break;
            case InputDataType.JoystickNegative:
                break;
            case InputDataType.JoystickPositive:
                targetKeyText = _altPositionButtonList[targetAxesListCount];
                break;
        }//switch
        targetKeyText = targetKeyText.Replace("joystick ", "");
        return targetKeyText.ToUpper();
    }//GetConficButtonText

    /// <summary>
    /// �L�[����ύX���鏈��
    /// </summary>
    /// <param name="targetAxesName">�Ώۂ�Axes��</param>
    /// <param name="targetInputDataType">�Ώۂ�InputDataType</param>
    /// <param name="changeValue">�ύX����l</param>
    public static void InputDataChange(string targetAxesName,InputDataType targetInputDataType, string changeValue) {
        InputDataLoad();
        int targeAxesListCount = GetInputDataID(targetAxesName);
        switch (targetInputDataType) {
            case InputDataType.KeyNegative:
                _negativeButtonList[targeAxesListCount] = changeValue;
                break;
            case InputDataType.KeyPositive:
                _positiveButtonList[targeAxesListCount] = changeValue;
                break;
            case InputDataType.JoystickNegative:
                break;
            case InputDataType.JoystickPositive:
                _altPositionButtonList[targeAxesListCount] = changeValue;
                break;
            default:
                break;
        }//switch
        InputDataSave();
        InputManagerUpdate();
    }//InputDataUpdate

    /// <summary>
    /// �X�V�������L�[���Ɠ��������X�g�z��ԍ��擾����
    /// </summary>
    /// <param name="inputName"></param>
    /// <returns></returns>
    public static int GetInputDataID(string inputName) {
        int inputNum = 0;
        while (_nameList[inputNum].ToString() != inputName) {
            inputNum++;
        }//while
        return inputNum;
    }//GetInputDataID

    /// <summary>
    /// ���͂���l�̑Ή�����^�C�v�̗񋓑�
    /// </summary>
    public enum InputDataType {
        KeyNegative,
        KeyPositive,
        JoystickNegative,
        JoystickPositive
    }//InputDataType

    /// <summary>
    /// �����ҏW����(���͂����L�[���� �� axesButoon�p����)
    /// </summary>
    /// <param name="inputKeyCodeText">���͂����L�[����/param>
    /// <returns>�ҏW�������̓{�^������</returns>
    public static string EditText_InputKeyCodeText_To_AxesButtonText(string inputKeyCodeText) {
        string text = inputKeyCodeText;
        text = text.Replace("arrow", "");//���L�[
        text = text.Replace("alpha", "");//�����L�[
        text = text.Replace("page", "page ");//pageup,pagedown
        text = text.Replace("shift", " shift");//Shift�L�[
        text = text.Replace("control", " ctrl");//Control�L�[
        text = text.Replace("alt", " alt");//Alt�L�[
        text = text.Replace("command", " cmd");//Command�L�[
        text = text.Replace("sys", "sys ");//PRTSC�L�[
        text = text.Replace("scroll", "scroll ");//SCRLK�L�[
        text = text.Replace("joystickbutton", "joystick button ");//�R���g���[���L�[
        if (text.Contains("keypad")){
            text = text.Replace("keypad","");
            switch (text) {
                case "plus":
                    text = "+";
                    break;
                case "minus":
                    text = "-";
                    break;
                case "multiply":
                    text = "*";
                    break;
                case "divide":
                    text = "/";
                    break;
                case "period":
                    text = ".";
                    break;
                default:
                    break;
            }//switch
            if(text != "enter")
                text = "[" + text + "]";
            return text;
        }//if//�e���L�[
        if (text.Contains("bracket")) {
            text = text.Replace("bracket", "");
            switch (text) {
                case "left":
                    text = "[";
                    break;
                case "right":
                    text = "]";
                    break;
            }//switch
            return text;
        }//if//[]�L�[
        switch (text) {
            case "minus":
                text = "-";
                break;
            case "quote":
                text = "'";
                break;
            case "backquote":
                text = "`";
                break;
            case "equals":
                text = "=";
                break;
            case "semicolon":
                text = ";";
                break;
            case "comma":
                text = ",";
                break;
            case "period":
                text = ".";
                break;
            case "slash":
                text = "/";
                break;
            case "backslash":
                text="\\";
                break;
        }//switch//���̑��̃L�[
        return text;
    }//EditText_InputKeyCodeText_To_AxesButtonText

    /// <summary>
    /// AxesButton�̑ΏۊO�������`�F�b�N���鏈��
    /// </summary>
    /// <param name="checkText">���ׂ镶��</param>
    /// <returns>�ΏۊO�̕������̔���</returns>
    public static bool GetNonTargetTextCheck_AxesButton(string checkText) {
        checkText = checkText.Replace(" ", "");
        if (checkText.Contains("return"))
            return false;
        if (checkText.Contains("[") && checkText.Contains("]"))
            return false;
        if (checkText.Contains("button"))
            return false;
        if (checkText.Length == 1)
            return false;
        for (int i = 0; i <= 12; i++) {
            if (checkText == i.ToString()||checkText =="f"+i.ToString())
                return false;
        }//for//���l�EF�L�[�̃`�F�b�N
        foreach (TargetTextData_AxesButton data in Enum.GetValues(typeof(TargetTextData_AxesButton))){
            string targetText = Enum.GetName(typeof(TargetTextData_AxesButton), data);
            if (checkText == targetText)
                return false;
        }//foreach//�Ώە����񋓑̂̃`�F�b�N
        return true;
    }//GetNonTartetText

    /// <summary>
    /// AxesButton�̑Ώە����̗񋓑�
    /// </summary>
    private enum TargetTextData_AxesButton {
        tab,escape,leftshift,leftctrl,leftcmd,leftalt,
        space,rightalt,menu,rightctrl,rightshift,backspace,
        up,down,left,right,insert,delete,home,end,pageup,pagedown,
        sysreq,scrolllock,pause,enter,numlock,
    }//TargetTextData_AxesButton

    /// <summary>
    /// ���͂����l��ύX���鏈��
    /// </summary>
    /// <param name="targetAxesName">�Ώۂ�Axes��</param>
    /// <param name="changeInputValue">�ύX�\��̓��͒l</param>
    /// <param name="nowInputValue">���݂̓��͒l</param>
    /// <param name="nowInputValueType">���݂̓��͒l��InputDataType</param>
    public static void InputDataUpdate(
        string targetAxesName, string changeInputValue,string nowInputValue,InputDataType nowInputValueType) {
        int changeInputValueCount = -1;
        int nowInputValueCount = -1;
        InputDataType changeInputValueType = InputDataType.KeyPositive;

        nowInputValue = nowInputValue.Replace("button", "joystick button");//�R���g���[���l�̕����C��

        if (changeInputValue == nowInputValue)//�ύX�O�E�㋤�ɓ������͒l�̏ꍇ
            return;

        for (int i = 0; i < 12; i++) {//�ύX����InputManager�z���I��
            if (targetAxesName == _nameList[i]) {
                nowInputValueCount = i;
                break;
            }//if
        }//for

        for (int i = 0; i < 12; i++) {//�d�������L�[�̊m�F
            if (changeInputValue == _positiveButtonList[i]) {
                changeInputValueCount = i;
                changeInputValueType = InputDataType.KeyPositive;
            }else if (changeInputValue == _negativeButtonList[i]) {
                changeInputValueCount = i;
                changeInputValueType = InputDataType.KeyNegative;
            }else if (changeInputValue == _altPositionButtonList[i]) {
                changeInputValueCount = i;
                changeInputValueType = InputDataType.JoystickPositive;
            }//if

            if (changeInputValueCount != -1) {//�d�������L�[�̓���ւ��K�v�m�F
                if ((nowInputValueCount < 10 && changeInputValueCount < 10)|| 
                    (nowInputValueCount > 9 && nowInputValueCount < 12 && changeInputValueCount > 9 && changeInputValueCount < 12)) {
                    InputDataChange(_nameList[nowInputValueCount], nowInputValueType, changeInputValue);
                    InputDataChange(_nameList[changeInputValueCount], changeInputValueType, nowInputValue);
                    return;
                }//if
            }//if

        }//for
        //�d�����Ă��Ȃ��ꍇ
        InputDataChange(targetAxesName, nowInputValueType, changeInputValue);
    }//InputTextDuplicationCheack

}//InputManagerEdit
