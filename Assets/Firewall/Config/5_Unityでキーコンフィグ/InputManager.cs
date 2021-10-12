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
            Debug.LogError("InputManager.cs__SetCurrentKey");
            //HACK:�}�E�X���͂��󂯕t����悤�ɂ���ׂ��Ȃ̂ō�����P
            //�}�E�X{0~6}�̓��͂�e��
            var currentInput = GetCurrentInputKeyCode().Where(c => c < KeyCode.Mouse0 || KeyCode.Mouse6 < c).ToList();
            Debug.Log("currentInput__" + currentInput);
            if (currentInput == null || currentInput.Count < 1)
                return false;
            var code = inputManager.keyConfig.GetKeyCode(key.String);
            Debug.Log("code__"+currentInput[0]);
            //���ɐݒ肳��Ă���L�[�ƈꕔ�ł������L�[��������Ă���ꍇ
            if (code.Count > currentInput.Count && currentInput.All(k => code.Contains(k)))
                return false;
            RemoveKey(key);
            Debug.LogWarning("InputManager.cs__SetCurrentKeyEnd");
            return SetKey(key, currentInput);
        }

        /// <summary>
        /// �f�t�H���g�̃L�[�ݒ��K�p����
        /// </summary>
        public void SetDefaultKeyConfig() {
            //Debug.LogError("InputManager__SetDefaultKeyConfig");
            foreach (var key in Key.AllKeyData) {
                //Debug.Log("key__"+key);///Action,Jump,Balloon...
                SetKey(key, key.DefaultKeyCode);
            }
            Debug.LogWarning("InputManager.cs__SetDefaultKeyConfigEnd");
        }

        //public List<KeyCode> GetKeyCode(Key keyName) {
        //    return inputManager.keyConfig.GetKeyCode(keyName.String);
        //}

        public void LoadSetting() {
            InputManager.Instance.keyConfig.LoadConfigFile();
        }

        public void SaveSetting() {
            InputManager.Instance.keyConfig.SaveConfigFile();
        }
    }

    #endregion InnerClass

    /// <summary>
    /// �g�p����L�[�R���t�B�O
    /// </summary>
    private KeyConfig keyConfig = new KeyConfig(ExternalFilePath.KEYCONFIG_PATH);

    public void Awake() {
        Debug.LogError("InputManager.cs_Awake");

        //�ŏ��̓f�t�H���g�̐ݒ���R���t�B�O�Ɋi�[
        KeyConfigSetting.Instance.SetDefaultKeyConfig();

        //�R���t�B�O�t�@�C��������Γǂݏo��
        try {
            InputManager.Instance.keyConfig.LoadConfigFile();
        } catch (IOException e) {
            Debug.Log(e.Message);
            InputManager.Instance.keyConfig.SaveConfigFile();
        }
        //InputManager.Instance.keyConfig.CheckConfig();
        Debug.LogWarning("InputManager.cs_Awake");

    }

    public void Update() {
        if (Input.anyKeyDown) {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {//���̓L�[�擾
                if (Input.GetKeyDown(code)) {
                    
                }//if
            }//foreach
        }
        KeyConfigSetting.Instance.SetCurrentKey(Key.Balloon);
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