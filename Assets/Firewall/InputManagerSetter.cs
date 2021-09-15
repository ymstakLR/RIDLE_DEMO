#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// InputManager�������I�ɐݒ肵�Ă����N���X
/// </summary>
public class InputManagerSetter {


    /// <summary>
    /// �C���v�b�g�}�l�[�W���[���Đݒ肵�܂��B
    /// </summary>
    [MenuItem("Util/Reset InputManager")]
    public static void ResetInputManager() {
        Debug.Log("�C���v�b�g�}�l�[�W���[�̐ݒ���J�n���܂��B");
        InputManagerGenerator inputManagerGenerator = new InputManagerGenerator();

        Debug.Log("�ݒ��S�ăN���A���܂��B");
        inputManagerGenerator.Clear();

        Debug.Log("�v���C���[���Ƃ̐ݒ��ǉ����܂��B");
        for (int i = 0; i < 4; i++) {
            AddPlayerInputSettings(inputManagerGenerator, i);
        }

        Debug.Log("�O���[�o���ݒ��ǉ����܂��B");
        AddGlobalInputSettings(inputManagerGenerator);

        Debug.Log("�C���v�b�g�}�l�[�W���[�̐ݒ肪�������܂����B");
    }

    /// <summary>
    /// �O���[�o���ȓ��͐ݒ��ǉ�����iOK�A�L�����Z���Ȃǁj
    /// </summary>
    /// <param name="inputManagerGenerator">Input manager generator.</param>
    private static void AddGlobalInputSettings(InputManagerGenerator inputManagerGenerator) {

        // ������
        {
            var name = "Horizontal";
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, 0, 1));
            inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, "a", "d", "left", "right"));
        }

        // �c����
        {
            var name = "Vertical";
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, 0, 2));
            inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, "s", "w", "down", "up"));
        }

        // ����
        {
            var name = "OK";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "z", "joystick button 0"));
        }

        // �L�����Z��
        {
            var name = "Cancel";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "x", "joystick button 1"));
        }

        // �|�[�Y
        {
            var name = "Pause";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "escape", "joystick button 7"));
        }
    }

    /// <summary>
    /// �v���C���[���Ƃ̓��͐ݒ��ǉ�����
    /// </summary>
    /// <param name="inputManagerGenerator">Input manager generator.</param>
    /// <param name="playerIndex">Player index.</param>
    private static void AddPlayerInputSettings(InputManagerGenerator inputManagerGenerator, int playerIndex) {
        if (playerIndex < 0 || playerIndex > 3)
            Debug.LogError("�v���C���[�C���f�b�N�X�̒l���s���ł��B");
        string upKey = "", downKey = "", leftKey = "", rightKey = "", attackKey = "";
        GetAxisKey(out upKey, out downKey, out leftKey, out rightKey, out attackKey, playerIndex);

        int joystickNum = playerIndex + 1;

        // ������
        {
            var name = string.Format("Player{0} Horizontal", playerIndex);
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, joystickNum, 1));
            inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, leftKey, rightKey, "", ""));
        }

        // �c����
        {
            var name = string.Format("Player{0} Vertical", playerIndex);
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, joystickNum, 2));
            inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, downKey, upKey, "", ""));
        }


        // �U��
        {
            var axis = new InputAxis();
            var name = string.Format("Player{0} Attack", playerIndex);
            var button = string.Format("joystick {0} button 0", joystickNum);
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, button, attackKey));
        }
    }

    /// <summary>
    /// �L�[�{�[�h�Ńv���C�����ꍇ�A���蓖�����Ă���L�[���擾����
    /// </summary>
    /// <param name="upKey">Up key.</param>
    /// <param name="downKey">Down key.</param>
    /// <param name="leftKey">Left key.</param>
    /// <param name="rightKey">Right key.</param>
    /// <param name="attackKey">Attack key.</param>
    /// <param name="playerIndex">Player index.</param>
    private static void GetAxisKey(out string upKey, out string downKey, out string leftKey, out string rightKey, out string attackKey, int playerIndex) {
        upKey = "";
        downKey = "";
        leftKey = "";
        rightKey = "";
        attackKey = "";

        switch (playerIndex) {
            case 0:
                upKey = "w";
                downKey = "s";
                leftKey = "a";
                rightKey = "d";
                attackKey = "e";
                break;
            case 1:
                upKey = "i";
                downKey = "k";
                leftKey = "j";
                rightKey = "l";
                attackKey = "o";
                break;
            case 2:
                upKey = "up";
                downKey = "down";
                leftKey = "left";
                rightKey = "right";
                attackKey = "[0]";
                break;
            case 3:
                upKey = "[8]";
                downKey = "[5]";
                leftKey = "[4]";
                rightKey = "[6]";
                attackKey = "[9]";
                break;
            default:
                Debug.LogError("�v���C���[�C���f�b�N�X�̒l���s���ł��B");
                upKey = "";
                downKey = "";
                leftKey = "";
                rightKey = "";
                attackKey = "";
                break;
        }
    }
}

#endif