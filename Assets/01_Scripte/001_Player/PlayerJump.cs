using ConfigDataDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[HideInInspector]
public enum EnumJumpTypeFlag {
    wait,
    normal,
    flipUp,
    flipFall,
    spring,
    wallFall,
    wallFlipFall
}//EnumJumpTypeFlag

/// <summary>
/// 自機のジャンプを行うための処理
/// 更新日時:20211026
/// </summary>
public class PlayerJump : MonoBehaviour {
    private PlayerAnimator _pAnimator;
    private PlayerBody _pBody;
    private PlayerBodyBack _pBodyBack;
    private PlayerUnderAttack _pUnderAttack;
    private PlayerUnderTrigger _pUnderTrigger;
    private PlayerWork _pWork;

    public EnumJumpTypeFlag JumpTypeFlag { get; set; }

    private readonly int FALL_POWER = 12;//一回しか使わないが変更しやすくするため定数にした(0916)
    private readonly int FIRST_JUMP_POWER = 150;//一回しか使わないが変更しやすくするため定数にした(0916)
    private readonly int GRAVITY = -300;
    private readonly int JUMP_POWER = 3;//一回しか使わないが変更しやすくするため定数にした(0916)
    private readonly float KEY_DOWN_TIME = (float)0.3;

    public readonly float SIDE_GRAVITY_FLIP_TIME = (float)0.5;//Workスクリプトで使用するのでpublicになっている(0928)

    private float _sideGravityFlipTimer;//左右重力の時にFlipJumpをした時のタイムフラグ
    public float SideGravityFlipTimer { get { return _sideGravityFlipTimer; } }//読み取り専用

    private float _keyDownTimer;
    public float PastTPY { get; set; }


    private bool _isFlipJumpFall;
    public bool IsFlipJumpFall { get { return _isFlipJumpFall; } }//読み取り専用

    public bool IsJump { get; set; }
    public bool IsWorkSpeedFlip { get; set; }

    public bool IsFlipUpsideDown { get; set; }//上下反転を行ったかの確認判定


    private void Start() {
        _pAnimator = this.GetComponent<PlayerAnimator>();
        _pBody = this.transform.Find("Body").GetComponent<PlayerBody>();
        _pBodyBack = this.transform.Find("BodyBack").GetComponent<PlayerBodyBack>();
        _pUnderAttack = this.transform.Find("UnderAttack").GetComponent<PlayerUnderAttack>();
        _pUnderTrigger = this.transform.Find("UnderTrigger").GetComponent<PlayerUnderTrigger>();
        _pWork = this.transform.GetComponent<PlayerWork>();

        _normalJumpInput = new Dictionary<string, bool>() {
            {"Button",false},
            {"ButtonDown",false},
            {"ButtonUp",false}
        };
        _flipJumpInput = new Dictionary<string, bool>() {
            {"Button",false},
            {"ButtonDown",false},
            {"ButtonUp",false}
        };

        JumpTypeFlag = EnumJumpTypeFlag.wait;
        _keyDownTimer = KEY_DOWN_TIME;
        _sideGravityFlipTimer = 2;
    }//Start

    public Dictionary<string, bool> _normalJumpInput;
    public Dictionary<string, bool> _flipJumpInput;

    /// <summary>
    /// ジャンプボタン各種の入力取得処理
    /// </summary>
    public void JumpButtonInput() {//似た条件文で見やすくするため{}を省略した(0804)
        if (ConfigManager.Instance.config.GetKey(ConfigData.NormalJump.String))
            _normalJumpInput["Button"] = true;
        if (ConfigManager.Instance.config.GetKeyDown(ConfigData.NormalJump.String))
            _normalJumpInput["ButtonDown"] = true;
        if (ConfigManager.Instance.config.GetKeyUp(ConfigData.NormalJump.String))
            _normalJumpInput["ButtonUp"] = true;
        if (ConfigManager.Instance.config.GetKey(ConfigData.FlipJump.String))
            _flipJumpInput["Button"] = true;
        if (ConfigManager.Instance.config.GetKeyDown(ConfigData.FlipJump.String))
            _flipJumpInput["ButtonDown"] = true;
        if (ConfigManager.Instance.config.GetKeyUp(ConfigData.FlipJump.String))
            _flipJumpInput["ButtonUp"] = true;
    }//JumpButtonInpu

    /// <summary>
    /// 各種ジャンプボタンの入力判定の削除処理
    /// </summary>
    private void JumpButtonRelease() {
        _normalJumpInput["Button"] = false;
        _normalJumpInput["ButtonDown"] = false;
        _normalJumpInput["ButtonUp"] = false;
        _flipJumpInput["Button"] = false;
        _flipJumpInput["ButtonDown"] = false;
        _flipJumpInput["ButtonUp"] = false;
    }//JumpButtonRelease


