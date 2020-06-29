using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Effekseer;

public class PlayerController : MonoBehaviour{
    private float speed = 10f; // 動く速さ
    private float maxSpeed = 15f;
    private float jumpPower = 10f;
    private bool jumpFlg, landFlg;
    private Rigidbody rb; // Rididbody
    public GameObject shotObject2;
    public GameObject muzzle;
    public GameObject muzzleFlash;
    public GameObject muzzleFlashPrefab;
    [SerializeField]
    private Vector3 muzzleFlashScale;
    [SerializeField]
    private GameObject impulse;
    [SerializeField]
    private AudioClip shotAudio;
    private AudioSource shotAudioSource;
    [SerializeField]
    private AudioClip damageAudio;
    private AudioSource damageAudioSource;

    private bool untouchable;
    private int untouchableTime;
    private int dribble;
    private int maxDribble = 5;
    private float dribbleBoost = 25f;
    private int dribbleTime;
    private int angle;
    private bool dribbleFlg = false;
    private bool shot1Way;
    private int shot1WayTime;

    private GameObject uiManager; //他オブジェクトのコンポーネントを取り込む
    public GameObject countDownTimer;
    private GameObject messageUI;
    
    private GameObject changeSwitchState;
    private GameObject eventCamera;

    // private float moveHorizontal;
    // private float moveVertical;

    private Vector3 previousPos = new Vector3(-51f,1.1f,69f);

    private GameObject playerSpark;
    private bool sparkFlg;
    
    StageUIManager suim;

    void Start(){
        rb = GetComponent<Rigidbody>();
        
        messageUI = GameObject.Find("MessageUI");
        muzzleFlash.SetActive(true);
        shotAudioSource = GetComponent<AudioSource>();
        damageAudioSource = GetComponent<AudioSource>();
        changeSwitchState = GameObject.Find("Switch1");
        eventCamera = GameObject.Find("Event Camera"); //exceptionを直すため
        playerSpark = GameObject.Find("PlayerSpark");
        
        uiManager = GameObject.Find("UIManager");
        suim = uiManager.GetComponent<StageUIManager>();
        
        if(suim.getNextPrevFlg()){
            this.gameObject.transform.position = previousPos;
            suim.setNextPrevFlg(false);
        }
        
    }
    
    private float moveHorizontal2;
    private float moveVertical2;
    
