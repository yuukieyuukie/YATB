using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metal{
public class MetalImpacts : MonoBehaviour
{

    // public GameObject metalImpacts;
    // public GameObject metalImpactsPrefab;
    // [SerializeField]
    // public Vector3 metalImpactsScale;
    // private GameObject metalImpactsClone;

    private float totalTime;

    void Start(){
        // metalImpacts.SetActive(true);
        // metalImpactsClone = (GameObject)Instantiate(metalImpacts);
        // metalImpactsClone.name = metalImpacts.name;
    }

    void Update(){
        totalTime += Time.deltaTime;
        if(totalTime>5.0f){
            Destroy(this.gameObject);
        }
        //Debug.Log(totalTime);
    }

    // public void CallImpactEffect(Transform hitPos){
    //     metalImpacts = Instantiate(metalImpactsPrefab, transform.position, transform.rotation);
    //     metalImpacts.transform.localPosition = hitPos.localPosition;
    //     metalImpacts.transform.localScale = metalImpactsScale;
    // }
}
}
