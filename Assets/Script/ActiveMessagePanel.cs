using UnityEngine;
using System.Collections;
 
public class ActiveMessagePanel : MonoBehaviour {
 
	//　MessageUIに設定されているMessageスクリプトを設定
	[SerializeField]
	private Message messageScript;
 
	private string message;
	
	void Start(){
		message = "the return of new despair\n"
								+ "lost rainbow\n＠"
								+ "unityもっと自由に～\n"
								+ "天網恢恢疎にして漏らさず\n";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire2")) {
			//　表示させるメッセージ
			messageScript.SetMessagePanel (message);
		}
	}
}