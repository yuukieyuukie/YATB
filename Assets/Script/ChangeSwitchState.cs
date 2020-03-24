using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSwitchState : MonoBehaviour{
    public Texture offTexture;
    public Texture onTexture;
    private static bool onOff;
    private Renderer m_Renderer;

    void Start(){
        m_Renderer = GetComponent<Renderer> ();
    }

    void Update(){
        if(!onOff){
            m_Renderer.material.SetTexture("_MainTex", offTexture);
        }else{
            m_Renderer.material.SetTexture("_MainTex", onTexture);
        }
        
    }

    public void setSwitchState(bool on){
        onOff = on;
    }

    public bool getSwitchState(){
        return onOff;
    }
}
