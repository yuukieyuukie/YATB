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

    private GameObject countDownTimer; //Pickup取得時タイム加算

    private Message message;

    void Start(){
        SetCountText();
        SetSecretCountText();
        messageUI = GameObject.Find("MessageUI");
        countDownTimer = GameObject.Find("MessageUI/HUD/TimeBox");
        message = messageUI.GetComponent<Message>();
        
    }

    void Update(){

        
    }

    // ボールが他のオブジェクトにぶつかった時に呼び出される
    void OnTriggerEnter(Collider col){
        if (col.gameObject.CompareTag("PickUp")){ //アイテムに接触
            col.gameObject.SetActive(false);
            score = score + 1;
            SetCountText ();
            MessageUIManager muim = messageUI.GetComponent<MessageUIManager>();
            muim.checkPlayerColType(PlayerColType.Pickup);
            CountDownTimer cdt = countDownTimer.GetComponent<CountDownTimer>();
            cdt.addDamageToTime(-20f);
        }else if(col.gameObject.CompareTag("Dialogue")){
            col.gameObject.SetActive(false);
            message.setNextMessage();
        }else if(col.gameObject.CompareTag("Secret")){
            col.gameObject.SetActive(false);
            secretScore = secretScore + 1;
            SetSecretCountText ();
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
