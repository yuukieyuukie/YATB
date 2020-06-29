using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSample;

public class EnemyStatus : MonoBehaviour, ParentStatus{
    private float maxLife = 2;
    private float life;
    //private GameObject countDownTimer;
    private float shotDamage = 1f;

    private bool isLife = true;

    void Awake(){
        // Debug.Log(isLife);
    }

    void Start(){
        isLife = true;
        life = maxLife;
        //countDownTimer = GameObject.Find("MessageUI/HUD/Timer");
    }


    void Update(){
        if(isLife && life<=0) {
            isLife = false;
            gameObject.GetComponent<MoveEnemy>().changeStateExplode();
        }
    }

    public float GetHp() {
        return life;
    }

    public float GetMaxHp() {
        return maxLife;
    }

    public bool isLifeZero(){
        return isLife;
    }

    public void TakeDamage(int damage){
        //life -= damage;
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.CompareTag("Shot")){
            life -= shotDamage;
        }
    }
    
}