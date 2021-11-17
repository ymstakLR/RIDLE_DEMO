///�ꕔ�Q�l�T�C�g�Q�l http://wordpress.notargs.com/blog/blog/2015/01/23/92/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// InputManager�̕ҏW
/// �X�V����:20211006
/// </summary>
public static class InputManagerDataEdit {

    /// <summary>
    /// ���͂���l�̑Ή�����^�C�v�̗񋓑�
    /// </summary>
    public enum InputDataType {
        KeyNegative,
        KeyPositive,
        JoystickNegative,
        JoystickPositive,
        AxesPositive,
        AxesNegative
    }//InputDataType


    /// <summary>
    /// �R���t�B�O�l��ύX���鏈��
    /// </summary>
    /// <param name="targetAxesName">�Ώۂ�Axes��</param>
    /// <param name="changeKeyCode">�ύX�\��̓��͒l</param>
    /// <param name="nowInputKeyCode">���݂̓��͒l</param>
    /// <param name="inputType">���݂̓��͒l��InputDataType</param>
    public static void ConfigDataUpdate(
        string targetAxesName, KeyCode changeKeyCode, KeyCode nowInputKeyCode, int inputType) {
        if (changeKeyCode == nowInputKeyCode)//�ύX�O�E�㋤�ɓ������͒l�̏ꍇ
            return;
        switch (inputType) {
            case 0:
            case 1:
                ConfigManager.Instance.config.DuplicationCodeCheck(targetAxesName, changeKeyCode, nowInputKeyCode, inputType);
                break;
            case 2:
            case 3:
                ConfigManager.Instance.config.DuplicationCodeCheck(targetAxesName, changeKeyCode, nowInputKeyCode, inputType-2);
                break;
        }//switch
    }//InputTextDuplicationCheack   


    /// <summary>
    /// �R���t�B�O�{�^���S�Ă̕����X�V���s������
    /// </summary>
    public static void ConfigButtonsTextUpdate(GameObject buttonCanvas) {
        foreach (Transform childTransform in buttonCanvas.transform) {
            string axesName;
            InputManagerDataEdit.InputDataType inputDataType;
            (axesName, inputDataType) = ConfigButtonInfoSelect(childTransform.name.ToString());
            axesName = InputManagerDataEdit.GetConficButtonKeyCode(axesName, inputDataType).ToString();
            childTransform.GetChild(0).GetComponent<Text>().text = EditText_InputCodeText_To_AxesButtonText(axesName.ToLower()).ToUpper();
        }//foreach
    }//ConfigButtonsTextUpdate

    /// <summary>
    /// �R���t�B�O�{�^���̃L�[�R�[�h���擾���鏈��
    /// </summary>
    /// <param name="targetName">�Ώۂ�Axes��</param>
    /// <param name="targetInputDataType">�Ώۂ�InputDataType</param>
    /// <returns>�ΏۃR���t�B�O�{�^���̃L�[�R�[�h</returns>
    public static KeyCode GetConficButtonKeyCode(string targetName, InputDataType targetInputDataType) {
        KeyCode targetKeyCode = KeyCode.None;
        switch (targetInputDataType) {//Type���Ƃ�targetKeyText���Ăяo���X�V
            case InputDataType.KeyPositive:
                targetKeyCode = ConfigManager.Instance.config.GetInputKeyCodeCheck(targetName, Config.KeyType.KeyBoard);
                break;
            case InputDataType.JoystickPositive:
                targetKeyCode = ConfigManager.Instance.config.GetInputKeyCodeCheck(targetName, Config.KeyType.JoyStick);
                break;
            case InputDataType.AxesPositive:
                targetKeyCode = ConfigManager.Instance.config.GetInputAxisCodeCheck(targetName, Config.AxisType.Positive);
                break;
            case InputDataType.AxesNegative:
                targetKeyCode = ConfigManager.Instance.config.GetInputAxisCodeCheck(targetName, Config.AxisType.Negative);
                break;
        }//switch
        return targetKeyCode;
    }//GetConficButtonTex

