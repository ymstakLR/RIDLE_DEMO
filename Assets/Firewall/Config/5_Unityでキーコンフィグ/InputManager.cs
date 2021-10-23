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
        /// �R���t�B�O����l������
        /// </summary>
        /// <param name="key">�L�[��\�����ʎq</param>
        /// <returns>�l�̏���������ɏI���������ǂ���</returns>
        public bool RemoveKey(Key key) {
            return inputManager.keyConfig.RemoveKey(key.String);
        }

        /// <summary>
        /// �f�t�H���g�̃L�[�ݒ��K�p����
        /// </summary>
        public void SetDefaultKeyConfig() {
            foreach (var key in Key.AllKeyData) {
                SetKey(key, key.DefaultKeyCode);
            }
        }

        /// <summary>
        /// �R���t�B�O�ɃL�[���Z�b�g����
        /// </summary>
        /// <param name="key">�L�[��\�����ʎq</param>
        /// <param name="keyCode">���蓖�Ă�L�[�R�[�h</param>
        /// <returns>���蓖�Ă�����ɏI���������ǂ���</returns>
        public bool SetKey(Key key, List<KeyCode> keyCode) {
            return inputManager.keyConfig.SetKey(key.String, keyCode);
        }

    }

    #endregion InnerClass

    /// <summary>
    /// �g�p����L�[�R���t�B�O
    /// </summary>
    public KeyConfig keyConfig = new KeyConfig();

    public void Awake() {
        //�ŏ��̓f�t�H���g�̐ݒ���R���t�B�O�Ɋi�[
        KeyConfigSetting.Instance.SetDefaultKeyConfig();
        //�R���t�B�O�t�@�C��������Γǂݏo��
        try {
            InputManager.Instance.keyConfig.LoadKeyConfigFile();
        } catch (IOException e) {
            Debug.Log(e.Message);
            InputManager.Instance.keyConfig.SaveKeyConfigFile();
        }
    }

    public void Update() {
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

}