using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadExample : MonoBehaviour {
    void Update() {
        // ゲームパッドが接続されていないとnullになる。
        if (Gamepad.current == null)
            return;

        if (Gamepad.current.buttonNorth.wasPressedThisFrame) {
            Debug.Log("↑が押された！");
            Debug.Log("↑__"+Gamepad.current.buttonNorth.ReadValue());
        }
        if (Gamepad.current.buttonSouth.wasPressedThisFrame) {
            Debug.Log("↓が離された！");
        }
        if (Gamepad.current.buttonWest.wasPressedThisFrame) {
            Debug.Log("←が離された！");
        }
        if (Gamepad.current.buttonEast.wasPressedThisFrame) {
            Debug.Log("→が離された！");
        }
    }

    void OnGUI() {
        if (Gamepad.current == null)
            return;

        GUILayout.Label($"leftStick: {Gamepad.current.leftStick.ReadValue()}");
        GUILayout.Label($"buttonNorth: {Gamepad.current.buttonNorth.isPressed}");
        GUILayout.Label($"buttonSouth: {Gamepad.current.buttonSouth.isPressed}");
        GUILayout.Label($"buttonEast: {Gamepad.current.buttonEast.isPressed}");
        GUILayout.Label($"buttonWest: {Gamepad.current.buttonWest.isPressed}");
        GUILayout.Label($"leftShoulder: {Gamepad.current.leftShoulder.ReadValue()}");
        GUILayout.Label($"leftTrigger: {Gamepad.current.leftTrigger.ReadValue()}");
        GUILayout.Label($"rightShoulder: {Gamepad.current.rightShoulder.ReadValue()}");
        GUILayout.Label($"rightTrigger: {Gamepad.current.rightTrigger.ReadValue()}");
    }
}