using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクト関連の処理(今後クラス名を変更する必要がある)
/// 更新日時:0810
/// </summary>
public static class AttackEffect{

    /// <summary>
    /// エフェクトの生成処理
    /// </summary>
    /// <param name="effectName">生成するエフェクト名</param>
    /// <param name="targetObj">対象のオブジェクト</param>
    /// <param name="posCorrection">生成するエフェクトの配置補正値</param>
    public static void EffectGenerate(string effectName,GameObject targetObj,Vector2 posCorrection) {
        Vector2 pos = targetObj.transform.position;
        GameObject instance = (GameObject)UnityEngine.Object.Instantiate(
            (GameObject)Resources.Load(effectName),
            new Vector2(pos.x + posCorrection.x, pos.y + posCorrection.y), 
            Quaternion.identity);
        instance.transform.parent = targetObj.transform;
    }//EffectGenerate
}
