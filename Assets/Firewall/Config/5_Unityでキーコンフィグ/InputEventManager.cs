//�Q�lURL:https://qiita.com/Es_Program/items/fde067254cfc68035173
using MBLDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ���͂ɂ��C�x���g���Ǘ�����
/// </summary>
public class InputEventManager : SingletonMonoBehaviour<InputEventManager> {
    private Dictionary<Key, EventHandler> onKeyEvents = new Dictionary<Key, EventHandler>();
    private Dictionary<Key, EventHandler> onKeyDownEvents = new Dictionary<Key, EventHandler>();
    private Dictionary<Key, EventHandler> onKeyUpEvents = new Dictionary<Key, EventHandler>();
    private Dictionary<Key, EventHandler> onKeyNotPressedEvents = new Dictionary<Key, EventHandler>();
    private Dictionary<Axes, EventHandler> onAxesEvents = new Dictionary<Axes, EventHandler>();
    private Dictionary<Axes, EventHandler> onAxesRowEvents = new Dictionary<Axes, EventHandler>();

    /// <summary>
    /// �L�[���̓C�x���g�̎��s�𐧌䂷��
    /// </summary>
    public bool Execute { get; set; }

    public void Awake() {
        Debug.LogError("InputEventManager.cs_Awake");
        Execute = true;

        //�L�[�̎�ނ̐������C�x���g�𐶐�����
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
        //HACK:EventArgs���p�����ėl�X�ȃf�[�^���󂯓n����悤�ɏo����

        KeyEventInvoke(InputManager.Instance.GetKey, onKeyEvents, new EventArgs());
        KeyEventInvoke(InputManager.Instance.GetKeyDown, onKeyDownEvents, new EventArgs());
        KeyEventInvoke(InputManager.Instance.GetKeyUp, onKeyUpEvents, new EventArgs());
        KeyEventInvoke((key) => { return !InputManager.Instance.GetKey(key); }, onKeyNotPressedEvents, new EventArgs());

        AxesEventInvoke(onAxesEvents, new EventArgs());
        AxesEventInvoke(onAxesRowEvents, new EventArgs());
    }

    /// <summary>
    /// �L�[���̓C�x���g���Z�b�g����
    /// </summary>
    /// <param name="key">�L�[�̎��</param>
    /// <param name="eventHandler">���s����C�x���g</param>
    internal void SetKeyEvent(Key key, EventHandler eventHandler) {
        onKeyEvents[key] += eventHandler;
    }

    /// <summary>
    /// �L�[���͊J�n���C�x���g���Z�b�g����
    /// </summary>
    /// <param name="key">�L�[�̎��</param>
    /// <param name="eventHandler">���s����C�x���g</param>
    internal void SetKeyDownEvent(Key key, EventHandler eventHandler) {
        onKeyDownEvents[key] += eventHandler;
    }

    /// <summary>
    /// �L�[���͏I�����C�x���g���Z�b�g����
    /// </summary>
    /// <param name="key">�L�[�̎��</param>
    /// <param name="eventHandler">���s����C�x���g</param>
    internal void SetKeyUpEvent(Key key, EventHandler eventHandler) {
        onKeyUpEvents[key] += eventHandler;
    }

    /// <summary>
    /// �L�[��������Ă��Ȃ��ꍇ�̃C�x���g���Z�b�g����
    /// </summary>
    /// <param name="key">�L�[�̎��</param>
    /// <param name="eventHandler">���s����C�x���g</param>
    internal void SetKeyNotPressedEvent(Key key, EventHandler eventHandler) {
        onKeyNotPressedEvents[key] += eventHandler;
    }

    /// <summary>
    /// �����͎��C�x���g���Z�b�g����
    /// </summary>
    /// <param name="axes">���̎��</param>
    /// <param name="eventHandler">���s����C�x���g</param>
    internal void SetAxesEvent(Axes axes, EventHandler eventHandler) {
        onAxesEvents[axes] += eventHandler;
    }

