using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の移動値の更新処理
/// 更新日時:0602
/// </summary>
public class EnemyWork : MonoBehaviour {

    public float SkyTime { get; set; }//向き変更の時間待ち用変数

    private float _scaleX;//Enemyの向き情報を保持する変数
    private float _velocityY;//Enemyのふらつき値を保持する変数

    private bool _isDirection;//向きを変更させるための判定(条件で向きを変更させる敵用の変数)
    private bool _isWobbling;//Enemyふらつき判定用変数



    /// <summary>
    /// EnemyNで使用する
    /// EnemyNの初期ScaleXを取得する
    /// </summary>
    /// <param name="EnemyTransformLocalScaleX"></param>
    public void WorkStart(float EnemyTransformLocalScaleX,Rigidbody2D rb2d) {
        _scaleX = EnemyTransformLocalScaleX;
        _isDirection = true;
        _isWobbling = true;
    }//WorkStart


    /// <summary>
    /// 陸上用
    /// 壁に触れると反対方向に移動する
    /// 崖から降りず反対方向に移動する
    /// 崖から降りず反対方向に移動する
    /// </summary>
    /// <param name="workType"></param>
    /// <param name="enemySpeed"></param>
    /// <param name="rb2d"></param>
    /// <param name="animator"></param>
    /// <param name="transform"></param>
    /// <param name="enemySideDecision"></param>
    /// <param name="enemyUnderDecisionTrigger"></param>
    /// <returns></returns>
    public float LandWork(
            string workType,
            float enemySpeed, 
            Rigidbody2D rb2d, 
            Transform transform, 
            EnemySideDecision enemySideDecision,
            EnemyUnderDecisionTrigger enemyUnderDecisionTrigger) {
        _velocityY = rb2d.velocity.y;
        enemySpeed = DecisionCheck(enemySideDecision.SideDecisionCol, enemyUnderDecisionTrigger.UnderDecisionTri, enemySpeed,transform, rb2d);
        //振り向き上の条件をすべてオフにする
        enemySideDecision.SideDecisionCol = false;
        enemyUnderDecisionTrigger.UnderDecisionTri = false;
        return IsWorkCheck(workType, enemySpeed, rb2d);
    }//LandWork


    /// <summary>
    /// 陸上用移動処理
    /// 壁に触れると反対方向に移動
    /// 崖から降りる
    /// </summary>
    /// <param name="workType"></param>
    /// <param name="enemySpeed"></param>
    /// <param name="rb2d"></param>
    /// <param name="animator"></param>
    /// <param name="transform"></param>
    /// <param name="enemySideDecision"></param>
    /// <returns></returns>
    public float LandWork(string workType, float enemySpeed,Rigidbody2D rb2d, 
        Animator animator, Transform transform,EnemySideDecision enemySideDecision) {
        _velocityY = rb2d.velocity.y;
        enemySpeed = DecisionCheck(enemySideDecision.SideDecisionCol,enemySpeed,transform);
        //振り向き上の条件をすべてオフにする
        enemySideDecision.SideDecisionCol = false;
        return IsWorkCheck(workType, enemySpeed, rb2d);
    }//LandWork


    /// <summary>
    /// 空中で移動する適用の処理
    /// </summary>
    /// <param name="workType"></param>
    /// <param name="enemySpeed"></param>
    /// <param name="wobblingTime"></param>
    /// <param name="wobblingSpeed"></param>
    /// <param name="rb2d"></param>
    /// <param name="animator"></param>
    /// <param name="transform"></param>
    /// <param name="enemySideDecision"></param>
    /// <returns></returns>
    public float SkyHorizontalMove(string workType, float enemySpeed,float wobblingTime,float wobblingSpeed,
        Rigidbody2D rb2d, Animator animator, Transform transform,EnemySideDecision enemySideDecision) {

        WobblingJudge(wobblingTime, wobblingSpeed);
        enemySpeed = DecisionCheck(enemySideDecision.SideDecisionCol,enemySpeed,transform);
        enemySideDecision.SideDecisionCol = false;//向きの条件オフ
        return IsWorkCheck(workType, enemySpeed, rb2d);
    }//SkyWork

