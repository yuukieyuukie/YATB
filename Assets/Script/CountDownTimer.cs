using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 
public class CountDownTimer : MonoBehaviour{
 
	//　トータル制限時間
	public static float totalTime;
	//　制限時間（分,秒）
	public static int minute = 1;
	public static float seconds = 10;
	//　前回Update時の秒数
	private float oldSeconds;
	private Text timerText;
 
	private GameObject stageUIManager;
	private StageUIManager suim;

	void OnActiveSceneChanged( Scene prevScene, Scene nextScene ){
        Debug.Log ( prevScene.name + "->"  + nextScene.name );
		if(nextScene.name=="Stage3-b"){
				minute = 2;
				seconds = 30;
		}

    }

	void Start(){
		SceneManager.activeSceneChanged += OnActiveSceneChanged;
		oldSeconds = 0f;
		timerText = GetComponentInChildren<Text>();
		stageUIManager = GameObject.Find("UIManager");
		suim = stageUIManager.GetComponent<StageUIManager>();
		if(suim.getCurrentScreen() == StageUIScreen.Briefing){
			if(SceneManager.GetActiveScene().name=="Stage3-a"){
				minute = 1;
				seconds = 10;
			}else if(SceneManager.GetActiveScene().name=="Stage3-b"){
				
			}
		}
		
	}
 
	void Update(){

		//　制限時間が0秒以下orボール動かしモードでないなら何もしない
		if (totalTime <= 0f || suim.getCurrentScreen() != StageUIScreen.Game) {
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

	//タイマー初期化。Breifingでのみ呼出し
	public void initTotalTime(){
		totalTime = minute * 60 + seconds;
	}

	//GameOver判定
	public float getTotalTime(){
		return totalTime;
	}

	//他スクリプトから呼び出し、ダメージを時間に反映
	public void addDamageToTime(float damageValue){
		seconds -= damageValue;
	}

	public void addRegainToTime(float regainValue){
		seconds += regainValue;
	}

	public int getMinute(){
		return minute;
	}

	public float getSeconds(){
		return seconds;
	}
}