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
        private static InputManager inputManager;
        private static KeyConfigSetting instance;

        public static KeyConfigSetting Instance {
            get {
                if (inputManager == null)
                    inputManager = InputManager.Instance;
                return instance = instance != null ? instance : new KeyConfigSetting();
            }
        }

        /// <summary>
        /// �f�t�H���g�̃L�[�ݒ��K�p����
        /// </summary>
        public void SetDefaultKeyConfig() {
            foreach (var key in Key.AllKeyData) {
                inputManager.keyConfig.SetKey(key.String, key.DefaultKeyCode);
            }
        }

    }
    #endregion InnerClass

    /// <summary>
    /// �g�p����L�[�R���t�B�O
    /// </summary>
    public Config keyConfig = new Config();

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

}