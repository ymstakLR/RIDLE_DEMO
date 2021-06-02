using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// アイテムの共通情報の宣言
/// 更新日時:0603
/// </summary>
public class ItemProperty : MonoBehaviour {
    public int EnemyNumMax { get; set; }
    public int EnemyNum { get; set; }
    public int PlayerMissCount { get; set; }
    public int SpecialItem { get; set; }
    public int StageTime { get; set; }


    private void Start() {
        PlayerMissCount = 0;
        SpecialItem = 0;
    }//Start

}//ItemProperty
