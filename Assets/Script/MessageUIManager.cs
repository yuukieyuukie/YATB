using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageUIManager : MonoBehaviour {

    private GameObject pickupPanel;
    private float pickupTime = 0f; //pickupPanel表示用
    private GameObject enemyNearPanel;
    private float enemyNearTime = 0f; //enemyCollisionPanel表示用
    private GameObject enemyCollisionPanel;
    private float enemyColTime = 0f; //enemyCollisionPanel表示用
    private GameObject damagePanel;
    private float damageTime = 0f; //damagePanel表示用
    public float speed = 0.5f;  //透明化の速さ
    float alfa;    //A値を操作するための変数
    float red, green, blue;    //RGBを操作するための変数

    void Start(){
        pickupPanel = GameObject.Find("PickupPanel");
        enemyNearPanel = GameObject.Find("EnemyNearPanel");
        enemyCollisionPanel = GameObject.Find("EnemyCollisionPanel");
        damagePanel = GameObject.Find("DamagePanel");

        pickupPanel.SetActive(false);
        enemyNearPanel.SetActive(false);
        enemyCollisionPanel.SetActive(false);
        damagePanel.SetActive(false);

        red = damagePanel.GetComponent<Image>().color.r;
        green = damagePanel.GetComponent<Image>().color.g;
        blue = damagePanel.GetComponent<Image>().color.b;
    }

    void Update(){
        // アイテムゲットを示すパネルを表示
        if(0f<pickupTime && pickupTime<2f){
            pickupTime += Time.deltaTime;
            pickupPanel.gameObject.SetActive(true);
        }else if(pickupTime>=2f){
            pickupTime = 0;
            pickupPanel.gameObject.SetActive(false);
        }        
        // 敵の接近を示すパネルを表示
        if(0f<enemyNearTime && enemyNearTime<2f){
            enemyNearTime += Time.deltaTime;
            enemyNearPanel.SetActive(true);
        }else if(enemyNearTime>=2f){
            enemyNearTime = 0;
            enemyNearPanel.SetActive(false);
        }
        // ダメージを示すパネルを表示
        if(0f<enemyColTime && enemyColTime<2f){
            enemyColTime += Time.deltaTime;
            enemyCollisionPanel.SetActive(true);
        }else if(enemyColTime>=2f){
            enemyColTime = 0;
            enemyCollisionPanel.SetActive(false);
        }

        if(0f<damageTime && damageTime<0.25f){
            damageTime += Time.deltaTime;
            damagePanel.SetActive(true);
            damagePanel.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            alfa += speed;
        }else if(0.25f<=damageTime && damageTime<0.5f){
            damageTime += Time.deltaTime;
            damagePanel.SetActive(true);
            damagePanel.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            alfa -= speed;
        }else if(damageTime>=0.5f){
            damageTime = 0;
            damagePanel.SetActive(false);
        }
        



    }


    //プレイヤーが衝突したオブジェクト種類に対し処理を行う
    public void checkPlayerColType(PlayerColType colType){
        if(colType==PlayerColType.Pickup){
            pickupTime += Time.deltaTime;
        }else if(colType==PlayerColType.EnemyNear){
            enemyNearTime += Time.deltaTime;
        }else if(colType==PlayerColType.EnemyCol){
            enemyColTime += Time.deltaTime;
            damageTime += Time.deltaTime;
        }else{

        }
    }

}

public enum PlayerColType{
    Normal,
    Pickup,
    EnemyNear,
    EnemyCol
}