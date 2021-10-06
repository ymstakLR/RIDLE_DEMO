using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// InputManagerの各ボタンごとの初期情報
/// 更新日時:20211006
/// </summary>
public static class InputManagerDataInitialSetting {
    public static void DefaultNameInsert(List<string>list,int i) {
        string[] name = {
            "Horizontal", "Vertical","Horizontal","Vertical",
            "Horizontal","Vertical","Attack","NormalJump",
            "FlipJump", "Pause","Submit","Cancel"
        };
        list.Insert(i, name[i]);
    }
    //public static void DefaultDescriptiveNameInsert(List<string>list,int i) {
    //    string[] descriptiveName = {
    //        "HorizontalKey","VerticalKey","HorizontalJoystick","VerticalJoystick",
    //        "HorizontalCrossKey","VerticalCrossKey","Attack","Jump",
    //        "FlipJump", "Pause","Submit","Cancel"
    //    };
    //    list.Insert(i, descriptiveName[i]);
    //}
    public static void DefaultNegativeButtonInsert(List<string> list,int i) {
        string[] negativeButton = { "a", "s", "", "", "", "", "", "", "", "", "", "" };
        list.Insert(i, negativeButton[i]);
    }
    public static void DefaultPositiveButtonInsert(List<string> list, int i) {
        string[] positiveButton = { "d", "w", "", "", "", "", "k", "j", "l", "h",  "k", "l" };
        list.Insert(i, positiveButton[i]);
    }
    public static void DefaultAltPositiveButtonInsert(List<string> list, int i) {
        string[] altPositiveButton = { "", "", "", "","", "", "joystick button 1", "joystick button 0", "joystick button 2",
                                        "joystick button 9", "joystick button 1", "joystick button 2" };
        list.Insert(i, altPositiveButton[i]);
    }
    public static void DefaultInvert(List<string>list,int i) {
        string invert;
        if (i == 3) {
            invert = true.ToString();
        } else {
            invert = false.ToString();
        }
        list.Insert(i, invert);
    }
    public static void DefaultType(List<string>list,int i) {
        string type;
        if(2 <= i&&  i <=5) {
            type = AxisType.JoystickAxis.ToString();
        } else {
            type = AxisType.KeyOrMouseButton.ToString();
        }//if
        list.Insert(i, type);
    }
    public static void DefaultAxis(List<string> list, int i) {
        string axis;
        switch (i) {
            case 3:
                axis = 2.ToString();
                break;
            case 4:
                axis = 7.ToString();
                break;
            case 5:
                axis = 8.ToString();
                break;
            default:
                axis = 1.ToString();
                break;
        }
        list.Insert(i, axis);
    }
}//InputManagerInfo

/// <summary>
/// 軸のタイプ
/// </summary>
public enum AxisType {
    KeyOrMouseButton = 0,
    MouseMovement = 1,
    JoystickAxis = 2
};
