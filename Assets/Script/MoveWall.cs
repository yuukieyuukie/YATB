using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour{

    private GameObject changeCamera;
    private Vector3 work;
    private Vector3 preWork;

    public static Vector3 pos;

    void Start(){
        changeCamera = GameObject.Find("ChangeCamera");
        work = this.gameObject.transform.position;
        preWork = work;
        
    }

    void Update(){
        
    }

    void FixedUpdate(){
        if(changeCamera.GetComponent<ChangeCamera>().getEventCameraFlg()){
            if(preWork.z-work.z>=-60f){
                work.z += 0.1f;
                this.gameObject.transform.position = work;
                pos = this.gameObject.transform.position;
            }else{
                changeCamera.GetComponent<ChangeCamera>().setEventCameraFlg(false);
            }
        }
    }


    public void setPreviousPos(){
        this.gameObject.transform.position = pos; //シーン遷移後の位置の変更を適用
    }

    public void setPos(){
        pos = work;
    }

    public Vector3 getPos(){
        return pos;
    }
}
