using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LockOn{
public class Lockon : MonoBehaviour
{

    public bool isLockOn = false; //ロックオンボタンを押したかどうか

    public GameObject search;
    private GameObject camera;
    private GameObject player;
    private GameObject target;
    private SearchingBehavior searchingBehavior;

    private GameObject cursor;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        searchingBehavior = search.GetComponent<SearchingBehavior>();

        cursor=GameObject.Find("Cursor");

    }

    void Update(){
        target = searchingBehavior.getTarget();
        //ボタン押下でロックオン
        // if (Input.GetButtonDown("LockOn")) {
        //     if(isLockOn){
        //         isLockOn = false;
        //     }
        //     else{
        //         isLockOn = true;
        //     }
        // }
        if(target!=null){
            LockEnemy();
        }else{
            if(cursor!=null)cursor.transform.position = new Vector3(0, 50, 0);
            Quaternion angle = camera.transform.rotation;
            this.transform.rotation = angle;
        }
   }

    private void LockEnemy(){
        transform.LookAt(target.transform);
        Vector3 pos = target.transform.position;
            cursor.transform.position = new Vector3(pos.x, pos.y+3f, pos.z);
            cursor.transform.LookAt(this.gameObject.transform);
        
    }

    // public GameObject getTarget(){
    //     return this.target;
    // }
}
}