    /// <summary>
    /// ・プレイヤーのジャンプ量を変更する処理
    /// ・PlayerManagerクラスで使用する
    /// </summary>
    /// <param name="jumpSpeed">現在のプレイヤーのジャンプ量</param>
    /// <returns>変更後のジャンプ量</returns>
    public float MoveJump(float jumpSpeed) {
        if (_pUnderTrigger.IsGimmickJump) {
            IsJump = true;
        }//if
        jumpSpeed = UnderAttackPower(jumpSpeed);
        jumpSpeed = NormalJump(jumpSpeed);//FlipJump中は処理更新しない
        jumpSpeed = FlipJump(jumpSpeed);//NormalJump中は処理更新しない
        jumpSpeed = JumpDown(jumpSpeed);//ジャンプの上昇中は処理更新しない
        JumpButtonRelease();
        return jumpSpeed;
    }//MoveJump

    /// <summary>
    /// 敵を踏みつけたときに発生するジャンプ量
    /// </summary>
    /// <param name="jumpSpeed">現在のプレイヤーのジャンプ量</param>
    /// <returns>変更後のジャンプ量</returns>
    private float UnderAttackPower(float jumpSpeed) {
        switch (_pUnderAttack.UnderAttackPower) {
            case 1:
                jumpSpeed = 150;
                break;
            case 2:
                jumpSpeed = 300;
                break;
        }//switch
        _pUnderAttack.UnderAttackPower = 0;
        return jumpSpeed;
    }//UnderAttackPower

    /// <summary>
    /// ・通常ジャンプのジャンプ量を変更する処理
    /// ・MoveJumpメソッドで使用する
    /// </summary>
    /// <param name="jumpSpeed">現在のプレイヤーのジャンプ量</param>
    /// <returns>変更後のジャンプ量</returns>
    private float NormalJump(float jumpSpeed) {
        if (JumpTypeFlag == EnumJumpTypeFlag.flipUp)//FlipJumpボタンを押してるとき
            return jumpSpeed;
        if (_normalJumpInput["ButtonDown"] &&
            _pUnderTrigger.IsUnderTrigger) {//NormalJampボタンを押しているとき
            JumpTypeFlag = EnumJumpTypeFlag.normal;
            _pAnimator.AniJump = true;
        }//if
        return JumpUp(jumpSpeed, _normalJumpInput);
    }//NormalJump

    /// <summary>
    /// ・反転ジャンプのジャンプ量を変更する処理
    /// ・MoveJumpメソッドで使用する
    /// </summary>
    /// <param name="jumpSpeed">現在のプレイヤーのジャンプ量</param>
    /// <returns>変更後のジャンプ量</returns>
    private float FlipJump(float jumpSpeed) {
        if (JumpTypeFlag == EnumJumpTypeFlag.normal)//NormalJumpボタンを押してるとき
            return jumpSpeed;

        //個々の処理を変更する
        if (JumpTypeFlag == EnumJumpTypeFlag.flipUp &&
            _pUnderTrigger.IsUnderTrigger)//重力を反対角度に変更する
            return JumpWithWall(jumpSpeed);

        if (_pUnderTrigger.IsUnderTrigger &&
            _flipJumpInput["ButtonDown"] &&
            JumpTypeFlag < EnumJumpTypeFlag.flipFall) {//FlipJumpボタンを押したとき
            JumpTypeFlag = EnumJumpTypeFlag.flipUp;
        }//if
        if (_keyDownTimer == 0) {//このジャンプを行った瞬間の場合 //上の処理に組み込めそうだが条件文が複雑になるので分割した(0916)
            this.transform.localScale = new Vector2(this.transform.localScale.x, -this.transform.localScale.y);
            _pAnimator.AniFall = true;
        }//if
        return JumpUp(jumpSpeed, _flipJumpInput);
    }//FlipJump


    /// <summary>
    /// ・ジャンプ量を上昇させる処理
    /// ・NormalJump,FlipJumpメソッドで使用する
    /// </summary>
    /// <param name="jumpSpeed">現在のプレイヤーのジャンプ量</param>
    /// <param name="jumpButton">入力したキー</param>
    /// <returns>変更後のジャンプ量</returns>
    private float JumpUp(float jumpSpeed, Dictionary<string,bool>jumpInput) {
        _keyDownTimer += Time.deltaTime;
        if (jumpInput["ButtonDown"] && _pUnderTrigger.IsUnderTrigger) {//入力直後
            _pAnimator.AudioManager.PlaySE("Jump");
            _keyDownTimer = 0;
            _pUnderTrigger.IsUnderTrigger = false;
            IsJump = true;
            return jumpSpeed = FIRST_JUMP_POWER;
        }//if
        if (jumpSpeed < 0) {
            _pUnderTrigger.IsJumpUp = false;
        }//if

        if (_keyDownTimer > KEY_DOWN_TIME) { //入力時間経過
            return jumpSpeed;
        }
        if (jumpInput["ButtonUp"]) {//入力終了
            _keyDownTimer = KEY_DOWN_TIME;
        }//if
        if (jumpInput["Button"]) { //入力中
            _pUnderTrigger.IsJumpUp = true;
            return jumpSpeed +  JUMP_POWER;
        }//if
        
        return jumpSpeed;
    }//JumppingUp