    /// <summary>
    /// �R���t�B�O�{�^���̏��I������
    /// </summary>
    /// <param name="checkButtonName">���ׂ�{�^���I�u�W�F�N�g��</param>
    /// <returns>
    /// string �C��������(���ׂ�{�^������InputManager�̑Ώ�Axes��)
    /// InputDataType �Ώۂ�InputDataType�^�C�v
    /// </returns>
    public static (string, InputManagerDataEdit.InputDataType) ConfigButtonInfoSelect(string checkButtonName) {//�ʃX�N���v�g�Ɉړ�
        string editText = checkButtonName;
        InputManagerDataEdit.InputDataType type = InputManagerDataEdit.InputDataType.JoystickNegative;
        if (editText.Contains("Key")) {//�L�[�{�[�h�{�^���̏ꍇ
            editText = editText.Replace("Key", "");
            type = InputManagerDataEdit.InputDataType.KeyPositive;
        } else if (editText.Contains("Controller")) {//�R���g���[���{�^���̏ꍇ
            editText = editText.Replace("Controller", "");
            type = InputManagerDataEdit.InputDataType.JoystickPositive;
        }//if

        switch (editText) {//�ړ��L�[�̃e�L�X�g�ύX
            case "Up":
                editText = "Vertical";
                type = InputDataType.AxesPositive;
                break;
            case "Down":
                editText = "Vertical";
                type = InputDataType.AxesNegative;
                break;
            case "Left":
                editText = "Horizontal";
                type = InputDataType.AxesNegative;
                break;
            case "Right":
                editText = "Horizontal";
                type = InputDataType.AxesPositive;
                break;
            default:
                break;
        }//switch
        return (editText, type);
    }//ConfigButtonInfoSelect


    /// <summary>
    /// �����ҏW����(���͂����L�[���� �� axesButoon�p����)
    /// </summary>
    /// <param name="inputKeyCodeText">���͂����L�[����/param>
    /// <returns>�ҏW�������̓{�^������</returns>
    public static string EditText_InputCodeText_To_AxesButtonText(string inputKeyCodeText) {
        string text = inputKeyCodeText;
        text = text.Replace("arrow", "");//���L�[
        text = text.Replace("alpha", "");//�����L�[
        text = text.Replace("page", "page ");//pageup,pagedown
        text = text.Replace("shift", " shift");//Shift�L�[
        text = text.Replace("control", " ctrl");//Control�L�[
        text = text.Replace("alt", " alt");//Alt�L�[
        text = text.Replace("command", " cmd");//Command�L�[
        text = text.Replace("joystickbutton", "button");//�R���g���[���L�[
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
            case "scrolllock":
                text = "scrlk";
                break;
            case "sysreq":
                text = "prtsc";
                break;
        }//switch//���̑��̃L�[
        return text;
    }//EditText_InputKeyCodeText_To_AxesButtonText

    /// <summary>
    /// AxesButton�̑ΏۊO�������`�F�b�N���鏈��
    /// </summary>
    /// <param name="checkText">���ׂ镶��</param>
    /// <returns>�ΏۊO�̕������̔���</returns>
    public static bool GetNonTargetTextCheck(string checkText) {
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
        foreach (TargetTextData data in Enum.GetValues(typeof(TargetTextData))){
            string targetText = Enum.GetName(typeof(TargetTextData), data);
            if (checkText == targetText)
                return false;
        }//foreach//�Ώە����񋓑̂̃`�F�b�N
        return true;
    }//GetNonTartetText

    /// <summary>
    /// AxesButton�̑Ώە����̗񋓑�
    /// </summary>
    private enum TargetTextData {
        tab,escape,leftshift,leftctrl,leftcmd,leftalt,
        space,rightalt,menu,rightctrl,rightshift,backspace,
        up,down,left,right,insert,delete,home,end,pageup,pagedown,
        prtsc, scrlk, pause,enter,numlock,
    }//TargetTextData_AxesButton

}//InputManagerEdit