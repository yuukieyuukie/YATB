using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour{

    private GameObject changeCamera;
    private Vector3 work;

    public static Vector3 pos;

    void Start(){
        changeCamera = GameObject.Find("ChangeCamera");
        work = this.gameObject.transform.position;
        
        
    }

    void Update(){
        if(changeCamera.GetComponent<ChangeCamera>().getEventCameraFlg()){
            work.z += 0.08f;
            this.gameObject.transform.position = work;
            pos = this.gameObject.transform.position;
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
