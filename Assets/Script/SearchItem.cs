using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchItem : MonoBehaviour{

    public Text scoreText; // スコアの UI
    static private int score = 0; // スコア    
    public Text secretText;
    static private int secretScore = 0;
    private GameObject messageUI;
    private MessageUIManager muim;

    // private GameObject stageUI;
    // private StageUIManager suim;

    private GameObject countDownTimer; //Pickup取得時タイム加算

    private Message message;

    private GameObject player;
    private PlayerController playerController;

    void Start(){
        SetCountText();
        SetSecretCountText();
        messageUI = GameObject.Find("MessageUI");
        muim = messageUI.GetComponent<MessageUIManager>();
        // stageUI = GameObject.Find("UIManager");
        // suim = stageUI.GetComponent<StageUIManager>();
        message = messageUI.GetComponent<Message>();
        countDownTimer = GameObject.Find("MessageUI/HUD/TimeBox");
        
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        
    }

    void Update(){

        
    }

    // ボールが他のオブジェクトにぶつかった時に呼び出される
    void OnTriggerEnter(Collider col){
        if (col.gameObject.CompareTag("PickUp")){ //アイテムに接触
            col.gameObject.SetActive(false);
            score = score + 1;
            SetCountText ();
            muim.checkPlayerColType(PlayerColType.Pickup);
            playerController.setSparkFlg(false);
        }else if(col.gameObject.CompareTag("Dialogue")){
            col.gameObject.SetActive(false);
            message.setNextMessage();
        }else if(col.gameObject.CompareTag("Secret")){
            col.gameObject.SetActive(false);
            secretScore = secretScore + 1;
            muim.checkPlayerColType(PlayerColType.Secret);
            SetSecretCountText ();
            muim.setSecretFlg(true, col.gameObject.name);
        }
    }

    // UI の表示を更新する
    private void SetCountText(){
        scoreText.text = "Count: " + score.ToString();
    }

    private void SetSecretCountText(){
        secretText.text = "SecretCount: " + secretScore.ToString();
    }
}
