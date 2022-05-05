using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType{
    mathfs,
    characterController,
    rigidBody
}

public enum enMathfs{
    mathf,
    move,
    translate
}

//推荐使用
public enum enCharacterController{
    move,
    sampleMove,

}

public enum enRigidBody{
    vcelocity,
    addForce,
    movePosition
}


public class Controller : MonoBehaviour
{
    public MoveType moveType;
    public enMathfs mathfs;
    public enCharacterController characterController;
    public enRigidBody rigidBody;

    public float speed;
    public float rotateSpeed;

    public CharacterController mcharacterController;
    public Rigidbody mrigidbody;
    private Animator animator;
    private AudioSource mAudio;
    private ScreenEffect screenEffect;
    public bool hasKey;
    public float hp;//玩家血量

    private void Awake() {
        hp = 100;
        animator = transform.Find("girl role").GetComponent<Animator>();
        mAudio = GetComponent<AudioSource>();
        screenEffect = GameObject.FindObjectOfType<ScreenEffect>();

        //moveType = MoveType.mathfs;
        if(moveType == MoveType.characterController){
            mcharacterController = transform.GetComponent<CharacterController>();
            if(mcharacterController == null){
                mcharacterController = transform.gameObject.AddComponent<CharacterController>();
                mcharacterController.center = new Vector3(0,0.65f,0);
                mcharacterController.radius = 0.3f;
                mcharacterController.height = 1.2f;
            }
        }
        else if(moveType == MoveType.rigidBody){
            mrigidbody = transform.GetComponent<Rigidbody>();
            if(mrigidbody == null){
                mrigidbody = transform.gameObject.AddComponent<Rigidbody>();
                CapsuleCollider collider = transform.gameObject.AddComponent<CapsuleCollider>();
                collider.center = new Vector3(0,0.5f,0);
                mrigidbody.constraints = RigidbodyConstraints.FreezeRotationX|
                                         RigidbodyConstraints.FreezeRotationY|
                                         RigidbodyConstraints.FreezeRotationZ|
                                         RigidbodyConstraints.FreezePositionY;
                mrigidbody.drag = 5f;//阻力 
            }
        }
        else if(moveType == MoveType.mathfs){
            CapsuleCollider collider = transform.gameObject.AddComponent<CapsuleCollider>();
            collider.center = new Vector3(0,0.55f,0);
        }
    }

    private void Update() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        animator.SetBool("bWalk",h != 0 || v !=0);

        if(h != 0 || v !=0){
            //开始移动
            //调整角色的正方向为当前移动方向
            Vector3 dir = new Vector3(h,0,v);
            Quaternion target = Quaternion.LookRotation(dir,Vector3.up);
            //transform.rotation = target;
            transform.rotation = Quaternion.Lerp(transform.rotation,target,Time.deltaTime * rotateSpeed);

            switch(moveType){
                case MoveType.mathfs:{
                    if(mathfs == enMathfs.mathf){
                        transform.position += transform.forward * Time.deltaTime * speed;
                    }
                    else if(mathfs == enMathfs.translate){
                        transform.Translate(dir*Time.deltaTime*speed,Space.Self);
                    }
                    else if(mathfs == enMathfs.move){
                        transform.position = Vector3.MoveTowards(transform.position,
                        transform.position + transform.forward * Time.deltaTime * speed,Time.deltaTime*speed);
                    }
                }

                break;
                case MoveType.characterController:{
                    //有阻挡物需求时
                    if(characterController == enCharacterController.move){
                        mcharacterController.Move( -transform.up * Time.deltaTime * speed);//模拟重力
                        mcharacterController.Move(transform.forward * Time.deltaTime * speed);//按帧移动
                    }
                    //有触发器接收事件
                    else if(characterController == enCharacterController.sampleMove){
                        mcharacterController.SimpleMove(transform.forward * speed);//按秒移动  有重力
                    }
                }
                break;
                case MoveType.rigidBody:{
                    //有阻挡需求
                    if(rigidBody == enRigidBody.vcelocity){
                        mrigidbody.velocity = transform.forward * speed;//按秒移动  需要在有重力的时候使用
                    }
                    //有阻力需求
                    else if(rigidBody == enRigidBody.addForce){
                        mrigidbody.AddForce(transform.forward * speed);//按秒移动   需要在有重力的时候使用  有一个加速度
                    }
                    //有碰撞器事件时
                    else if(rigidBody == enRigidBody.movePosition){
                        mrigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);//在还有重力时加上碰撞器有阻挡的效果,运动学模式下会无视碰撞效果
                    }
                }
 
                break;
            }
            if(!mAudio.isPlaying){
                mAudio.Play();
            }
        }
    }

    public void TakeDamage(EnemyController enemy,float damageValue){
        hp -= damageValue;
        if(hp <= 0){
            animator.SetBool("bDie",true);//死亡动画
            enemy.ChangeState(EnemyState.Patrol);//让怪物重新回到巡逻状态
            screenEffect.ShowResult();
        }
        else{
            animator.SetTrigger("bHurt");//受伤动画
        }
    }
}
