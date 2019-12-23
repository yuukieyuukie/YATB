// using UnityEngine;
// using System.Collections;
// using UnityEngine.UI;
// using Status;

// public class HPStatusUI : MonoBehaviour {
 
// 	//　敵のステータス
//     public GameObject targetObject;
// 	private EnemyStatus EnemyStatus;
// 	//　HP表示用スライダー
// 	private Slider hpSlider;

//     public Transform target;
//     // ターゲットオブジェクトの座標からオフセットする値
//     public float offset;
 
// 	void Start() {
//         EnemyStatus = targetObject.GetComponent<EnemyStatus>();
// 		//　HP用Sliderを子要素から取得
// 		hpSlider = transform.Find ("HPBar").GetComponent <Slider>();
// 		//　スライダーの値0～1の間になるように比率を計算
// 		hpSlider.value = (float) EnemyStatus.GetMaxHp () / (float) EnemyStatus.GetMaxHp ();
// 	}
 
// 	// Update is called once per frame
// 	void Update () {
// 		//　カメラと同じ向きに設定
// 		transform.rotation = Camera.main.transform.rotation;
//         UpdateHPValue();
//         checkDisable();
//         // オブジェクトの座標を変数 pos に格納
//         Vector3 pos = transform.position;
//         // ターゲットオブジェクトのY座標に変数 offset のオフセット値を加えて
//         // 変数 posのY座標に代入
//         pos.x = target.position.x + offset;
//         pos.z = target.position.z + offset;
//         // 変数 pos の値をオブジェクト座標に格納
//         transform.position = pos;
// 	}
// 	//　死んだらHPUIを非表示にする
// 	public void SetDisable() {
// 		gameObject.SetActive (false);
// 	}

//     public void checkDisable(){
//         if (EnemyStatus.GetHp() <= 0){
//             SetDisable ();
//         }
//     }
 
// 	public void UpdateHPValue() {
// 		hpSlider.value = (float) EnemyStatus.GetHp () / (float) EnemyStatus.GetMaxHp ();
// 	}
// }