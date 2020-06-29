using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour{
    
    RectTransform rectTransform2;

    void Start(){
        rectTransform2 = this.GetComponent<RectTransform>();
    }

    void Update(){
        
    }

    public void Shake( float duration, float magnitude ){
        if(this.gameObject.activeSelf){
            StartCoroutine( DoShake( duration, magnitude ) );
        }
    }

    private IEnumerator DoShake( float duration, float magnitude ){
        
        var pos = rectTransform2.localPosition;
        var elapsed = 0f;

        while( elapsed < duration ){
            var x = pos.x + Random.Range( -1f, 1f ) * magnitude;
            var y = pos.y + Random.Range( -1f, 1f ) * magnitude;

            rectTransform2.localPosition = new Vector3(x, y, pos.z);
            elapsed += Time.deltaTime;

            yield return null;

        }

        rectTransform2.localPosition = pos;
    }
}