    /// <summary>
    /// ・重力を現在の反対側に変更する処理
    /// ・FlipJumpメソッドで使用する
    /// </summary>
    /// <param name="jumpSpeed">現在のプレイヤーのジャンプ量</param>
    /// <returns>プレイヤーにかかる重力値</returns>
    private float JumpWithWall(float jumpSpeed) {//重力上側にする場合
        RotationChange(this.transform.localEulerAngles.z + 180);
        this.transform.localScale = new Vector2(-this.transform.localScale.x, -this.transform.localScale.y);
        IsFlipUpsideDown = true;
        return GRAVITY;
    }//JumpWithWall


    /// <summary>
    /// ・プレイヤーの降下中の処理
    /// ・MoveJumpメソッドで使用する
    /// </summary>
    /// <param name="jumpSpeed">現在のプレイヤーのジャンプ量</param>
    /// <returns>変更後のジャンプ量</returns>
    private float JumpDown(float jumpSpeed) {
        Falling();
        PastTPY = this.transform.position.y;//自機の高さ更新
        jumpSpeed = LandingJudgment(jumpSpeed);
        if (_pUnderTrigger.IsUnderTrigger)
            return -200;

        //左右重力の時にFlipJumpを行い自機のBodyからステージに触れた場合
        if (_pBody.IsBody != BodyType.wait && _isFlipJumpFall && (int)this.transform.localEulerAngles.z != 0) {
            //上部のステージに触れたとき
            if (!_pBodyBack.IsBodyBack &&
                ((this.transform.localEulerAngles.z == 270 && this.transform.localScale.x < 0) ||
                (this.transform.localEulerAngles.z == 90 && this.transform.localScale.x > 0))) {
                return jumpSpeed - 7;
            }//if
            RotationChange(0);
            float posX = this.transform.position.x;
            float posY = this.transform.position.y + 3f;
            this.transform.position = new Vector2(posX, posY);
            _isFlipJumpFall = false;
        }//if

        ///FlipJump状態を解除させる
        if (_sideGravityFlipTimer > SIDE_GRAVITY_FLIP_TIME && _sideGravityFlipTimer < SIDE_GRAVITY_FLIP_TIME * 2) {
            float jumpPower = FIRST_JUMP_POWER;
            //下向きに落下する場合
            if (this.transform.localScale.x > 0 && this.transform.localEulerAngles.z == 270 ||
                this.transform.localScale.x < 0 && this.transform.localEulerAngles.z == 90 ||
                    this.transform.localEulerAngles.z == 0) {
                IsWorkSpeedFlip = true;
                jumpPower = GRAVITY;
            }//if
            ///上昇,下降共通処理
            _sideGravityFlipTimer = SIDE_GRAVITY_FLIP_TIME * 3;
            _isFlipJumpFall = true;
            return jumpPower;
        } else if (_sideGravityFlipTimer < SIDE_GRAVITY_FLIP_TIME * 3) {
            _sideGravityFlipTimer += Time.deltaTime;
        }//if

        ///左右重力でNormalJumpをする場合
        if (_normalJumpInput["ButtonDown"] &&
            (this.transform.localEulerAngles.z == 90 || this.transform.localEulerAngles.z == 270) &&
            JumpTypeFlag < EnumJumpTypeFlag.wallFall) {
            float jumpPower = GRAVITY;

            //上に上昇する場合
            if ((this.transform.localEulerAngles.z == 90 && this.transform.localScale.x > 0) ||
                this.transform.localEulerAngles.z == 270 && this.transform.localScale.x < 0) {
                this.transform.localScale = new Vector2(-this.transform.localScale.x, this.transform.localScale.y);
                jumpPower = 250;
                _pAnimator.AniJump = true;
            } else {//下に下降
                _pAnimator.AniFall = true;
            }//if

            ///上昇、下降共通処理
            RotationChange(0);
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 3f);
            JumpTypeFlag = EnumJumpTypeFlag.wallFall;
            return jumpPower;
        }//if


