using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 放物線上に移動する大砲玉の処理
/// 更新日時:0416
/// </summary>
public class DamegeBoll_Parabola : MonoBehaviour {
    //　このスクリプトで変更しない場合もある(0827)
    [SerializeField]
    private float ballGravity;

    private Vector3 _angles;

    private float _gravityTime;
    private float _destroyTime;

    private bool _isDestroy;

    private void Start() {
        if (0 < transform.localScale.x) {
            _angles = new Vector3(-45, 45, 0);
        } else {
            _angles = new Vector3(45, 45, 0);
        }//if
    }//Start

    private void FixedUpdate() {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(_angles.x / (float)2.5, (_angles.y / (float)2.5) + _gravityTime * ballGravity);
    }//FixedUpdate

    private void Update() {
        _gravityTime -= Time.deltaTime; 
        BallDestroy();
    }//Update

    /// <summary>
    /// ボールを消滅させる処理
    /// </summary>
    private void BallDestroy() {
        if (!_isDestroy)
            return;
        if (_destroyTime < (float)0.25) {
            _destroyTime += Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }//if
    }//BallDestroy

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Stage") {
            _isDestroy = true;
        }//if
    }//OnTriggerEnter2D

}//DamageBoll_Parabola
