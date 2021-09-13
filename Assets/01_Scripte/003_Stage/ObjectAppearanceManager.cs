using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �w��ʒu�ŃI�u�W�F�N�g�𐶐����鏈��
/// �X�V����:20210914
/// </summary>
public class ObjectAppearanceManager : MonoBehaviour {
    [SerializeField, Tooltip("�����������I�u�W�F�N�g")]
    private GameObject _object;

    [SerializeField, Tooltip("�I�u�W�F�N�g�ōX�V���������l�̃��X�g(�g�p���镪�����쐬����)")]
    private List<float> _objectFloatList;
    public List<float> ObjectFloatList { get { return _objectFloatList; } }

    private GameObject _player;
    private StageStatusManagement _stageClearManagement;

    private int _hierarchCount;

    void Start() {
        _player = GameObject.Find("Ridle");
        _stageClearManagement = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
        GetHierarchyCount();
    }//Start

    void Update() {
        ObjectGenerateJudge();
    }//Update

    /// <summary>
    /// ���C���[���l�̕␳�l���擾���邽�߂̏���
    /// </summary>
    private void GetHierarchyCount() {
        foreach (Transform child in this.transform.parent.GetComponentsInChildren<Transform>()) {
            if (child.gameObject == this.gameObject) {
                _hierarchCount = child.GetSiblingIndex();
                return;
            }//if
        }//foreach
    }//GetHierarchyCount

    /// <summary>
    /// �Ώۂ̃I�u�W�F�N�g�𐶐����锻�菈��
    /// </summary>
    private void ObjectGenerateJudge() {
        //�w��̃X�e�[�W�X�e�[�^�X�łȂ��ꍇ
        if ((_stageClearManagement.StageStatus != EnumStageStatus.Normal &&
            _stageClearManagement.StageStatus != EnumStageStatus.Pause))
            return;
        //�J�����͈͓��̏ꍇ
        if (30 > Mathf.Abs(_player.transform.position.x - this.transform.position.x) &&
           20 > Mathf.Abs(_player.transform.position.y - this.transform.position.y))
            return;
        //�����͈͊O�̏ꍇ
        if (40 < Mathf.Abs(_player.transform.position.x - this.transform.position.x) ||
            30 < Mathf.Abs(_player.transform.position.y - this.transform.position.y))
            return;
        ObjectGenerate();
    }//EnemyGenarateJudge

    /// <summary>
    /// �I�u�W�F�N�g�𐶐����鏈��
    /// </summary>
    private void ObjectGenerate() {
        GameObject instance = (GameObject)Instantiate(_object,this.transform);
        instance.GetComponent<SpriteRenderer>().sortingOrder = instance.GetComponent<SpriteRenderer>().sortingOrder - _hierarchCount;
        this.GetComponent<ObjectAppearanceManager>().enabled = false;
    }//EnemyGenerate
}
