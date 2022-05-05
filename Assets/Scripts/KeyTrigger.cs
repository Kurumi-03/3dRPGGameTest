using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : MonoBehaviour
{
    private Controller palyer;
    private EnemyController enemy;
    private ScreenEffect screenEffect;

    private void Awake() {
        palyer = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        screenEffect = GameObject.FindObjectOfType<ScreenEffect>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag != "Player"){
            return;
        }
        palyer.hasKey = true;
        enemy.ChangeState(EnemyState.Patrol);//怪物重新回到巡逻状态
        screenEffect.ShowResult();
        Destroy(gameObject);
    }
}
