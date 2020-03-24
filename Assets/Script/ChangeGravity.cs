using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGravity : MonoBehaviour {
    [SerializeField]private Vector3 localGravity;
    private Rigidbody rBody;

    private GameObject player;
    PlayerController playerController;

    private void Start(){
        rBody = this.GetComponent<Rigidbody>();

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        
    }

    private void FixedUpdate(){
        if(playerController.getJumpFlg()){
            SetLocalGravity(); //重力をAddForceでかけるメソッドを呼ぶ。FixedUpdateが好ましい。
        }
    }

    private void SetLocalGravity(){
        rBody.AddForce (localGravity, ForceMode.Acceleration);
    }
}
