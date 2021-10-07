using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボス敵1の処理
/// 更新日時:20211007
/// </summary>
public class BossEnemy1 : BossEnemyManager {
    private enum EnumMotionFlag {//モーションフラグの列挙体
        appearance,
        wait,
        dush,
        fire
    }//EnumMotionFlag

    private EnemyBodyTrigger _enemyBodyTrigger;
    private EnumMotionFlag _motionFlag;//モーションのフラグ変数

    //共通
    private float _motionChangeTimer;//モーションを変更するための時間
    private float _randomMax;//ランダム値のMax補正
    private float _randomMin;//ランダム値のMin補正

    //Wait
    private readonly float FLOATING_TIME = (float)1;//上昇と下降を変更するための時間
    private float _floatingTimer;//上昇と下降を行うための時間
    private float _waitVelocityY;//上昇・下降値
    private bool _isFloatingRising;//浮遊時に上昇している判定

    //Dush
    private Vector2 _positionNow;//現在のposition
    private Vector2 _positionBefore;//一つ前のposition
    private readonly float DUSH_SPEED = 70;

    private float _dushMoveX;//移動速度保管用変数
    private float _dushMoveY;//移動速度保管用変数
    private float _motionTimer;//各モーションの行われている時間

    //Fire
    private float _fireShotTimer;//炎を吐くまでの時間

    private new void Start() {
        base.Start();
        _enemyBodyTrigger = BodyTrigger.GetComponent<EnemyBodyTrigger>();
        _randomMax = System.Enum.GetNames(typeof(EnumMotionFlag)).Length;
        _randomMin = _randomMax -2;
    }//Start

    private void FixedUpdate() {
        if (NowLifeNum == 0) {//ミス判定になった場合
            Rigidbody.velocity = new Vector2(0, 0);
            return;
        }//if
        switch (_motionFlag) {//各モーションに遷移
            case EnumMotionFlag.appearance:
                Appearance();
                break;
            case EnumMotionFlag.wait:
                Wait();
                break;
            case EnumMotionFlag.dush:
                Dush();
                break;
            case EnumMotionFlag.fire:
                Fire();
                break;
        }//switch
    }

    // Update is called once per frame
    void Update() {
        ParentUpdate();
    }//Update

    /// <summary>
    /// 登場時の処理
    /// </summary>
    private void Appearance() {
        //アニメーションの遷移を記載する
        AnimatorChangeBool("AniDush", true);
        this.transform.rotation = Quaternion.Euler(
            this.transform.rotation.x, this.transform.rotation.y, 90);
        Rigidbody.velocity = new Vector2(_dushMoveX, -_dushMoveY);
        StartCoroutine(AppearanceCorutine());
    }//Appearance

    /// <summary>
    /// 登場時の移動処理
    /// 登場してから2秒後に移動値を反映させる
    /// </summary>
    /// <returns></returns>
    IEnumerator AppearanceCorutine() {
        yield return new WaitForSeconds(2.0f);
        this.GetComponent<SpriteRenderer>().enabled = true;
        _dushMoveY = DUSH_SPEED / 2;
        _dushMoveX = 0;
        if (!_enemyBodyTrigger.IsStageTouch)
            yield break;
        DushEndCorrection();
        DushEnd();
        _motionFlag = EnumMotionFlag.wait;
        IsAppearanceEnd = true;
        StartCoroutine(BGMCorutine(1.0f));
    }//AppearanceCorutine


    /// <summary>
    /// 待機中の処理
    /// </summary>
    private void Wait() {
        _motionChangeTimer += Time.deltaTime;
        Floating();
        if (_motionChangeTimer < 2)
            return;
        _motionFlag = (EnumMotionFlag)Random.Range(_randomMin, _randomMax);
        if (_motionFlag == EnumMotionFlag.dush) {
            _randomMax = System.Enum.GetNames(typeof(EnumMotionFlag)).Length;
            _randomMin += (float)0.25;
        } else {
            _randomMax -= (float)0.25;
            _randomMin = System.Enum.GetNames(typeof(EnumMotionFlag)).Length - 2;
        }//if
        _motionChangeTimer = 0;
    }//Wait


