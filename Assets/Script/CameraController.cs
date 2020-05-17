using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour{

    public GameObject mainCamera;
    public GameObject otherCamera;

    private float offset; // 玉からカメラまでの距離

    private Vector3 prevPlayerPos;
    private Vector3 posVector;
    public float mainScale;
    public float otherScale;
    public float cameraSpeed;

    private Quaternion mainQuaternion;

    private float height;
    float height3a;
    float height3b;

    void Start(){
        prevPlayerPos = this.gameObject.transform.position;
        mainCamera.transform.position = prevPlayerPos + new Vector3(-1f, 4f, 1f);
        mainCamera.transform.LookAt(prevPlayerPos);
        otherCamera.transform.position = prevPlayerPos;
        height3a = 3.5f;
        height3b = 7.0f;
        if(SceneManager.GetActiveScene().name=="Stage3-a" || SceneManager.GetActiveScene().name=="Stage3-a2"){
            height = height3a;
        }else if(SceneManager.GetActiveScene().name=="Stage3-b"){
            height = height3b;
        }
    }

    void LateUpdate(){
        
        if (Input.GetButton("CameraMove")) {
            if (Input.GetButton("CameraRight")){
                transform.RotateAround(this.gameObject.transform.position, Vector3.up, 1.0f * Time.deltaTime * 200f);
            }else if(Input.GetButton("CameraLeft")){
                transform.RotateAround(this.gameObject.transform.position, Vector3.up, -1.0f * Time.deltaTime * 200f);
            }
            else if(Input.GetButton("CameraUp")){
                transform.RotateAround(this.gameObject.transform.position, Vector3.forward, -1.0f * Time.deltaTime * 200f);
            }else if(Input.GetButton("CameraDown")){
                transform.RotateAround(this.gameObject.transform.position, Vector3.forward, 1.0f * Time.deltaTime * 200f);
            }
        }
        // offset = (transform.position - this.gameObject.transform.position).sqrMagnitude;
        
        if(this.GetComponent<PlayerController>().getBallMagnitude()>0.3f){
            moveMainCamera();
            moveOtherCamera();
        }
        
    }

    //常にボールの背後に付いて追っかけるカメラ
    private void moveMainCamera(){



        offset = (mainCamera.transform.position - this.gameObject.transform.position).sqrMagnitude;
        Vector3 currentPlayerPos = this.gameObject.transform.position;
        Vector3 backVector = (prevPlayerPos - currentPlayerPos).normalized;
        posVector = (backVector == Vector3.zero) ? posVector : backVector;
        Vector3 targetPos = currentPlayerPos + mainScale * posVector;
        targetPos.y = targetPos.y + height;
        mainCamera.transform.position = Vector3.Lerp (
            mainCamera.transform.position,
            targetPos,
            cameraSpeed * Time.deltaTime * 0.7f
        );
        mainCamera.transform.LookAt (this.gameObject.transform.position);
        prevPlayerPos = this.gameObject.transform.position;

        mainQuaternion = mainCamera.transform.rotation;        
    }

    //常にボールの背後に付いて追っかけるカメラ
    private void moveOtherCamera(){

        offset = (otherCamera.transform.position - this.gameObject.transform.position).sqrMagnitude;
        Vector3 currentPlayerPos = this.gameObject.transform.position;
        
        // Vector3 backVector = (prevPlayerPos - currentPlayerPos).normalized;
        // posVector = (backVector == Vector3.zero) ? posVector : backVector;
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