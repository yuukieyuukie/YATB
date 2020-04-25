using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSwitchState : MonoBehaviour{
    public Texture offTexture;
    public Texture onTexture;
    private static bool onOff;
    private Renderer m_Renderer;

    //public static Vector3 pos;
    private Vector3 work;
    private Vector3 preWork;
    private GameObject changeCamera;

    private static bool flg = false;

    void Start(){
        m_Renderer = GetComponent<Renderer> ();
        work = this.gameObject.transform.position;
        preWork = work;

        changeCamera = GameObject.Find("ChangeCamera");
    }

    void Update(){
        

    }

    void FixedUpdate(){
        if(!onOff){
            m_Renderer.material.SetTexture("_MainTex", offTexture);
        }else{
            m_Renderer.material.SetTexture("_MainTex", onTexture);
            if(preWork.z-work.z<=1.5f){
                work.z -= 0.05f;
                this.gameObject.transform.position = work;
            }else{
                if(!flg) changeCamera.GetComponent<ChangeCamera>().setEventCameraFlg(true);
                flg = true;
            }
        }
    }

    public static void setSwitchState(bool on){
        onOff = on;
        flg = !on;
    }

    public bool getSwitchState(){
        return onOff;
    }
}
