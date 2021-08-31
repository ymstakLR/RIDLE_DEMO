using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�t�F�N�g�֘A�̏���(����N���X����ύX����K�v������)
/// �X�V����:0810
/// </summary>
public static class AttackEffect{

    /// <summary>
    /// �G�t�F�N�g�̐�������
    /// </summary>
    /// <param name="effectName">��������G�t�F�N�g��</param>
    /// <param name="targetObj">�Ώۂ̃I�u�W�F�N�g</param>
    /// <param name="generatePos">��������G�t�F�N�g�̔z�u�␳�l</param>
    public static void EffectGenerate(string effectName,GameObject targetObj,Vector2 generatePos) {
        GameObject instance = (GameObject)UnityEngine.Object.Instantiate(
            (GameObject)Resources.Load(effectName),
            new Vector2(generatePos.x,generatePos.y), 
            Quaternion.identity);
        instance.transform.localRotation = targetObj.transform.localRotation;
        instance.transform.localScale = targetObj.transform.localScale;
        //instance.transform.parent = targetObj.transform;
    }//EffectGenerate
}
