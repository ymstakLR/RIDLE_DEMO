using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 横に移動する大砲処理
/// 更新日時:20211007
/// </summary>
public class Cannon : CannonBase {

    private Transform _screw1;
    private Transform _screw2;

    private float _screwRotationZ;

    private void Start() {
        base._genarateObject = (GameObject)Resources.Load("GameObject/Drill");
        base._generateSpeed = 3;
        _screw1 = transform.Find("Cannon_Screw1");
        _screw2 = transform.Find("Cannon_Screw2");
    }//Start

    private void Update() {
        ScrewRotation();
        base.GenarateObject();
    }//Update

    /// <summary>
    /// 飾りの回転処理
    /// </summary>
    private void ScrewRotation() {
        _screwRotationZ += Time.deltaTime*100;
        _screw1.localRotation = Quaternion.Euler(0,0,_screwRotationZ);
        _screw2.localRotation = Quaternion.Euler(0, 0,-_screwRotationZ);
    }//ScrewRotation
    
}//Cannon
