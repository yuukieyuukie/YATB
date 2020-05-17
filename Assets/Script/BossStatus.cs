using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSample;

public class BossStatus : MonoBehaviour, ParentStatus{
    private int maxLife = 10;
    private int life;
    private GameObject countDownTimer;

    private bool isLife;
    
    void Start(){
        life = maxLife;
        isLife = true;
        countDownTimer = GameObject.Find("MessageUI/HUD/Timer");
    }

    void Update(){
        Debug.Log("BossHP : "+life);
        if(isLife && life<=0) {
            isLife = false;
            gameObject.GetComponent<MoveBoss>().TakeDamage();
        }
    }

    public int GetHp() {
        return life;
    }

    public int GetMaxHp() {
        return maxLife;
    }

    public bool isLifeZero(){
        return isLife;
    }

    public void TakeDamage(int damage){
        life -= damage;

    }

    //タグ付オブジェクトに触れたときの処理
    void OnCollisionEnter(Collision col){
        if(col.gameObject.CompareTag("BossTrap")){
            life -= 40;
        }
    }
}