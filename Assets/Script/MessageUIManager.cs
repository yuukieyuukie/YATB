using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MessageUIManager : MonoBehaviour {

    private GameObject imageObtain, imageApproach, imageCrushed, imageDamage;
    private float timeObtain = 0f, timeApproach = 0f, timeCrushed = 0f, timeDamage = 0f;
    private float alfaObtain = 0f, alfaApproach = 0f, alfaCrushed = 0f, alfaDamage = 0f;
    private Color colorObtain, colorApproach, colorCrushed, colorDamage;

    private GameObject player;
    private int dribbleCount, oldDribbleCount;
    private GameObject dribbleGauge;
    private List<GameObject> dribbleGaugeLeft = new List<GameObject>();
    private List<GameObject> dribbleGaugeRight = new List<GameObject>();

    void Start(){
        imageObtain = GameObject.Find("ReactionPanel/ImageObtain");
        colorObtain = imageObtain.GetComponent<Image>().color;
        colorObtain.a = alfaObtain;
        imageObtain.GetComponent<Image>().color = colorObtain;
        imageApproach = GameObject.Find("ReactionPanel/ImageApproach");
        colorApproach = imageApproach.GetComponent<Image>().color;
        colorApproach.a = alfaApproach;
        imageApproach.GetComponent<Image>().color = colorApproach;
        imageCrushed = GameObject.Find("ReactionPanel/ImageCrushed");
        colorCrushed = imageCrushed.GetComponent<Image>().color;
        colorCrushed.a = alfaCrushed;
        imageCrushed.GetComponent<Image>().color = colorCrushed;
        imageDamage = GameObject.Find("ImageDamage");
        colorDamage = imageDamage.GetComponent<Image>().color;
        colorDamage.a = alfaDamage;
        imageDamage.GetComponent<Image>().color = colorDamage;

        player = GameObject.Find("Player");
        dribbleGauge = GameObject.Find("HUD/DribbleGauge");
        for(int i=0;i<5;i++){
            dribbleGaugeLeft.Add(dribbleGauge.transform.Find("Left"+(i+1)).gameObject);
            dribbleGaugeLeft[i].SetActive(false);
            dribbleGaugeRight.Add(dribbleGauge.transform.Find("Right"+(i+1)).gameObject);
            dribbleGaugeRight[i].SetActive(false);
        }
        
    }

    void Update(){
        // アイテムゲットを示すパネルを表示
        if(0f<timeObtain && timeObtain<2f){
            timeObtain += Time.deltaTime;
        }else if(timeObtain>=2f){
            if(alfaObtain<=0f){
                timeObtain = 0f;
            }
            colorObtain.a = alfaObtain;
            imageObtain.GetComponent<Image>().color = colorObtain;
            alfaObtain -= Time.deltaTime;
        }        
        // 敵の接近を示すパネルを表示
        if(0f<timeApproach && timeApproach<2f){
            timeApproach += Time.deltaTime;
        }else if(timeApproach>=2f){
            if(alfaApproach<=0f){
                timeApproach = 0f;
            }
            colorApproach.a = alfaApproach;
            imageApproach.GetComponent<Image>().color = colorApproach;
            alfaApproach -= Time.deltaTime;
        }
        // 衝突を示すパネルを表示
        if(0f<timeCrushed && timeCrushed<2f){
            timeCrushed += Time.deltaTime;
        }else if(timeCrushed>=2f){
            if(alfaCrushed<=0f){
                timeCrushed = 0f;
            }
            colorCrushed.a = alfaCrushed;
            imageCrushed.GetComponent<Image>().color = colorCrushed;
            alfaCrushed -= Time.deltaTime;
        }
        // ダメージを示す効果を表示
        if(0f<timeDamage && timeDamage<0.25f){
            timeDamage += Time.deltaTime;
            colorDamage.a = alfaDamage;
            imageDamage.GetComponent<Image>().color = colorDamage;
            alfaDamage += Time.deltaTime*0.5f;
        }else if(0.25f<=timeDamage && timeDamage<0.5f){
            timeDamage += Time.deltaTime;
            colorDamage.a = alfaDamage;
            imageDamage.GetComponent<Image>().color = colorDamage;
            alfaDamage -= Time.deltaTime*0.5f;
        }else if(timeDamage>=0.5f){
            timeDamage = 0;
        }
        
        oldDribbleCount = dribbleCount;
        dribbleCount = player.GetComponent<PlayerController>().getDribbleCount();
        if(oldDribbleCount < dribbleCount){ // ゲージが増えた
            dribbleGaugeLeft[dribbleCount-1].SetActive(true);
            dribbleGaugeRight[dribbleCount-1].SetActive(true);
        }else if(oldDribbleCount > dribbleCount){ // ゲージが減った
            dribbleGaugeLeft[oldDribbleCount-1].SetActive(false);
            dribbleGaugeRight[oldDribbleCount-1].SetActive(false);
        }


    }


    //プレイヤーが衝突したオブジェクト種類に対し処理を行う
    public void checkPlayerColType(PlayerColType colType){
        if(colType==PlayerColType.Pickup){
            timeObtain += Time.deltaTime;
            alfaObtain = 1f;
            colorObtain.a = alfaObtain;
            imageObtain.GetComponent<Image>().color = colorObtain;
        }else if(colType==PlayerColType.EnemyNear){
            timeApproach += Time.deltaTime;
            alfaApproach = 1f;
            colorApproach.a = alfaApproach;
            imageApproach.GetComponent<Image>().color = colorApproach;
        }else if(colType==PlayerColType.EnemyCol){
            timeCrushed += Time.deltaTime;
            alfaCrushed = 1f;
            colorCrushed.a = alfaCrushed;
            imageCrushed.GetComponent<Image>().color = colorCrushed;
            timeDamage += Time.deltaTime;
        }else if(colType==PlayerColType.SceneChange){
            
        }else{

        }
    }

}

public enum PlayerColType{
    Normal,
    Pickup,
    EnemyNear,
    EnemyCol,
    SceneChange
}