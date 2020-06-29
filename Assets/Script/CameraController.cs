using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour{

    public GameObject mainCamera, otherCamera;

    private float offset; // ボールからカメラまでの距離

    private Vector3 prevPlayerPos;
    private Vector3 posVector;
    public float mainScale, otherScale;
    public float cameraSpeed;

    private float height, height3a, height3b;

    private Vector3 previousPos = new Vector3(-55f,3f,69f);
    private GameObject sceneChanger;
    private StageUIManager suim;

    private Vector3 pos;

    void Start(){
        prevPlayerPos = this.gameObject.transform.position;
        mainCamera.transform.position = prevPlayerPos + new Vector3(5f, 2f, 0f);
        mainCamera.transform.LookAt(prevPlayerPos);
        otherCamera.transform.position = prevPlayerPos;
        
        height3a = 3.5f;
        height3b = 7.0f;
        if(SceneManager.GetActiveScene().name=="Stage3-a" || SceneManager.GetActiveScene().name=="Stage3-a2"){
            height = height3a;
        }else if(SceneManager.GetActiveScene().name=="Stage3-b"){
            height = height3b;
        }

        sceneChanger = GameObject.Find("UIManager");
        suim = sceneChanger.GetComponent<StageUIManager>();
        
        if(suim.getNextPrevFlg()){
            mainCamera.transform.position = previousPos;
            mainCamera.transform.LookAt(this.gameObject.transform.position);
        }
    }

    void LateUpdate(){

        rotateCamera();

        //Debug.Log(offset);
        
        if(this.GetComponent<PlayerController>().getBallMagnitude()>0.3f){
            moveMainCamera();
            moveOtherCamera();
        }
        
    }

    private void rotateCamera(){
        if (Input.GetButton("CameraRight")){
            mainCamera.transform.RotateAround(this.gameObject.transform.position, Vector3.up, 1.0f * Time.deltaTime * 200f);
            otherCamera.transform.RotateAround(this.gameObject.transform.position, Vector3.up, 1.0f * Time.deltaTime * 200f);
        }else if(Input.GetButton("CameraLeft")){
            mainCamera.transform.RotateAround(this.gameObject.transform.position, Vector3.up, -1.0f * Time.deltaTime * 200f);
            otherCamera.transform.RotateAround(this.gameObject.transform.position, Vector3.up, -1.0f * Time.deltaTime * 200f);
        }
    }

    //常にボールの背後に付いて追っかけるカメラ
    private void moveMainCamera(){

        offset = (mainCamera.transform.position - this.gameObject.transform.position).sqrMagnitude;
        Vector3 currentPlayerPos = this.gameObject.transform.position;
        Vector3 backVector = (prevPlayerPos - currentPlayerPos).normalized;
        posVector = (backVector == Vector3.zero) ? posVector : backVector;
        Vector3 targetPos = currentPlayerPos + mainScale * posVector;

        pos = mainCamera.transform.position;
        pos.y = this.gameObject.transform.position.y+2f;
        mainCamera.transform.position = pos;
        
        targetPos.y = targetPos.y + height;
        mainCamera.transform.position = Vector3.Lerp (
            mainCamera.transform.position,
            targetPos,
            cameraSpeed * Time.deltaTime * 0.7f
        );
        
        mainCamera.transform.LookAt (this.gameObject.transform.position);
        prevPlayerPos = this.gameObject.transform.position;

    }

    //ボス戦時のメモ
    //BossState.AttackTackleに遷移した時、ボスにカメラ焦点を当てるとドリブルで当てやすいかも！

    //常にボールの背後に付いて追っかけるカメラ
    private void moveOtherCamera(){

        offset = (otherCamera.transform.position - this.gameObject.transform.position).sqrMagnitude;
        Vector3 currentPlayerPos = this.gameObject.transform.position;
        Vector3 targetPos = currentPlayerPos + otherScale * posVector;
        targetPos.y = targetPos.y + 1.5f;
        otherCamera.transform.position = Vector3.Lerp (
            otherCamera.transform.position,
            targetPos,
            cameraSpeed * Time.deltaTime
        );

        Vector3 wkpos = this.gameObject.transform.position;
        wkpos.y += 3.0f;
        otherCamera.transform.LookAt (wkpos);
        prevPlayerPos = this.gameObject.transform.position;
    }
}