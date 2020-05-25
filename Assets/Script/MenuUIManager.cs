//#define TMP_PROFILE_ON
//#define TMP_PROFILE_PHASES_ON


using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuUIManager : MonoBehaviour {

    public GameObject menuCam;
    public GameObject StartScene;
    public GameObject LoadScene;
    public GameObject SelectScene;
    public GameObject OptionsScene;
    public GameObject SpecialScene;

    public GameObject NewGameSceneObj;
    public GameObject LoadSceneObj;
    public GameObject SelectSceneObj;
    public GameObject OptionSceneObj;
    public GameObject SpecialSceneObj;
    public GameObject CameraStartPos;
    Vector3 targetPos, fromPos, toPos; //カメラの移動始点・終点
    private float cameraMove = 0f;
    private bool cameraMoveFlg;

    private UIScreen currentScreen;

    private static int language;

    ScoreManager scoreManager = null;

    private GameObject nowIcon, nowLine, nowText;
    private GameObject startIcon, optionIcon, exitIcon, specialIcon;
    private GameObject startUnderline, optionUnderline, exitUnderline, specialUnderline;
    private GameObject startText, optionText, exitText, specialText;
    private SelectMode selectMode;

    private  GameObject jpIcon, enIcon;

    //　非同期動作で使用するAsyncOperation
	private AsyncOperation async;
	//　シーンロード中に表示するUI画面
	[SerializeField]
	private GameObject loadUI;
    private Color colorLoadUI;
    private float alfaLoadUI = 0f;

    void Start(){
        
        currentScreen = UIScreen.MainMenu;

        targetPos = CameraStartPos.gameObject.transform.position;

        fromPos = menuCam.gameObject.transform.position;
        toPos = targetPos;

        startIcon = GameObject.Find("MainCanvas/StartScene/StartIcon");
        optionIcon = GameObject.Find("MainCanvas/StartScene/OptionIcon");
        exitIcon = GameObject.Find("MainCanvas/StartScene/ExitIcon");
        specialIcon = GameObject.Find("MainCanvas/StartScene/SpecialNotIcon");
        startUnderline = GameObject.Find("MainCanvas/StartScene/StartUnderline");
        optionUnderline = GameObject.Find("MainCanvas/StartScene/OptionUnderline");
        exitUnderline = GameObject.Find("MainCanvas/StartScene/ExitUnderline");
        specialUnderline = GameObject.Find("MainCanvas/StartScene/SpecialUnderline");
        startText = GameObject.Find("MainCanvas/StartScene/StartText");
        optionText = GameObject.Find("MainCanvas/StartScene/OptionText");
        exitText = GameObject.Find("MainCanvas/StartScene/ExitText");
        specialText = GameObject.Find("MainCanvas/StartScene/SpecialText");
        nowIcon = startIcon;
        nowLine = startUnderline;
        nowText = startText;
        selectMode = SelectMode.Start;

        jpIcon = GameObject.Find("MainCanvas/OptionsScene/JPIcon");
        enIcon = GameObject.Find("MainCanvas/OptionsScene/ENIcon");
        scoreManager = gameObject.AddComponent<ScoreManager>();
        if(scoreManager.getLanguage()==0){
            enIcon.SetActive(false);
        }else{
            jpIcon.SetActive(false);
        }
        scoreManager = null;
        
        colorLoadUI = loadUI.GetComponent<Image>().color;
        colorLoadUI.a = alfaLoadUI;
        loadUI.GetComponent<Image>().color = colorLoadUI;

        Time.timeScale = 1f;
    }

    void Update(){

        if(currentScreen != UIScreen.MainMenu && Input.GetButtonDown("Cancel")){
            CancelButtonClicked();
        }

        //画面遷移する項目選択時、カメラに移動量を与える
        if(cameraMoveFlg){
            cameraMove += Time.deltaTime;
            //項目選択時の画面遷移を実施
            menuCam.transform.position = Vector3.Lerp (
                fromPos,
                toPos,
                cameraMove
            );
            if(cameraMove>=1f){
                cameraMoveFlg = false;
                cameraMove = 0f;
            }
        }

        if(currentScreen == UIScreen.MainMenu){
            if(Input.GetButtonDown("Down")){
                setSelectUI(exitIcon, exitUnderline, exitText);
                selectMode = SelectMode.Exit;
            }else if(Input.GetButtonDown("Up")){ //special未実装
                setSelectUI(specialIcon, specialUnderline, specialText);
                selectMode = SelectMode.Special;
            }else if(Input.GetButtonDown("Right")){
                setSelectUI(optionIcon, optionUnderline, optionText);
                selectMode = SelectMode.Option;
            }else if(Input.GetButtonDown("Left")){
                setSelectUI(startIcon, startUnderline, startText);
                selectMode = SelectMode.Start;
            }
            if(Input.GetButtonDown("Submit")){
                StartButtonClicked();
            }
        }else if(currentScreen == UIScreen.NewGame){
            if(Input.GetButtonDown("Left")){
                selectMode = SelectMode.Story;
            }else if(Input.GetButtonDown("Right")){
                selectMode = SelectMode.StageSelect;
            }
            if(Input.GetButtonDown("Submit")){
                NewGameButtonClicked();
            }
        }else if(currentScreen == UIScreen.LoadScene){
        }else if(currentScreen == UIScreen.SelectScene){
        }else if(currentScreen == UIScreen.Options){
            if(Input.GetButtonDown("Right")){
                jpIcon.SetActive(false);
                enIcon.SetActive(true);
                language = 1; //英語
            }else if(Input.GetButtonDown("Left")){
                jpIcon.SetActive(true);
                enIcon.SetActive(false);
                language = 0; //日本語
            }
        }else if(currentScreen == UIScreen.Special){
        }
    }

	private void NextScene() {
		StartCoroutine("LoadData"); //　コルーチン開始
	}

	IEnumerator LoadData() {
		// シーンの読み込みをする
		async = SceneManager.LoadSceneAsync("Stage3-a");
        
		//　読み込みが終わるまで
		while(async.progress<1f){
            colorLoadUI.a = alfaLoadUI;
            loadUI.GetComponent<Image>().color = colorLoadUI;
            alfaLoadUI = async.progress+0.3f;
			yield return null;
		}
	}

    private void setSelectUI(GameObject icon, GameObject line, GameObject text){
        nowIcon.SetActive(false);
        nowLine.SetActive(false);
        nowText.SetActive(false);
        nowIcon = icon;
        nowLine = line;
        nowText = text;
        nowIcon.SetActive(true);
        nowLine.SetActive(true);
        nowText.SetActive(true);
    }

    private bool bossDefeatFlg = true;
    
    //スタート画面でいずれかのモードを選択した
    private void StartButtonClicked(){
        cameraMoveFlg = true;
        switch(selectMode){
            case SelectMode.Start:
                currentScreen = UIScreen.NewGame;
                targetPos = NewGameSceneObj.transform.position;
                break;
            case SelectMode.Option:
                currentScreen = UIScreen.Options;
                targetPos = OptionSceneObj.transform.position;
                break;
            case SelectMode.Exit:
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #elif UNITY_STANDALONE
                    UnityEngine.Application.Quit();
                #endif
                break;
            case SelectMode.Special:
                if(bossDefeatFlg){
                    currentScreen = UIScreen.Special;
                    targetPos = SpecialSceneObj.transform.position;
                }
                break;
        }
        fromPos = menuCam.gameObject.transform.position;
        toPos = targetPos;
    }

    //ニューゲームモードで決定ボタンを押した
    private void NewGameButtonClicked(){
        NextScene();
        
    }

    //ステージ選択モードでステージを選んだ
    private void StageButtonClicked(){
        // if(cursorNum==0){
        //     SceneManager.LoadScene("Stage1");
        // }else if(cursorNum==1){
        //     SceneManager.LoadScene("Stage2");
        // }else if(cursorNum==2){
        //     SceneManager.LoadScene("Stage3-a");
        // }else if(cursorNum==3){
        //     SceneManager.LoadScene("Stage3-b");
        // }
    }

    //スペシャルモードでモードを選んだ
    private void SpecialButtonClicked(){

    }

    private void CancelButtonClicked(){

        if(currentScreen == UIScreen.Options){
            //言語を保存
            if(scoreManager==null){
                scoreManager = gameObject.AddComponent<ScoreManager>();
                scoreManager.saveLanguage(language);
                Debug.Log("sm.language:"+scoreManager.getLanguage());
                scoreManager=null;
            }
        }

        cameraMoveFlg = true;
        currentScreen = UIScreen.MainMenu;

        targetPos = CameraStartPos.transform.position;
        fromPos = menuCam.gameObject.transform.position;
        toPos = targetPos;
    }
}

public enum UIScreen{
    MainMenu
    ,NewGame
    ,LoadScene
    ,SelectScene
    ,Options
    ,Special
    ,Tutorial
    ,Exit
    // ,Stage1
    // ,Stage2
    // ,Stage3_a
    // ,Stage3_b
    // ,BadEnd
    // ,GoodEnd

}

public enum SelectMode{
    Start
    ,Option
    ,Exit
    ,Special
    ,Story
    ,StageSelect
}

//クリア評価
public enum Mode{
    Normal, //NewGame or LoadGame を選びクリア
    Bronze, //銅ランククリア
    Silver, //銀ランククリア
    Gold    //金ランククリア
}