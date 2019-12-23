using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Status;

public class HPStatusUI2 : MonoBehaviour {

	private EnemyStatus EnemyStatus;

	//　HP表示用スライダー
    private RectTransform hpUI;
    private Slider hpSlider;

    // ターゲットオブジェクトの座標からオフセットする値
    public float offset;
 
	void Start() {
        hpUI=GameObject.Find("HP"+(this.gameObject.name.Replace("enemy",""))).GetComponent<RectTransform>(); //GameObjectから親要素を取得
        hpSlider = hpUI.transform.Find("HPBar").GetComponent <Slider>(); //transformで子要素を取得
		
        EnemyStatus = GetComponent<EnemyStatus>();
		//　スライダーの値0～1の間になるように比率を計算
		hpSlider.value = (float) EnemyStatus.GetMaxHp () / (float) EnemyStatus.GetMaxHp ();
	}
	
	// Update is called once per frame
	void Update () {
		hpUI.transform.rotation = Camera.main.transform.rotation;
		UpdateHPValue();
        checkDisable();
	}

    public void checkDisable(){
        if (hpUI.gameObject!=null&&EnemyStatus.GetHp() <= 0){
            hpUI.gameObject.SetActive (false); //destroyするとexception出る
        }else{
			//　HPバーが下にずれるため上方に修正
			Vector3 pos = this.transform.position;
			hpUI.transform.position = new Vector3(pos.x, pos.y+2f, pos.z);
		}
    }
 
	public void UpdateHPValue() {
		hpSlider.value = (float) EnemyStatus.GetHp () / (float) EnemyStatus.GetMaxHp ();
	}
}