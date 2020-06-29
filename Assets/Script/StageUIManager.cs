using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageUIManager : MonoBehaviour{

    public static StageUIScreen currentScreen;
    private StageUIScreen oldCurrentScreen;
    public CountDownTimer cdt;
    private MessageUIManager messageUI;

    public GameObject briefingUIPrefab;
    public GameObject gameClearUIPrefab;
    public GameObject gameOverUIPrefab;
	public GameObject pauseUIPrefab;

    //Pause
    public GameObject pauseContinueIcon, pauseExitIcon;
    //GameOver
    public GameObject overRetryIcon, overExitIcon;

    private StageMode stageMode;

    public GameObject transitionCanvas;

    private GameObject moveWall;

    private static bool nextToPrev; //a2 -> a した時のプレイヤー位置設定

    ScoreManager scoreManager = null;

    [SerializeField]
	private GameObject loadUI;
    private Color colorLoadUI;
    private float alfaLoadUI = 1f;

    void Start(){
        messageUI = GameObject.Find("MessageUI").GetComponent<MessageUIManager>();
        moveWall = GameObject.Find("Wall/Fence (1)");
        if(SceneManager.GetActiveScene().name!="Menu" && SceneManager.GetActiveScene().name!="Prologue"){
            if(currentScreen==StageUIScreen.Previous || currentScreen==StageUIScreen.Next){
                currentScreen = StageUIScreen.Game;
                Time.timeScale = 1f;
                briefingUIPrefab.SetActive(false);
                moveWall.GetComponent<MoveWall>().setPreviousPos();
            }else{
                currentScreen = StageUIScreen.Briefing;
                Time.timeScale = 0f;
                briefingUIPrefab.SetActive(true);
                NextScene();
            }
        }
        gameClearUIPrefab.SetActive(false);
        gameOverUIPrefab.SetActive(false);
        pauseUIPrefab.SetActive(false);
        transitionCanvas.SetActive(false);
        oldCurrentScreen = currentScreen;

    }

    private void NextScene() {
        colorLoadUI = loadUI.GetComponent<Image>().color;
        colorLoadUI.a = alfaLoadUI;
        loadUI.GetComponent<Image>().color = colorLoadUI;
		StartCoroutine("LoadData"); //　コルーチン開始
	}

	IEnumerator LoadData() {
		//　読み込みが終わるまで
		while(alfaLoadUI>0f){
            colorLoadUI.a = alfaLoadUI;
            loadUI.GetComponent<Image>().color = colorLoadUI;
            alfaLoadUI -= 0.1f;
			yield return null;
		}
	}
    
    private float moveHorizontal3;
    private float moveVertical3;
    private bool pushTimeL, pushTimeR, pushTimeU, pushTimeD;

    void Update(){

        moveHorizontal3 = Input.GetAxisRaw("Axis 7"); //十字キー横, adキー
        moveVertical3 = Input.GetAxisRaw("Axis 8"); //十字キー縦, wsキー
        // Debug.Log(moveHorizontal3+"  "+moveVertical3);

        if(moveHorizontal3==0f){
            pushTimeL = true;
            pushTimeR = true;
        }
        if(moveVertical3==0f){
            pushTimeU = true;
            pushTimeD = true;
        }

        // if(pushTimeR && moveHorizontal3>0f){
        //     Debug.Log("right");
        //     pushTimeR = false;
        // }else if(pushTimeL && moveHorizontal3<0f){
        //     Debug.Log("left");
        //     pushTimeL = false;
        // }else if(pushTimeU && moveVertical3>0f){
        //     Debug.Log("up");
        //     pushTimeU = false;
        // }else if(pushTimeD && moveVertical3<0f){
        //     Debug.Log("down");
        //     pushTimeD = false;
        // }

        if(currentScreen == StageUIScreen.Briefing){
            //取り合えず準備画面でボタン押したらゲーム開始
            if(Input.GetButtonDown ("Submit")){
                briefingUIPrefab.SetActive(false);
                currentScreen = StageUIScreen.Game;
                Time.timeScale = 1f;
                cdt.initTotalTime();
                if(SceneManager.GetActiveScene().name=="Stage3-a"){
                    moveWall.GetComponent<MoveWall>().setPos();
                }
                ChangeSwitchState.setSwitchState(false);
            }else if (Input.GetButtonDown ("Cancel")){
                Time.timeScale = 1f;
                SceneManager.LoadScene("Menu");
            }
            oldCurrentScreen = currentScreen;

        }else if(currentScreen == StageUIScreen.Game){
            oldCurrentScreen = currentScreen;
            if (Input.GetButtonDown ("Pause")) {
                pauseUIPrefab.SetActive(true);
                Time.timeScale = 0f;
                currentScreen = StageUIScreen.Pause;
            }
            //失敗条件を満たすとゲームオーバー画面に遷移
            if(messageUI.isLifeZero()){
                currentScreen = StageUIScreen.GameOver;
            }
            
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
                gameClearUIPrefab.SetActive(true);
                oldCurrentScreen = currentScreen;
            }
            if(Input.GetButtonDown ("Submit")){
                gameClearUIPrefab.SetActive(false);
                if(SceneManager.GetActiveScene().name=="Stage3-a"){
                    SceneManager.LoadScene("stage3-b");
                }else if(SceneManager.GetActiveScene().name=="Stage3-b"){
                    SceneManager.LoadScene("Menu");
                }
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
        }else if(currentScreen == StageUIScreen.GameOver){
            if(oldCurrentScreen!=currentScreen){
                gameOverUIPrefab.SetActive(true);
                Time.timeScale = 0f;
                oldCurrentScreen = currentScreen;
            }
            if(pushTimeD && moveVertical3==-1f){
                pushTimeD = false;
                overRetryIcon.SetActive(false);
                overExitIcon.SetActive(true);
                stageMode = StageMode.Exit;
            }else if(pushTimeU && moveVertical3==1f){
                pushTimeU = false;
                overRetryIcon.SetActive(true);
                overExitIcon.SetActive(false);
                stageMode = StageMode.Retry;
            }
            if(Input.GetButtonDown ("Submit")){
                Time.timeScale = 1f;
                if(stageMode == StageMode.Retry){
                    gameOverUIPrefab.SetActive(false);
                    currentScreen = StageUIScreen.Briefing;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }else if(stageMode == StageMode.Exit){
                    SceneManager.LoadScene("Menu");
                }
            }
            oldCurrentScreen = currentScreen;
        }else if(currentScreen == StageUIScreen.Pause){
            if(pushTimeD && moveVertical3==-1f){
                pushTimeD = false;
                pauseContinueIcon.SetActive(false);
                pauseExitIcon.SetActive(true);
                stageMode = StageMode.Exit;
            }else if(pushTimeU && moveVertical3==1f){
                pushTimeU = false;
                pauseContinueIcon.SetActive(true);
                pauseExitIcon.SetActive(false);
                stageMode = StageMode.Continue;
            }
            if(Input.GetButtonDown ("Submit")){
                Time.timeScale = 1f;
                if(stageMode == StageMode.Continue){
                    pauseUIPrefab.SetActive(false);
                    currentScreen = StageUIScreen.Game;
                }else if(stageMode == StageMode.Exit){
                    SceneManager.LoadScene("Menu");
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

public enum StageMode{
    Continue,
    Retry,
    Exit
}