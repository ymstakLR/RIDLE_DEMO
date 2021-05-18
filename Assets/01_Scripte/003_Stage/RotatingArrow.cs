using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 矢印を指定方向に向ける処理
/// 更新日時:0415
/// </summary>
public class RotatingArrow : MonoBehaviour {

    private GameObject _goal;
    private GameObject _key;

    private StageStatusManagement _stageClearManagement;

    private SpriteRenderer _arrowRenderer;
    private Transform _arrowTransform;
    private Vector2 _directionPosition;

    public bool IsTouchGoal {get; set;}
    public bool IsTouchKey {get; set;}


    void Start() {
        _goal = GameObject.Find("Goal");
        _key = GameObject.Find("KeyItem/Key");
        _stageClearManagement = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
        _arrowRenderer = this.GetComponent<SpriteRenderer>();
        _arrowRenderer.enabled = false;
        _arrowTransform = this.GetComponent<Transform>();
    }//Start


    void Update() {
        Arrow();
    }//Update

    /// <summary>
    /// 矢印についての処理
    /// </summary>
    private void Arrow() {
        if (!IsTouchKey && !IsTouchGoal)
            return;
        ArrowDrawingJudge();
        ArrowDirection();
    }//DirectionDecision

    /// <summary>
    /// 矢印の描画判定
    /// </summary>
    private void ArrowDrawingJudge() {
        if (_stageClearManagement.StageStatus == EnumStageStatus.Normal) {
            _arrowRenderer.enabled = true;
        } else {
            _arrowRenderer.enabled = false;
        }//if
    }//ArrowDrawingJudge

    /// <summary>
    /// 矢印を指定方向に向ける処理
    /// </summary>
    private void ArrowDirection() {
        if (IsTouchKey) {//ゴール方向に向ける
            _directionPosition = _goal.GetComponent<Transform>().position;
        } else if (IsTouchGoal) {//キー方向に向ける
            _directionPosition = _key.GetComponent<Transform>().position;
        }//if
        if (_arrowRenderer.enabled) {
            Vector2 arrowPosition = _arrowTransform.position;
            Vector2 dt = _directionPosition - arrowPosition;
            float rad = Mathf.Atan2(dt.x, dt.y);
            float angle = 90 - (rad * Mathf.Rad2Deg);
            _arrowTransform.rotation = Quaternion.Euler(
                _arrowTransform.rotation.x, _arrowTransform.rotation.y, angle);
        }//if
    }//if

}//RotatingArrow
