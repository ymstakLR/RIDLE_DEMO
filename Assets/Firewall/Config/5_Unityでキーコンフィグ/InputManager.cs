//�Q�lURL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using MBLDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// ���͂��Ǘ�����
/// </summary>
internal class InputManager : SingletonMonoBehaviour<InputManager> {
    #region InnerClass //#region �w��͈͂̐܂��݋@�\

    /// <summary>
    /// �L�[�R���t�B�O�ݒ�̕ύX��ۑ��E���[�h���Ǘ�����
    /// </summary>
    internal class KeyConfigSetting {
        private Array keyCodeValues;
        private static InputManager inputManager;
        private static KeyConfigSetting instance;

        public static KeyConfigSetting Instance {
            get {
                if (inputManager == null)
                    inputManager = InputManager.Instance;
                return instance = instance != null ? instance : new KeyConfigSetting();
            }
        }

        private KeyConfigSetting() {
        }

        /// <summary>
        /// �Ăяo���ꂽ�t���[���ŉ�����Ԃ�KeyCode���X�g��Ԃ�
        /// </summary>
        /// <returns>������Ԃ�KeyCode���X�g</returns>
        private List<KeyCode> GetCurrentInputKeyCode() {
            List<KeyCode> ret = new List<KeyCode>();
            if (keyCodeValues == null) {
                keyCodeValues = Enum.GetValues(typeof(KeyCode));
            } 
            foreach (var code in keyCodeValues) {
                if (Input.GetKey((KeyCode)(int)code)) {
                    //Debug.Log("code_" + code);//�L�[���͂����l
                    ret.Add((KeyCode)(int)code);
                } 
            }
            return ret;
        }

        /// <summary>
        /// �R���t�B�O�ɃL�[���Z�b�g����
        /// </summary>
        /// <param name="key">�L�[��\�����ʎq</param>
        /// <param name="keyCode">���蓖�Ă�L�[�R�[�h</param>
        /// <returns>���蓖�Ă�����ɏI���������ǂ���</returns>
        public bool SetKey(Key key, List<KeyCode> keyCode) {
            //Debug.LogError("InputManager.cs_SetKey");
            //Debug.Log("keyCode.Count__" + keyCode.Count);//1
            return inputManager.keyConfig.SetKey(key.String, keyCode);
        }

        /// <summary>
        /// �R���t�B�O����l������
        /// </summary>
        /// <param name="key">�L�[��\�����ʎq</param>
        /// <returns>�l�̏���������ɏI���������ǂ���</returns>
        public bool RemoveKey(Key key) {
            return inputManager.keyConfig.RemoveKey(key.String);
        }

        /// <summary>
        /// ������Ă���L�[�𖼑O������ɑ΂���L�[�Ƃ��Đݒ肷��
        /// </summary>
        /// <param name="key">�L�[�Ɋ���t���閼�O</param>
        /// <returns>�L�[�R�[�h�̐ݒ肪����Ɋ����������ǂ���</returns>
        public bool SetCurrentKey(Key key) {
            //HACK:�}�E�X���͂��󂯕t����悤�ɂ���ׂ��Ȃ̂ō�����P
            //�}�E�X{0~6}�̓��͂�e��
            var currentInput = GetCurrentInputKeyCode().Where(c => c < KeyCode.Mouse0 || KeyCode.Mouse6 < c).ToList();
            if (currentInput == null || currentInput.Count < 1)
                return false;
            var code = inputManager.keyConfig.GetKeyCode(key.String);
            Debug.LogWarning("code__"+currentInput[0]);//���̓L�[
            //���ɐݒ肳��Ă���L�[�ƈꕔ�ł������L�[��������Ă���ꍇ
            if (code.Count > currentInput.Count && currentInput.All(k => code.Contains(k)))
                return false;
            RemoveKey(key);
            return SetKey(key, currentInput);
        }

        /// <summary>
        /// �f�t�H���g�̃L�[�ݒ��K�p����
        /// </summary>
        public void SetDefaultKeyConfig() {
            //Debug.LogError("InputManager__SetDefaultKeyConfig");
            foreach (var key in Key.AllKeyData) {
                //Debug.Log("key__"+key+" : DefalutKeyCode_"+key.DefaultKeyCode);///Action,Jump,Balloon...
                SetKey(key, key.DefaultKeyCode);
            }
            Debug.LogWarning("InputManager.cs__SetDefaultKeyConfigEnd");
        }


