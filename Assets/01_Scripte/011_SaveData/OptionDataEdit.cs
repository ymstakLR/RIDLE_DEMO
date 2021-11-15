using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�v�V�����f�[�^��ҏW���鏈��
/// �X�V����:0728
/// </summary>
public static class OptionDataEdit{
    public static ArrayList optionData = new ArrayList();

    /// <summary>
    /// �I�v�V�����f�[�^��ǂݍ��ޏ���
    /// </summary>
    private static void OptionDataLoad() { 
        optionData.Insert(0, SaveManager.optionDataStruct.bgmVol);
        optionData.Insert(1, SaveManager.optionDataStruct.seVol);
        optionData.Insert(2, SaveManager.optionDataStruct.resolutionH);
        optionData.Insert(3, SaveManager.optionDataStruct.resolutionW);
        optionData.Insert(4, SaveManager.optionDataStruct.isFullscreen);
    }//OptionDataLoad

    /// <summary>
    /// �I�v�V�����f�[�^��ۑ����鏈��
    /// </summary>
    private static void OptionDataSave() {
        SaveManager.OptionDataUpdate(optionData);
    }//OptionDataSave

    /// <summary>
    /// BGM�̃{�����[�����X�V����
    /// </summary>
    /// <param name="bgmVol">BGM�̃{�����[��</param>
    public static void BGMVolumeUpadate(float bgmVol) {
        OptionDataLoad();
        bgmVol =Mathf.RoundToInt(bgmVol* 10);
        optionData[0] = (int)bgmVol;
        OptionDataSave();
    }//BGMVoluemUpadate

    /// <summary>
    /// SE�̃{�����[�����X�V����
    /// </summary>
    /// <param name="seVol">SE�̃{�����[��</param>
    public static void SEVolumeUpdate(float seVol) {
        OptionDataLoad();
        seVol = Mathf.RoundToInt(seVol * 10);
        optionData[1] = (int)seVol;
        OptionDataSave();
    }//SEVolumeUpdate

    /// <summary>
    /// ��ʔ䗦���X�V����
    /// </summary>
    /// <param name="height">��ʏc�̒���</param>
    /// <param name="width">��ʉ��̒���</param>
    public static void ResolutionUpdate(int height,int width) {
        OptionDataLoad();
        optionData[2] = height;
        optionData[3] = width;
        OptionDataSave();
        SetResolution();
    }//ResolutionUpdate

    /// <summary>
    /// �t���X�N���[������̍X�V����
    /// </summary>
    /// <param name="isFullScreen">�t���X�N���[���̔���</param>
    public static void FullScreenCheackUpdate(bool isFullScreen) {
        OptionDataLoad();
        optionData[4] = isFullScreen;
        OptionDataSave();
        SetResolution();
    }//FullScreenCheackUpdate

    /// <summary>
    /// ��ʕ`��̔��f����
    /// </summary>
    public static void SetResolution() {
        
        Screen.SetResolution(
            SaveManager.optionDataStruct.resolutionH,
            SaveManager.optionDataStruct.resolutionW,
            SaveManager.optionDataStruct.isFullscreen
            );
    }//SetResolution

}//OptionDataEdit
