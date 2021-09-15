#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// ���̃^�C�v
/// </summary>
public enum AxisType {
    KeyOrMouseButton = 0,
    MouseMovement = 1,
    JoystickAxis = 2
};

/// <summary>
/// ���͂̏��
/// </summary>
public class InputAxis {
    /// <summary>
    /// inputManager�e����R�}���h�̏��
    /// </summary>
    public string name = "";
    public string descriptiveName = "";
    public string descriptiveNegativeName = "";
    public string negativeButton = "";
    public string positiveButton = "";
    public string altNegativeButton = "";
    public string altPositiveButton = "";

    public float gravity = 0;
    public float dead = 0;
    public float sensitivity = 0;

    public bool snap = false;
    public bool invert = false;

    public AxisType type = AxisType.KeyOrMouseButton;

    // 1����n�܂�B
    public int axis = 1;
    // 0�Ȃ�S�ẴQ�[���p�b�h����擾�����B1�ȍ~�Ȃ�Ή������Q�[���p�b�h�B
    public int joyNum = 0;





    /// <summary>
    /// ������1�ɂȂ�L�[�̐ݒ�f�[�^���쐬����
    /// </summary>
    /// <returns>The button.</returns>
    /// <param name="name">Name.</param>
    /// <param name="positiveButton">Positive button.</param>
    /// <param name="altPositiveButton">Alternate positive button.</param>
    /// //�W�����v�E�U���{�^���Ŏg�p�ł���
    public static InputAxis CreateButton(string name, string positiveButton, string altPositiveButton) {
        var axis = new InputAxis();
        axis.name = name;
        axis.positiveButton = positiveButton;
        axis.altPositiveButton = altPositiveButton;
        axis.gravity = 1000;
        axis.dead = 0.001f;
        axis.sensitivity = 1000;
        axis.type = AxisType.KeyOrMouseButton;

        return axis;
    }

    /// <summary>
    /// �Q�[���p�b�h�p�̎��̐ݒ�f�[�^���쐬����
    /// </summary>
    /// <returns>The joy axis.</returns>
    /// <param name="name">Name.</param>
    /// <param name="joystickNum">Joystick number.</param>
    /// <param name="axisNum">Axis number.</param>
    /// ///�R���g���[���̍��E�ړ��Ŏg�p�ł���
    public static InputAxis CreatePadAxis(string name, int joystickNum, int axisNum) {
        var axis = new InputAxis();
        axis.name = name;
        axis.dead = 0.2f;
        axis.sensitivity = 1;
        axis.type = AxisType.JoystickAxis;
        axis.axis = axisNum;
        axis.joyNum = joystickNum;

        return axis;
    }

    /// <summary>
    /// �L�[�{�[�h�p�̎��̐ݒ�f�[�^���쐬����
    /// </summary>
    /// <returns>The key axis.</returns>
    /// <param name="name">Name.</param>
    /// <param name="negativeButton">Negative button.</param>
    /// <param name="positiveButton">Positive button.</param>
    /// <param name="axisNum">Axis number.</param>
    /// �L�[�{�[�h�̍��E�ړ��Ŏg�p�ł���
    public static InputAxis CreateKeyAxis(string name, string negativeButton, string positiveButton, string altNegativeButton, string altPositiveButton) {
        var axis = new InputAxis();
        axis.name = name;
        axis.negativeButton = negativeButton;
        axis.positiveButton = positiveButton;
        axis.altNegativeButton = altNegativeButton;
        axis.altPositiveButton = altPositiveButton;
        axis.gravity = 3;
        axis.sensitivity = 3;
        axis.dead = 0.001f;
        axis.type = AxisType.KeyOrMouseButton;

        return axis;
    }
}






/// <summary>
/// InputManager��ݒ肷�邽�߂̃N���X
/// </summary>
public class InputManagerGenerator {

    SerializedObject serializedObject;
    SerializedProperty axesProperty;
    

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public InputManagerGenerator() {
        // InputManager.asset���V���A���C�Y���ꂽ�I�u�W�F�N�g�Ƃ��ēǂݍ���
        serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        axesProperty = serializedObject.FindProperty("m_Axes");
    }

    /// <summary>
    /// ����ǉ����܂��B
    /// </summary>
    /// <param name="serializedObject">Serialized object.</param>
    /// <param name="axis">Axis.</param>
    public void AddAxis(InputAxis axis) {
        if (axis.axis < 1)
            Debug.LogError("Axis��1�ȏ�ɐݒ肵�Ă��������B");
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.arraySize++;
        serializedObject.ApplyModifiedProperties();

        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

        GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
        GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
        GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
        GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
        GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
        GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

        serializedObject.ApplyModifiedProperties();

    }

    /// <summary>
    /// �q�v�f�̃v���p�e�B���擾���܂��B
    /// </summary>
    /// <returns>The child property.</returns>
    /// <param name="parent">Parent.</param>
    /// <param name="name">Name.</param>
    private SerializedProperty GetChildProperty(SerializedProperty parent, string name) {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do {
            if (child.name == name)
                return child;
        }
        while (child.Next(false));
        return null;
    }

    /// <summary>
    /// �ݒ��S�ăN���A���܂��B
    /// </summary>
    public void Clear() {
        axesProperty.ClearArray();
        serializedObject.ApplyModifiedProperties();
    }
}

#endif