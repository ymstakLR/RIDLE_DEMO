using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBallSidePoint : MonoBehaviour {

    private bool _isSidePoint;

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag != "Stage")
            return;
        _isSidePoint = true;
    }
    private void OnTriggerExit(Collider col) {
        if (col.gameObject.tag != "Stage")
            return;
        _isSidePoint = false;
    }
}