    /// <summary>
    /// 上下移動値の更新
    /// </summary>
    /// <param name="wobblingTime"></param>
    /// <param name="wobblingSpeed"></param>
    private void WobblingJudge(float wobblingTime,float wobblingSpeed) {
        SkyTime += Time.deltaTime;//時間を反映させる
        if (_isWobbling) {
            _velocityY -= wobblingSpeed;//下降させる
        } else {
            _velocityY += wobblingSpeed;//上昇させる
        }//if
        if (SkyTime < wobblingTime)
            return;
        _isWobbling = !_isWobbling;//true false を逆にする
        _velocityY = 0;//重力地をなくす
        SkyTime = 0;//リミットの初期化
    }//WebblingJudge

    /// <summary>
    /// 時期に向かって空中を移動する処理
    /// </summary>
    /// <param name="EnemyAnimator"></param>
    /// <param name="underTrigger"></param>
    /// <param name="player"></param>
    /// <param name="RB2D"></param>
    /// <param name="transform"></param>
    /// <param name="EnemySpeed"></param>
    /// <param name="distancePlayerX"></param>
    /// <param name="distancePlayerY"></param>
    /// <returns></returns>
    public float SkyDiagonalMove(
        Animator EnemyAnimator,EnemyUnderTrigger underTrigger,
        GameObject player,Rigidbody2D RB2D,Transform transform,
        float EnemySpeed,float distancePlayerX,float distancePlayerY) {
        EnemyAnimator.SetBool("AniFlight", true);
        EnemyAnimator.SetBool("AniDescent", false);
        if (underTrigger.IsUnderTrigger) {
            underTrigger.IsUnderTrigger = false;
        }//if
        float VerticaMove = 0;
        //自機の座標を取得してその場所に一定のスピードで移動する
        if (transform.position.x > player.transform.position.x) {//右移動
            if (transform.localScale.x > 0 && Mathf.Abs(distancePlayerX) > 3) {
                EnemySpeed = -EnemySpeed;
                transform.localScale = new Vector2(-(transform.localScale.x),transform.localScale.y);
            }//if
        } else {//左移動
            if (transform.localScale.x < 0 && Mathf.Abs(distancePlayerX) > 3) {
                EnemySpeed = -EnemySpeed;
                transform.localScale = new Vector2(-(transform.localScale.x),transform.localScale.y);
            }//if  
        }//if
        if(Mathf.Abs(distancePlayerY) > 0.5) {
            if (transform.position.y < player.transform.position.y) {//上移動 
                VerticaMove = Mathf.Abs(EnemySpeed);
            } else {//下移動
                VerticaMove = -Mathf.Abs(EnemySpeed);
            }//if
        }//if
        RB2D.velocity = new Vector2(EnemySpeed, VerticaMove);
        return EnemySpeed;
    }

    /// <summary>
    /// ジャンプするための最低限の処理
    /// Enemy10で使用する
    /// </summary>
    /// <param name="eAnimator"></param>
    /// <param name="eUnderTrigger"></param>
    /// <param name="underDecisionTrigger"></param>
    /// <param name="RB2D"></param>
    /// <param name="transform"></param>
    /// <param name="EnemySpeed"></param>
    /// <param name="jumpPower"></param>
    /// <param name="pastTPY"></param>
    /// <param name="jumpTimer"></param>
    /// <param name="JUMP_POWER_MAX"></param>
    /// <param name="isFall"></param>
    /// <returns></returns>
    public (float,float,bool)Jump(
        Animator eAnimator, EnemyUnderTrigger eUnderTrigger, 
        EnemyLandingCheck eLandingCheck,Rigidbody2D RB2D, Transform transform,
        float EnemySpeed, float jumpPower, float pastTPY, float jumpTimer, int JUMP_POWER_MAX, bool isFall) {
        //ジャンプ中処理
        //自機のジャンプ処理を参考にして作成する
        //壁にぶつかったときは逆方向を向くようにする
        if (pastTPY > transform.position.y) {
            eAnimator.SetBool("AniFall", true);
        }//if
        if (jumpPower != JUMP_POWER_MAX && !isFall) {
            jumpPower = JUMP_POWER_MAX;
            GameObject.Find("GameManager").GetComponent<AudioManager>().PlaySE("Enemy_Jump");
            eAnimator.SetBool("AniJump", true);
        } else {
            jumpPower -= 5;
            isFall = true;
            (jumpPower,jumpTimer,isFall)=LandingDecision(
                eAnimator,eUnderTrigger,eLandingCheck, jumpPower, jumpTimer, isFall);
        }//if
        RB2D.velocity = new Vector2(EnemySpeed, jumpPower / 10);
        return (jumpPower,jumpTimer,isFall);
    }//Jump

