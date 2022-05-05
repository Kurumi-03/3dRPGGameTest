using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private Controller player;
    private EnemyController enemy;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
    }
    private void TakeDamage(float damageValue){
        player.TakeDamage(enemy,damageValue);
    }
}
