using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MessageUIManager : MonoBehaviour {

    private GameObject imageObtain, imageApproach, imageCrushed, imageDamage, imageSecret;
    private float timeObtain = 0f, timeApproach = 0f, timeCrushed = 0f, timeDamage = 0f, timeSecret = 0f;
    private float alfaObtain = 0f, alfaApproach = 0f, alfaCrushed = 0f, alfaDamage = 0f, alfaSecret = 0f;
    private Color colorObtain, colorApproach, colorCrushed, colorDamage, colorSecret;

    private GameObject player;
    private int dribbleCount, oldDribbleCount;
    private GameObject dribbleGauge;
    private List<GameObject> dribbleGaugeLeft = new List<GameObject>();

    private static float life = 300f;
    private float maxLife = 300f;
    private float damage = 100f;
    private float regain = -30f;

    private RectTransform healthGauge, bossHealthGauge;
    private Image gauge, bossGauge;
    [SerializeField] private GameObject bossHealthGaugeObj, bossHealthImage;
    private GameObject stageUIManager;
    private StageUIManager suim;

    private static bool secret1Flg, secret2Flg, secret3Flg;
    [SerializeField] private GameObject secret1, secret2, secret3;
    private GameObject gem1, gem2, gem3;
    
    void OnActiveSceneChanged( Scene prevScene, Scene nextScene ){
        //Menuから遷移した時の初期化
		if(nextScene.name=="Menu"){
            life = maxLife;
			initSecretFlg();
		}

    }

    void Start(){
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
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
        imageSecret = GameObject.Find("ReactionPanel/ImageSecret");
        colorSecret = imageSecret.GetComponent<Image>().color;
        colorSecret.a = alfaSecret;
        imageSecret.GetComponent<Image>().color = colorSecret;

        player = GameObject.Find("Player");
        dribbleGauge = GameObject.Find("DribbleGauge");
        for(int i=0;i<5;i++){
            dribbleGaugeLeft.Add(dribbleGauge.transform.Find("Left"+(i+1)).gameObject);
            dribbleGaugeLeft[i].SetActive(false);
        }
        
        healthGauge = GameObject.Find("HealthGauge").GetComponent<RectTransform>(); //GameObjectから親要素を取得
        gauge = healthGauge.transform.Find("Gauge").GetComponent<Image>(); //transformで子要素を取得



        if(SceneManager.GetActiveScene().name=="Stage3-b"){
            bossHealthImage.SetActive(true);
            bossHealthGaugeObj.SetActive(true);
            // 3bのみ使用
            // bossHealthGauge = GameObject.Find("BossHealthGauge").GetComponent<RectTransform>();
            // bossGauge = bossHealthGauge.transform.Find("Gauge").GetComponent<Image>();
        }else{
            bossHealthImage.SetActive(false);
            bossHealthGaugeObj.SetActive(false);
        }
		
        stageUIManager = GameObject.Find("UIManager");
        suim = stageUIManager.GetComponent<StageUIManager>();

        gem1 = GameObject.Find("gem1");
        gem2 = GameObject.Find("gem2");
        gem3 = GameObject.Find("gem3");

        //Briefingから遷移した時の初期化
        if(suim.getCurrentScreen()==StageUIScreen.Briefing){
            setSecretActive();
            life = maxLife;
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
        if(0f<timeSecret && timeSecret<2f){
            timeSecret += Time.deltaTime;
        }else if(timeSecret>=2f){
            if(alfaSecret<=0f){
                timeSecret = 0f;
            }
            colorSecret.a = alfaSecret;
            imageSecret.GetComponent<Image>().color = colorSecret;
            alfaSecret -= Time.deltaTime;
        }        
        
        oldDribbleCount = dribbleCount;
        dribbleCount = player.GetComponent<PlayerController>().getDribbleCount();
        if(oldDribbleCount < dribbleCount){ // ゲージが増えた
            dribbleGaugeLeft[dribbleCount-1].SetActive(true);
        }else if(oldDribbleCount > dribbleCount){ // ゲージが減った
            dribbleGaugeLeft[oldDribbleCount-1].SetActive(false);
        }

    }

 
	private void UpdateHPValue(float zogen) {
        Vector3 work = gauge.transform.localPosition;
        work.x -= zogen;
		gauge.transform.localPosition = work;
	}

    public bool isLifeZero(){
        return life <= 0 ? true : false;
    }

    private void initSecretFlg(){
        secret1Flg = secret2Flg = secret3Flg = false;
    }

    public void setSecretFlg(bool a, string gemStr){
        if(gemStr=="gem1"){
            secret1Flg = a;
        }else if(gemStr=="gem2"){
            secret2Flg = a;
        }else if(gemStr=="gem3"){
            secret3Flg = a;
        }
    }
    private bool getSecret1Flg(){
        return secret1Flg;
    }
    private bool getSecret2Flg(){
        return secret2Flg;
    }
    private bool getSecret3Flg(){
        return secret3Flg;
    }

    public void setSecretActive(){
        if(!secret1.activeSelf && getSecret1Flg()){
            secret1.SetActive(true);
            gem1.SetActive(false);
        }
        if(!secret2.activeSelf && getSecret2Flg()){
            secret2.SetActive(true);
            gem2.SetActive(false);
        }
        if(!secret3.activeSelf && getSecret3Flg()){
            secret3.SetActive(true);
            gem3.SetActive(false);
        }
    }

    //プレイヤーが衝突したオブジェクト種類に対し処理を行う
    public void checkPlayerColType(PlayerColType colType){
        if(colType==PlayerColType.Pickup){
            timeObtain += Time.deltaTime;
            alfaObtain = 1f;
            colorObtain.a = alfaObtain;
            colorObtain = new Color(1.0f, 1.0f, 1.0f, colorObtain.a);
            imageObtain.GetComponent<Image>().color = colorObtain;
            if(life<maxLife){
                life -= regain;
                UpdateHPValue(regain);
            }
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
            life -= damage;
            UpdateHPValue(damage);
        }else if(colType==PlayerColType.Secret){
            timeSecret += Time.deltaTime;
            alfaSecret = 1f;
            colorSecret.a = alfaSecret;
            imageSecret.GetComponent<Image>().color = colorSecret;
        }
    }

}

public enum PlayerColType{
    Normal,
    Pickup,
    EnemyNear,
    EnemyCol,
    SceneChange,
    Secret
}