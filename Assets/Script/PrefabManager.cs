using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrefabManager : MonoBehaviour{

	Vector3[] pickupPosStage3;
	Vector3[] kirehashiPosStage3;
	Vector3[] enemyPosStage3;
	
	List<GameObject> enemy_obj = new List<GameObject>();
	List<EnemyStatus> es = new List<EnemyStatus>();

	void Awake(){

		//配置するオブジェクトのプレハブのパスを設定
		string ballPrefabPath = "Prefabs/Enemy Ball";
        string hpPrefabPath = "Prefabs/HPUI2";
		string pickupPrefabPath = "Prefabs/PickUp";
		string cursorPrefabPath = "Prefabs/Cursor";
		string kirehashiPrefabPath = "Prefabs/Kirehashi";
		
		//配置するオブジェクトの大きさを設定
		Vector3 ballScale = new Vector3 (8f, 8f, 8f);
        Vector3 hpScale = new Vector3 (0.045f, 0.045f, 0.045f);
		Vector3 pickupScale = new Vector3 (1.5f, 1.5f, 1.5f);
		Vector3 cursorScale = new Vector3 (0.1f, 1f, 1f);
		Vector3 kirehashiScale = new Vector3 (1f, 0.1f, 1f);

		List<GameObject> pickup_obj = new List<GameObject>();
		List<GameObject> kirehashi_obj = new List<GameObject>();
		
		List<GameObject> hpbar_obj = new List<GameObject>();
		GameObject cursor_obj;

		//ステージごとに生成
		if(SceneManager.GetActiveScene().name=="Stage1"){
			//stage1のアイテム場所を設定
		}else if(SceneManager.GetActiveScene().name=="Stage2"){
			//stage2のアイテム場所を設定
		}else if(SceneManager.GetActiveScene().name=="Stage3-a"){
			pickupPosStage3 = new Vector3[8] {
				new Vector3(-130f, 2f, -70f),
				new Vector3(-72f, 7.5f, -16f),
				new Vector3(-45f, 2f, 77f),
				new Vector3(-39f, 2f, 77f),
				new Vector3(-30f, 2f, 25f),
				new Vector3(-30f, 2f, 20f),
				new Vector3(-150f, 2f, 10f),
				new Vector3(-140f, 2f, 10f)
			};

			kirehashiPosStage3 = new Vector3[3] {
				new Vector3(-100f, 2f, 10f),
				new Vector3(-105f, 2f, 20f),
				new Vector3(-110f, 2f, 10f)
			};

			enemyPosStage3 = new Vector3[3] {
				new Vector3(Random.Range(-105.0f, -95.0f), 2f, Random.Range(-25.0f, -10.0f)),
				new Vector3(-105.0f, 3f, 60.0f),
				new Vector3(-90.0f, 3f, 60.0f),

			};
			for(int i=0;i<pickupPosStage3.Length;i++){
				pickup_obj.Add(createPrefab(pickupPrefabPath, pickupPosStage3[i], this.gameObject.transform.rotation));
				pickup_obj[i].transform.localScale = pickupScale;
				pickup_obj[i].name = "PickUp"+i;
			}
			for(int i=0;i<kirehashiPosStage3.Length;i++){
				kirehashi_obj.Add(createPrefab(kirehashiPrefabPath, kirehashiPosStage3[i], this.gameObject.transform.rotation));
				kirehashi_obj[i].transform.localScale = kirehashiScale;
				kirehashi_obj[i].name = "Kirehashi"+i;
			}
			for(int i=0;i<enemyPosStage3.Length;i++){
				enemy_obj.Add(createPrefab(ballPrefabPath, enemyPosStage3[i], this.gameObject.transform.rotation));
				enemy_obj[i].transform.localScale = ballScale;
				enemy_obj[i].name = "enemy"+i;
				hpbar_obj.Add(createPrefab(hpPrefabPath, enemyPosStage3[i], this.gameObject.transform.rotation));
				hpbar_obj[i].transform.localScale = hpScale;
				hpbar_obj[i].name = "HP"+i;
			}

		}else if(SceneManager.GetActiveScene().name=="Stage3-a2"){
			pickupPosStage3 = new Vector3[5] {
				new Vector3(-95f, 11.5f, 93f),
				new Vector3(-105f, 11.5f, 93f),
				new Vector3(-115f, 11.5f, 93f),
				new Vector3(-63f, 11.5f, 0f),
				new Vector3(-137f, 11.5f, 4f)
			};
			for(int i=0;i<pickupPosStage3.Length;i++){
				pickup_obj.Add(createPrefab(pickupPrefabPath, pickupPosStage3[i], this.gameObject.transform.rotation));
				pickup_obj[i].transform.localScale = pickupScale;
				pickup_obj[i].name = "PickUp"+i;
			}
			
		}else if(SceneManager.GetActiveScene().name=="Stage3-b"){

		}

		cursor_obj = createPrefab(cursorPrefabPath, new Vector3(0,0,0), this.gameObject.transform.rotation);
		cursor_obj.transform.localScale = cursorScale;
		cursor_obj.name = "Cursor";
	}

	void Update(){

	}

	/// <summary>
	/// プレハブを作成する(pos, Quaternion有)
	/// </summary>
	/// <returns>GameObject</returns>
	/// <param name="path">プレハブのパス</param>
	/// <param name="pos">Position.</param>
	/// <param name="q">Quaternion.</param>
	public GameObject createPrefab(string path, Vector3 pos, Quaternion q){
		GameObject prefabObj = (GameObject)Resources.Load (path);
		GameObject prefab = (GameObject)Instantiate (prefabObj, pos, q);
		return prefab;
	}

	public Vector3 getEnemyPosStage3(int i){ //MoveEnemyに目標地点をの座標を提供
		return enemyPosStage3[i];
	}
}
