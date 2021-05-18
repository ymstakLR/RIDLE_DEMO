using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 円状に回転するダメージギミック処理
/// 更新日時:0416
/// </summary>
public class CircularMoveing : MonoBehaviour {
    [SerializeField]
    private int decisionRadius = 1;
    [SerializeField]
    private float decisionSpeed = 1;
    [SerializeField]
    private bool isObjectRotation;

    private Transform _decisionTF;
    private Transform _lineTF;

    private Vector2 _centerPosition;

    private void Start() {
        _lineTF = transform.Find("CircularMoveingDamage_Line");
        _decisionTF = transform.Find("CircularMoveingDamage_Decision");
        _lineTF.localScale = new Vector2(_lineTF.localScale.x, decisionRadius / 5);
        _centerPosition = transform.position;
    }//Start

    private void Update() {
        CircularMovingWork();
    }//Update

    /// <summary>
    /// オブジェクトを円状に回転させる処理
    /// </summary>
    private void CircularMovingWork() {
        float decisionPositionX = decisionRadius * Mathf.Sin(Time.time * decisionSpeed);
        float decisionPositionY = decisionRadius * Mathf.Cos(Time.time * decisionSpeed);
  
        _decisionTF.position = new Vector2(_centerPosition.x - decisionPositionX, _centerPosition.y + decisionPositionY);

        Vector2 linePosition = _lineTF.position;//dt計算用
        Vector2 decisionPosition = _decisionTF.position;//dt計算用
        Vector2 dt = decisionPosition - linePosition;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float angle = (float) 90-(rad * Mathf.Rad2Deg);
        _lineTF.localRotation = Quaternion.Euler(_lineTF.localRotation.x, _lineTF.localRotation.y, 270 + angle);

        if (isObjectRotation) {
            _decisionTF.localRotation = Quaternion.Euler(_decisionTF.localRotation.x, _decisionTF.localRotation.y, 2 * angle);
        }//if

    }//CircularMoveingWork

}//CirularMoveing
