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
            AllKeyData.Add(this);
            
        }

        public override string ToString() {
            return String;
        }

        public static readonly Key NormalJump = new Key("NormalJump", new List<KeyCode> { KeyCode.J, KeyCode.JoystickButton0 });
        public static readonly Key FlipJump = new Key("FlipJump", new List<KeyCode> { KeyCode.L, KeyCode.JoystickButton2 });
        public static readonly Key Attack = new Key("Attack", new List<KeyCode> { KeyCode.K, KeyCode.JoystickButton1 });
        public static readonly Key Pause = new Key("Pause", new List<KeyCode> { KeyCode.H, KeyCode.JoystickButton9 });
        public static readonly Key Horizontal = new Key("Horizontal", new List<KeyCode> { KeyCode.A, KeyCode.D});
        public static readonly Key Vertical =new Key("Vertical", new List<KeyCode> { KeyCode.S, KeyCode.W });

        public static readonly Key Submit = new Key("Submit", new List<KeyCode> { KeyCode.K, KeyCode.JoystickButton1 });
        public static readonly Key Cancel = new Key("Cancel", new List<KeyCode> { KeyCode.L, KeyCode.JoystickButton2 });
    }

    /// <summary>
    /// �g�p����L�[��\���N���X
    /// </summary>
    internal sealed class DefalutKey : InputValue {
        public readonly List<KeyCode> DefaultKeyCode;
        public readonly static List<DefalutKey> AllKeyData = new List<DefalutKey>();
        private DefalutKey(string keyName, List<KeyCode> defaultKeyCode)
            : base(keyName) {
            DefaultKeyCode = defaultKeyCode;
            AllKeyData.Add(this);

        }

        public override string ToString() {
            return String;
        }

        private static readonly DefalutKey NormalJump = new DefalutKey("NormalJump", new List<KeyCode> { KeyCode.J, KeyCode.JoystickButton0 });
        private static readonly DefalutKey FlipJump = new DefalutKey("FlipJump", new List<KeyCode> { KeyCode.L, KeyCode.JoystickButton2 });
        private static readonly DefalutKey Attack = new DefalutKey("Attack", new List<KeyCode> { KeyCode.K, KeyCode.JoystickButton1 });
        private static readonly DefalutKey Pause = new DefalutKey("Pause", new List<KeyCode> { KeyCode.H, KeyCode.JoystickButton9 });
        private static readonly DefalutKey Horizontal = new DefalutKey("Horizontal", new List<KeyCode> { KeyCode.A, KeyCode.D });
        private static readonly DefalutKey Vertical = new DefalutKey("Vertical", new List<KeyCode> { KeyCode.S, KeyCode.W });

        private static readonly DefalutKey Submit = new DefalutKey("Submit", new List<KeyCode> { KeyCode.K, KeyCode.JoystickButton1 });
        private static readonly DefalutKey Cancel = new DefalutKey("Cancel", new List<KeyCode> { KeyCode.L, KeyCode.JoystickButton2 });
    }
}