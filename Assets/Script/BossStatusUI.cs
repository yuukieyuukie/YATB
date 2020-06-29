using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossStatusUI : MonoBehaviour{
    
	private BossStatus bossStatus;

	//　HP表示用スライダー
    private RectTransform hpUI;
    private Image gauge;
	//private float shotDamage = 5f, trapDamage = 35f;
 
	void Start() {
        hpUI = GameObject.Find("MessageUI/BossHealthGauge").GetComponent<RectTransform>(); //GameObjectから親要素を取得
        gauge = hpUI.transform.Find("Gauge").GetComponent <Image>(); //transformで子要素を取得
		Debug.Log(hpUI);
		Debug.Log(gauge);

        bossStatus = GetComponent<BossStatus>();
		//　スライダーの値0～1の間になるように比率を計算
		//hpSlider.value = (float) BossStatus.GetMaxHp () / (float) BossStatus.GetMaxHp ();		

	}
	
	void Update () {
		//hpUI.transform.rotation = Camera.main.transform.rotation;
		
		// UpdateHPValue();
        //checkDisable();
	}

	public void UpdateHPValue(float zogen) {
		Vector3 work = gauge.transform.localPosition;
        work.x -= zogen*10f;
		gauge.transform.localPosition = work;
		//hpSlider.value = (float) BossStatus.GetHp () / (float) BossStatus.GetMaxHp ();
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.CompareTag("Shot")){
			hpUI.GetComponent<ScreenShake>().Shake( 0.25f, 5.0f );
			UpdateHPValue(bossStatus.getShotDamage());
		}
		else if(col.gameObject.CompareTag("BossTrap")){
			hpUI.GetComponent<ScreenShake>().Shake( 0.25f, 20.0f );
			UpdateHPValue(bossStatus.getTrapDamage());
		}
	}
}
