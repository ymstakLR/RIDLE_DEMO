using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ボス敵2の処理
/// 更新日時:0602
/// </summary>
public class BossEnemy2 : BossEnemyManager {
    private enum EnumMotionFlag {//モーションフラグの列挙体
        appearance,//登場時の処理
        wait,
        attackA,
        attackB,
        jump,
        fall
    }//EnumMotionFlag
    private EnumMotionFlag _motionFlag;//モーションのフラグ変数
    private List<EnumMotionFlag> _motionFlagList = new List<EnumMotionFlag>();

    //画像の取得
    [SerializeField]
    private Sprite attackA4;
    [SerializeField]
    private Sprite attackB2;
    [SerializeField]
    private GameObject drill;

    //共通
    private float _motionTimer;//モーション変更用のタイマー
    private float _addForceX;
    private float _addForceY;

    //Wait
    private float _patternMax;//ランダム値のMax補正
    private float _patternMin;//ランダム値のMin補正

    //Fall
    private EnemyUnderTrigger _eUnderTrigger;
    private float _pastPosY;//
    private bool _isLandSE;

    //Sword
    private GameObject _sword;//子要素の情報取得
    private Animator _swordAnimator;
    private Sprite _swordSprite;


    private new void Start() {
        base.Start();
        _motionFlag = EnumMotionFlag.appearance;

        _eUnderTrigger = this.transform.Find("UnderTrigger").GetComponent<EnemyUnderTrigger>();

        _sword = this.gameObject.transform.Find("Sword").gameObject;
        _swordAnimator = _sword.GetComponent<Animator>();
        _swordSprite = _sword.GetComponent<SpriteRenderer>().sprite;
    }//Start

    private void FixedUpdate() {
        if (NowLifeNum == 0)
            return;
        _addForceY = 0;
        _addForceX = 0;
        string spriteName = this.GetComponent<SpriteRenderer>().sprite.name.ToString();
        SwordColChange(spriteName);
        switch (_motionFlag) {//各モーションに遷移
            case EnumMotionFlag.appearance:
                Appearance();
                break;
            case EnumMotionFlag.wait:
                Wait();
                break;
            case EnumMotionFlag.attackA:
                
                AttackA();
                break;
            case EnumMotionFlag.attackB:
                AttackB();
                break;
            case EnumMotionFlag.jump:
                Jump();
                break;
            case EnumMotionFlag.fall:
                Fall(0.05f);
                break;
        }//switch
        Rigidbody.AddForce(new Vector2(_addForceX, _addForceY));//移動値の反映
    }//FixedUpdate

    // Update is called once per frame
    void Update() {
        ParentUpdate();
        if (NowLifeNum != 0)
            return;
        Rigidbody.velocity = new Vector2(0, 0);
    }//Update

    /// <summary>
    /// 登場時の処理
    /// </summary>
    private void Appearance() {
        _motionTimer += Time.deltaTime;
        if (_motionTimer < 2) {
            AnimatorChangeBool("AniJump", true);
            AnimatorChangeBool("AniFall", true);
            return;
        }//if
        TurnToThePlayer();
        _pastPosY = this.transform.position.y + 1;
        Fall(0.5f);
    }//Appearance

    /// <summary>
    /// 待機中の処理
    /// </summary>
    private void Wait() {
        Rigidbody.velocity = new Vector2(0, -10);
        if (_stageClearManagement.StageStatus != EnumStageStatus.BossBattle)
            return;
        _motionTimer += Time.deltaTime;
        if (_motionTimer > 1.5) {
            MotionPatternSelect();
            _motionTimer = 0;
        }else if(_motionTimer > 1.4 && _motionTimer < 1.45) {
            TurnToThePlayer();
        }//if
    }//Wait

    /// <summary>
    /// モーションの選択を行う処理
    /// </summary>
    private void MotionPatternSelect() {
        _motionFlagList.Clear();
        float distance = Vector2.Distance(Player.transform.position, this.transform.position);
        ///自機との距離に応じて行う行動パターンを変更する
        if (distance < 20) {//自機が近くにいる場合
            _motionFlagList.Add(EnumMotionFlag.attackA);
            _motionFlagList.Add(EnumMotionFlag.jump);
        } else {//自機が遠くにいる場合
            _motionFlagList.Add(EnumMotionFlag.attackB);
            _motionFlagList.Add(EnumMotionFlag.jump);
        }//if

        float flagMax = _motionFlagList.Count +_patternMax;//補正値を入れる
        float flagMin = 0 +_patternMin;//補正値を入れる
        _motionFlag = _motionFlagList[(int)Random.Range(flagMin,flagMax)];
        ///メソッド化する
        if (_motionFlag == EnumMotionFlag.jump) {
            _patternMax -= 0.3f;
            _patternMin = 0;
        } else {
            _patternMax = 0;
            _patternMin += 0.3f;
        }//if
    }//if


    private void AttackA() {
        AnimatorChangeBool("AniAttackA", true);
        if (Sprite != attackA4) {//待機中の場合
            _addForceY = -110;
            return;
        }//if
        if (_motionTimer == 0) {
            _audioManager.PlaySE("Enemy_Cut");
        }//if
        _motionTimer += Time.deltaTime;
        _addForceX = 0;
        if (_motionTimer < 0.2) {
            _addForceX = this.transform.localScale.x * 10;
        } else if (_motionTimer > 1) {
            AnimatorChangeBool("AniAttackA", false);
            _motionFlag = EnumMotionFlag.wait;
            _motionTimer = 0;
        }//if

    }//AttackA