    /// <summary>
    /// 浮遊処理
    /// </summary>
    private void Floating() {
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x,_waitVelocityY);
        _floatingTimer += Time.deltaTime;
        if (_floatingTimer < FLOATING_TIME)
            return;
        //上昇と下降の変更
        if (_isFloatingRising) {//上昇中の場合
            _isFloatingRising = false;
            _waitVelocityY = -(float)0.5;
        } else {//下降中の場合
            _isFloatingRising = true;
            _waitVelocityY = (float)0.5;
        }//if
        _floatingTimer = 0;
    }//Floating


    /// <summary>
    /// ダメージ球生成処理
    /// </summary>
    private void Fire() {
        TurnToThePlayer();
        Floating();
        _fireShotTimer += Time.deltaTime;
        if (_fireShotTimer < 1) {
            AnimatorChangeBool("AniFire1", true);
        } else if(_fireShotTimer<2){
            float generatePositionX;//ダメージ球の生成Position
            int generateScaleX;//ダメージ球の生成Scale

            AnimatorChangeBool("AniFire2", true);
            if(this.transform.localScale.x < 0) {
                generatePositionX = this.transform.position.x - 3;
                generateScaleX = 1;
            } else {
                generatePositionX = this.transform.position.x + 3;
                generateScaleX = -1;
            }//if
            _audioManager.PlaySE("BossEnemy_Fire");
            GameObject instance = (GameObject)Instantiate(
                (GameObject)Resources.Load("GameObject/BossEnemy1FireBall"), new Vector2(generatePositionX, this.transform.position.y + 3), Quaternion.identity);
            instance.transform.localScale = new Vector2(
                generateScaleX * instance.transform.localScale.x, instance.transform.localScale.y);
            instance.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder - 1;
            _fireShotTimer = 2;
        } else if(_fireShotTimer >3){
            _motionFlag = EnumMotionFlag.wait;
            AnimatorChangeBool("AniFire1", false);
            AnimatorChangeBool("AniFire2", false);
            _fireShotTimer = 0;
        }//if

    }//Fire

    /// <summary>
    /// ダッシュ攻撃処理
    /// </summary>
    private void Dush() {
        AnimatorChangeBool("AniDush",true);
        _motionTimer += Time.deltaTime;
        DushTimer();
        if (_motionTimer < 1) {//角度の設定
            Floating();
            AngleConfirm();
            _enemyBodyTrigger.IsStageTouch = false;
        }else if(_motionTimer < 1.5) {
            Rigidbody.velocity = new Vector2(0,0);
        } else{//移動開始
            if (this.transform.localScale.x < 0)
                Rigidbody.velocity = new Vector2(-_dushMoveX,_dushMoveY);
            else
                Rigidbody.velocity = new Vector2(_dushMoveX, _dushMoveY);
        }//if

        if(_motionTimer > 0.5) {//移動終了
            if (!_enemyBodyTrigger.IsStageTouch)
                return;
            DushEndCorrection();
            DushEnd();
        }//if

    }//Dush

    /// <summary>
    /// ダッシュ攻撃終了時の処理
    /// </summary>
    private void DushEnd() {
        Rigidbody.velocity = new Vector2(0, 0);
        AnimatorChangeBool("AniDush", false);
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        _motionFlag = EnumMotionFlag.wait;
        _enemyBodyTrigger.IsStageTouch = false;
        _motionTimer = 0;
    }//DushEnd

    /// <summary>
    /// ダッシュが終了したときの補正処理
    /// </summary>
    private void DushEndCorrection() {
        float posX = this.transform.position.x;
        float posCheackX = Mathf.Abs(Mathf.Abs(_positionNow.x) - Mathf.Abs(_positionBefore.x));

        if (posCheackX > (float)0.1) {
            if (_positionNow.x < _positionBefore.x) {//左に移動したとき
                posX = this.transform.position.x + (float)2.5;
            } else { 
                posX = this.transform.position.x - (float)2.5;
            }//if
        }//if

        ///上下の補正取得
        float posY = this.transform.position.y;
        float posCheackY = Mathf.Abs(Mathf.Abs(_positionNow.y) - Mathf.Abs(_positionBefore.y));
        if (posCheackY > (float)0.1) {
            if (_positionNow.y < _positionBefore.y) {//下に移動したとき
                posY = this.transform.position.y + (float)5;
            } else { 
                posY = this.transform.position.y - (float)5;
            }//if
        }//if
        this.transform.position = new Vector2(posX,posY);//補正の反映
    }//DushEndCorrection

    /// <summary>
    /// ダッシュ時の補正を求めるための位置情報を更新する処理
    /// </summary>
    private void DushTimer() {
        _positionBefore = _positionNow;
        _positionNow = this.transform.position;
    }//DushTimer

    /// <summary>
    /// Dush時に向かう場所を確定させる処理
    /// </summary>
    private void AngleConfirm() {
        Vector2 arrowPosition = this.transform.position;
        Vector2 dt = (Vector2)Player.transform.position - arrowPosition;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float angle = (270 - (rad * Mathf.Rad2Deg)) % 360;//角度取得
        float scaleX;
        if (angle < 270 && angle > 90) {
            scaleX = Mathf.Abs(this.transform.localScale.x);
            angle += 180;
        } else {
            scaleX = -Mathf.Abs(this.transform.localScale.x);
        }//if

        this.transform.localScale = new Vector2(scaleX, this.transform.localScale.y);
        this.transform.rotation = Quaternion.Euler(
            this.transform.rotation.x, this.transform.rotation.y, angle);
        float directionPosX = Mathf.Abs(Player.transform.localPosition.x - this.transform.position.x);
        float directionPosY = Mathf.Abs(Player.transform.localPosition.y - this.transform.position.y);
        if(directionPosX > directionPosY) {
            _dushMoveY = (Mathf.Floor(((directionPosY / directionPosX)*10))/10)*DUSH_SPEED;
            _dushMoveX = DUSH_SPEED;
        } else {
            _dushMoveX = (Mathf.Floor(((directionPosX / directionPosY) * 10)) / 10*DUSH_SPEED);
            _dushMoveY = DUSH_SPEED;
        }//if
        if (this.transform.position.y > Player.transform.position.y) {
            _dushMoveY = -_dushMoveY;
        }//if
    }//AngleConfirm

}//BigEnemy1
