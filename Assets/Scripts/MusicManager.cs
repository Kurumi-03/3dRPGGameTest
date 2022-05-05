using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager _instance;

    public AudioSource normal;
    public AudioSource warn;
    public bool iswarn;

    public AudioSource[] megaphone;//喇叭物体的引用

    private void Awake() {
        _instance = this;
    }

    private void Update() {
        if(iswarn){
            if(normal.isPlaying){
                normal.Stop();
            }
            if(!warn.isPlaying){
                warn.Play();
            }

            for(int i = 0;i < megaphone.Length;i++){
                if(!megaphone[i].isPlaying)
                    megaphone[i].Play();
            }
        }
    }


}
