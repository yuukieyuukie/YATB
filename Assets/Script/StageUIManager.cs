using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageUIManager : MonoBehaviour{

    public static StageUIScreen currentScreen;
    private  StageUIScreen oldCurrentScreen;
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
    private int cursorNum;
    private Vector3 cursorWkPos;

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
        oldCurrentScreen = currentScreen;
        
    }

    void Update(){
        if(currentScreen == StageUIScreen.Briefing){
            //取り合えず準備画面でボタン押したらゲーム開始
            if(Input.GetButtonDown ("Submit")){
                BriefingUIPrefab.SetActive(false);
                currentScreen = StageUIScreen.Game;
                Time.timeScale = 1f;
                cdt.initTotalTime();
                if(SceneManager.GetActiveScene().name=="Stage3-a") moveWall.GetComponent<MoveWall>().setPos();
                ChangeSwitchState.setSwitchState(false);
            }else if (Input.GetButtonDown ("Pause")){
                Time.timeScale = 1f;
                SceneManager.LoadScene("Menu");
            }
            oldCurrentScreen = currentScreen;

        }else if(currentScreen == StageUIScreen.Game){
            if (Input.GetButtonDown ("Pause")) {
                // if (pauseUIInstance == null) {
                //     pauseUIInstance = GameObject.Instantiate (pauseUIPrefab) as GameObject;
                //     pauseUIPrefab.SetActive(true);
                //     Time.timeScale = 0f;
                // } else {
                //     Destroy (pauseUIInstance);
                //     pauseUIPrefab.SetActive(false);
                //     Time.timeScale = 1f;
                // }
                pauseUIPrefab.SetActive(true);
                Time.timeScale = 0f;
                currentScreen = StageUIScreen.Pause;
                Cursor.transform.position = new Vector3(150f,225f, 0f);
                cursorNum = 0;
            }

            //ゲーム中、失敗条件を満たすとゲームオーバー画面を表示フラグが立つ
            if(cdt!=null && cdt.getTotalTime()<=0f){
                currentScreen = StageUIScreen.GameOver;
                retryButtonFlag = true;
                Cursor.SetActive(true);
            }
            oldCurrentScreen = currentScreen;
        }else if(currentScreen == StageUIScreen.Dialogue){

        }else if(currentScreen == StageUIScreen.Previous){
            //transition
            transitionCanvas.SetActive(true);
            if(transitionCanvas.transform.Find("Image").GetComponent <TransitionController>().getCorEndFlg()){
                SceneManager.LoadScene("stage3-a");
                nextToPrev = true;
            }
            oldCurrentScreen = currentScreen;
        }else if(currentScreen == StageUIScreen.Next){
            //transition
            transitionCanvas.SetActive(true);
            if(transitionCanvas.transform.Find("Image").GetComponent <TransitionController>().getCorEndFlg()){
                SceneManager.LoadScene("stage3-a2");
            }
            oldCurrentScreen = currentScreen;
        }else if(currentScreen == StageUIScreen.GameClear){
            if(oldCurrentScreen!=currentScreen){
                GameClearUIPrefab.SetActive(true);
                oldCurrentScreen = currentScreen;
            }
            //クリアタイムを保存
            if(scoreManager==null){
                scoreManager = gameObject.AddComponent<ScoreManager>();
                if(SceneManager.GetActiveScene().name=="Stage3-a"){
                    scoreManager.saveScore(cdt.getMinute(), cdt.getSeconds(), "3a");
                }else if(SceneManager.GetActiveScene().name=="Stage3-b"){
                    scoreManager.saveScore(cdt.getMinute(), cdt.getSeconds(), "3b");
                }
            }
            if(Input.GetButtonDown ("Submit")){
                GameClearUIPrefab.SetActive(false);
                if(SceneManager.GetActiveScene().name=="Stage3-a"){
                    SceneManager.LoadScene("stage3-b");
                }else if(SceneManager.GetActiveScene().name=="Stage3-b"){
                    SceneManager.LoadScene("Menu");
                }
            }

        }else if(currentScreen == StageUIScreen.GameOver){
            if(retryButtonFlag) {
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
            }else if (!retryButtonFlag && Input.GetButtonDown ("Pause")){
                Time.timeScale = 1f;
                retryButtonFlag = true;
                SceneManager.LoadScene("Menu");
            }
            oldCurrentScreen = currentScreen;
        }else if(currentScreen == StageUIScreen.Pause){
            if(cursorNum==0 && Input.GetButtonDown ("Down")){
                cursorWkPos = Cursor.transform.position;
                cursorWkPos.y -= 60f;
                Cursor.transform.position = cursorWkPos;
                cursorNum++;
            }else if(cursorNum==1 && Input.GetButtonDown ("Up")){
                cursorWkPos = Cursor.transform.position;
                cursorWkPos.y += 60f;
                Cursor.transform.position = cursorWkPos;
                cursorNum--;
            }
            if(Input.GetButtonDown ("Submit")){
                if(cursorNum==0){
                    pauseUIPrefab.SetActive(false);
                    Time.timeScale = 1f;
                    currentScreen = StageUIScreen.Game;
                }else if(cursorNum==1){
                    SceneManager.LoadScene("Menu");
                    Time.timeScale = 1f;
                }

            }
            oldCurrentScreen = currentScreen;
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

    //以下、他スクリプトから呼び出して画面を設定する
    public void setCurrentScreen(StageUIScreen state){
        currentScreen = state;
    }

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
    GameOver,
    Pause

}
