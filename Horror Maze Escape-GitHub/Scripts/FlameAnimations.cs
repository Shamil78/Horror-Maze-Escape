using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlameAnimations : MonoBehaviour{
    public int LightMode;
    public GameObject FlameLight;

    void Update (){
        if (LightMode == 0){
            StartCoroutine (AnimateLight ());
        }
    }
    IEnumerator AnimateLight(){
        LightMode = Random.Range (1,4);
        if(LightMode == 1){
            FlameLight.GetComponent<Animation>().Play ("Animation1");
        }
        if(LightMode == 2){
            FlameLight.GetComponent<Animation>().Play ("Animation2");
        }
        if(LightMode == 3){
            FlameLight.GetComponent<Animation>().Play ("Animation3");
        }
        yield return new WaitForSeconds(0.99f);
        LightMode = 0;
    }                       
}