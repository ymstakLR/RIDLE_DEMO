using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクト関連の処理(今後クラス名を変更する必要がある)
/// 更新日時:20210910
/// </summary>
public static class AttackEffect{

    /// <summary>
    /// エフェクトの生成処理
    /// </summary>
    /// <param name="effectName">生成するエフェクト名</param>
    /// <param name="targetObj">対象のオブジェクト</param>
    /// <param name="generatePos">生成するエフェクトの配置補正値</param>
    public static void EffectGenerate(string effectName,Vector2 generatePos, GameObject targetObj,bool targetParent) {
        GameObject instance = (GameObject)UnityEngine.Object.Instantiate(
            (GameObject)Resources.Load(effectName),
            new Vector2(generatePos.x,generatePos.y), 
            Quaternion.identity);
        instance.transform.localEulerAngles = targetObj.transform.localEulerAngles;
        instance.transform.localScale = targetObj.transform.localScale;
        if (targetParent) {
            instance.transform.parent = targetObj.transform;
        }//if
    }//EffectGenerate

}
