//éQçlURL:https://anchan828.hatenablog.jp/entry/2013/02/27/184519
using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Move), typeof(Jump))]
public class Player : MonoBehaviour {
    private Move move;
    private Jump jump;

    void Awake() {
        move = GetComponent<Move>();
        jump = GetComponent<Jump>();

        move.Walk += (sender, e) => Walk();
        move.Run += (sender, e) => Run();

        jump.Jumping += (sender, e) => Jumping();
        jump.JumpEnd += (sender, e) => JumpEnd();

    }

    void Walk() {
        Debug.Log("Walk");
    }

    void Run() {
        Debug.Log("Run");
    }

    void Jumping() {
        Debug.Log("Jumping");
    }

    void JumpEnd() {
        Debug.Log("JumpEnd");
    }
}