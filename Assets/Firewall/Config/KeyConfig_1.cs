using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2020年5月10日
// キー設定用クラス。
/*  i = KeyConfig.GetButton(Fire1);
 *  i > 0   True
 *  i < 0   KeyUp
 *  i == 0  Flase
 *  i > 2   KeyDown
 *  i == 2  LongTap
 *  i == 4  DoubleTap
 */

public class KeyConfig_1 {
    public static readonly string filename = "KeyConfig";
    static KeyConfig_1 instance;

    public float DoubleTapTime = 0.2f;
    public float LongTapTime = 1.0f;
    public float LongTapNextTime = 0.3f;

    float[] prevTapTime = { };
    int[] curKey = { };
    public KeyCode[] keyCodes = { };
    int lastFrameCount = 0;

    //public
    public static int GetButton(Key key) {
        int i = 0;
        if (Time.frameCount != instance.lastFrameCount) {
            instance.lastFrameCount = Time.frameCount;
            for (int j = 0; j < instance.curKey.Length; j++) {
                instance.curKey[j] = -2;
            }
        }
        if (instance.curKey[(int)key] > -2) {
            i = instance.curKey[(int)key];
        } else {
            if (Input.GetKey(instance.keyCodes[(int)key])) {
                if (Input.GetKeyDown(instance.keyCodes[(int)key])) {
                    if (instance.prevTapTime[(int)key] < 0 && Time.realtimeSinceStartup + instance.prevTapTime[(int)key] - instance.DoubleTapTime < 0) {
                        i = 4;
                    } else {
                        i = 3;
                    }
                    instance.prevTapTime[(int)key] = Time.realtimeSinceStartup + instance.DoubleTapTime;
                } else {
                    if (instance.prevTapTime[(int)key] > 0 && Time.realtimeSinceStartup - instance.prevTapTime[(int)key] > 0) {
                        instance.prevTapTime[(int)key] += instance.LongTapNextTime;
                        i = 2;
                    } else {
                        i = 1;
                    }
                }
            } else if (Input.GetKeyUp(instance.keyCodes[(int)key])) {
                i = -1;
                instance.prevTapTime[(int)key] = -Time.realtimeSinceStartup;
            }
            instance.curKey[(int)key] = i;
        }
        return i;
    }

    // static
    static public void Save() {
        FileManager_1.Instance.SaveFile<KeyConfig_1>(instance, filename);
    }

    static public void Set() {
        if (!FileManager_1.Instance.LoadFile<KeyConfig_1>(ref instance, filename)) {
            instance = new KeyConfig_1();
        }
        instance.prevTapTime = new float[System.Enum.GetNames(typeof(Key)).Length];
        instance.curKey = new int[System.Enum.GetNames(typeof(Key)).Length];
    }

    static public KeyCode[] KeyCodes {
        get { return instance.keyCodes; }
        set { instance.keyCodes = value; }
    }

    //

    KeyConfig_1() {
        // 初期設定
        KeyCode[] tempCodes =
            {
                KeyCode.UpArrow,
                KeyCode.DownArrow,
                KeyCode.LeftArrow,
                KeyCode.RightArrow,
                KeyCode.Z,
                KeyCode.X,
                KeyCode.Escape
            };
        keyCodes = tempCodes;
    }

    public enum Key {
        // スクリプト側参照キー
        up,
        down,
        left,
        right,
        action,
        back,
        menu
    }
}