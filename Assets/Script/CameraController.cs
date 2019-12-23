using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour{
    public GameObject player; // 玉のオブジェクト

    private float offset; // 玉からカメラまでの距離

    private Vector3 prevPlayerPos;
    private Vector3 posVector;
    public float scale;
    public float cameraSpeed;

    void Start(){
        prevPlayerPos = player.transform.position;
        this.gameObject.transform.position = prevPlayerPos + new Vector3(1f, 2f, 1f);
    }

    void LateUpdate(){
        
        if (Input.GetButton("CameraMove")) {
            if (Input.GetButton("CameraRight")){
                transform.RotateAround(player.transform.position, Vector3.up, 1.0f * Time.deltaTime * 200f);
            }else if(Input.GetButton("CameraLeft")){
                transform.RotateAround(player.transform.position, Vector3.up, -1.0f * Time.deltaTime * 200f);
            }
            else if(Input.GetButton("CameraUp")){
                transform.RotateAround(player.transform.position, Vector3.forward, -1.0f * Time.deltaTime * 200f);
            }else if(Input.GetButton("CameraDown")){
                transform.RotateAround(player.transform.position, Vector3.forward, 1.0f * Time.deltaTime * 200f);
            }
        }
        offset = (transform.position - player.transform.position).sqrMagnitude;
        moveCamera();
        //Debug.Log(offset);
        
    }

    //常にボールの背後に付いて追っかけるカメラ
    private void moveCamera(){
        Vector3 currentPlayerPos = player.transform.position;
        Vector3 backVector = (prevPlayerPos - currentPlayerPos).normalized;
        posVector = (backVector == Vector3.zero) ? posVector : backVector;
        Vector3 targetPos = currentPlayerPos + scale * posVector;
        targetPos.y = targetPos.y + 3.5f;
        this.transform.position = Vector3.Lerp (
            this.transform.position,
            targetPos,
            cameraSpeed * Time.deltaTime
        );
        this.transform.LookAt (player.transform.position);
        prevPlayerPos = player.transform.position;
    }
}