        public void SetDefaultAxesConfig() {
            foreach(var axes in Axes.AllAxesData) {
                SetAxes(axes, axes.DefaultKeyCode);
            }
        }

        public bool SetAxes(Axes key, List<KeyCode> keyCode) {
            return inputManager.axesConfig.SetAxes(key.String, keyCode);
        }

        public void LoadSetting() {
            InputManager.Instance.axesConfig.LoadAxesConfigFile();
        }

        public void SaveSetting() {
            InputManager.Instance.keyConfig.SaveKeyConfigFile();
        }

        //public KeyCode KeyCodeCheck(string keyName) {
        //    return InputManager.Instance.keyConfig.GetInputKeyCodeCheck(Key.Submit.String);
        //} 

    }

    #endregion InnerClass

    /// <summary>
    /// �g�p����L�[�R���t�B�O
    /// </summary>
    public KeyConfig keyConfig = new KeyConfig(ExternalFilePath.KEYCONFIG_PATH);
    public AxesConfig axesConfig = new AxesConfig(ExternalFilePath.AXESCONFIG_PATH);

    public void Awake() {
        Debug.LogError("InputManager.cs_Awake");

        //�ŏ��̓f�t�H���g�̐ݒ���R���t�B�O�Ɋi�[
        KeyConfigSetting.Instance.SetDefaultKeyConfig();
        KeyConfigSetting.Instance.SetDefaultAxesConfig();
        //�R���t�B�O�t�@�C��������Γǂݏo��
        try {
            InputManager.Instance.keyConfig.LoadKeyConfigFile();
        } catch (IOException e) {
            Debug.Log(e.Message);
            InputManager.Instance.keyConfig.SaveKeyConfigFile();
        }
        try {
            InputManager.Instance.axesConfig.LoadAxesConfigFile();
        } catch (IOException e) {
            Debug.Log(e.Message);
            InputManager.Instance.axesConfig.SaveAxesConfigFile();
        }


    }

    public void Update() {

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            InputManager.Instance.keyConfig.CheckKeyConfig();
        }
        //KeyConfigSetting.Instance.SetCurrentKey(Key.Cancel)//�L�[���͂����Ƃ������̃L�[�̑Ή����̓L�[��ύX����//�f�[�^�ۑ��͂���Ȃ�
        //if (Input.GetKeyDown(InputManager.Instance.keyConfig.GetInputKeyCodeCheck(Key.Submit.String))) {
        //    Debug.Log(InputManager.Instance.keyConfig.GetInputKeyCodeCheck(Key.Submit.String));
        //}

        //Debug.Log(GetAxesRaw(Axes.Horizontal));

    }




    /// <summary>
    /// �w�肵���L�[��������Ԃ��ǂ�����Ԃ�
    /// </summary>
    /// <returns>���͏��</returns>
    internal bool GetKey(Key key) {
        return keyConfig.GetKey(key.String);
    }

    /// <summary>
    /// �w�肵���L�[�����͂��ꂽ���ǂ�����Ԃ�
    /// </summary>
    /// <returns>���͏��</returns>
    internal bool GetKeyDown(Key key) {
        return keyConfig.GetKeyDown(key.String);
    }

    /// <summary>
    /// �w�肵���L�[�������ꂽ���ǂ�����Ԃ�
    /// </summary>
    /// <returns>���͏��</returns>
    internal bool GetKeyUp(Key key) {
        return keyConfig.GetKeyUp(key.String);
    }

    /// <summary>
    /// �����͂ɑ΂���l��Ԃ�
    /// </summary>
    /// <returns>���͒l</returns>
    internal float GetAxes(Axes axes) {
        return Input.GetAxis(axes.String);
    }

    /// <summary>
    /// �����͂ɑ΂���l��Ԃ�
    /// </summary>
    /// <returns>�������t�B���^�[���K�p����Ă��Ȃ����͒l</returns>
    internal float GetAxesRaw(Axes axes) {
        return Input.GetAxisRaw(axes.String);
    }
}