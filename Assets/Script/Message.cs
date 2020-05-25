using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Message : MonoBehaviour {
 
	//　メッセージUI
	private Text messageText;
	//　表示するメッセージ
	private string message;

	private struct hoge{
		public string dialogue;
		public float nextTimer;
		public int num;
	}
	private List<hoge> messageAll = new List<hoge>();
	private List<string> messageNow = new List<string>();

	private List<float> nextTimerNow = new List<float>(); //1表示ごとのテキストの止める時間
	private float restTimer;
	private int dai_i = 0;
	private int chu_i = 0;
	
	private RectTransform TextGauge;
	private Slider bar;

	private bool isEndMessage = false;

	ScoreManager scoreManager = null;

	void Start(){

		messageText = GameObject.Find("DialoguePanel/Text").GetComponent<Text>();
		messageText.text = "";

		TextGauge = GameObject.Find("DialoguePanel/TextGauge").GetComponent<RectTransform>(); //GameObjectから親要素を取得
		bar = TextGauge.transform.Find("bar").GetComponent <Slider>(); //transformで子要素を取得

		if(scoreManager==null){
			scoreManager = gameObject.AddComponent<ScoreManager>();
        }

		switch(SceneManager.GetActiveScene().name){
			case "Prologue":
				// SetMessage("25XX年、日本、某地方都市。世間は高齢化の反動を受け人口の著しい減少に見舞われていた。\n＠"
				// 	+ "舞台となるこの地方都市では労働人口が減り、文化そのものが消滅する危機に直面した。\n＠"
				// 	+ "そこで政府は学問・技術・スポーツなどを存続させるため”親機”(しんき)と呼ばれる機械化した道具を投入した。\n＠"
				// 	+ "”親機”とは、減少した将来を担う若者たちに代わる、指導プログラムが施された機械を指す。\n＠"
				// 	+ "経験豊富な上位者の存在が欠かせなく、かつ文化形成に不可欠な学力・体力・倫理の分野の成長を促進すべく、これらに関わる道具自体が機械化された。\n＠"
				// 	+ "こうしてボールをはじめとする旧来の道具は瞬く間に廃棄され、この地方都市では”親機”化したボールが量産されるようになった。\n"
				// );
				break;
			case "Stage1":
				break;
			case "ScenarioScene1":
				// SetMessage("街の生き残りA「ボールがこっちに向かって飛んできやがる・・・やべえよやべえよ逃げよ」\n＠"
				// 	+ "ノベレ「ん、あっちで何か逃げてますよ。追いかけて話を聞いてみましょうか。」\n＠"
				// 	+ "ノベレ「どうかなさったんですか。」\n＠"
				// 	+ "街の生き残りB「この建物の中は危ないボールだらけだ、気をつけろ」\n＠"
				// 	+ "ノベレ「面白そうですね。イレボン、やっちまいましょう。」\n"
				// );
				break;
			case "Stage2":
				// SetMessage("ノベレ「もうStage2ですよお兄さん。」\n＠"
				// 	+ "ノベレ「強いっすね皆さん。」\n"
				// );
				break;
			case "ScenarioScene2":
				// SetMessage("街の生き残りA「！」\n＠"
				// 	+ "ノベレ「ん、あっちで何か逃げてますよ。追いかけて話を聞いてみましょうか。」\n＠"
				// 	+ "ノベレ「どうかなさったんですか。」\n＠"
				// 	+ "街の生き残りB「この建物の中は危ないボールだらけだ、気をつけろ」\n＠"
				// 	+ "ノベレ「面白そうですね。イレボン、やっちまいましょう。」\n"
				// );
				break;
			case "Stage3-a":
				dai_i = 0;
				if(scoreManager.getLanguage()==0){	
					SetMessage2("ここは何に使われた場所でしょうか", 0);
					SetMessage2("コンテナだらけでサビ臭いです", 0);
					SetMessage2("敵機も多そうです。慎重に進んでください", 0);
					SetMessage2("さっそく敵機がうろついてますね", 1);
					SetMessage2("さっさと倒して向こうに見える階段に向かいましょう", 1);
				}else if(scoreManager.getLanguage()==1){
					SetMessage2("I wonder what this place was used for.", 0);
					SetMessage2("It's full of containers and it smells rusty.", 0);
					SetMessage2("There seems to be a lot of enemies. Please proceed with caution.", 0);
					SetMessage2("The enemy is already on the prowl, isn't it?", 1);
					SetMessage2("Let's just get rid of them and head to the stairs you can see over there.", 1);
				}
				break;
			case "Stage3-a2":
				dai_i = 2;
				if(scoreManager.getLanguage()==0){
					SetMessage2("周囲に敵機はいなさそうです", 2);
					SetMessage2("ただ地形が少々複雑なようです。迷わないようお気をつけて", 2);
					SetMessage2("イレボン、左の窓の奥の方に怪しげなスイッチがありますよ！", 3);
					SetMessage2("アレを押せば１階のフェンスが動きそうですがこちらからは入れないようです", 3);
				}else if(scoreManager.getLanguage()==1){
					SetMessage2("There seems to be no enemies around.", 2);
					SetMessage2("However, the terrain seems to be a bit complicated. Be careful not to get lost.", 2);
					SetMessage2("Elevon, there's a suspicious switch at the back of the window on the left!", 3);
					SetMessage2("The fence on the first floor seems to move if you push on it, but you can't get in from here.", 3);
				}
				break;
			case "Stage3-b":
				dai_i = 0;
				if(scoreManager.getLanguage()==0){	
					SetMessage2("敵機に改造されたビリヤードのようです", 0);
					SetMessage2("ショットも効くと思いますが、このエリアには他にも有効な攻撃手段がありそうですよ！", 0);
					SetMessage2("相手の動きを利用したり、突き飛ばしたりするといいと思います！", 0);
				}else if(scoreManager.getLanguage()==1){	
					SetMessage2("It's like a modified billiard.", 0);
					SetMessage2("I'm sure the shots will do the trick, but I think there are other effective offensive measures in this area!", 0);
					SetMessage2("It would be nice if he could take advantage of his opponent's moves and shove them!", 0);
				}
				break;
			case "BadEnd":
				// SetMessage("ノベレ「壊れちゃった。悲しい。」\n"
				// );
				break;
			case "GoodEnd":
				// SetMessage("こうして人類は救われました。\n"
				// );
				break;
		}

		//表示するメッセージをセット
		for(int i=0;i<messageAll.Count;i++){
			if(dai_i==messageAll[i].num){ //大区分と等しい第二引数の場合
				messageNow.Add(messageAll[i].dialogue);
				nextTimerNow.Add(messageAll[i].nextTimer);
			}
		}
	}

	void Update(){
		//Pause中の入力を受け付けない
        if (Mathf.Approximately(Time.timeScale, 0f)) {
		    return;
	    }

		//メッセージを全て表示したらウィンドウを破棄
		if(chu_i<messageNow.Count){ //0<0はelse

			messageText.text = messageNow[chu_i];

            //残り時間に応じgaugeを減らす
            bar.value = 1.0f - (restTimer / messageText.text.Length);

			//メッセージに対応するタイマーが一定時間を超える 次のメッセージをセット
			if(restTimer>nextTimerNow[chu_i]){
				chu_i++;
				restTimer = 0.0f;
			}
			else{
				if(scoreManager.getLanguage()==0){
					restTimer += Time.deltaTime*5.0f;
				}else if(scoreManager.getLanguage()==1){
					restTimer += Time.deltaTime*12.0f;
				}
			}
		
		}else{
			transform.GetChild(0).gameObject.SetActive(false);
			messageNow.Clear();
			nextTimerNow.Clear();
		}

	}

	// private void SetMessage(string message){ //3-a以外(old)
	// 	this.message = message;
	// }

	private void SetMessage2(string message, int number){
		this.messageAll.Add(new hoge { dialogue=message, nextTimer=message.Length*1.0f, num=number });
	}

	public void setNextMessage(){
		dai_i++;
		chu_i = 0;
		//表示するメッセージをセット
		for(int i=0;i<messageAll.Count;i++){
			if(dai_i==messageAll[i].num){
				messageNow.Add(messageAll[i].dialogue);
				nextTimerNow.Add(messageAll[i].nextTimer);
			}
		}
		transform.GetChild(0).gameObject.SetActive(true);
	}

	public bool getIsEnd(){
		return isEndMessage;
	}

}