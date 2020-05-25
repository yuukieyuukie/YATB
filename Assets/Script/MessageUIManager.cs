using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    private GameObject lifeText;
    private Text lifeTextChild;
    private static float life = 60f;
    private float maxLife = 60f;
	//　HP表示用スライダー
    private RectTransform hpUI;
    private Slider hpSlider;
    private GameObject stageUIManager;
    private StageUIManager suim;

    private static bool secret1Flg, secret2Flg, secret3Flg;
    private GameObject secret1, secret2, secret3;
    private GameObject gem1, gem2, gem3;
    
    void OnActiveSceneChanged( Scene prevScene, Scene nextScene ){
        //Menuから遷移した時の初期化
		if(nextScene.name=="Menu"){
            life = 60f;
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

        player = GameObject.Find("Player");
        dribbleGauge = GameObject.Find("HUD/DribbleGauge");
        for(int i=0;i<5;i++){
            dribbleGaugeLeft.Add(dribbleGauge.transform.Find("Left"+(i+1)).gameObject);
            dribbleGaugeLeft[i].SetActive(false);
            dribbleGaugeRight.Add(dribbleGauge.transform.Find("Right"+(i+1)).gameObject);
            dribbleGaugeRight[i].SetActive(false);
        }
        
        lifeText = GameObject.Find("HUD/LifeText");
        lifeTextChild = lifeText.GetComponentInChildren<Text>();
        hpUI = GameObject.Find("HUD/HPUIME").GetComponent<RectTransform>(); //GameObjectから親要素を取得
        hpSlider = hpUI.transform.Find("HPBar").GetComponent<Slider>(); //transformで子要素を取得
		
		//　スライダーの値0～1の間になるように比率を計算
		UpdateHPValue();
        stageUIManager = GameObject.Find("UIManager");
        suim = stageUIManager.GetComponent<StageUIManager>();
        //Briefingから遷移した時の初期化
        if(suim.getCurrentScreen()==StageUIScreen.Briefing){
            life = 60f;
        }

        secret1 = GameObject.Find("HUD/Secret/Secret1");
        secret2 = GameObject.Find("HUD/Secret/Secret2");
        secret3 = GameObject.Find("HUD/Secret/Secret3");
        gem1 = GameObject.Find("gem1");
        gem2 = GameObject.Find("gem2");
        gem3 = GameObject.Find("gem3");
        

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

		UpdateHPValue();

        if(SceneManager.GetActiveScene().name=="Stage3-a" || SceneManager.GetActiveScene().name=="Stage3-a2"){
            if(!secret1.activeSelf && getSecret1Flg()){
                Debug.Log("gem1get:true");
                secret1.SetActive(true);
                gem1.SetActive(false);
            }
            if(!secret2.activeSelf && getSecret2Flg()){
                Debug.Log("gem2get:true");
                secret2.SetActive(true);
                gem2.SetActive(false);
            }
            if(!secret3.activeSelf && getSecret3Flg()){
                Debug.Log("gem3get:true");
                secret3.SetActive(true);
                gem3.SetActive(false);
            }
        }
    }

 
	private void UpdateHPValue() {
        lifeTextChild.text = life.ToString("00");
		hpSlider.value = life / maxLife;
	}

    public bool isLifeZero(){
        return life <= 0 ? true : false;
    }

    public void initSecretFlg(){
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
    public bool getSecret1Flg(){
        return secret1Flg;
    }
    public bool getSecret2Flg(){
        return secret2Flg;
    }
    public bool getSecret3Flg(){
        return secret3Flg;
    }

    //プレイヤーが衝突したオブジェクト種類に対し処理を行う
    public void checkPlayerColType(PlayerColType colType){
        if(colType==PlayerColType.Pickup){
            timeObtain += Time.deltaTime;
            alfaObtain = 1f;
            colorObtain.a = alfaObtain;
            colorObtain = new Color(1.0f, 1.0f, 1.0f, colorObtain.a);
            imageObtain.GetComponent<Image>().color = colorObtain;
            if(life<maxLife) life += 10;
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
            life -= 20;
        }else if(colType==PlayerColType.Secret){
            timeObtain += Time.deltaTime;
            alfaObtain = 1f;
            colorObtain.a = alfaObtain;
            colorObtain = new Color(0.0f, 1.0f, 0.0f, colorObtain.a);
            imageObtain.GetComponent<Image>().color = colorObtain;
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