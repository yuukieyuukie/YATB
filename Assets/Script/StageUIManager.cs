using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic; //Dictionary

public class StageUIManager : MonoBehaviour{

    private StageUIScreen currentScreen;
    public CountDownTimer cdt;

    public GameObject GameClearUIPrefab;

    public GameObject GameOverUIPrefab;
    private GameObject GameOverUIInstance;
    private bool retryButtonFlag;

    public GameObject BriefingUIPrefab;

    //　ポーズした時に表示するUIのプレハブ
	public GameObject pauseUIPrefab;
	//　ポーズUIのインスタンス
	private GameObject pauseUIInstance;

    public GameObject Cursor;


    void Start(){
        if(SceneManager.GetActiveScene().name=="Stage3-a" || 
            SceneManager.GetActiveScene().name=="Stage3-b" || 
            SceneManager.GetActiveScene().name=="Stage2" || 
            SceneManager.GetActiveScene().name=="Stage1"){
                currentScreen = StageUIScreen.Briefing;
                Time.timeScale = 0f;
        }
        pauseUIPrefab.SetActive(false);
        BriefingUIPrefab.SetActive(true);
        GameClearUIPrefab.SetActive(false);
        GameOverUIPrefab.SetActive(false);
    }

    void Update(){

        if(currentScreen == StageUIScreen.Briefing){
            //取り合えず準備画面でボタン押したらゲーム開始
            if(Input.GetButtonDown ("Submit")){
                BriefingUIPrefab.SetActive(false);
                currentScreen = StageUIScreen.Game;
                Time.timeScale = 1f;
            }

        }else if(currentScreen == StageUIScreen.Game){
            if (Input.GetButtonDown ("Pause")) {
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

            //ゲーム中、失敗条件を満たすとゲームオーバー画面を表示フラグが立つ
            if(cdt!=null && cdt.getTotalTime()<=0f){
                currentScreen = StageUIScreen.GameOver;
                retryButtonFlag = true;
                Cursor.SetActive(true);
            }

        }else if(currentScreen == StageUIScreen.Dialogue){

        }else if(currentScreen == StageUIScreen.GameClear){

        }else if(currentScreen == StageUIScreen.GameOver){
            if (retryButtonFlag) {
                if (GameOverUIInstance == null) {
                    GameOverUIInstance = GameObject.Instantiate (GameOverUIPrefab) as GameObject;
                    GameOverUIPrefab.SetActive(true);
                    Time.timeScale = 0f;
                } else {
                    Destroy (GameOverUIInstance);
                    GameOverUIPrefab.SetActive(false);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name); //ステージ再読み込み
                }
                retryButtonFlag = false;
		    }
            //決定ボタン押下でゲームオーバー画面削除
            if (!retryButtonFlag && Input.GetButtonDown ("Submit")){
                retryButtonFlag = true;
            }

        }



        // //ゲームクリア画面表示
        // if (currentScreen == StageUIScreen.GameClear && NextButtonFlag) {
		// 	if (GameOverUIInstance == null) {
		// 		GameOverUIInstance = GameObject.Instantiate (GameOverUIPrefab) as GameObject;
        //         GameOverUIPrefab.SetActive(true);
		// 		Time.timeScale = 0f;
		// 	} else {
		// 		Destroy (GameOverUIInstance);
        //         GameOverUIPrefab.SetActive(false);
		// 		SceneManager.LoadScene(SceneManager.GetActiveScene().name); //ステージ再読み込み
		// 	}
        //     NextButtonFlag = false;
		// }
        // //決定ボタン押下で画面削除
        // if (currentScreen == StageUIScreen.GameClear && !NextButtonFlag && Input.GetButtonDown ("Submit")){
        //     NextButtonFlag = true;
        // }



    
    }

    private void NextButtonClicked(){
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

    private void DetailButtonClicked(){
        
    }

    private void RetryButtonClicked(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndButtonClicked(){
        SceneManager.LoadScene("Menu");
    }

    //クリア条件に応じてゴールエリアに触れた時の挙動を変化
    private void checkGoal(){
        if(true){ //ゴールルール：ゴールエリアに到達
            //goalFlg = true;
        }

    }

    //他スクリプトから呼び出して画面を設定する
    public void setCurrentScreen(StageUIScreen state){
        currentScreen = state;
    }

    //他スクリプトから呼び出して画面モード取得
    public StageUIScreen getCurrentScreen(){
        return currentScreen;
    }

}

public enum StageUIScreen{
    Briefing,
    Game,
    Dialogue,
    GameClear,
    GameOver

}
