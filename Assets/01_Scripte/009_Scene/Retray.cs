using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// リトライする場合の画面停止・遷移処理
/// 更新日時:0419
/// </summary>
public class Retray : MonoBehaviour {

    private float MAX_MISS_TIMER = 5f;

    private float _missTimer;

    private void Awake() {
        Time.timeScale = 1;
        _missTimer = MAX_MISS_TIMER;
    }//Awake

    private void Update() {
        if(_missTimer >= MAX_MISS_TIMER)
            return;
        if(_missTimer >= 3) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }else if(_missTimer > 2) {
            Time.timeScale = 0f;
        }//if
        _missTimer += (float)0.01;
    }//Update

    public void SceneRetray(float missTimerNum) {
        _missTimer = missTimerNum;
    }//SceneRetray

}//Retray