    /// <summary>
    /// 地面に着地しているかの判定をとる処理
    /// </summary>
    /// <param name="eAnimator"></param>
    /// <param name="underDecisionTrigger"></param>
    /// <param name="jumpPower"></param>
    /// <param name="jumpTimer"></param>
    /// <param name="isFall"></param>
    /// <returns></returns>
    private (float,float,bool) LandingDecision(
         Animator eAnimator,EnemyUnderTrigger eUnderTrigger,EnemyLandingCheck eLandingCheck,
         float jumpPower,float jumpTimer,bool isFall) {

        if (eUnderTrigger.IsUnderTrigger && eAnimator.GetBool("AniFall")) {//地面に着地している場合
            isFall = false;
            jumpTimer = 0;
            jumpPower = 0;
            eAnimator.SetBool("AniJump", false);
            eAnimator.SetBool("AniFall", false);
        }//if
        return (jumpPower, jumpTimer, isFall);
    }//LandingDecision

    /// <summary>
    /// 壁・崖での向き変更処理
    /// </summary>
    /// <param name="sideDecisionCol"></param>
    /// <param name="underDecisionTri"></param>
    /// <param name="enemySpeed"></param>
    /// <param name="transform"></param>
    /// <param name="rb2d"></param>
    /// <returns></returns>
    private float DecisionCheck(bool sideDecisionCol,bool underDecisionTri,float enemySpeed,Transform transform,Rigidbody2D rb2d) {
        if (_isDirection) {
            transform.localScale = new Vector2(_scaleX, transform.localScale.y);
            _isDirection = false;
        }//if
        if (sideDecisionCol || underDecisionTri) {
            _velocityY = rb2d.velocity.y;
            enemySpeed = -enemySpeed;//移動する向きの移動量に変更
            transform.localScale = new Vector2(-(transform.localScale.x), transform.localScale.y);//向きの反転を反映
        }//if
        return enemySpeed;
    }//DecisionCheck

    /// <summary>
    /// 壁での向き変更処理
    /// </summary>
    /// <param name="sideDecisionCol"></param>
    /// <param name="enemySpeed"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    private float DecisionCheck(bool sideDecisionCol,float enemySpeed,Transform transform) {
        if (sideDecisionCol) {
            enemySpeed = -enemySpeed;//移動する向きの移動量に変更
            _scaleX = -_scaleX;//向きの反転
            transform.localScale = new Vector2(-(transform.localScale.x), transform.localScale.y);//向きの反転を反映
        }//if
        return enemySpeed;
    }//DecisionCheck

    /// <summary>
    /// 移動量の反映処理
    /// </summary>
    /// <param name="workType"></param>
    /// <param name="enemySpeed"></param>
    /// <param name="isWork"></param>
    /// <param name="rb2d"></param>
    /// <returns></returns>
    private float IsWorkCheck(string workType, float enemySpeed, Rigidbody2D rb2d) {
        WorkType(workType, enemySpeed, rb2d);
        return enemySpeed;//更新情報の受け渡し用
    }//IsWorkCheck

    /// <summary>
    /// 移動向きの設定処理
    /// </summary>
    /// <param name="workType"></param>
    /// <param name="enemySpeed"></param>
    /// <param name="rb2d"></param>
    private void WorkType(string workType,float enemySpeed, Rigidbody2D rb2d) {

        switch (workType) {//各Enemyの移動タイプごとに処理を変更する
            case "Left_Right"://左右移動の処理
                rb2d.velocity = new Vector2(enemySpeed, _velocityY);//移動処理の反映
                break;
            case "Up_Down"://上下移動の処理
                break;
            case "Diagonal"://斜め移動の処理
                break;
        }//switch

    }//WorkType

}//class
