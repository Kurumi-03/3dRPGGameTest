using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState{
    Stand,//常态
    Patrol,//巡逻
    Trace,//追击
    Attack//攻击
}

public class EnemyController : MonoBehaviour
{

    private Animator animator;
    private NavMeshAgent agent;
    private Controller player;
    private EnemyState enemyState;

    private float standTimer;//静止计时器
    public float standTime = 2;//静止等待时间

    public Transform[] wayPoints;//巡逻点
    private int pointIndex = 0;//巡逻点序号
    private Vector3 pointPos;//当前巡逻位置

    public float patrolSpeed;//巡逻速度

    private void Awake() {
        animator = transform.GetChild(0).GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
        agent = transform.GetComponent<NavMeshAgent>();
        enemyState = EnemyState.Stand;//默认为常态
    }
    private void Update() {
        switch(enemyState){
            case EnemyState.Stand:
                Standing();
                break;
            case EnemyState.Patrol:
                Patroling();
                break;
            case EnemyState.Trace:
                Tracing();
                break;
            case EnemyState.Attack:
                Attacking();
                break;
        }
    }

    public void ChangeState(EnemyState state){
        enemyState = state;
    }

    private void Standing(){
        animator.SetBool("bWalk",false);
        
        standTimer += Time.deltaTime;
        if(standTimer >= standTime){
            standTimer = 0;
            ChangeState(EnemyState.Patrol);//切换为巡逻
        }
    }

    private void Patroling(){
        if(pointPos == Vector3.zero){
            //当巡逻点序号大于数组时，将序号归零
            if(pointIndex >= wayPoints.Length){
                pointIndex = 0;
            }
            pointPos = wayPoints[pointIndex++].position; //进入下一个巡逻点 得到数值后++

            agent.speed = patrolSpeed;//巡逻速度
            agent.destination = pointPos;//巡逻终点
            agent.stoppingDistance = 0.8f;//达到终点时的判定距离
            
            animator.SetBool("bWalk",true);//播放移动动画
            animator.SetBool("bAttack",false);
        }
        //当前位置到巡逻点的模长 小于 判定距离时
        if(Vector3.Distance(transform.position,pointPos) <= agent.stoppingDistance){
            pointPos = Vector3.zero;//将终点重置
            ChangeState(EnemyState.Stand);//将状态切换为静止
        }

    }

    private void Tracing(){
        pointPos = player.transform.position;//将玩家位置实时设为巡逻点
        if(Vector3.Distance(transform.position,pointPos) <= agent.stoppingDistance){
            pointPos = Vector3.zero;
            ChangeState(EnemyState.Attack);
            return;
        }
        
        agent.speed = patrolSpeed;//巡逻速度
        agent.destination = pointPos;//巡逻终点
        agent.stoppingDistance = 1.5f;//达到终点时的判定距离

        animator.SetBool("bWalk",true);//播放移动动画
    }

    private void Attacking(){
        //当怪物与玩家的位置大于判定距离时
        if(Vector3.Distance(transform.position,player.transform.position) >= agent.stoppingDistance){
            ChangeState(EnemyState.Trace);//继续追踪
            animator.SetBool("bWalk",true);
            animator.SetBool("bAttack",false);
            return;
        }
        animator.SetBool("bWalk",false);
        animator.SetBool("bAttack",true);//播放攻击动画
        
    }
}
