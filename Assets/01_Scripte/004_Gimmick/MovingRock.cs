using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 横に転がる岩ギミックの処理
/// 更新日時:0416
/// </summary>
public class MovingRock : MonoBehaviour {
    [Tooltip("左向きに移動させる場合はチェックを入れる")]
    [SerializeField]
    private bool leftDirection;

    [Tooltip("移動量(絶対値)を設定する")]
    [SerializeField]
    private float speed;

    [Tooltip("重力(絶対値)を設定する")]
    [SerializeField]
    private float gravity;

    public bool IsUnderTrigger { get; set; }
    public bool IsSideTrigger { get; set; }

    private readonly float DESTROY_TIME = (float)0.75;
    private float _destroyTimer;
    private float _rendererEnableTime;

    private void Start() {
        if (leftDirection) {
            speed = -speed;
            this.GetComponent<SpriteRenderer>().flipX = true;
        }//if
    }//Start

    private void Update() {
        if (!IsUnderTrigger || !IsSideTrigger) {//消滅判定でなければ
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, -gravity);
            return;
        }//if
        RockDestroy();
    }//Update

    /// <summary>
    /// オブジェクトを消滅させる処理
    /// </summary>
    private void RockDestroy() {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        this.gameObject.layer = LayerMask.NameToLayer("MissEnemy");
        this.GetComponent<Animator>().SetBool("AniStop", true);
        _destroyTimer += Time.deltaTime;
        if (_destroyTimer > DESTROY_TIME) {
            Destroy(this.gameObject);
        }//if
        if (_destroyTimer < _rendererEnableTime)
            return;
        this.GetComponent<SpriteRenderer>().enabled = !this.GetComponent<SpriteRenderer>().enabled;
        _rendererEnableTime += (float)0.1;
    }//RockDestroy

}//MovingRock
