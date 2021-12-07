//�Q�lURL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

//[assembly: InternalsVisibleTo("UnitTest")]//�K�v���m�F����K�v����(20211026)
/// <summary>
/// �S�̂Ŏg�p����R���t�B�O�f�[�^���`����
/// �X�V����:20211026
/// </summary>
namespace ConfigDataDefine {
    /// <summary>
    /// ���͒l�̊��N���X
    /// </summary>
    internal class InputValue {
        public readonly string String;
        protected InputValue(string name) {
            String = name;
        }//InputValue
    }//InputValue   

    /// <summary>
    /// �R���t�B�O�f�[�^�̒�`
    /// </summary>
    internal sealed class ConfigData : InputValue {
        public readonly List<KeyCode> DefaultCode;
        public readonly static List<ConfigData> AllCodeData = new List<ConfigData>();
        private ConfigData(string configName, List<KeyCode> defaultCode)
            : base(configName) {
            DefaultCode = defaultCode;
            AllCodeData.Add(this);
        }//ConfigData

        public override string ToString() {
            return String;
        }//ToString

        public static readonly ConfigData NormalJump = new ConfigData("NormalJump", new List<KeyCode> { KeyCode.Z, KeyCode.JoystickButton2 });
        public static readonly ConfigData FlipJump = new ConfigData("FlipJump", new List<KeyCode> { KeyCode.C, KeyCode.JoystickButton1 });
        public static readonly ConfigData Attack = new ConfigData("Attack", new List<KeyCode> { KeyCode.X, KeyCode.JoystickButton0 });
        public static readonly ConfigData Pause = new ConfigData("Pause", new List<KeyCode> { KeyCode.V, KeyCode.JoystickButton7 });
        public static readonly ConfigData Horizontal = new ConfigData("Horizontal", new List<KeyCode> { KeyCode.LeftArrow, KeyCode.RightArrow});
        public static readonly ConfigData Vertical =new ConfigData("Vertical", new List<KeyCode> { KeyCode.DownArrow, KeyCode.UpArrow });

        public static readonly ConfigData Submit = new ConfigData("Submit", new List<KeyCode> { KeyCode.X, KeyCode.JoystickButton0 });
        public static readonly ConfigData Cancel = new ConfigData("Cancel", new List<KeyCode> { KeyCode.Z, KeyCode.JoystickButton1 });
    }//ConfigData

}//MBDefine