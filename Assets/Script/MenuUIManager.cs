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
    Vector3 targetPos;
    private float cameraMove = 0f;
    private bool cameraMoveFlg;


    private UIScreen currentScreen;

    private static int language;

    ScoreManager scoreManager = null;

    private GameObject nowIcon, nowLine,nowText;
    private GameObject startIcon, optionIcon, exitIcon;
    private GameObject startUnderline, optionUnderline, exitUnderline;
    private GameObject startText, optionText, exitText;
    private SelectMode selectMode;

    //　非同期動作で使用するAsyncOperation
	private AsyncOperation async;
	//　シーンロード中に表示するUI画面
	[SerializeField]
	private GameObject loadUI;
    private Color c;

	private void NextScene() {

		//　コルーチンを開始
		StartCoroutine("LoadData");
	}

	IEnumerator LoadData() {
		// シーンの読み込みをする
		async = SceneManager.LoadSceneAsync("Stage3-a");
        
		//　読み込みが終わるまで
		while(!async.isDone && c.a<1f){
            c.a = async.progress;
            
			yield return null;
		}
	}

    void Start(){
        
        currentScreen = UIScreen.MainMenu;

        targetPos = CameraStartPos.gameObject.transform.position;

        fromPos = menuCam.gameObject.transform.position;
        toPos = targetPos;

        startIcon = GameObject.Find("MainCanvas/StartMenu/StartIcon");
        optionIcon = GameObject.Find("MainCanvas/StartMenu/OptionIcon");
        exitIcon = GameObject.Find("MainCanvas/StartMenu/ExitIcon");
        startUnderline = GameObject.Find("MainCanvas/StartMenu/StartUnderline");
        optionUnderline = GameObject.Find("MainCanvas/StartMenu/OptionUnderline");
        exitUnderline = GameObject.Find("MainCanvas/StartMenu/ExitUnderline");
        startText = GameObject.Find("MainCanvas/StartMenu/StartText");
        optionText = GameObject.Find("MainCanvas/StartMenu/OptionText");
        exitText = GameObject.Find("MainCanvas/StartMenu/ExitText");
        nowIcon = startIcon;
        nowLine = startUnderline;
        nowText = startText;
        selectMode = SelectMode.Start;
    }

    void Update(){

        if(currentScreen != UIScreen.MainMenu && Input.GetButtonDown("Cancel")){
            Debug.Log("this.language:"+language);
            CancelButtonClicked();
        }

        if(currentScreen == UIScreen.MainMenu){
            if(Input.GetButtonDown("Down")){
                setSelectUI(exitIcon, exitUnderline, exitText);
                selectMode = SelectMode.Exit;
            }else if(Input.GetButtonDown("Up")){
                //special未実装
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
                language = 1; //英語
            }else if(Input.GetButtonDown("Left")){
                language = 0; //日本語
            }
            Debug.Log("choose:"+language);
        }else if(currentScreen == UIScreen.Special){
        }
    }

    Vector3 fromPos;
    Vector3 toPos;

    void FixedUpdate(){
        //画面遷移する項目選択時、カメラに移動量を与える
        if(cameraMoveFlg){
            cameraMove += 0.02f;
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

    //スタート画面でいずれかのモードを選択した
    private void StartButtonClicked(){
        cameraMoveFlg = true;
        switch(selectMode){
            case SelectMode.Start:
                currentScreen = UIScreen.NewGame;
                targetPos = NewGameSceneObj.transform.position;
                break;
            case SelectMode.Option:
                Vector3 pos = new Vector3(10.0f, 550.0f, 0.0f);
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
            }
        }

        cameraMoveFlg = true;
        currentScreen = UIScreen.MainMenu;
        Vector3 pos = new Vector3(-190.0f, 95.0f, 0.0f);

        targetPos = CameraStartPos.transform.position;
        fromPos = menuCam.gameObject.transform.position;
        toPos = targetPos;
    }

    //オブジェクトの子要素をすべて取得する
    // GameObject[] GetChildren(string parentName) {
    //     // 検索し、GameObject型に変換
    //     var parent = GameObject.Find(parentName) as GameObject;
    //     // 見つからなかったらreturn
    //     if(parent == null) return null;
    //     // 子のTransform[]を取り出す
    //     var transforms = parent.GetComponentsInChildren<Transform>();
    //     // 使いやすいようにtransformsからgameObjectを取り出す
    //     var gameObjects = from t in transforms select t.gameObject;
    //     // 配列に変換してreturn
    //     return gameObjects.ToArray();
    // }
    
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