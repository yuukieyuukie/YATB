using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchItem : MonoBehaviour{

    private GameObject messageUI;
    private MessageUIManager muim;

    private Message message;

    private GameObject player;
    private PlayerController playerController;

    void Start(){
        messageUI = GameObject.Find("MessageUI");
        muim = messageUI.GetComponent<MessageUIManager>();
        message = messageUI.GetComponent<Message>();
        
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        
    }

    void Update(){

        
    }

    // ボールが他のオブジェクトにぶつかった時に呼び出される
    void OnTriggerEnter(Collider col){
        if (col.gameObject.CompareTag("PickUp")){ //アイテムに接触
            col.gameObject.SetActive(false);
            muim.checkPlayerColType(PlayerColType.Pickup);
            playerController.setSparkFlg(false);
        }else if(col.gameObject.CompareTag("Dialogue")){
            col.gameObject.SetActive(false);
            message.setNextMessage();
        }else if(col.gameObject.CompareTag("Secret")){
            col.gameObject.SetActive(false);
            muim.checkPlayerColType(PlayerColType.Secret);
            muim.setSecretFlg(true, col.gameObject.name);
            muim.setSecretActive();
        }
    }

}
