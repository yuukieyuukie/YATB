using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSample;

public class EnemyStatus : MonoBehaviour{
    private int maxLife = 2;
    private int life;
    private GameObject countDownTimer;

    private bool isLife = true;

    void Awake(){
        // Debug.Log(isLife);
    }

    void Start(){
        isLife = true;
        life = maxLife;
        countDownTimer = GameObject.Find("MessageUI/HUD/Timer");
    }


    void Update(){
        if(isLife && life<=0) {
            isLife = false;
            gameObject.GetComponent<MoveEnemy>().TakeDamage();
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
    
}