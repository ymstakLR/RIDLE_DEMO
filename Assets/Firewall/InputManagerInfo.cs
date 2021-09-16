using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public static class InputManagerInfo {
    public static void DefaultNameInsert(List<string>list,int i) {
        string[] name = { "Horizontal", "Vertical","Horizontal","Vertical",
                            "Attack","NormalJump", "FlipJump", "Pause",
                            "Submit", "Submit","Cancel","Cancel" };
        list.Insert(i, name[i]);
    }

    public static void DefaultNegativeButtonInsert(List<string> list,int i) {
        string[] negativeButton = { "a", "s", "", "", "", "", "", "", "", "", "", "" };
        list.Insert(i, negativeButton[i]);
    }

    public static void DefaultPositiveButtonInsert(List<string> list, int i) {
        string[] positiveButton = { "d", "w", "", "", "k", "j", "l", "h", "j", "k", "", "l" };
        list.Insert(i, positiveButton[i]);
    }
    public static void DefaultAltPositiveButtonInsert(List<string> list, int i) {
        string[] altPositiveButton = { "", "", "", "", "joystick button 1", "joystick button 0", "joystick button 2",
                                        "joystick button 9", "joystick button 0", "joystick button 1", "joystick button 2", "joystick button 3" };
        list.Insert(i, altPositiveButton[i]);
    }

    public static void DefaultInvert(List<bool>list,int i) {

    }

    public static void DefaultType(List<AxisType>list,int i) {

    }
    public static void DefaultAxis(List<int> list, int i) {

    }


}
