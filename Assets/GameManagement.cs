using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    //スコア関連
    public Text scoreText;
    private int score;
    public int currentScore; //※なんでscoreとcurrentScoreを分けたかよくわからなかった
    public int clearScore = 1500; //とりあえず1500点を超えるとクリアにする。あとでclear処理は消してもいいかも。

    //タイマー関連
    public Text timerText;
    public float gameTime = 60f;
    int seconds;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        TimeManagement();
    }

    //ゲームを開始前の状態に戻す
    private void Initialize()
    {
        score = 0;
    }

    //タイマー処理？
    public void TimeManagement()
    {
        gameTime -= Time.deltaTime; //deltaTime:直前のフレームと今のフレーム間で経過した時間[秒] を返す　らしい 
        // https://qiita.com/toRisouP/items/930100e25e666494fcd6
        seconds = (int)gameTime; //残り時間をintにして小数点表示しなくする
        timerText.text = seconds.ToString();

        if (seconds == 0) //残り時間0秒になったらゲームオーバー
        {
            Debug.Log("TimeOut");
            GameOver();
        }

    }

    //スコアの追加
    public void AddScore()
    {
        score += 100;
        //scoreText.text = "Score: " + score.ToString();
        currentScore = score;
        scoreText.text = "Score: " + currentScore.ToString();

        //Debug.Log("Add 100");
        Debug.Log(currentScore);
        if(currentScore >= clearScore)
        {
            GameClear();
        }
    }

    //GameOverしたときの処理
    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //※これだとSCORE: 0でリセットするだけになってるので、自分でGameOverシーンを作りたい
    }

    //GameClearしたときの処理
    public void GameClear()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//※これだとSCORE: 0でリセットするだけになってるので、自分でGameClearシーンを作りたい
    }
}
