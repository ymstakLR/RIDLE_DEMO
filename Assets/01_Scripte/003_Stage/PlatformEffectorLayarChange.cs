using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機や敵をplatformeffectorのレイヤー順より後ろに配置するための処理
/// 更新日時:0414
/// </summary>
public class PlatformEffectorLayarChange : MonoBehaviour {
    private Renderer _renderer;

    private void Start() {
        string rendererName = this.gameObject.name.Substring(0,this.gameObject.name.Length - 3)+"Ren";
        _renderer = this.transform.parent.Find(rendererName).GetComponent<Renderer>();
    }//Start

    private void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.tag != "UnderTrigger")
            return;
        LayerChange(col);
    }//OnTriggerEnter2D

    /// <summary>
    /// 対象のレイヤーの順番を変化させる
    /// </summary>
    /// <param name="col"></param>
    private void LayerChange(Collider2D col) {
        if (col.GetComponent<BaseUnderTrigger>().IsRise ||
            col.GetComponent<BaseUnderTrigger>().IsGimmickJump)//バネジャンプ上昇中の場合
            return;
        if (col.transform.parent.GetComponent<Renderer>().sortingOrder <
            _renderer.sortingOrder)
            return;
        //レイヤー一の変更
        col.transform.parent.GetComponent<Renderer>().sortingOrder =
             _renderer.sortingOrder +
             col.transform.parent.GetComponent<Renderer>().sortingOrder;
        
    }//LayerChange

    private void OnTriggerExit2D(Collider2D col) {
        switch (col.gameObject.tag) {
            case "Player":
            case "UnderTrigger":
                if (col.transform.parent.GetComponent<Renderer>().sortingOrder > _renderer.sortingOrder)
                    return;
                //レイヤーの変更
                col.transform.parent.GetComponent<Renderer>().sortingOrder =
                    col.transform.parent.GetComponent<Renderer>().sortingOrder -
                    _renderer.sortingOrder;
                break;
        }//switch
    }//OnTriggerExit2D

}//PlatformEffectorLayarChange
