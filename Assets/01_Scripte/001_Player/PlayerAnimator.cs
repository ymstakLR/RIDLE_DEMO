using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機のアニメーションの更新処理
/// 更新日時:0602
/// </summary>
public class PlayerAnimator : MonoBehaviour {
    public AudioManager AudioManager { get; set; }

    public bool AniJump { get; set; }
    public bool AniFall { get; set; }
    public bool AniAttack { get; set; }
    public bool AniDamage { get; set; }
    public bool AniMiss { get; set; }
    public bool IsEnable { get; set; }

    private Animator _animator;

    private void Start() {
        AudioManager = GameObject.Find("GameManager").GetComponent<AudioManager>();
        _animator = this.GetComponent<Animator>();
    }//Start

    public void AnimatorMove(float workSpeed) {
        _animator.SetFloat("Speed", Mathf.Abs(workSpeed));
        _animator.SetBool("AniJump", AniJump);
        _animator.SetBool("AniFall", AniFall);
        _animator.SetBool("AniAttack", AniAttack);
        _animator.SetBool("AniDamage", AniDamage);
        _animator.SetBool("AniMiss", AniMiss);
    }//AnimatorMove

}//PlayerAnimator