    /// <summary>
    /// �����͎��C�x���g���Z�b�g����
    /// </summary>
    /// <param name="axes">���̎��</param>
    /// <param name="eventHandler">���s����C�x���g</param>
    internal void SetAxesRowEvent(Axes axes, EventHandler eventHandler) {
        onAxesRowEvents[axes] += eventHandler;
    }

    /// <summary>
    /// �L�[���̓C�x���g����w�肵���C�x���g���폜����
    /// </summary>
    /// <param name="key">�L�[�̎��</param>
    /// <param name="eventHandler">�폜����C�x���g</param>
    internal void RemoveKeyEvent(Key key, EventHandler eventHandler) {
        onKeyEvents[key] -= eventHandler;
    }

    /// <summary>
    /// �L�[���͎��C�x���g����w�肵���C�x���g���폜����
    /// </summary>
    /// <param name="key">�L�[�̎��</param>
    /// <param name="eventHandler">�폜����C�x���g</param>
    internal void RemoveKeyDownEvent(Key key, EventHandler eventHandler) {
        onKeyDownEvents[key] -= eventHandler;
    }

    /// <summary>
    /// �L�[���͏I�����C�x���g����w�肵���C�x���g���폜����
    /// </summary>
    /// <param name="key">�L�[�̎��</param>
    /// <param name="eventHandler">�폜����C�x���g</param>
    internal void RemoveKeyUpEvent(Key key, EventHandler eventHandler) {
        onKeyUpEvents[key] -= eventHandler;
    }

    /// <summary>
    /// �L�[�����͂���Ă��Ȃ��ꍇ�̃C�x���g����w�肵���C�x���g���폜����
    /// </summary>
    /// <param name="key">�L�[�̎��</param>
    /// <param name="eventHandler">�폜����C�x���g</param>
    internal void RemoveKeyNotPressedEvent(Key key, EventHandler eventHandler) {
        onKeyNotPressedEvents[key] -= eventHandler;
    }

    /// <summary>
    /// �����͎��C�x���g���폜����
    /// </summary>
    /// <param name="axes">���̎��</param>
    /// <param name="eventHandler">�폜����C�x���g</param>
    internal void RemoveAxesEvent(Axes axes, EventHandler eventHandler) {
        onAxesEvents[axes] -= eventHandler;
    }

    /// <summary>
    /// �����͎��C�x���g���폜����
    /// </summary>
    /// <param name="axes">���̎��</param>
    /// <param name="eventHandler">�폜����C�x���g</param>
    internal void RemoveAxesRowEvent(Axes axes, EventHandler eventHandler) {
        onAxesRowEvents[axes] -= eventHandler;
    }

    /// <summary>
    /// �L�[�C�x���g�����s����
    /// �L�[�����͂���Ă��Ȃ���Ԃ̎��̓C�x���g�����s���Ȃ�
    /// </summary>
    /// <param name="keyEntryDecision">�L�[���͔�����s���q��</param>
    /// <param name="keyEvent">�L�[���Ƃ̃C�x���g���i�[����n�b�V���}�b�v</param>
    /// <param name="args">�C�x���g���s�ɗp�������</param>
    private void KeyEventInvoke(Func<Key, bool> keyEntryDecision, Dictionary<Key, EventHandler> keyEvent, EventArgs args) {
        foreach (Key key in Key.AllKeyData)
            if (keyEntryDecision(key)) {
                if (keyEvent[key] != null) {
                    //Debug.Log("keyEvent[key]__" + keyEvent[key]+"__"+key);//�Ή��L�[��(Action,Balloon...)
                    keyEvent[key](this, args);
                }
            }
                
    }

    /// <summary>
    /// ���C�x���g�����s����
    /// </summary>
    /// <param name="axesEntryDecision">�����͒l�擾���s���q��</param>
    /// <param name="axesEvent">�����Ƃ̃C�x���g���i�[����n�b�V���}�b�v</param>
    /// <param name="args">�C�x���g���s�ɗp�������</param>
    private void AxesEventInvoke(Dictionary<Axes, EventHandler> axesEvent, EventArgs args) {
        foreach (Axes axes in Axes.AllAxesData)
            if (axesEvent[axes] != null)
                axesEvent[axes](this, args);
    }
}