    //rbはFixedで処理
    void FixedUpdate(){

        Vector3 cameraForward = Vector3.zero;
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;// カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 moveForward2 = cameraForward * -moveVertical2 + Camera.main.transform.right * moveHorizontal2;// 方向キーの入力値とカメラの向きから、移動方向を決定

        //減速キーor移動処理をしない時は減速、移動処理をする時は移動
        if(!jumpFlg && moveHorizontal2==0f && moveVertical2>=0f){
            if(rb.velocity.magnitude>=2f){
                rb.AddForce(-300f * rb.velocity * Time.deltaTime);
            }
            else if(rb.velocity.magnitude<2f){
                rb.constraints = RigidbodyConstraints.FreezePosition;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.velocity = Vector3.zero;
            }
        }else if((-1f<=moveHorizontal2&&moveHorizontal2<=1f) || moveVertical2<0f || Input.GetButton("Jump")){            
            if(rb.velocity.magnitude < maxSpeed){
                rb.AddForce(100f * moveForward2.normalized * Time.deltaTime * (speed/Mathf.Sqrt(2.0f)) + new Vector3(0, rb.velocity.y, 0));// 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            }
            rb.constraints = RigidbodyConstraints.None;
        }
        if(dribbleFlg){
            rb.velocity = Quaternion.Euler( 0, angle, 0 ) * cameraForward * dribbleBoost;
            dribbleFlg = false;
        }

        if(jumpFlg && landFlg){
            rb.velocity = Vector3.up * jumpPower;
            landFlg = false;
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
            dribbleTime ++;
            if(dribbleTime>180){
                dribble++;
                dribbleTime = 0;
            }
        }

        //発射ボタン押下、一定感覚で弾を連射
        if(shot1Way){
            if(shot1WayTime%15==0 && shot1WayTime<=30){
                CreateShotObject2(muzzle.transform.forward);
                shotAudioSource.PlayOneShot(shotAudio);
            }
            shot1WayTime++;

            if(shot1WayTime>70){
                shot1WayTime = 0;
                shot1Way = false;
            }
        }
    }

    void Update(){

        moveHorizontal2 = Input.GetAxis("Axis 1"); //左アナログスティック横, adキー
        moveVertical2 = Input.GetAxis("Axis 2"); //左アナログスティック縦, wsキー

        //Pause中の入力を受け付けない
        if (Mathf.Approximately(Time.timeScale, 0f)) {
            return;
        }

        // //左右
        // if (Input.GetAxis("Axis 1") > 0f)
        //     axis1Value.text = "positive"; //右
        // else if (Input.GetAxis("Axis 1") < 0f)
        //     axis1Value.text = "negative"; //左

        // //上下
        // if (Input.GetAxis("Axis 2") > 0f)
        //     axis2Value.text = "positive"; //下
        // else if (Input.GetAxis("Axis 2") < 0f)
        //     axis2Value.text = "negative"; //上
        
        if (!shot1Way&&Input.GetButtonDown("Shot_1way")) {
            shot1Way = true;
        }

        if (!jumpFlg && Input.GetButtonDown("Jump")) {
            jumpFlg = true;
            landFlg = true;
        }
        // Debug.Log(moveHorizontal2+"  "+moveVertical2);
        if(dribble>0 && Input.GetButtonDown("Dribble")){
            if(Input.GetButton("Up")){
                angle = 0;
            }else if(Input.GetButton("Right")){
                angle = 90;
            }else if(Input.GetButton("Left")){
                angle = -90;
            }
            if(moveHorizontal2>0f){
                angle = 90;
            }else if(moveHorizontal2<0f){
                angle = -90;
            }else if(moveVertical2<0f){
                angle = 0;
            }
            dribbleFlg = true;
            dribble--;
            impulse.GetComponent<EffekseerEmitter>().Play();
            untouchable = true;
            untouchableTime = 0;
        }

        if(sparkFlg){
            playerSpark.transform.position = this.transform.position;
        }
        if(!sparkFlg && playerSpark.activeSelf){
            playerSpark.SetActive(false);
        }
        if(sparkFlg && !playerSpark.activeSelf){
            playerSpark.SetActive(true);
        }

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

    //タグ付オブジェクトに触れたときの処理
    void OnCollisionEnter(Collision col){
        if(col.gameObject.CompareTag("Goal")){
            suim.setCurrentScreen(StageUIScreen.GameClear);
        }else if(col.gameObject.CompareTag("Previous")){
            suim.setCurrentScreen(StageUIScreen.Previous);
        }else if(col.gameObject.CompareTag("Next")){
            suim.setCurrentScreen(StageUIScreen.Next);
        }else if((col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Trap") || col.gameObject.CompareTag("Boss") || col.gameObject.CompareTag("BossChild")) && !untouchable){
            damageAudioSource.PlayOneShot(damageAudio);
            MessageUIManager muim = messageUI.GetComponent<MessageUIManager>();
            muim.checkPlayerColType(PlayerColType.EnemyCol);
            untouchable = true;
            sparkFlg = true;
            foreach (Transform childTransform in messageUI.transform){
                childTransform.GetComponent<ScreenShake>().Shake( 0.25f, 12.1f );
            }
        }else if(col.gameObject.CompareTag("Floor")){
            jumpFlg = false;
        }else if(col.gameObject.CompareTag("Switch") && !changeSwitchState.GetComponent<ChangeSwitchState>().getSwitchState()){
            ChangeSwitchState.setSwitchState(true);
        }else if(col.gameObject.CompareTag("BossTrap")){
            MessageUIManager muim = messageUI.GetComponent<MessageUIManager>();
            muim.checkPlayerColType(PlayerColType.EnemyCol);
            foreach (Transform childTransform in messageUI.transform){
                childTransform.GetComponent<ScreenShake>().Shake( 0.25f, 12.1f );
            }
        }
    }
    
    public int getDribbleCount(){
        return dribble;
    }

    public bool getJumpFlg(){
        return jumpFlg;
    }

    public float getBallMagnitude(){
        return rb.velocity.magnitude;
    }

    public void setSparkFlg(bool a){
        sparkFlg = a; 
    }

}


public enum MoveMode{
    Idle = 1,
    Follow = 2
}
