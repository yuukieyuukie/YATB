using UnityEngine;
using System.Collections;
 
public class ChangeCamera : MonoBehaviour {
 
	//　メインカメラ
	[SerializeField]
    private GameObject mainCamera;
	//　切り替える他のカメラ
	[SerializeField]
    private GameObject otherCamera;
	//　イベント用のカメラ
	[SerializeField]
    private GameObject eventCamera;
	private bool eventFlg;
	private int eventTime;

	void Start(){
		eventTime = 0;
	}

	void Update () {
		//　キーを押したらカメラの切り替えをする
		if(Input.GetButtonDown("ChangeCamera")) {
			mainCamera.SetActive(!mainCamera.activeSelf);
			otherCamera.SetActive(!otherCamera.activeSelf);
		}
		if(eventFlg){
			if(mainCamera.activeSelf || otherCamera.activeSelf){
				mainCamera.SetActive(false);
				otherCamera.SetActive(false);
				eventCamera.SetActive(true);
			}
			eventTime++;
		}

		if(eventTime>210){
			eventCamera.SetActive(false);
			mainCamera.SetActive(true);
			setEventCameraFlg(false);
			eventTime = 0;
		}
	}

    public bool isMainCamera(){
        return mainCamera.activeSelf;
    }

	public bool isOtherCamera(){
        return otherCamera.activeSelf;
	}

	public void setEventCameraFlg(bool active){
        eventFlg = active;
    }

	public bool getEventCameraFlg(){
		return eventFlg;
	}
}
