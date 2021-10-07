using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 放物線大砲の処理
/// 更新日時:20211007
/// </summary>
public class DigonalCannon : CannonBase {

    private Transform _screw1;
    private Transform _screw2;
    private Transform _screw3;

    private float _screwRotationZ;

    private void Start() {
        base._genarateObject = (GameObject)Resources.Load("GameObject/DamegeBoll_Parabole");
        base._generateSpeed = 2;
        _screw1 = transform.Find("Cannon_Screw1");
        _screw2 = transform.Find("Cannon_Screw2");
        _screw3 = transform.Find("Cannon_Screw3");
    }//Start

    private void Update() {
        ScrewRotation();
        base.GenarateObject();
    }//Update

    /// <summary>
    /// 飾りの回転処理
    /// </summary>
    private void ScrewRotation() {
        _screwRotationZ += Time.deltaTime * 100;
        _screw1.localRotation = Quaternion.Euler(0, 0, _screwRotationZ);
        _screw2.localRotation = Quaternion.Euler(0, 0, _screwRotationZ);
        _screw3.localRotation = Quaternion.Euler(0, 0, _screwRotationZ);
    }//ScrewRotation

}//DigonalCannon
