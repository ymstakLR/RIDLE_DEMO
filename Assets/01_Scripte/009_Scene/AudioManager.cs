using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// SE,BGM関連の管理処理
/// 更新日時:20211117
/// </summary>
public class AudioManager : MonoBehaviour {
    [SerializeField, Tooltip("BGM用のAudio Source")]
    private AudioSource bgmAudio;
    public AudioSource BGMAudio { get { return bgmAudio; }set { bgmAudio = value; } }

    public float BGMSettingVolume { get; set; }

    [SerializeField, Tooltip("SE用のAudio Source")]
    private AudioSource seAudio;
    public AudioSource SEAudio { get { return seAudio; } set { seAudio = value; } }

    //新たに追加
    private Dictionary<string, AudioClip> _bgmDic;
    private Dictionary<string, AudioClip> _seDic;

    private const string BGM_PATH = "Audio/BGM";
    private const string SE_PATH = "Audio/SE";

    //フェードアウト・フェードイン
    private float _fadeTimeS;
    private float _volumeBeforeFade;

    private bool _isFade;
    private bool _isFadeOut;

    void Awake() {
        _bgmDic = new Dictionary<string, AudioClip>();
        _seDic = new Dictionary<string, AudioClip>();

        object[] bgmList = Resources.LoadAll(BGM_PATH);
        object[] seList = Resources.LoadAll(SE_PATH);

        foreach(AudioClip bgm in bgmList) {
            _bgmDic[bgm.name] = bgm;
        }//foreach
        foreach(AudioClip se in seList) {
            _seDic[se.name] = se;
        }//foreach
    }//Awake
    private void Start() {
        BGMSettingVolume = bgmAudio.volume;
    }


    public void BGMInit() {
        bgmAudio.volume = BGMSettingVolume;
    }

    /// <summary>
    /// SE名を確認して存在する場合SEを再生
    /// </summary>
    /// <param name="seName"></param>
    public void PlaySE(string seName) {
        if (!_seDic.ContainsKey(seName)) {
            Debug.LogError("[SE"+seName + "]が存在しません フォルダに入っているか確認する");
            return;
        }//if
        seAudio.PlayOneShot(_seDic[seName]);
    }//PlaySE

    /// <summary>
    /// BGM名を確認して存在する場合BGMを再生する
    /// </summary>
    /// <param name="bgmName"></param>
    public void PlayBGM(string bgmName) {
        if (!_bgmDic.ContainsKey(bgmName)) {
            Debug.LogError("[BGM"+bgmName + "]が存在しません フォルダに入っているか確認する");
            return;
        }//if
        if (bgmAudio.clip.name.ToString() == bgmName && bgmAudio.clip.name.Substring(0,5) != "Stage") {
            Debug.LogWarning(bgmName + "はすでに流れています");
            return;
        }//if
        bgmAudio.clip = _bgmDic[bgmName];
        bgmAudio.Play();
        LoopBGM(isLoop: true);
    }//PlayBGM

    /// <summary>
    /// 再生中のBGMを停止させる処理
    /// </summary>
    public void StopBGM() {
        bgmAudio.Stop();
    }//StopBGM

    /// <summary>
    /// BGMをループの有無変更処理
    /// </summary>
    /// <param name="isLoop"></param>
    public void LoopBGM(bool isLoop) {
        bgmAudio.loop = isLoop;
    }//LoopBGM


    private void Update() {
        FadeJudge();
    }//Update

    /// <summary>
    /// フェードを行うかの判定処理
    /// </summary>
    private void FadeJudge() {
        if (!_isFade)
            return;
        if (_isFadeOut) {
            FadeOut();
        } else {
            FadeIn();
        }//if
    }//FadeJudge

    /// <summary>
    /// フェードアウトの処理
    /// </summary>
    private void FadeOut() {
        if (bgmAudio.volume > 0) {
            bgmAudio.volume -= _volumeBeforeFade / (_fadeTimeS * 60);
        } else {
            _isFade = false;
        }//if
    }//FadeOut

    /// <summary>
    /// フェードインの処理
    /// </summary>
    private void FadeIn() {
        if(bgmAudio.volume <= _volumeBeforeFade) {
            bgmAudio.volume += _volumeBeforeFade / (_fadeTimeS * 60);
        } else {
            bgmAudio.volume = BGMSettingVolume;
            _isFade = false;
        }//if
    }//FadeIn

    /// <summary>
    /// フェードアウトの開始処理
    /// </summary>
    /// <param name="fadeOutTimeS">フェードアウトに要する時間(秒)</param>
    public void FadeOutStart(float fadeOutTimeS) {
        _fadeTimeS = fadeOutTimeS;
        _volumeBeforeFade = bgmAudio.volume;
        _isFadeOut = true;
        _isFade = true;
    }//FadeOutStart

    /// <summary>
    /// フェードインの開始処理
    /// </summary>
    /// <param name="fadeInTimeS">フェードインに要する時間(秒)</param>
    public void FadeInStart(float fadeInTimeS) {
        _fadeTimeS = fadeInTimeS;
        _volumeBeforeFade = BGMSettingVolume;
        _isFadeOut = false;
        _isFade = true;
        bgmAudio.volume = 0;
    }//FadeInStart

}//AudioManager
