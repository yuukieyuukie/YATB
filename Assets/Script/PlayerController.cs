using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Effekseer;

public class PlayerController : MonoBehaviour{
    public float speed; // 動く速さ
    public float jumpPower;
    private bool jumpFlg;
    private Rigidbody rb; // Rididbody
    public GameObject shotObject;
    public GameObject shotObject2;
    public GameObject muzzle;
    public GameObject muzzleFlash;
    public GameObject muzzleFlashPrefab;
    [SerializeField]
    private Vector3 muzzleFlashScale;
    [SerializeField]
    private GameObject impulse;
    [SerializeField]
    private AudioClip sound1;
    private AudioSource audioSource;

    private int state; //状態（生存、死亡、ゴール 後々enum？）
    private bool untouchable;
    private int untouchableTime;
    private int dribble;
    private int maxDribble = 5;
    [SerializeField]
    private float dribbleBoost;
    private int dribbleTime;
    private bool shot1Way, shotAllWay;
    private int shot1WayTime, shotAllWayTime;

    //敵の接近数による見た目変化
    [SerializeField]
    private Material    m_defaultMaterial   = null;
    [SerializeField]
    private Material    m_foundMaterial     = null;
    [SerializeField]
    private Material    m_manyFoundMaterial     = null;

    private Renderer            m_renderer  = null;
    private List<GameObject>    m_targets   = new List<GameObject>();

    private GameObject sceneChanger; //他オブジェクトのコンポーネントを取り込む
    private GameObject countDownTimer;
    private GameObject hud;
    private GameObject dialoguePanel;
    private GameObject pickupPanel;
    private GameObject enemyNearPanel;
    private GameObject enemyCollisionPanel;
    private GameObject messageUI;
    
