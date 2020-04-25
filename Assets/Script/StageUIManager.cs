using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageUIManager : MonoBehaviour{

    public static StageUIScreen currentScreen;
    public CountDownTimer cdt;

    public GameObject GameClearUIPrefab;

    public GameObject GameOverUIPrefab;
    private GameObject GameOverUIInstance;
    private bool retryButtonFlag;

    public GameObject BriefingUIPrefab;

    //　ポーズした時に表示するUIのプレハブ・ポーズUIのインスタンス
	public GameObject pauseUIPrefab;
	private GameObject pauseUIInstance;

    public GameObject Cursor;

    public GameObject transitionCanvas;

    private GameObject moveWall;

    private static bool nextToPrev;

    ScoreManager scoreManager = null;

    void Start(){
        moveWall = GameObject.Find("Wall/Fence (1)");
        if(SceneManager.GetActiveScene().name!="Menu" && SceneManager.GetActiveScene().name!="Prologue"){
                if(currentScreen==StageUIScreen.Previous || currentScreen==StageUIScreen.Next){
                    currentScreen = StageUIScreen.Game;
                    Time.timeScale = 1f;
                    BriefingUIPrefab.SetActive(false);
                    moveWall.GetComponent<MoveWall>().setPreviousPos();
                }else{
                    currentScreen = StageUIScreen.Briefing;
                    Time.timeScale = 0f;
                    BriefingUIPrefab.SetActive(true);
                    
                }
        }
        pauseUIPrefab.SetActive(false);
        GameClearUIPrefab.SetActive(false);
        GameOverUIPrefab.SetActive(false);
        transitionCanvas.SetActive(false);
        
    }

    void Update(){
        if(currentScreen == StageUIScreen.Briefing){
            //取り合えず準備画面でボタン押したらゲーム開始
            if(Input.GetButtonDown ("Submit")){
                BriefingUIPrefab.SetActive(false);
                currentScreen = StageUIScreen.Game;
                Time.timeScale = 1f;
                cdt.initTotalTime();
                moveWall.GetComponent<MoveWall>().setPos();
                ChangeSwitchState.setSwitchState(false);
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

        }else if(currentScreen == StageUIScreen.Previous){
            //transition
            transitionCanvas.SetActive(true);
            if(transitionCanvas.transform.Find("Image").GetComponent <TransitionController>().getCorEndFlg()){
                SceneManager.LoadScene("stage3-a");
                nextToPrev = true;
            }
        }else if(currentScreen == StageUIScreen.Next){
            //transition
            transitionCanvas.SetActive(true);
            if(transitionCanvas.transform.Find("Image").GetComponent <TransitionController>().getCorEndFlg()){
                SceneManager.LoadScene("stage3-a2");
            }

        }else if(currentScreen == StageUIScreen.GameClear){
            GameClearUIPrefab.SetActive(true);
            //クリアタイムを保存
            if(scoreManager==null){
                scoreManager = gameObject.AddComponent<ScoreManager>();
                scoreManager.saveScore(cdt.getMinute(), cdt.getSeconds());
            }
            if(Input.GetButtonDown ("Submit")){
                GameClearUIPrefab.SetActive(false);
                SceneManager.LoadScene("Menu");
            }

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


    private void RetryButtonClicked(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndButtonClicked(){
        SceneManager.LoadScene("Menu");
    }

    //クリア条件に応じてゴールエリアに触れた時の挙動を変化
    private void checkGoal(){
        // if(true){ //ゴールルール：ゴールエリアに到達
        //     goalFlg = true;
        // }

    }

    //他スクリプトから呼び出して画面を設定する
    public void setCurrentScreen(StageUIScreen state){
        currentScreen = state;
    }

    //他スクリプトから呼び出して画面モード取得
    public StageUIScreen getCurrentScreen(){
        return currentScreen;
    }

    public void setNextPrevFlg(bool nextPrev){
        nextToPrev = nextPrev;
    }

    public bool getNextPrevFlg(){
        return nextToPrev;
    }


}

public enum StageUIScreen{
    Briefing,
    Game,
    Dialogue,
    Previous,
    Next,
    GameClear,
    GameOver

}
