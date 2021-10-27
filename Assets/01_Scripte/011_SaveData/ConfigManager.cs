//�Q�lURL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using ConfigDataDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using UnityEngine;

/// <summary>
/// �R���t�B�O�@�\���Ǘ�����
/// �X�V����:20211026
/// </summary>
public class ConfigManager : SingletonMonoBehaviour<ConfigManager> {

    public Config config = new Config();

    public new void Awake() {
        //�ŏ��̓f�t�H���g�̐ݒ���R���t�B�O�Ɋi�[
        config.SetDefaultConfig();
        //�R���t�B�O�t�@�C��������Γǂݏo��
        try {
            config.LoadConfigFile();
        } catch (IOException e) {
            config.SaveConfigFile();
        }//tru
    }//Awake
    
}//InputManager