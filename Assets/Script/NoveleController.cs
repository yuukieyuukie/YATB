using UnityEngine;
using System.Collections;

public class NoveleController : MonoBehaviour{
    public GameObject player; // 玉のオブジェクト
    public GameObject headLight;
    public GameObject muzzle;

    private Vector3 offset; // 玉からの距離

    private Vector3 prevPlayerPos;
    private Vector3 posVector;
    public float scale;
    public float noveleSpeed;

    void Start(){
        offset = transform.position - player.transform.position;
        prevPlayerPos = new Vector3 (0, 0, -1);
    }

    void LateUpdate(){
        transform.Rotate(new Vector3(0, 30* Time.deltaTime, 0) );
        moveNovele();
    }

    //常にボールの背後に付いて追っかける
    private void moveNovele(){
        Vector3 currentPlayerPos = player.transform.position;
        Vector3 backVector = (prevPlayerPos - currentPlayerPos).normalized;
        posVector = (backVector == Vector3.zero) ? posVector : backVector;
        Vector3 targetPos = currentPlayerPos + scale * posVector;
        targetPos.x = targetPos.x + 1.5f;
        targetPos.z = targetPos.z + 1.5f;
        targetPos.y = targetPos.y + 3.5f;
        this.transform.position = Vector3.Lerp (
            this.transform.position,
            targetPos,
            noveleSpeed * Time.deltaTime
        );
        //this.transform.LookAt (player.transform.position);
        prevPlayerPos = player.transform.position;
    }
}