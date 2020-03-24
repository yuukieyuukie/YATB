using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossStatusUI : MonoBehaviour{
    
	private BossStatus BossStatus;

	//　HP表示用スライダー
    private RectTransform hpUI;
    private Slider hpSlider;

	private GameObject eventCamera;

 
	void Start() {
        hpUI=GameObject.Find("HP"+(this.gameObject.name.Replace("enemy",""))).GetComponent<RectTransform>(); //GameObjectから親要素を取得
        hpSlider = hpUI.transform.Find("HPBar").GetComponent <Slider>(); //transformで子要素を取得
		
        BossStatus = GetComponent<BossStatus>();
		//　スライダーの値0～1の間になるように比率を計算
		hpSlider.value = (float) BossStatus.GetMaxHp () / (float) BossStatus.GetMaxHp ();

		eventCamera = GameObject.Find("Event Camera");
		

	}
	
	void Update () {
		if(!eventCamera.activeSelf){
			hpUI.transform.rotation = Camera.main.transform.rotation;
		}else{
			hpUI.transform.rotation = eventCamera.transform.rotation;
		}
		UpdateHPValue();
        checkDisable();
	}

    public void checkDisable(){
        if (hpUI.gameObject!=null&&BossStatus.GetHp() <= 0){
            hpUI.gameObject.SetActive (false); //destroyするとexception出る
        }else{
			//　HPバーが下にずれるため上方に修正
			Vector3 pos = this.transform.position;
			hpUI.transform.position = new Vector3(pos.x, pos.y+5f, pos.z);
		}
    }
 
	public void UpdateHPValue() {
		hpSlider.value = (float) BossStatus.GetHp () / (float) BossStatus.GetMaxHp ();
	}
}
