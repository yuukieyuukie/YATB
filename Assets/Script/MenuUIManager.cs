//#define TMP_PROFILE_ON
//#define TMP_PROFILE_PHASES_ON


using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuUIManager : MonoBehaviour {

    public GameObject menuCam;
    public GameObject StartMenu;
    public GameObject LoadScene;
    public GameObject SelectScene;
    public GameObject OptionsScene;
    public GameObject SpecialScene;
    private Transform childSpecial_a;
    private Transform childSpecial_b;

    public GameObject StartSceneObj;
    public GameObject LoadSceneObj;
    public GameObject SelectSceneObj;
    public GameObject OptionSceneObj;
    public GameObject SpecialSceneObj;
    Vector3 targetPos = new Vector3(0,0,-400);
    private float cameraMove = 0f;
    private bool cameraMoveFlg;

    public GameObject cursor;
    private int cursorNum = 0;
    private Transform cursorTf;
    private Vector3 cursorPos;

    private UIScreen currentScreen;
    private UIScreen prevCurScreen;
    
    private int[] clearState = new int[4]{1,0,0,0}; //各ステージのクリアランクを保持

    void Start(){
        currentScreen = UIScreen.MainMenu;
        cursorTf = cursor.transform;
        cursorPos = cursorTf.localPosition;

        childSpecial_a = SpecialScene.transform.Find("Kirehashi");
        childSpecial_b = SpecialScene.transform.Find("Cheat");

        // StartMenu.SetActive(true);
        // LoadScene.SetActive(false);
        // SelectScene.SetActive(false);
        // OptionsScene.SetActive(false);
        // SpecialScene.SetActive(false);
    }

    void Update(){

        if(currentScreen != UIScreen.MainMenu && Input.GetButtonDown("Cancel")){
            CancelButtonClicked();
        }

        if(currentScreen == UIScreen.MainMenu){
            if(cursorNum < 4 && Input.GetButtonDown("Down")){
                cursorTf.Translate(0,-70,0);
                cursorPos = cursorTf.localPosition;
                cursorNum++;
            }else if(cursorNum > 0 && Input.GetButtonDown("Up")){
                cursorTf.Translate(0,70,0);
                cursorPos = cursorTf.localPosition;
                cursorNum--;
            }
            if(Input.GetButtonDown("Submit")){
                StartButtonClicked();
                cameraMoveFlg = true;
            }
        }else if(currentScreen == UIScreen.LoadScene){
            
        }else if(currentScreen == UIScreen.SelectScene){
            if(cursorNum < 3 && Input.GetButtonDown("Right")){
                cursorTf.Translate(150,0,0);
                cursorPos = cursorTf.localPosition;
                cursorNum++;
            }else if(cursorNum > 0 && Input.GetButtonDown("Left")){
                cursorTf.Translate(-150,0,0);
                cursorPos = cursorTf.localPosition;
                cursorNum--;
            }
            if(Input.GetButtonDown("Submit")){
                StageButtonClicked();
            }

        }else if(currentScreen == UIScreen.Options){

        }else if(currentScreen == UIScreen.Special){
            if(cursorNum < 1 && Input.GetButtonDown("Right")){
                cursorTf.Translate(150,0,0);
                cursorPos = cursorTf.localPosition;
                cursorNum++;
            }else if(cursorNum > 0 && Input.GetButtonDown("Left")){
                cursorTf.Translate(-150,0,0);
                cursorPos = cursorTf.localPosition;
                cursorNum--;
            }
            if(Input.GetButtonDown("Submit")){
                SpecialButtonClicked();
            }

        }else if(currentScreen == UIScreen.Special_a){
            childSpecial_a.gameObject.SetActive(true);
            childSpecial_b.gameObject.SetActive(false);
            
        }else if(currentScreen == UIScreen.Special_b){
            childSpecial_a.gameObject.SetActive(false);
            childSpecial_b.gameObject.SetActive(true);
        }

        if(cameraMoveFlg){
            cameraMove += 0.01f;
            if(cameraMove>=1f){
                cameraMoveFlg = false;
                cameraMove = 0f;
            }
        }

        menuCam.transform.position = Vector3.Lerp (
            menuCam.gameObject.transform.position,
            targetPos,
            cameraMove
        );

    }


    //スタート画面でいずれかのモードを選択した
    private void StartButtonClicked(){
        
        if(cursorNum==0){
            currentScreen = UIScreen.NewGame;
            //SceneManager.LoadScene("Prologue");
            targetPos = StartSceneObj.transform.position;
        }else if(cursorNum==1){
            currentScreen = UIScreen.LoadScene;
            // StartMenu.SetActive(false);
            // LoadScene.SetActive(true);
            // SelectScene.SetActive(false);
            // OptionsScene.SetActive(false);
            // SpecialScene.SetActive(false);
            targetPos = LoadSceneObj.transform.position;
        }else if(cursorNum==2){
            Vector3 pos = new Vector3(-200.0f, 90.0f, 0.0f);
            cursorTf.localPosition = pos;
            cursorNum = 0;
            currentScreen = UIScreen.SelectScene;
            // StartMenu.SetActive(false);
            // LoadScene.SetActive(false);
            // SelectScene.SetActive(true);
            // OptionsScene.SetActive(false);
            // SpecialScene.SetActive(false);
            targetPos = SelectSceneObj.transform.position;

        }else if(cursorNum==3){
            currentScreen = UIScreen.Options;
            // StartMenu.SetActive(false);
            // LoadScene.SetActive(false);
            // SelectScene.SetActive(false);
            // OptionsScene.SetActive(true);
            // SpecialScene.SetActive(false);
            targetPos = OptionSceneObj.transform.position;
        }else if(cursorNum==4){
            Vector3 pos = new Vector3(-120.0f, 40.0f, 0.0f);
            cursorTf.localPosition = pos;
            cursorNum = 0;
            currentScreen = UIScreen.Special;
            // StartMenu.SetActive(false);
            // LoadScene.SetActive(false);
            // SelectScene.SetActive(false);
            // OptionsScene.SetActive(false);
            // SpecialScene.SetActive(true);
            var uiObjects = GetChildren("Kirehashi");
            foreach(var uiObject in uiObjects) uiObject.SetActive(false);
            targetPos = SpecialSceneObj.transform.position;
        }


    }

    //ステージ選択モードでステージを選んだ
    private void StageButtonClicked(){
        if(cursorNum==0){
            SceneManager.LoadScene("Stage1");
        }else if(cursorNum==1){
            SceneManager.LoadScene("Stage2");
        }else if(cursorNum==2){
            SceneManager.LoadScene("Stage3-a");
        }else if(cursorNum==3){
            SceneManager.LoadScene("Stage3-b");
        }
    }

    //スペシャルモードでモードを選んだ
    private void SpecialButtonClicked(){
        if(cursorNum==0){
            currentScreen = UIScreen.Special_a;
            var uiObjects = GetChildren("Kirehashi");
            foreach(var uiObject in uiObjects) uiObject.SetActive(true);
        }else if(cursorNum==1){
            currentScreen = UIScreen.Special_b;
            SceneManager.LoadScene("Stage2");
        }
    }

    private void CancelButtonClicked(){
        currentScreen = UIScreen.MainMenu;
        Vector3 pos = new Vector3(-190.0f, 95.0f, 0.0f);
        cursorTf.localPosition = pos;
        cursorNum = 0;
        // StartMenu.SetActive(true);
        // LoadScene.SetActive(false);
        // SelectScene.SetActive(false);
        // OptionsScene.SetActive(false);
        // SpecialScene.SetActive(false);

        targetPos = new Vector3(0,0,-400);
    }

    //オブジェクトの子要素をすべて取得する
    GameObject[] GetChildren(string parentName) {
        // 検索し、GameObject型に変換
        var parent = GameObject.Find(parentName) as GameObject;
        // 見つからなかったらreturn
        if(parent == null) return null;
        // 子のTransform[]を取り出す
        var transforms = parent.GetComponentsInChildren<Transform>();
        // 使いやすいようにtransformsからgameObjectを取り出す
        var gameObjects = from t in transforms select t.gameObject;
        // 配列に変換してreturn
        return gameObjects.ToArray();
    }

}

public enum UIScreen{
    MainMenu
    ,NewGame
    ,LoadScene
    ,SelectScene
    ,Options
    ,Special
    ,Special_a //Kirehashi
    ,Special_b //Cheat
    ,Tutorial
    ,Stage1
    ,Stage2
    ,Stage3_a
    ,Stage3_b
    ,BadEnd
    ,GoodEnd

}

//クリア評価
public enum Mode{
    Normal, //NewGame or LoadGame を選びクリア
    Bronze, //銅ランククリア
    Silver, //銀ランククリア
    Gold    //金ランククリア
}