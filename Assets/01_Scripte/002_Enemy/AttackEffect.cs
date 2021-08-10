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
    /// <param name="posCorrection">��������G�t�F�N�g�̔z�u�␳�l</param>
    public static void EffectGenerate(string effectName,GameObject targetObj,Vector2 posCorrection) {
        Vector2 pos = targetObj.transform.position;
        GameObject instance = (GameObject)UnityEngine.Object.Instantiate(
            (GameObject)Resources.Load(effectName),
            new Vector2(pos.x + posCorrection.x, pos.y + posCorrection.y), 
            Quaternion.identity);
        instance.transform.parent = targetObj.transform;
    }//EffectGenerate
}
