using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オプションデータを編集する処理
/// 更新日時:0728
/// </summary>
public static class OptionDataEdit{
    public static ArrayList optionData = new ArrayList();

    /// <summary>
    /// オプションデータを読み込む処理
    /// </summary>
    private static void OptionDataLoad() { 
        optionData.Insert(0, SaveManager.optionDataStruct.bgmVol);
        optionData.Insert(1, SaveManager.optionDataStruct.seVol);
        optionData.Insert(2, SaveManager.optionDataStruct.resolutionH);
        optionData.Insert(3, SaveManager.optionDataStruct.resolutionW);
        optionData.Insert(4, SaveManager.optionDataStruct.isFullscreen);
    }//OptionDataLoad

    /// <summary>
    /// オプションデータを保存する処理
    /// </summary>
    private static void OptionDataSave() {
        SaveManager.OptionDataUpdate(optionData);
    }//OptionDataSave

    /// <summary>
    /// BGMのボリュームを更新する
    /// </summary>
    /// <param name="bgmVol">BGMのボリューム</param>
    public static void BGMVolumeUpadate(float bgmVol) {
        OptionDataLoad();
        bgmVol =Mathf.RoundToInt(bgmVol* 10);
        optionData[0] = (int)bgmVol;
        OptionDataSave();
    }//BGMVoluemUpadate

    /// <summary>
    /// SEのボリュームを更新する
    /// </summary>
    /// <param name="seVol">SEのボリューム</param>
    public static void SEVolumeUpdate(float seVol) {
        OptionDataLoad();
        seVol = Mathf.RoundToInt(seVol * 10);
        optionData[1] = (int)seVol;
        OptionDataSave();
    }//SEVolumeUpdate

    /// <summary>
    /// 画面比率を更新する
    /// </summary>
    /// <param name="height">画面縦の長さ</param>
    /// <param name="width">画面横の長さ</param>
    public static void ResolutionUpdate(int height,int width) {
        OptionDataLoad();
        optionData[2] = height;
        optionData[3] = width;
        OptionDataSave();
        SetResolution();
    }//ResolutionUpdate

    /// <summary>
    /// フルスクリーン判定の更新処理
    /// </summary>
    /// <param name="isFullScreen">フルスクリーンの判定</param>
    public static void FullScreenCheackUpdate(bool isFullScreen) {
        OptionDataLoad();
        optionData[4] = isFullScreen;
        OptionDataSave();
        SetResolution();
    }//FullScreenCheackUpdate

    /// <summary>
    /// 画面描画の反映処理
    /// </summary>
    public static void SetResolution() {
        
        Screen.SetResolution(
            SaveManager.optionDataStruct.resolutionH,
            SaveManager.optionDataStruct.resolutionW,
            SaveManager.optionDataStruct.isFullscreen
            );
    }//SetResolution

}//OptionDataEdit
