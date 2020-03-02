using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSample;

namespace Status { //外部スクリプトがこのスクリプトを特定できるようにする
    public class EnemyStatus : MonoBehaviour{
        private int maxLife = 2;
        private int life;
        private GameObject countDownTimer;

        private bool isLife;
        // Start is called before the first frame update
        void Start(){
            life = maxLife;
            isLife = true;
            countDownTimer = GameObject.Find("MessageUI/HUD/Timer");
            
        }

        // Update is called once per frame
        void Update(){

        }
        public void SetHp(int life) {

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
}