        //重力が下以外の場合にジャンプボタンを初めて押した場合
        if ((_normalJumpInput["ButtonDown"] || _flipJumpInput["ButtonDown"]) &&
            this.transform.localEulerAngles.z != 0 && JumpTypeFlag < EnumJumpTypeFlag.wallFall) {
            RotationChange(this.transform.localEulerAngles.z + 180);
            this.transform.localScale = new Vector2(-this.transform.localScale.x, this.transform.localScale.y);
            _sideGravityFlipTimer = 0;
            //FlipJumpを押したとき
            if (this.transform.localEulerAngles.z == 90 || this.transform.localEulerAngles.z == 270) {
                _pAnimator.AniFall = true;
            }
            JumpTypeFlag = EnumJumpTypeFlag.wallFlipFall;
            return GRAVITY;
        }//if

        if (jumpSpeed > GRAVITY && _keyDownTimer > KEY_DOWN_TIME) {//個々の条件文を変更させる(0914)
            jumpSpeed -= FALL_POWER;
        }//if

        return jumpSpeed;
    }//JumppingDown

    /// <summary>
    /// 落下中の処理
    /// </summary>
    private void Falling() {
        if (PastTPY <= transform.position.y || _keyDownTimer <= 0f)
            return;
        if (JumpTypeFlag == EnumJumpTypeFlag.flipUp && _keyDownTimer > KEY_DOWN_TIME) {//FlipJump中の処理
            this.transform.localScale = new Vector2(this.transform.localScale.x, -this.transform.localScale.y);
            JumpTypeFlag = EnumJumpTypeFlag.flipFall;
            _isFlipJumpFall = true;
        }//if
        if (!_pUnderTrigger.IsUnderTrigger) {//ステージに触れていないとき
            _pAnimator.AniFall = true;
            _pAnimator.AniJump = false;
        }//if
        if (_pUnderTrigger.IsRise) {//PlatformEffectorタグに触れたときの着地判定用の判定で使用する
            _pUnderTrigger.IsRise = false;
        }//if
    }//Falling

    /// <summary>
    /// プレイヤーの角度を変更する処理
    /// Workメソッドも同じ処理を行うのでpublicにしてWorkメソッドでも使用する(0918)
    /// </summary>
    /// <param name="TurningAngle">変更したい角度</param>
    public void RotationChange(float anglesZ) {
        this.transform.localRotation = Quaternion.Euler(//一行だと長くなり見にくくなるので改行した
                this.transform.localRotation.x,
                this.transform.localRotation.y,
                anglesZ % 360);
    }//RotationChange

    /// <summary>
    /// 自機が着地している場合の処理
    /// </summary>
    /// <param name="jumpSpeed"></param>
    /// <returns></returns>
    private float LandingJudgment(float jumpSpeed) {
        ///足が地面に触れている場合
        if (!_pUnderTrigger.IsUnderTrigger)
            return jumpSpeed;
        if (!_pUnderTrigger.IsGimmickJump) {
            _pAnimator.AniJump = false;
        }//if
        if (PastTPY < transform.position.y) {
            _pAnimator.AniFall = false;
        }//if
        _sideGravityFlipTimer = SIDE_GRAVITY_FLIP_TIME * 3;
        JumpTypeFlag = 0;
        IsJump = false;
        _isFlipJumpFall = false;
        return LandingJudgSpeed(jumpSpeed);
    }//LandingJudgment

    /// <summary>
    /// 移動量の更新判定処理
    /// </summary>
    /// <param name="jumpSpeed"></param>
    /// <returns></returns>
    private float LandingJudgSpeed(float jumpSpeed) {
        if (jumpSpeed < GRAVITY)
            return jumpSpeed;
        if (this.transform.localEulerAngles.z == 0) {
            return jumpSpeed - FALL_POWER;
        } else {
            return GRAVITY;
        }//if
    }//LandingJudgmentSpeed

    /// <summary>
    /// ジャンプを停止させるための処理
    /// PlayerManagerで使用
    /// </summary>
    /// <param name="jumpSpeed"></param>
    /// <returns></returns>
    public float JumpStop(float jumpSpeed) {
        JumpButtonRelease();
        jumpSpeed = UnderAttackPower(jumpSpeed);
        if (_pAnimator.AniJump) {
            _pAnimator.AniFall = true;
            _pAnimator.AniJump = false;
        }//if
        _keyDownTimer = KEY_DOWN_TIME + 1;
        JumpDown(jumpSpeed);
        _pUnderTrigger.IsJumpUp = false;
        _pUnderTrigger.IsRise = false;
        if (jumpSpeed < GRAVITY) {
            return LandingJudgment(jumpSpeed);
        }//if
        return LandingJudgment(jumpSpeed) - FALL_POWER;
    }//JumpStop

    /// <summary>
    /// ジャンプボタンを制限させる
    /// バネや敵踏みつけジャンプ量とジャンプボタンのジャンプ量が合わされないようにするための処理
    /// </summary>
    public void JumpInputLimit() {
        _keyDownTimer = KEY_DOWN_TIME + 1;
    }//JumpInputLimit

}//Jump
