using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoorTrigger : MonoBehaviour
{
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    //进入范围时
    private void OnTriggerEnter(Collider other) {
        animator.SetBool("bOpen",true);
    }

    //离开范围时
    private void OnTriggerExit(Collider other) {
        animator.SetBool("bOpen",false);
    }
}
