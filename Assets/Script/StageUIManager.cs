using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageUIManager : MonoBehaviour{

    public static StageUIScreen currentScreen;
    private StageUIScreen oldCurrentScreen;
    public CountDownTimer cdt;
    private MessageUIManager messageUI;

    public GameObject GameClearUIPrefab;
    public GameObject GameOverUIPrefab;
    public GameObject BriefingUIPrefab;
	public GameObject pauseUIPrefab;
    //Pauseで使用中
    public GameObject Cursor;
    private int cursorNum;
    private Vector3 cursorWkPos;

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
                BriefingUIPrefab.SetActive(false);
                moveWall.GetComponent<MoveWall>().setPreviousPos();
            }else{
                currentScreen = StageUIScreen.Briefing;
                Time.timeScale = 0f;
                BriefingUIPrefab.SetActive(true);
                NextScene();
            }
        }
        GameClearUIPrefab.SetActive(false);
        GameOverUIPrefab.SetActive(false);
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

    void Update(){
        if(currentScreen == StageUIScreen.Briefing){
            //取り合えず準備画面でボタン押したらゲーム開始
            if(Input.GetButtonDown ("Submit")){
                BriefingUIPrefab.SetActive(false);
                currentScreen = StageUIScreen.Game;
                Time.timeScale = 1f;
                cdt.initTotalTime();
                if(SceneManager.GetActiveScene().name=="Stage3-a"){
                    moveWall.GetComponent<MoveWall>().setPos();
                }
                ChangeSwitchState.setSwitchState(false);
            }else if (Input.GetButtonDown ("Pause")){
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
                Cursor.transform.position = new Vector3(150f,225f, 0f);
                cursorNum = 0;
            }
            //失敗条件を満たすとゲームオーバー画面に遷移
            if(messageUI.isLifeZero()){
                currentScreen = StageUIScreen.GameOver;
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
            if(Input.GetButtonDown ("Submit")){
                GameClearUIPrefab.SetActive(false);
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
                GameOverUIPrefab.SetActive(true);
                Time.timeScale = 0f;
                oldCurrentScreen = currentScreen;
            }
            if (Input.GetButtonDown ("Submit")){
                GameOverUIPrefab.SetActive(false);
                currentScreen = StageUIScreen.Briefing;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }else if (Input.GetButtonDown ("Pause")){
                Time.timeScale = 1f;
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