    private void AttackB() {
        AnimatorChangeBool("AniAttackB", true);
        if (Sprite != attackB2)
            return;

        if (_motionTimer == 0) {
            DrillGeneration();
        }//if

        _motionTimer += Time.deltaTime;

        if (_motionTimer > 1) {
            AnimatorChangeBool("AniAttackB", false);
            _motionFlag = EnumMotionFlag.wait;
            _motionTimer = 0;
        }//if

    }//AttackB

    /// <summary>
    /// Drillの生成処理
    /// </summary>
    private void DrillGeneration() {
        float generatePosX = this.transform.position.x;
        if (Player.transform.position.x > this.transform.position.x) {//自機が左にいる場合
            generatePosX += 1.5f;
        } else {
            generatePosX -= 1.5f;
        }//if
        _audioManager.PlaySE("BossEnemy_Fire");
        //生成
        GameObject instance = (GameObject)Instantiate(
            drill, new Vector2(generatePosX, this.transform.position.y - 3f), Quaternion.identity);
        //向き設定
        instance.transform.localScale = new Vector2(
            -this.transform.localScale.x * instance.transform.localScale.x, 
            instance.transform.localScale.y);
        instance.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder - 1;
    }//DrillGeneration

    /// <summary>
    /// ジャンプ中の処理
    /// </summary>
    private void Jump() {
        if (!Animator.GetBool("AniJump")) {
            _audioManager.PlaySE("Enemy_Jump");
            _isLandSE = true;
        }//if
        AnimatorChangeBool("AniJump", true);
        _motionTimer += Time.deltaTime;
        if(_motionTimer < 0.5) {
            _addForceY = 80;
            float distance = Vector2.Distance(Player.transform.position, this.transform.position);
            _addForceX = this.transform.localScale.x * (distance/0.7f);//0.65にすると自機に攻撃が当たる
        } else {
            _pastPosY = this.transform.position.y;
            _motionFlag = EnumMotionFlag.fall;
            _motionTimer = 0;
        }//if
    }//Jump

    //落下する際の処理
    private void Fall(float coroutineTime) {
        _addForceY = -110;
        if (_pastPosY > this.transform.position.y) {
            AnimatorChangeBool("AniFall", true);
        }//if
        _pastPosY = this.transform.position.y;
        StartCoroutine(FallCoroutine(coroutineTime));
    }//Fall

    /// <summary>
    /// 落下後の時間調整用の処理
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator FallCoroutine(float waitTime) {
        if (_eUnderTrigger.IsUnderTrigger) {
            if (_isLandSE) {
                _audioManager.PlaySE("Enemy_Land");
                _isLandSE = false;
            }//if
            yield return new WaitForSeconds(waitTime);
            AnimatorChangeBool("AniJump", false);
            AnimatorChangeBool("AniFall", false);
            _motionFlag = EnumMotionFlag.wait;
            if (_stageClearManagement.StageStatus != EnumStageStatus.BeforeBossBattle)
                yield return null;
            _motionTimer = 0;
            IsAppearanceEnd = true;
            StartCoroutine(BGMCorutine(1.0f));
        }//if
    }//FallCoroutine

    /// <summary>
    /// BigEnemy2のswordオブジェクトの当たり判定を表示させる処理
    /// </summary>
    /// <param name="spriteName"></param>
    private void SwordColChange(string spriteName) {
        spriteName = spriteName.Replace("BossEnemy2_", "");//画像名を変更する場合はその画像名に文字列を変更する
        SwordSpriteCheack(spriteName, "AttackA1");
        SwordSpriteCheack(spriteName, "AttackA2");
        SwordSpriteCheack(spriteName, "AttackA3");
        SwordSpriteCheack(spriteName, "AttackA4");
        SwordSpriteCheack(spriteName, "Jump");
        SwordSpriteCheack(spriteName, "Fall");
    }//SwordColChange

    /// <summary>
    /// swordオブジェクトのspriteの変更確認処理
    /// </summary>
    /// <param name="spriteName"></param>
    /// <param name="cheackStr"></param>
    private void SwordSpriteCheack(string spriteName,string cheackStr) {
        ///不要なColliderの削除
        if (spriteName != cheackStr) {//画像が違う場合
            if (_swordAnimator.GetBool("Ani" + cheackStr) == false)
                return;
            _swordAnimator.SetBool("Ani" + cheackStr, false);
            Destroy(_sword.GetComponent<PolygonCollider2D>());
            return;
        }//if
        ///Colliderの更新
        _swordAnimator.SetBool("Ani" + cheackStr, true);
        if (_sword.GetComponent<SpriteRenderer>().sprite != _swordSprite) {
            _swordSprite = _sword.GetComponent<SpriteRenderer>().sprite;
            Destroy(_sword.GetComponent<PolygonCollider2D>());
            _sword.AddComponent<PolygonCollider2D>();
            
        }//if
        ///isTriggerの反映
        if(_sword.GetComponent<PolygonCollider2D>() && 
            _sword.GetComponent<PolygonCollider2D>().isTrigger == false) {
            _sword.GetComponent<PolygonCollider2D>().isTrigger = true;
        }//if
    }//SwordSpriteCheack

}//BossEnemy2
