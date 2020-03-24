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
        if(life==0) {
            isLife = false;
            gameObject.GetComponent<MoveEnemy>().TakeDamage();
            CountDownTimer cdt = countDownTimer.GetComponent<CountDownTimer>();
            cdt.addRegainToTime(10f);
            life -= damage;
        }
    }
}