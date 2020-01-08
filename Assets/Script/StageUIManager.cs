using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic; //Dictionary

public class StageUIManager : MonoBehaviour{

    public GameObject GameClearNextBt;
    public GameObject GameClearDetailBt;
    public GameObject GameClearPanel;
    public GameObject GameClearText;
    public GameObject GameOverRetryBt;
    public GameObject GameOverEndBt;
    public GameObject GameOverPanel;
    public GameObject GameOverText;
    public CountDownTimer cdt;
    public GameObject BriefingUIPrefab;

    //　ポーズした時に表示するUIのプレハブ
	public GameObject pauseUIPrefab;
	//　ポーズUIのインスタンス
	private GameObject pauseUIInstance;
    private UIScreen2 currentScreen;


    void Start(){
        if(SceneManager.GetActiveScene().name=="Stage3-a" || 
            SceneManager.GetActiveScene().name=="Stage3-b" || 
            SceneManager.GetActiveScene().name=="Stage2" || 
            SceneManager.GetActiveScene().name=="Stage1"){
                currentScreen = UIScreen2.Briefing;
        }
        pauseUIPrefab.SetActive(false);
        BriefingUIPrefab.SetActive(true);
    }

    void Update(){

        //ゲーム中（ボールが動いている間）
        if(currentScreen == UIScreen2.Briefing){
            
            Time.timeScale = 0f;

        }else if(currentScreen == UIScreen2.Game){
            GameClearPanel.SetActive(false);
            GameClearText.SetActive(false);
            GameClearNextBt.SetActive(false);
            GameClearDetailBt.SetActive(false);
            GameOverPanel.SetActive(false);
            GameOverText.SetActive(false);
            GameOverRetryBt.SetActive(false);
            GameOverEndBt.SetActive(false);
        }else if(currentScreen == UIScreen2.Dialogue){
            //今のところUI表示はなし
        }else if(currentScreen == UIScreen2.GameClear){
            GameClearPanel.SetActive(true);
            GameClearText.SetActive(true);
            GameClearNextBt.SetActive(true);
            GameClearDetailBt.SetActive(true);
        }else if(currentScreen == UIScreen2.GameOver){
            GameOverPanel.SetActive(true);
            GameOverText.SetActive(true);
            GameOverRetryBt.SetActive(true);
            GameOverEndBt.SetActive(true);
        }

        //クリア・失敗条件達成でResult画面
        if(cdt!=null && cdt.getTotalTime()<=0f){
            currentScreen = UIScreen2.GameOver;
        }


        //pauseキーで一時中断　これお手本に各画面遷移を実現したい
		if (currentScreen == UIScreen2.Game && Input.GetButtonDown ("Pause")) {
			if (pauseUIInstance == null) {
				pauseUIInstance = GameObject.Instantiate (pauseUIPrefab) as GameObject;
                pauseUIPrefab.SetActive(true);
				Time.timeScale = 0f;
			} else {
				Destroy (pauseUIInstance);
                pauseUIPrefab.SetActive(false);
				Time.timeScale = 1f;
			}
		}

        //取り合えず準備画面でボタン押したらゲーム開始
        if(currentScreen == UIScreen2.Briefing && Input.GetButtonDown ("Submit")){
            BriefingUIPrefab.SetActive(false);
            currentScreen = UIScreen2.Game;
            Time.timeScale = 1f;
        }
    
    }

    public void NextButtonClicked(){
        if(SceneManager.GetActiveScene().name=="Stage1"){
            SceneManager.LoadScene("ScenarioScene1");
        }else if(SceneManager.GetActiveScene().name=="Stage2"){
            SceneManager.LoadScene("ScenarioScene2");
        }else if(SceneManager.GetActiveScene().name=="Stage3-a"){
            SceneManager.LoadScene("ScenarioScene1");
        }else if(SceneManager.GetActiveScene().name=="Stage3-b"){
            SceneManager.LoadScene("ScenarioScene1");
        }
    }

    public void DetailButtonClicked(){
        
    }

    public void RetryButtonClicked(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndButtonClicked(){
        SceneManager.LoadScene("Menu");
    }

    //クリア条件に応じてゴールエリアに触れた時の挙動を変化
    public void checkGoal(){
        if(true){ //ゴールルール：ゴールエリアに到達
            //goalFlg = true;
        }

    }

    //他スクリプトから呼び出して画面を設定する
    public void setCurrentScreen(UIScreen2 state){
        currentScreen = state;
    }

    //他スクリプトから呼び出して画面モード取得
    public UIScreen2 getCurrentScreen(){
        return currentScreen;
    }

}

public enum UIScreen2{
    Briefing,
    Game,
    Dialogue,
    GameClear,
    GameOver

}
