//参考URL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using MBLDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 入力によるイベントを管理する
/// </summary>
public class InputEventManager : SingletonMonoBehaviour<InputEventManager> {
    private Dictionary<Key, EventHandler> onKeyEvents = new Dictionary<Key, EventHandler>();
    private Dictionary<Key, EventHandler> onKeyDownEvents = new Dictionary<Key, EventHandler>();
    private Dictionary<Key, EventHandler> onKeyUpEvents = new Dictionary<Key, EventHandler>();
    private Dictionary<Key, EventHandler> onKeyNotPressedEvents = new Dictionary<Key, EventHandler>();
    private Dictionary<Axes, EventHandler> onAxesEvents = new Dictionary<Axes, EventHandler>();
    private Dictionary<Axes, EventHandler> onAxesRowEvents = new Dictionary<Axes, EventHandler>();

    /// <summary>
    /// キー入力イベントの実行を制御する
    /// </summary>
    public bool Execute { get; set; }

    public void Awake() {
        Debug.LogError("InputEventManager.cs_Awake");
        Execute = true;

        //キーの種類の数だけイベントを生成する
        foreach (Key key in Key.AllKeyData) {
            onKeyEvents.Add(key, (o, a) => { });
            onKeyDownEvents.Add(key, (o, a) => { });
            onKeyUpEvents.Add(key, (o, a) => { });
            onKeyNotPressedEvents.Add(key, (o, a) => { });
        }

        foreach (Axes axes in Axes.AllAxesData) {
            onAxesEvents.Add(axes, (o, a) => { });
            onAxesRowEvents.Add(axes, (o, a) => { });
        }
        Debug.LogWarning("InputEventManager.cs_Awake");
    }

    public void Update() {
        if (!Execute)
            return;
        //HACK:EventArgsを継承して様々なデータを受け渡せるように出来る

        KeyEventInvoke(InputManager.Instance.GetKey, onKeyEvents, new EventArgs());
        KeyEventInvoke(InputManager.Instance.GetKeyDown, onKeyDownEvents, new EventArgs());
        KeyEventInvoke(InputManager.Instance.GetKeyUp, onKeyUpEvents, new EventArgs());
        KeyEventInvoke((key) => { return !InputManager.Instance.GetKey(key); }, onKeyNotPressedEvents, new EventArgs());

        AxesEventInvoke(onAxesEvents, new EventArgs());
        AxesEventInvoke(onAxesRowEvents, new EventArgs());
    }

    /// <summary>
    /// キー入力イベントをセットする
    /// </summary>
    /// <param name="key">キーの種類</param>
    /// <param name="eventHandler">実行するイベント</param>
    internal void SetKeyEvent(Key key, EventHandler eventHandler) {
        onKeyEvents[key] += eventHandler;
    }

    /// <summary>
    /// キー入力開始時イベントをセットする
    /// </summary>
    /// <param name="key">キーの種類</param>
    /// <param name="eventHandler">実行するイベント</param>
    internal void SetKeyDownEvent(Key key, EventHandler eventHandler) {
        onKeyDownEvents[key] += eventHandler;
    }

    /// <summary>
    /// キー入力終了時イベントをセットする
    /// </summary>
    /// <param name="key">キーの種類</param>
    /// <param name="eventHandler">実行するイベント</param>
    internal void SetKeyUpEvent(Key key, EventHandler eventHandler) {
        onKeyUpEvents[key] += eventHandler;
    }

    /// <summary>
    /// キーが押されていない場合のイベントをセットする
    /// </summary>
    /// <param name="key">キーの種類</param>
    /// <param name="eventHandler">実行するイベント</param>
    internal void SetKeyNotPressedEvent(Key key, EventHandler eventHandler) {
        onKeyNotPressedEvents[key] += eventHandler;
    }

    /// <summary>
    /// 軸入力時イベントをセットする
    /// </summary>
    /// <param name="axes">軸の種類</param>
    /// <param name="eventHandler">実行するイベント</param>
    internal void SetAxesEvent(Axes axes, EventHandler eventHandler) {
        onAxesEvents[axes] += eventHandler;
    }

