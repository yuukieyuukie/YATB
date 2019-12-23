﻿using UnityEngine;
using System.Collections;
 
public class ChangeCamera : MonoBehaviour {
 
	//　メインカメラ
	[SerializeField]
    private GameObject mainCamera;
	//　切り替える他のカメラ
	[SerializeField]
    private GameObject otherCamera;
	
	// Update is called once per frame
	void Update () {
		//　1キーを押したらカメラの切り替えをする
		if(Input.GetButtonDown("ChangeCamera")) {
			mainCamera.SetActive(!mainCamera.activeSelf);
			otherCamera.SetActive(!otherCamera.activeSelf);
		}	
	}

    public bool isMainCamera(){
        return mainCamera.activeSelf;
    }

	public bool isOtherCamera(){
        return otherCamera.activeSelf;
	}
}
