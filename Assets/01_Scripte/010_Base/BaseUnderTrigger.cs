using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機・敵の足元の当たり判定の共通の処理
/// 更新日時:0405
/// </summary>
public class BaseUnderTrigger : MonoBehaviour {
    public bool IsUnderTrigger { get; set; }//自機の着地判定
    public bool IsGimmickJump { get; set; }
    public bool IsRise { get; set; }//自機が上昇しているかの判定 True[上昇している]

    public GameObject ParentObj { get; set; }//privateからprotectedに変更　不具合が出たときに戻す0405

    protected void Start() {
        ParentObj = this.transform.parent.gameObject;
    }//Start
    
}//BaseUnderTrigger
