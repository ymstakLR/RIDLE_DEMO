//参考URL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

//[assembly: InternalsVisibleTo("UnitTest")]//必要か確認する必要あり(20211026)
/// <summary>
/// 全体で使用するコンフィグデータを定義する
/// 更新日時:20211026
/// </summary>
namespace MBLDefine {
    /// <summary>
    /// 入力値の基底クラス
    /// </summary>
    internal class InputValue {
        public readonly string String;
        protected InputValue(string name) {
            String = name;
        }//InputValue
    }//InputValue

    /// <summary>
    /// コンフィグデータの定義
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

        public static readonly ConfigData NormalJump = new ConfigData("NormalJump", new List<KeyCode> { KeyCode.J, KeyCode.JoystickButton0 });
        public static readonly ConfigData FlipJump = new ConfigData("FlipJump", new List<KeyCode> { KeyCode.L, KeyCode.JoystickButton2 });
        public static readonly ConfigData Attack = new ConfigData("Attack", new List<KeyCode> { KeyCode.K, KeyCode.JoystickButton1 });
        public static readonly ConfigData Pause = new ConfigData("Pause", new List<KeyCode> { KeyCode.H, KeyCode.JoystickButton9 });
        public static readonly ConfigData Horizontal = new ConfigData("Horizontal", new List<KeyCode> { KeyCode.A, KeyCode.D});
        public static readonly ConfigData Vertical =new ConfigData("Vertical", new List<KeyCode> { KeyCode.S, KeyCode.W });

        public static readonly ConfigData Submit = new ConfigData("Submit", new List<KeyCode> { KeyCode.K, KeyCode.JoystickButton1 });
        public static readonly ConfigData Cancel = new ConfigData("Cancel", new List<KeyCode> { KeyCode.L, KeyCode.JoystickButton2 });
    }//ConfigData

}//MBDefine