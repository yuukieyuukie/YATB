using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSample;

public class BossStatus : MonoBehaviour, ParentStatus{
    private float maxLife = 60;
    private float life;
    private bool isLife;
    private float shotDamage = 1f, trapDamage = 25f;

    private GameObject bossText, hpuiBoss;
    //private GameObject hud;
    
    void Start(){
        life = maxLife;
        isLife = true;
    }

    void Update(){
        //Debug.Log("BossHP : "+life);
        if(isLife && life<=0) {
            isLife = false;
            gameObject.GetComponent<MoveBoss>().changeStateExplode();
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

    public float getShotDamage(){
        return shotDamage;
    }
    public float getTrapDamage(){
        return trapDamage;
    }

    //タグ付オブジェクトに触れたときの処理
    void OnCollisionEnter(Collision col){
        if(col.gameObject.CompareTag("Shot")){
            life -= shotDamage;
        }
        else if(col.gameObject.CompareTag("BossTrap")){
            life -= trapDamage;
        }
    }
}