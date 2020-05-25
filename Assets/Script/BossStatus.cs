using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSample;

public class BossStatus : MonoBehaviour, ParentStatus{
    private int maxLife = 80;
    private int life;
    private bool isLife;
    
    void Start(){
        life = maxLife;
        isLife = true;
    }

    void Update(){
        Debug.Log("BossHP : "+life);
        if(isLife && life<=0) {
            isLife = false;
            gameObject.GetComponent<MoveBoss>().changeStateExplode();
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
            life -= 35;
        }
    }
}