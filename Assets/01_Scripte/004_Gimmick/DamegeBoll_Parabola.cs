using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 放物線上に移動する大砲玉の処理
/// 更新日時:0819
/// </summary>
public class DamegeBoll_Parabola : GenerateDamageObjBase {
    //　このスクリプトで変更しない場合もある(0827)
    [SerializeField,Tooltip("ボールにかかる重力値")]
    private float ballGravity;

    private Vector3 _angles;
    private float _gravityTime;

    private Rigidbody2D rb2D;

    private void Start() {
        rb2D = this.GetComponent<Rigidbody2D>();
        if (0 < transform.localScale.x) {
            _angles = new Vector3(-45, 45, 0);
        } else {
            _angles = new Vector3(45, 45, 0);
        }//if
    }//Start

    private void FixedUpdate() {
        rb2D.velocity = new Vector2(_angles.x / 2.5f, (_angles.y / 2.5f) + _gravityTime * ballGravity);
    }//FixedUpdate

    private void Update() {
        _gravityTime -= Time.deltaTime; 
        GenerateDamageObjDestroy();
    }//Update

}//DamageBoll_Parabola
