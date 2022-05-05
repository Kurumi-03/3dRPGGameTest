using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum emScreenEffect{
    //从全黑到透明
    FaderToClear,
    //从透明到全黑
    FaderToBlack,
    None
}

public class ScreenEffect : MonoBehaviour
{
    private Image screenFader;
    private emScreenEffect effectType;

    public Button button;
    private Text txt;
    private Controller player;
    private float temp;
    public float faderSpeed;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
        button.gameObject.SetActive(false);
        txt = button.transform.GetChild(0).GetComponent<Text>();
        screenFader = transform.GetChild(0).GetComponent<Image>();
        effectType = emScreenEffect.None;
        //Debug.Log("游戏开启");
        StartCoroutine(SetFader());
    }

    private void Update() {
        if(effectType == emScreenEffect.FaderToClear){
            FaderToClear();
        }
        else if(effectType == emScreenEffect.FaderToBlack){
            FaderToBlack();
        }
    }

    private void FaderToClear(){
        temp = Mathf.Lerp(screenFader.color.a , 0 , Time.deltaTime * faderSpeed);
        screenFader.color = new Color(screenFader.color.r,screenFader.color.g,screenFader.color.b,temp);
        if(screenFader.color.a <= 0.001f){
            //变为透明时
            //Debug.Log("变透明");
            effectType = emScreenEffect.None;//防止持续调用
        }
    }

    private void FaderToBlack(){
        temp = Mathf.Lerp(screenFader.color.a , 1 , Time.deltaTime * faderSpeed);
        screenFader.color = new Color(screenFader.color.r,screenFader.color.g,screenFader.color.b,temp);
        if(1 - screenFader.color.a <= 0.001f){
            //变为全黑时
            effectType = emScreenEffect.None;
            //重新加载场景
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator SetFader(){
        yield return new WaitForSeconds(2f);
        //Debug.Log("开启协程");
        effectType = emScreenEffect.FaderToClear;
    }

    public void OnClick(){
        effectType = emScreenEffect.FaderToBlack;
        button.gameObject.SetActive(false);
    }

    public void ShowResult(){
        if(player.hasKey){
            txt.text = "闯关成功";
        }
        else if(player.hp <= 0){
            txt.text = "闯关失败";
        }
        button.gameObject.SetActive(true);
    }
}
