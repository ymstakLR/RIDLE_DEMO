//�Q�lURL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("UnitTest")]

/// <summary>
/// ���̃Q�[���Œ�`����萔�Ȃǂ�����
/// </summary>
namespace MBLDefine {
    /// <summary>
    /// �O���t�@�C���ւ̎Q�ƂɕK�v�ȃp�X�Q
    /// </summary>
    internal struct ExternalFilePath {
        internal const string KEYCONFIG_PATH = "keyconf.dat";
        internal const string AXESCONFIG_PATH = "axesconf.dat";
    }

    /// <summary>
    /// ���͒l�̊��N���X
    /// </summary>
    internal class InputValue {
        public readonly string String;

        protected InputValue(string name) {
            String = name;
        }
    }

    /// <summary>
    /// �g�p����L�[��\���N���X
    /// </summary>
    internal sealed class Key : InputValue {
        public readonly List<KeyCode> DefaultKeyCode;
        public readonly static List<Key> AllKeyData = new List<Key>();

        private Key(string keyName, List<KeyCode> defaultKeyCode)
            : base(keyName) {
            DefaultKeyCode = defaultKeyCode;
            //Debug.Log("this_" + this);//Action,Balloon...
            AllKeyData.Add(this);
        }

        public override string ToString() {
            return String;
        }

        public static readonly Key NormalJump = new Key("NormalJump", new List<KeyCode> { KeyCode.J, KeyCode.JoystickButton0 });
        public static readonly Key FlipJump = new Key("FlipJump", new List<KeyCode> { KeyCode.L, KeyCode.JoystickButton2 });
        public static readonly Key Attack = new Key("Attack", new List<KeyCode> { KeyCode.K, KeyCode.JoystickButton1 });
        public static readonly Key Pause = new Key("Pause", new List<KeyCode> { KeyCode.H, KeyCode.JoystickButton9 });

        public static readonly Key Submit = new Key("Submit", new List<KeyCode> { KeyCode.K, KeyCode.JoystickButton1 });
        public static readonly Key Cancel = new Key("Cancel", new List<KeyCode> { KeyCode.L, KeyCode.JoystickButton2 });
    }

    /// <summary>
    /// �g�p���鎲���͂�\���N���X
    /// </summary>
    internal sealed class Axes : InputValue {
        public readonly List<KeyCode> DefaultKeyCode;
        public readonly static List<Axes> AllAxesData = new List<Axes>();

        private Axes(string axesName,List<KeyCode> defalutKeyCode)
            : base(axesName) {
            DefaultKeyCode = defalutKeyCode;
            //Debug.Log("axesName__" + axesName);
            //Debug.Log(defalutKeyCode[0][0]);
            //Debug.Log(defalutKeyCode[0][1]);
            AllAxesData.Add(this);
        }

        public override string ToString() {
            return String;
        }

        public static Axes Horizontal = new Axes("Horizontal",new List<KeyCode> {KeyCode.LeftArrow, KeyCode.RightArrow });
        public static Axes Vertical = new Axes("Vertical", new List<KeyCode> { KeyCode.DownArrow, KeyCode.UpArrow } );
        public static Axes Test = new Axes("Test", new List<KeyCode> { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 });
    }
}