    void Start(){
        // Rigidbody を取得
        rb = GetComponent<Rigidbody>();
        sceneChanger = GameObject.Find("UIManager");
        countDownTimer = GameObject.Find("MessageUI/HUD/Timer");
        hud = GameObject.Find("MessageUI/HUD");
        dialoguePanel = GameObject.Find("MessageUI/DialoguePanel");
        pickupPanel = GameObject.Find("MessageUI/PickupPanel");
        enemyNearPanel = GameObject.Find("MessageUI/EnemyNearPanel");
        enemyCollisionPanel = GameObject.Find("MessageUI/EnemyCollisionPanel");
        messageUI = GameObject.Find("MessageUI");
        muzzleFlash.SetActive(true);
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update(){
        //Pause中の入力を受け付けない
        if (Mathf.Approximately(Time.timeScale, 0f)) {
            return;
        }
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;// カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 moveForward = cameraForward * moveVertical + Camera.main.transform.right * moveHorizontal;// 方向キーの入力値とカメラの向きから、移動方向を決定

        rb.AddForce(1.5f*moveForward.normalized * (speed/Mathf.Sqrt(2.0f)) + new Vector3(0, rb.velocity.y, 0));// 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        
        //ボタン押下でショット発射
        if (!shotAllWay&&Input.GetButtonDown("Shot_4way")) {
            CreateShotObject(0f);
            shotAllWay = true;
        }
        if (!shot1Way&&Input.GetButtonDown("Shot_1way")) {
            shot1Way = true;
        }
        //ボタン押下で移動系発動
        if (!jumpFlg && Input.GetButtonDown("Jump")) {
            rb.velocity = Vector3.up * jumpPower;
            //rb.velocity.y = jumpPower;
            jumpFlg = true;
        }
        if(dribble>0 && Input.GetButtonDown("Dribble")){
            int angle = 0;
            if(Input.GetButton("Right")){
                angle = 90;
                Debug.Log("Right in");
            }else if(Input.GetButton("Left")){
                angle = -90;
                Debug.Log("Left in");
            }else if(Input.GetButton("Up") || Input.GetButton("Down")){
                angle = 0;
                Debug.Log("Other in");
            }

            //rb.AddForce (moveForward * dribbleBoost, ForceMode.Impulse);
            rb.velocity = Quaternion.Euler( 0, angle, 0 ) * cameraForward * dribbleBoost;
            dribble--;
            impulse.GetComponent<EffekseerEmitter>().Play();

        }


        //周囲の敵の数に応じて黄、赤色に発光（白部分が光る）
        if(m_targets.Count >= 3){
            m_renderer.material = m_manyFoundMaterial;
        }else if(m_targets.Count > 0){
            m_renderer.material = m_foundMaterial;
        }else{
            m_renderer.material = m_defaultMaterial;
        }


        //敵機に接触で一定時間無敵状態
        if(untouchable){
            untouchableTime ++;
            if(untouchableTime>90){
                untouchable = false;
                untouchableTime = 0;
            }
        }

        //時間経過でドリブル回数を回復
        if(dribble<maxDribble){
            dribbleTime++;
            if(dribbleTime>180){
                dribble++;
                dribbleTime = 0;
            }
        }

        //発射ボタン押下、一定感覚で弾を連射
        if(shot1Way){
            if(shot1WayTime%15==0 && shot1WayTime<=30){
                CreateShotObject2(muzzle.transform.forward);
                audioSource.PlayOneShot(sound1);
            }
            shot1WayTime++;

            if(shot1WayTime>70){
                shot1WayTime = 0;
                shot1Way = false;
            }
        }

        //一定時間経過で消去
        if(shotAllWay){
            if(shotAllWayTime>30){
                Destroy(GameObject.Find("Shot_allway(Clone)"));
            }
            if(shotAllWayTime>360){
                shotAllWayTime = 0;
                shotAllWay = false;
            }
            shotAllWayTime++;
        }

        if ( Input.GetKeyDown( KeyCode.Z ) ){
            //messageUI.GetComponent<ScreenShake>().Shake( 10.25f, 10.1f );
        }
    }

    //allwayショットの生成処理
    private void CreateShotObject(float axis){
        GameObject shot = Instantiate(shotObject, transform.position, Quaternion.identity);
        var shotObject4way = shot.GetComponent<ShotObject>();
        shotObject4way.SetCharacterObject(gameObject);
        shotObject4way.SetForward4Way(Quaternion.AngleAxis(axis, Vector3.up));
    }

    //1wayショットの生成処理
    private void CreateShotObject2(Vector3 axis){
        GameObject shot = Instantiate(shotObject2, transform.position, Quaternion.identity);
        var shotObject1way = shot.GetComponent<ShotObject>();
        shotObject1way.SetCharacterObject(muzzle.gameObject);
        shotObject1way.SetForward(axis);
        
        muzzleFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
        muzzleFlash.transform.SetParent(gameObject.transform);
        muzzleFlash.transform.localScale = muzzleFlashScale;
    }

    //サーチ系メソッド
    private void Awake(){
        m_renderer  = GetComponentInChildren<Renderer>();

        var searching   = GetComponentInChildren<SearchingBehavior>();
        searching.onFound   += OnFound;
        searching.onLost    += OnLost;
        searching.onStay   += OnStay;
    }

    private void OnFound( GameObject i_foundObject ){
        m_targets.Add( i_foundObject );
    }

    private void OnLost( GameObject i_lostObject ){
        m_targets.Remove( i_lostObject );
    }

    private void OnStay( GameObject[] enemys ){

        //Debug.Log("length:"+enemys.Length);
    }

    //タグ付オブジェクトに触れたときの処理
    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Goal"){
            StageUIManager suim = sceneChanger.GetComponent<StageUIManager>();
            suim.setCurrentScreen(StageUIScreen.GameClear);
        }else if((col.gameObject.tag == "Enemy" || col.gameObject.tag == "Trap") && !untouchable){
            CountDownTimer cdt = countDownTimer.GetComponent<CountDownTimer>();
            cdt.addDamageToTime(20f);
            MessageUIManager muim = messageUI.GetComponent<MessageUIManager>();
            muim.checkPlayerColType(PlayerColType.EnemyCol);
            untouchable=true;
            if(hud.activeSelf){
                hud.GetComponent<ScreenShake>().Shake( 0.25f, 12.1f );
            }
            if(dialoguePanel.activeSelf){
                dialoguePanel.GetComponent<ScreenShake>().Shake( 0.25f, 12.1f );
            }
            if(pickupPanel.activeSelf){
                pickupPanel.GetComponent<ScreenShake>().Shake( 0.25f, 12.1f );
            }
            if(enemyNearPanel.activeSelf){
                enemyNearPanel.GetComponent<ScreenShake>().Shake( 0.25f, 12.1f );
            }
            if(enemyCollisionPanel.activeSelf){
                enemyCollisionPanel.GetComponent<ScreenShake>().Shake( 0.25f, 12.1f );
            }
        }else if(col.gameObject.tag == "Floor"){
            jumpFlg = false;
        }
        
    }

    public int getState(){
        return state;
    }

    public bool getJumpFlg(){
        return jumpFlg;
    }

}


public enum MoveMode{
    Idle = 1,
    Follow = 2
}
