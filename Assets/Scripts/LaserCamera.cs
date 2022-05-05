using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCamera : MonoBehaviour
{
    private EnemyController enemy;
    private void Awake() {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag != "Palyer"){
            return;
        }
        LightEffect._instance.isWarn = true;
        MusicManager._instance.iswarn = true;
        enemy.ChangeState(EnemyState.Trace);
    }
}
