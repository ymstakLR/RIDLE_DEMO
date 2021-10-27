//参考URL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using ConfigDataDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using UnityEngine;

/// <summary>
/// コンフィグ機能を管理する
/// 更新日時:20211026
/// </summary>
public class ConfigManager : SingletonMonoBehaviour<ConfigManager> {

    public Config config = new Config();

    public new void Awake() {
        //最初はデフォルトの設定をコンフィグに格納
        config.SetDefaultConfig();
        //コンフィグファイルがあれば読み出す
        try {
            config.LoadConfigFile();
        } catch (IOException e) {
            config.SaveConfigFile();
        }//tru
    }//Awake
    
}//InputManager