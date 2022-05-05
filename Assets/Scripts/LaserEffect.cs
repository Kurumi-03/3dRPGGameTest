using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum enLaserEffect{
    Normal,
    Auto
}
public class LaserEffect : MonoBehaviour
{
    public enLaserEffect effectType;//设置一个枚举类型表示激光门的类型

    private float timer;//计时器
    public float time;//间隔时间
    private MeshRenderer mRenderer;
    private Collider mCollider;
    private EnemyController enemy;

    private void Awake() {
        mRenderer = GetComponent<MeshRenderer>();
        mCollider = GetComponent<Collider>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
    }

    private void Update() {

        if(effectType == enLaserEffect.Normal)
            return;
            //间隔time时间显示或隐藏
        else if(effectType == enLaserEffect.Auto){
            timer += Time.deltaTime;
            if(timer >= time){
                mRenderer.enabled = !mRenderer.enabled;
                mCollider.enabled = !mCollider.enabled;
                timer = 0;
            }
        }
    } 

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag != "Player"){
            return;
        }
        ProcessWarning();
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag != "Player"){
            return;
        }
        ProcessWarning();
    }

    private void ProcessWarning(){
        LightEffect._instance.isWarn = true;
        MusicManager._instance.iswarn = true;

        //怪物追击
        enemy.ChangeState(EnemyState.Trace);
    }
}
