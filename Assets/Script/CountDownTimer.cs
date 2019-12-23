using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
public class CountDownTimer : MonoBehaviour{
 
	//　トータル制限時間
	private float totalTime;
	//　制限時間（分）
	[SerializeField]
	private int minute;
	//　制限時間（秒）
	[SerializeField]
	private float seconds;
	//　前回Update時の秒数
	private float oldSeconds;
	private Text timerText;
 
	private GameObject sceneChanger;
	private SceneChanger sc;

	void Start(){
		totalTime = minute * 60 + seconds;
		oldSeconds = 0f;
		timerText = GetComponentInChildren<Text>();
		sceneChanger = GameObject.Find("UIManager");
		sc = sceneChanger.GetComponent<SceneChanger>();
	}
 
	void Update(){
		//　制限時間が0秒以下orボール動かしモードでないなら何もしない
		if (totalTime <= 0f || sc.getCurrentScreen() != UIScreen2.Game) {
			return;
		}
		//　一旦トータルの制限時間を計測；
		totalTime = minute * 60 + seconds;
		totalTime -= Time.deltaTime;
 
		//　再設定
		minute = (int) totalTime / 60;
		seconds = totalTime - minute * 60;
 
		//　タイマー表示用UIテキストに時間を表示する
		if((int)seconds != (int)oldSeconds) {
			timerText.text = minute.ToString("00") + ":" + ((int) seconds).ToString("00");
		}
		oldSeconds = seconds;
	}

	//画面変更の条件として残り時間を取得
	public float getTotalTime(){
		return this.totalTime;
	}

	//他スクリプトから呼び出し、ダメージを時間に反映
	public void addDamageToTime(float damageValue){
		Debug.Log("Damage");
		seconds -= damageValue;
	}
}