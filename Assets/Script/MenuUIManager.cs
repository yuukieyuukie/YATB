//#define TMP_PROFILE_ON
//#define TMP_PROFILE_PHASES_ON


using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuUIManager : MonoBehaviour {

    public GameObject mainCamera;
    public GameObject StartScene, LoadScene, SelectScene, OptionsScene, SpecialScene;
    public GameObject NewGameSceneObj, LoadSceneObj, SelectSceneObj, OptionSceneObj, SpecialSceneObj;
    public GameObject CameraStartPos;
    private Vector3 targetPos, fromPos, toPos; //カメラの移動始点・終点
    private float cameraMove = 0f;
    private bool cameraMoveFlg;

    private UIScreen currentScreen;

    private static int language;

    private ScoreManager scoreManager = null;

    private MenuMode menuMode;
    private GameObject nowIcon, nowLine, nowText;

    //StartScene
    private GameObject startIcon, optionIcon, exitIcon, specialIcon;
    private GameObject startUnderline, optionUnderline, exitUnderline, specialUnderline;
    private GameObject startText, optionText, exitText, specialText;
    
    //OptionsScene
    private  GameObject jpIcon, enIcon;

    //SpecialScene
    private GameObject page1Icon, page2Icon, page3Icon, page4Icon;
    private GameObject page1Underline, page2Underline, page3Underline, page4Underline;
    private GameObject page1Text, page2Text, page3Text, page4Text;
    private GameObject panel1;

    //NewGameScene
    private GameObject storyIcon, selectIcon;
    private GameObject storyUnderline, selectUnderline;
    private GameObject storyText, selectText, storyText2, selectText2;

    //　非同期動作で使用するAsyncOperation
	private AsyncOperation async;
	//　シーンロード中に表示するUI画面
	[SerializeField]
	private GameObject loadUI;
    private Color colorLoadUI;
    private float alfaLoadUI = 0f;

    [SerializeField]
    private AudioClip selectAudio;
    private AudioSource selectAudioSource;
    [SerializeField]
    private AudioClip cancelAudio;
    private AudioSource cancelAudioSource;

    void Start(){
        selectAudioSource = GetComponent<AudioSource>();
        cancelAudioSource = GetComponent<AudioSource>();
        
        currentScreen = UIScreen.MainMenu;

        targetPos = CameraStartPos.gameObject.transform.position;
        fromPos = mainCamera.gameObject.transform.position;
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

        page1Icon = GameObject.Find("MainCanvas/SpecialScene/Page1Icon");
        page1Underline = GameObject.Find("MainCanvas/SpecialScene/Page1Underline");
        page1Text = GameObject.Find("MainCanvas/SpecialScene/Page1Text");
        page2Icon = GameObject.Find("MainCanvas/SpecialScene/Page2Icon");
        page2Underline = GameObject.Find("MainCanvas/SpecialScene/Page2Underline");
        page2Text = GameObject.Find("MainCanvas/SpecialScene/Page2Text");
        page3Icon = GameObject.Find("MainCanvas/SpecialScene/Page3Icon");
        page3Underline = GameObject.Find("MainCanvas/SpecialScene/Page3Underline");
        page3Text = GameObject.Find("MainCanvas/SpecialScene/Page3Text");
        page4Icon = GameObject.Find("MainCanvas/SpecialScene/Page4Icon");
        page4Underline = GameObject.Find("MainCanvas/SpecialScene/Page4Underline");
        page4Text = GameObject.Find("MainCanvas/SpecialScene/Page4Text");
        panel1 = GameObject.Find("MainCanvas/SpecialScene/Panel1");

        storyIcon = GameObject.Find("MainCanvas/NewGameScene/StoryIcon");
        selectIcon = GameObject.Find("MainCanvas/NewGameScene/SelectIcon");
        storyUnderline = GameObject.Find("MainCanvas/NewGameScene/StoryUnderline");
        selectUnderline = GameObject.Find("MainCanvas/NewGameScene/SelectUnderline");
        storyText = GameObject.Find("MainCanvas/NewGameScene/StoryText");
        selectText = GameObject.Find("MainCanvas/NewGameScene/SelectText");
        storyText2 = GameObject.Find("MainCanvas/NewGameScene/StoryText2");
        selectText2 = GameObject.Find("MainCanvas/NewGameScene/SelectText2");

        jpIcon = GameObject.Find("MainCanvas/OptionsScene/JPIcon");
        enIcon = GameObject.Find("MainCanvas/OptionsScene/ENIcon");

        nowIcon = startIcon;
        nowLine = startUnderline;
        nowText = startText;
        menuMode = MenuMode.Start;

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

    private float moveHorizontal3;
    private float moveVertical3;
    private bool pushL, pushR, pushU, pushD;
    
    void Update(){
        moveHorizontal3 = Input.GetAxisRaw("Axis 7"); //十字キー横, adキー
        moveVertical3 = Input.GetAxisRaw("Axis 8"); //十字キー縦, wsキー
        // Debug.Log(moveHorizontal3+"  "+moveVertical3);
        
        if(moveHorizontal3==0f){
            pushL = true;
            pushR = true;
        }
        if(moveVertical3==0f){
            pushU = true;
            pushD = true;
        }

        moveCamera();

        if(currentScreen != UIScreen.MainMenu && Input.GetButtonDown("Cancel")){
            pushCancelButton();
        }

        if(currentScreen == UIScreen.MainMenu){
            if(pushD && moveVertical3<0f){
                pushD = false;
                setActiveIcon(exitIcon, exitUnderline, exitText, MenuMode.Exit);
            }else if(pushU && moveVertical3>0f){
                pushU = false;
                setActiveIcon(specialIcon, specialUnderline, specialText, MenuMode.Special);
            }else if(pushR && moveHorizontal3>0f){
                pushR = false;
                setActiveIcon(optionIcon, optionUnderline, optionText, MenuMode.Option);
            }else if(pushL && moveHorizontal3<0f){
                pushL = false;
                setActiveIcon(startIcon, startUnderline, startText, MenuMode.Start);
            }
            if(Input.GetButtonDown("Submit")){
                selectOnStartScene();
            }
        }else if(currentScreen == UIScreen.NewGame){
            if(pushD && moveVertical3<0f){
                pushD = false;
                setActiveIcon(selectIcon, selectUnderline, selectText, MenuMode.StageSelect);
                storyText2.SetActive(false);
                selectText2.SetActive(true);
            }else if(pushU && moveVertical3>0f){
                pushU = false;
                setActiveIcon(storyIcon, storyUnderline, storyText, MenuMode.Story);
                storyText2.SetActive(true);
                selectText2.SetActive(false);
                }
            if(Input.GetButtonDown("Submit")){
                selectOnNewGameScene();
            }
        }else if(currentScreen == UIScreen.LoadScene){
        }else if(currentScreen == UIScreen.SelectScene){
        }else if(currentScreen == UIScreen.Options){
            if(pushR && moveHorizontal3>0f){
                pushR = false;
                jpIcon.SetActive(false);
                enIcon.SetActive(true);
                language = 1; //英語
            }else if(pushL && moveHorizontal3<0f){
                pushL = false;
                jpIcon.SetActive(true);
                enIcon.SetActive(false);
                language = 0; //日本語
            }
        }else if(currentScreen == UIScreen.Special){
            if(pushD && moveVertical3<0f){
                pushD = false;
                setActiveIcon(page3Icon, page3Underline, page3Text, MenuMode.Page3);
            }else if(pushU && moveVertical3>0f){
                pushU = false;
                setActiveIcon(page2Icon, page2Underline, page2Text, MenuMode.Page2);
            }else if(pushR && moveHorizontal3>0f){
                pushR = false;
                setActiveIcon(page4Icon, page4Underline, page4Text, MenuMode.Page4);
            }else if(pushL && moveHorizontal3<0f){
                pushL = false;
                setActiveIcon(page1Icon, page1Underline, page1Text, MenuMode.Page1);
            }
            if(Input.GetButtonDown("Submit")){
                selectOnSpecialScene();
            }
        }
    }

    //選んだアイコンを活性化、選んでいたアイコンを非活性化
    private void setActiveIcon(GameObject icon, GameObject line, GameObject text, MenuMode mode){
        nowIcon.SetActive(false);
        nowLine.SetActive(false);
        nowText.SetActive(false);
        nowIcon = icon;
        nowLine = line;
        nowText = text;
        nowIcon.SetActive(true);
        nowLine.SetActive(true);
        nowText.SetActive(true);
        menuMode = mode;
    }

    private void moveCamera(){
        //画面遷移する項目選択時、カメラに移動量を与える
        if(cameraMoveFlg){
            cameraMove += Time.deltaTime;
            //項目選択時の画面遷移を実施
            mainCamera.transform.position = Vector3.Lerp (
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

    //スタート画面でいずれかのモードを選択した
    private void selectOnStartScene(){
        selectAudioSource.PlayOneShot(selectAudio);
        switch(menuMode){
            case MenuMode.Start:
                currentScreen = UIScreen.NewGame;
                targetPos = NewGameSceneObj.transform.position;
                setActiveIcon(storyIcon, storyUnderline, storyText, MenuMode.Story);
                break;
            case MenuMode.Option:
                currentScreen = UIScreen.Options;
                targetPos = OptionSceneObj.transform.position;
                break;
            case MenuMode.Exit:
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #elif UNITY_STANDALONE
                    UnityEngine.Application.Quit();
                #endif
                break;
            case MenuMode.Special:
                currentScreen = UIScreen.Special;
                targetPos = SpecialSceneObj.transform.position;
                setActiveIcon(page1Icon, page1Underline, page1Text, MenuMode.Special);
                break;
        }
        fromPos = mainCamera.gameObject.transform.position;
        toPos = targetPos;
        cameraMoveFlg = true;
    }

    //ニューゲームモードで決定ボタンを押した
    private void selectOnNewGameScene(){
        switch(menuMode){
            case MenuMode.Story:
                NextScene();
                break;
            case MenuMode.StageSelect:
                //未実装
                break;
        }
    }

    //スペシャルモードでモードを選んだ
    private void selectOnSpecialScene(){
        if(page1Icon.activeSelf){
            panel1.SetActive(true);
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

    private void pushCancelButton(){
        cancelAudioSource.PlayOneShot(cancelAudio);

        if(menuMode == MenuMode.Page1 || menuMode == MenuMode.Page2 || menuMode == MenuMode.Page3 || menuMode == MenuMode.Page4){

        }else{
            
        }
        setActiveIcon(startIcon, startUnderline, startText, MenuMode.Start);

        if(currentScreen == UIScreen.Options){
            //言語を保存
            if(scoreManager==null){
                scoreManager = gameObject.AddComponent<ScoreManager>();
                Debug.Log("Cancel:"+scoreManager);
                scoreManager.saveLanguage(language);
                Debug.Log("sm.language:"+scoreManager.getLanguage());
                scoreManager = null;
            }
        }else if(currentScreen == UIScreen.Special){
            if(menuMode==MenuMode.Page1)
                panel1.SetActive(false);
        }

        currentScreen = UIScreen.MainMenu;

        

        targetPos = CameraStartPos.transform.position;
        fromPos = mainCamera.gameObject.transform.position;
        toPos = targetPos;
        cameraMoveFlg = true;
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

}

public enum MenuMode{
    Start
    ,Option
    ,Exit
    ,Special
    ,Story
    ,StageSelect
    ,Page1
    ,Page2
    ,Page3
    ,Page4
}