    /// <summary>
    /// 軸入力時イベントをセットする
    /// </summary>
    /// <param name="axes">軸の種類</param>
    /// <param name="eventHandler">実行するイベント</param>
    internal void SetAxesRowEvent(Axes axes, EventHandler eventHandler) {
        onAxesRowEvents[axes] += eventHandler;
    }

    /// <summary>
    /// キー入力イベントから指定したイベントを削除する
    /// </summary>
    /// <param name="key">キーの種類</param>
    /// <param name="eventHandler">削除するイベント</param>
    internal void RemoveKeyEvent(Key key, EventHandler eventHandler) {
        onKeyEvents[key] -= eventHandler;
    }

    /// <summary>
    /// キー入力時イベントから指定したイベントを削除する
    /// </summary>
    /// <param name="key">キーの種類</param>
    /// <param name="eventHandler">削除するイベント</param>
    internal void RemoveKeyDownEvent(Key key, EventHandler eventHandler) {
        onKeyDownEvents[key] -= eventHandler;
    }

    /// <summary>
    /// キー入力終了時イベントから指定したイベントを削除する
    /// </summary>
    /// <param name="key">キーの種類</param>
    /// <param name="eventHandler">削除するイベント</param>
    internal void RemoveKeyUpEvent(Key key, EventHandler eventHandler) {
        onKeyUpEvents[key] -= eventHandler;
    }

    /// <summary>
    /// キーが入力されていない場合のイベントから指定したイベントを削除する
    /// </summary>
    /// <param name="key">キーの種類</param>
    /// <param name="eventHandler">削除するイベント</param>
    internal void RemoveKeyNotPressedEvent(Key key, EventHandler eventHandler) {
        onKeyNotPressedEvents[key] -= eventHandler;
    }

    /// <summary>
    /// 軸入力時イベントを削除する
    /// </summary>
    /// <param name="axes">軸の種類</param>
    /// <param name="eventHandler">削除するイベント</param>
    internal void RemoveAxesEvent(Axes axes, EventHandler eventHandler) {
        onAxesEvents[axes] -= eventHandler;
    }

    /// <summary>
    /// 軸入力時イベントを削除する
    /// </summary>
    /// <param name="axes">軸の種類</param>
    /// <param name="eventHandler">削除するイベント</param>
    internal void RemoveAxesRowEvent(Axes axes, EventHandler eventHandler) {
        onAxesRowEvents[axes] -= eventHandler;
    }

    /// <summary>
    /// キーイベントを実行する
    /// キーが入力されていない状態の時はイベントを実行しない
    /// </summary>
    /// <param name="keyEntryDecision">キー入力判定を行う述語</param>
    /// <param name="keyEvent">キーごとのイベントを格納するハッシュマップ</param>
    /// <param name="args">イベント実行に用いる引数</param>
    private void KeyEventInvoke(Func<Key, bool> keyEntryDecision, Dictionary<Key, EventHandler> keyEvent, EventArgs args) {
        foreach (Key key in Key.AllKeyData)
            if (keyEntryDecision(key)) {
                if (keyEvent[key] != null) {
                    //Debug.Log("keyEvent[key]__" + keyEvent[key]+"__"+key);//対応キー名(Action,Balloon...)
                    keyEvent[key](this, args);
                }
            }
                
    }

    /// <summary>
    /// 軸イベントを実行する
    /// </summary>
    /// <param name="axesEntryDecision">軸入力値取得を行う述語</param>
    /// <param name="axesEvent">軸ごとのイベントを格納するハッシュマップ</param>
    /// <param name="args">イベント実行に用いる引数</param>
    private void AxesEventInvoke(Dictionary<Axes, EventHandler> axesEvent, EventArgs args) {
        foreach (Axes axes in Axes.AllAxesData)
            if (axesEvent[axes] != null)
                axesEvent[axes](this, args);
    }
}