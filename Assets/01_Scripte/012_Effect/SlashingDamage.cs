using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �a���_���[�W�`�揈��
/// �X�V����:0811
/// </summary>
public class SlashingDamage : MonoBehaviour {

    private void Update() {
        if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("StateTest")) {
            Destroy(this.gameObject);
        }//if
    }//Update
}//SlahingDamage