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

        public static readonly Key Action = new Key("Action", new List<KeyCode> { KeyCode.Z });
        public static readonly Key Jump = new Key("Jump", new List<KeyCode> { KeyCode.Space });
        public static readonly Key Balloon = new Key("Balloon", new List<KeyCode> { KeyCode.X });
        public static readonly Key Squat = new Key("Squat", new List<KeyCode> { KeyCode.LeftShift });
        public static readonly Key Menu = new Key("Menu", new List<KeyCode> { KeyCode.Escape });

        public static readonly Key Submit = new Key("Submit", new List<KeyCode> {KeyCode.K });
    }

    /// <summary>
    /// �g�p���鎲���͂�\���N���X
    /// </summary>
    internal sealed class Axes : InputValue {
        public readonly static List<Axes> AllAxesData = new List<Axes>();

        private Axes(string axesName)
            : base(axesName) {
            AllAxesData.Add(this);
        }

        public override string ToString() {
            return String;
        }

        public static Axes Horizontal = new Axes("Horizontal");
        public static Axes Vertical = new Axes("Vertical");
    }
}