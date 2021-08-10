using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 斬撃ダメージ描画処理
/// 更新日時:0810
/// </summary>
public class SlashingDamage : MonoBehaviour {
    private void Start() {
        Debug.Log("確認");
    }
    void Update() {
        if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("StateTest")) {
            //Destroy(this.gameObject);
        }//if
    }//Update
}//SlahingDamage
