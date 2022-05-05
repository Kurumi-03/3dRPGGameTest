using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffect : MonoBehaviour
{
    public bool isWarn;
    public Light normalLight;
    public Light warnLight;

    public float time;
    private float timer;
    public static LightEffect _instance;//单例
    private void Awake() {
        _instance = this;
        isWarn = false;
    }

    private void Update() {
        if(isWarn){
            warnLight.intensity = 1.5f;//灯光强度
            timer += Time.deltaTime;
            if(timer > time){
                timer = 0;
                warnLight.enabled = !warnLight.enabled;
            }
        }

    }

}
