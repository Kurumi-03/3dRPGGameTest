using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitch : MonoBehaviour
{

    public GameObject[] lasers;//激光门的引用

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag != "Player"){
            return;
        }

        for(int i = 0;i < lasers.Length ;i++){
            lasers[i].SetActive(false);//将门关闭
        }
    }
}
