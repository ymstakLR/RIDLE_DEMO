using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大砲処理の共通処理
/// 更新日時:0416
/// </summary>
public class CannonBase : MonoBehaviour {

    protected GameObject _genarateObject;
    protected float _generateSpeed;

    private float _generateTime;

    /// <summary>
    /// オブジェクトの生成処理 
    /// </summary>
    protected void GenarateObject() {
        _generateTime += Time.deltaTime;
        if (_generateTime > _generateSpeed) {
            GameObject instanse = (GameObject)Instantiate(
            _genarateObject, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            instanse.transform.localScale = new Vector2(
                this.transform.localScale.x*instanse.transform.localScale.x,
                this.transform.localScale.y*instanse.transform.localScale.y);
            instanse.transform.localRotation = Quaternion.Euler(0, 0, this.transform.localEulerAngles.z);
            _generateTime = 0;
        }//if
    }//GenarateObject

}//CannonBase
