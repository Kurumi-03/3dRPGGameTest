using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Controller player;
    private Vector3 relativePos;
    private float magnitude;
    public float speed;//摄像机移动速度
    public float crotateSpeed;//摄像机旋转速度
    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
        relativePos = transform.position - player.transform.position;//相机位置减去玩家位置(向量加减法)
        magnitude = relativePos.magnitude;//获得向量长度
    }

    private void Update() {
        //transform.position = relativePos + player.transform.position;//相机位置等于玩家位置与相机位置的固定差值相加
        CalcPoints();
        //设定摄像机的方向
        Vector3 dir = player.transform.position - transform.position;
        Quaternion target = Quaternion.LookRotation(dir,Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation,target,Time.deltaTime * crotateSpeed);
    }

    private void CalcPoints(){
        Vector3 stanPos = player.transform.position + relativePos;//标准相机位置距离(起始点)
        Vector3 absovePos = player.transform.position + Vector3.up * magnitude;//得到玩家最顶部的点(终点)

        Vector3[] camPoints = new Vector3[5];//将起点与终点的连线等分为5个点
        camPoints[0] = stanPos;
        camPoints[1] = Vector3.Lerp(stanPos,absovePos,0.25f);
        camPoints[2] = Vector3.Lerp(stanPos,absovePos,0.50f);
        camPoints[3] = Vector3.Lerp(stanPos,absovePos,0.75f);
        camPoints[4] = absovePos;

        Vector3 newPos = Vector3.zero;//记录摄像机新位置
        RaycastHit hit;
        for(int i = 0;i < camPoints.Length;i++){
            //在每一点从摄像机位置发射射线，检测射线是否能碰到玩家
            if(Physics.Raycast(camPoints[i],player.transform.position - camPoints[i],out hit,magnitude)){
                //不是玩家  直接跳过
                if(hit.transform.tag != "Player"){
                    //修复摄像机找不到玩家时会丢失目标的问
                    if(i == camPoints.Length-1){
                        newPos = camPoints[camPoints.Length - 1];
                    }
                    continue;
                }
                //Debug.Log("射线点：" + i);
                newPos = camPoints[i];
                break;
            }
        }
        //摄像机位置的渐变效果
        transform.position = Vector3.Lerp(transform.position,newPos,Time.deltaTime * speed);
    }
}
