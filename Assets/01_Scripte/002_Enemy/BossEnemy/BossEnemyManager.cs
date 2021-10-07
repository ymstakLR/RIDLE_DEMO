using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ボス敵の共通処理をまとめて記載する
/// 更新日時:20211007
/// </summary>
public class BossEnemyManager : MonoBehaviour {

    [Tooltip("敵ごとの最大体力を指定する")]
    [SerializeField]
    private int maxLifeNum;//最大体力値

    private readonly float RECOVERY_TIME = (float)2;//回復するための時間

    public Animator Animator { get; set; }
    public GameObject BodyTrigger { get; set; }
    public GameObject Player { get; set; }
    public Rigidbody2D Rigidbody { get; set; }

    private Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }

    protected AudioManager _audioManager;
    private CameraChase _cameraChase;
    protected StageStatusManagement _stageClearManagement;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _mainCameraTF;

    private int _nowLifeNum;//現在の体力値
    public int NowLifeNum { get { return _nowLifeNum; } }

    private float _destroyPositionY;//ミス時のゲームオブジェクト消去position.Y;
    private float _recoveryTimer;//回復までの現在時間
    private float _rendererEnableTime;//ダメージ中の画像表示・非表示の切り替え時間 時間を更新していくので定数にはしない

    public bool IsAppearanceEnd { get; set; }

    private string _stageBGMName;

    public void Start() {
        _audioManager = GameObject.Find("GameManager").GetComponent<AudioManager>();
        _stageBGMName = _audioManager.BGMAudio.clip.name;
        Animator = this.GetComponent<Animator>();
        _cameraChase = Camera.main.GetComponent<CameraChase>();
        BodyTrigger = this.gameObject.transform.Find("BodyTrigger").gameObject;
        Player = GameObject.Find("Ridle");
        Rigidbody = this.GetComponent<Rigidbody2D>();
        _stageClearManagement = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
        _sprite = this.GetComponent<SpriteRenderer>().sprite;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _mainCameraTF = GameObject.Find("Main Camera").GetComponent<Transform>().position;

        _nowLifeNum = maxLifeNum;
        _recoveryTimer = RECOVERY_TIME;
    }//Start

    /// <summary>
    /// ボス戦共通のUpdate処理
    /// </summary>
    public void ParentUpdate() {
        ColliderChange();
        EnemyDamage();
        EnemyRecovery();
        EnemyMiss();
        if (IsAppearanceEnd) {
            StartCoroutine(BattleStartCorutine());
        }//if
    }//ParentUpdate

    /// <summary>
    /// ボス戦を始めるまでの処理
    /// </summary>
    /// <returns></returns>
    IEnumerator BattleStartCorutine() {
        yield return new WaitForSeconds(1f);
        _cameraChase.IsBossBattle = true;
    }//BattleStartCorutine

    /// <summary>
    /// PolygonCollider2Dを変更するための処理
    /// </summary>
    private void ColliderChange() {
        if (this.GetComponent<SpriteRenderer>().sprite != _sprite) {//画像が変わったとき
            _sprite = this.GetComponent<SpriteRenderer>().sprite;
            BodyTrigger.GetComponent<SpriteRenderer>().sprite = _sprite;
            Destroy(this.GetComponent<PolygonCollider2D>());
            Destroy(BodyTrigger.GetComponent<PolygonCollider2D>());
            this.gameObject.AddComponent<PolygonCollider2D>();
            BodyTrigger.AddComponent<PolygonCollider2D>();
        }//if
        if (!BodyTrigger.GetComponent<PolygonCollider2D>().isTrigger) {//isTriggerがAddComponentと同時に設定できないのでこの処理で設定する
            BodyTrigger.GetComponent<PolygonCollider2D>().isTrigger = true;
            if (_recoveryTimer >= RECOVERY_TIME) {
                BodyTrigger.layer = LayerMask.NameToLayer("EnemyAttack");
            }//if
        } //if
    }//ColliderChange

    /// <summary>
    /// ダメージを受けたときの処理
    /// </summary>
    private void EnemyDamage() {
        if (BodyTrigger.GetComponent<EnemyBodyTrigger>().EnemyDamageType == EnemyDamageType.None)
            return;
        this.gameObject.layer = LayerMask.NameToLayer("DamageBigEnemy");
        BodyTrigger.layer = LayerMask.NameToLayer("DamageBigEnemy");
        _recoveryTimer = 0;
        _rendererEnableTime = 0;
        _nowLifeNum--;
        if (_nowLifeNum == 0) {
            DamageEffectSelect();
        }
        BodyTrigger.GetComponent<EnemyBodyTrigger>().EnemyDamageType = EnemyDamageType.None;
        if (_recoveryTimer == 0) {
            GameObject.Find("GameManager").GetComponent<AudioManager>().PlaySE("EnemyMiss");
        }//if
    }//EnemyDamage

    /// <summary>
    /// ダメージエフェクトを選択する処理
    /// </summary>
    private void DamageEffectSelect() {
        Vector2 generatePos;
        switch (this.transform.Find("BodyTrigger").GetComponent<EnemyBodyTrigger>().EnemyDamageType) {
            case EnemyDamageType.Slashing:
                generatePos = new Vector2(
                    this.gameObject.transform.position.x + this.GetComponent<Collider2D>().offset.x,
                    this.gameObject.transform.position.y + this.GetComponent<Collider2D>().offset.y);
                AttackEffect.EffectGenerate("GameObject/SlashingDamage", generatePos, this.gameObject, true);
                break;
            case EnemyDamageType.Trampling:
                generatePos = new Vector2(
                    Player.transform.position.x,
                    Player.transform.position.y);
                AttackEffect.EffectGenerate("GameObject/ShockWave", generatePos, Player, false);
                break;
            default:
                break;
        }//switch
    }//DamageEffectSelect


    /// <summary>
    /// ダメージを受けた後の処理
    /// </summary>
    private void EnemyRecovery() {
        if (_nowLifeNum == 0 ||//体力がない場合
            (_recoveryTimer > RECOVERY_TIME))//回復済みの場合
            return;
        _recoveryTimer += Time.deltaTime;
        if (_recoveryTimer >= RECOVERY_TIME) {//回復する場合
            _spriteRenderer.enabled = true;
            this.GetComponent<Collider2D>().enabled = true;
            BodyTrigger.GetComponent<Collider2D>().enabled = true;
            this.gameObject.layer = LayerMask.NameToLayer("BigEnemy");
            BodyTrigger.layer = LayerMask.NameToLayer("EnemyAttack");
            return;
        }//if
        SpriteRendererEnable();
    }//EnemyRecovery

    /// <summary>
    /// 画像の表示・非表示についての処理 
    /// EnemyRecovery EnemyMissで使用
    /// </summary>    
    private void SpriteRendererEnable() {
        if (_recoveryTimer <= _rendererEnableTime)
            return;
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
        _rendererEnableTime += (float)0.1;
    }//SpriteRendererEnable

    /// <summary>
    /// ミスしたとき(体力がなくなったとき)の処理   
    /// </summary>
    private void EnemyMiss() {
        if (_nowLifeNum != 0)
            return;
        if (_recoveryTimer == 0) {
            _audioManager.PlaySE("BossEnemyMiss");
        }
        _stageClearManagement.StageStatus = EnumStageStatus.AfterBossBattle;
        _recoveryTimer += Time.deltaTime;
        Animator.SetBool("AniMiss", true);
        this.GetComponent<PolygonCollider2D>().isTrigger = true;
        SpriteRendererEnable();
        StartCoroutine("MissEnumerator");
    }//EnemyMiss


    /// <summary>
    /// ミス時の落下処理(1.5秒後)
    /// EnemyMissで使用
    /// </summary>
    /// <returns></returns>
    IEnumerator MissEnumerator() {
        yield return new WaitForSeconds((float)2.5);
        _destroyPositionY = _mainCameraTF.y - 25;
        if (this.transform.position.y > _destroyPositionY) {
            ;
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - (Time.deltaTime * (float)20));
        } else {//画面内から出た場合
            try {
                _stageClearManagement.BossEnemyArray.RemoveAt(0);
                _stageClearManagement.StageStatus = EnumStageStatus.Normal;
                _audioManager.PlayBGM(_stageBGMName);
                Destroy(this.gameObject);
            } catch (Exception) {
                Debug.LogError("ボス敵を削除するときにエラー発生");
            }//try
        }//if
    }//missEnumerator

    /// <summary>
    /// animatorのbool型変更処理
    /// </summary>
    /// <param name="boolName"></param>
    public void AnimatorChangeBool(string boolName, bool boolean) {
        if (Animator.GetBool(boolName) != boolean) {
            Animator.SetBool(boolName, boolean);
        }//if
    }//AnimatorChangeBool

    /// <summary>
    /// プレイヤーの方向を向く処理
    /// </summary>
    protected void TurnToThePlayer() {
        if (Player.transform.position.x < this.transform.position.x) {//自機が左にいる場合
            this.transform.localScale = new Vector2(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y);
        } else {
            this.transform.localScale = new Vector2(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y);
        }//if
    }//TurnToThePlayer

    /// <summary>
    /// BGM変更までの停止処理
    /// </summary>
    /// <param name="corutineTime"></param>
    /// <returns></returns>
    protected IEnumerator BGMCorutine(float corutineTime) {
        yield return new WaitForSeconds(corutineTime);
        _audioManager.PlayBGM("BossEnemy");
    }//BGMCorutine

}//BigEnemyManager
