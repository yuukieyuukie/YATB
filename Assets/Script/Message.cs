using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using System.ValueTuple;
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
	private GameObject player;

	private List<float> nextTimerNow = new List<float>(); //1表示ごとのテキストの止める時間
	private float restTimer;
	private int dai_i = 0;
	private int chu_i = 0;
	
	private RectTransform TextGauge;
	private Slider bar;

	//　1回のメッセージの最大文字数
	[SerializeField]
	private int maxTextLength = 90;
	//　1回のメッセージの現在の文字数
	private int textLength = 0;
	//　メッセージの最大行数
	[SerializeField]
	private int maxLine = 3;
	//　現在の行
	private int nowLine = 0;
	//　テキストスピード
	[SerializeField]
	private float textSpeed = 0.03f;
	//　経過時間
	private float elapsedTime = 0f;
	//　今見ている文字番号
	private int nowTextNum = 0;
	
	//　クリックアイコンの点滅秒数
	[SerializeField]
	private float clickFlashTime = 0.2f;
	//　1回分のメッセージを表示したかどうか
	private bool isOneMessage = false;
	//　メッセージをすべて表示したかどうか
	private bool isEndMessage = false;
	//＠がいくつ出たか
	private int atNum;


	void Start(){

		messageText = GameObject.Find("DialoguePanel/Text").GetComponent<Text>();
		//Debug.Log(messageText);
		messageText.text = "";

		TextGauge=GameObject.Find("DialoguePanel/TextGauge").GetComponent<RectTransform>(); //GameObjectから親要素を取得
		bar = TextGauge.transform.Find("bar").GetComponent <Slider>(); //transformで子要素を取得
Debug.Log(transform.GetChild (0).gameObject);
		player = GameObject.Find("Player");

		switch(SceneManager.GetActiveScene().name){
			case "Prologue":
				SetMessage("25XX年、日本、某地方都市。世間は高齢化の反動を受け人口の著しい減少に見舞われていた。\n＠"
					+ "舞台となるこの地方都市では労働人口が減り、文化そのものが消滅する危機に直面した。\n＠"
					+ "そこで政府は学問・技術・スポーツなどを存続させるため”親機”(しんき)と呼ばれる機械化した道具を投入した。\n＠"
					+ "”親機”とは、減少した将来を担う若者たちに代わる、指導プログラムが施された機械を指す。\n＠"
					+ "経験豊富な上位者の存在が欠かせなく、かつ文化形成に不可欠な学力・体力・倫理の分野の成長を促進すべく、これらに関わる道具自体が機械化された。\n＠"
					+ "こうしてボールをはじめとする旧来の道具は瞬く間に廃棄され、この地方都市では”親機”化したボールが量産されるようになった。\n"
				);
				break;
			case "Stage1":
				// SetMessage("ノベレ「Stage1ですよお兄さん。」\n＠"
				// 	+ "ノベレ「やっちまいましょう。」\n"
				// );
				//SetMessage2("ノベレ「Stage1ですよお兄さん。」\n＠");
				//SetMessage2("ノベレ「やっちまいましょう。」\n＠");

				break;
			case "ScenarioScene1":
				SetMessage("街の生き残りA「ボールがこっちに向かって飛んできやがる・・・やべえよやべえよ逃げよ」\n＠"
					+ "ノベレ「ん、あっちで何か逃げてますよ。追いかけて話を聞いてみましょうか。」\n＠"
					+ "ノベレ「どうかなさったんですか。」\n＠"
					+ "街の生き残りB「この建物の中は危ないボールだらけだ、気をつけろ」\n＠"
					+ "ノベレ「面白そうですね。イレボン、やっちまいましょう。」\n"
				);
				break;
			case "Stage2":
				SetMessage("ノベレ「もうStage2ですよお兄さん。」\n＠"
					+ "ノベレ「強いっすね皆さん。」\n"
				);
				break;
			case "ScenarioScene2":
				SetMessage("街の生き残りA「！」\n＠"
					+ "ノベレ「ん、あっちで何か逃げてますよ。追いかけて話を聞いてみましょうか。」\n＠"
					+ "ノベレ「どうかなさったんですか。」\n＠"
					+ "街の生き残りB「この建物の中は危ないボールだらけだ、気をつけろ」\n＠"
					+ "ノベレ「面白そうですね。イレボン、やっちまいましょう。」\n"
				);
				break;
			case "Stage3-a":
				SetMessage2("ここは何に使われた場所でしょうかね。", 0);
				SetMessage2("コンテナだらけでサビ臭いと言いますか。", 0);
				SetMessage2("敵機も多そうです。慎重に進んでくださいね。", 0);
				SetMessage2("おっ、あんなところにエスカレーターがある。", 1);
				SetMessage2("先に進めそうだから見とけよ見とけ。", 1);
				SetMessage2("何かゴールが近づいてるようなそんな気がする", 2);
				SetMessage2("でもまだ敵はいるみたいだから気を付けて進めよな", 2);
				SetMessage2("って感じ。", 2);

				break;
			case "Stage3-b":
				SetMessage("ノベレ「こっちにもいるのですね。まるで先回りしているかのよう・・・。」\n＠"
					+ "ノベレ「随分と数も多いようです。いよいよ\"アレ\"を試してみるときでしょうか。」\n＠"
					+ "ノベレ「イレボン、あなたにはさらに強力な自衛武装が装備されています。」\n＠"
					+ "ノベレ「本来の仕様であれば使用は不可能なのですが、緊急事態なので仕方ありません。汚れたボールどもに性能の差を思い知らせてやりましょう。」\n＠"
					+ "ノベレ「\"Xキー\"で周囲を一掃する衝撃波を繰り出せます。敵に囲まれそうになったら使って下さい。」\n＠"
					+ "ノベレ「まだ実装されていませんが。」\n"
				);
				break;
			case "BadEnd":
				SetMessage("ノベレ「壊れちゃった。悲しい。」\n"
				);
				break;
			case "GoodEnd":
				SetMessage("こうして人類は救われました。\n"
				);
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
		transform.GetChild (0).gameObject.SetActive (true);
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

			//メッセージに対応するタイマーが一定時間を超えるorメッセージ送りボタン押下で次のメッセージをセット
			if(restTimer>nextTimerNow[chu_i] || Input.GetButtonDown("Submit")){
				chu_i++;
				restTimer = 0.0f;
			}
			else{
				restTimer += Time.deltaTime*5.0f;
			}
		
		}else{
			transform.GetChild (0).gameObject.SetActive (false);
			messageNow.Clear();
			nextTimerNow.Clear();
		}

	}

	private void SetMessage(string message){ //3-a以外(old)
		this.message = message;
	}

	private void SetMessage2(string message, int number){
		this.messageAll.Add(new hoge { dialogue=message, nextTimer=message.Length*1.0f, num=number });
	}

	//　他のスクリプトから新しいメッセージを設定
	public void SetMessagePanel(string message){
		SetMessage (message);
		transform.GetChild (0).gameObject.SetActive (true);
		isEndMessage = false;
	}

	public bool getIsEnd(){
		return isEndMessage;
	}

	private int getAtNum(){
		return atNum;
	}
}