using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineSample{
    
    public enum BossState{
        Wander,
        Aim,
        AttackThrow,
        AttackTackle,
        Release,
        Explode,
    }
    
    public class MoveBoss : StatefulObjectBase<MoveBoss, BossState>{
        
        private GameObject sceneChanger; //他オブジェクトのコンポーネントを取り込む
        private Transform player;
        private Rigidbody rbBoss, rbChild1, rbChild2, rbChild3;
        private Vector3 localGravity;
        private GameObject releasePoint,releasePoint1,releasePoint2,releasePoint3,releasePoint4;
        private GameObject ballChild1;
        private GameObject ballChild2;
        private GameObject ballChild3;
        private List<GameObject> childList = new List<GameObject>();

        private float speedWander = 5f;
        private float speedWanderChild = 6f;
        private float speedAimChild = 4f;
        private float rotationSmooth = 3.5f;
        private float rotationSmoothWanderChild = 5f;
        private float rotationSmoothAimChild = 4f;
        private float speedThrow = 20f;
        private float speedTackle = 30f;
        
        void Start(){
            Initialize();
        }

        public void Initialize(){
            player = GameObject.FindWithTag("Player").transform;

            // ステートマシンの初期設定
            stateList.Add(new StateWander(this));
            stateList.Add(new StateAim(this));
            stateList.Add(new StateAttackThrow(this));
            stateList.Add(new StateAttackTackle(this));
            stateList.Add(new StateRelease(this));
            stateList.Add(new StateExplode(this));

            stateMachine = new StateMachine<MoveBoss>();
            rbBoss = GetComponent<Rigidbody>();
            localGravity = new Vector3(0f, -10f, 0f);
            sceneChanger = GameObject.Find("UIManager");
            releasePoint1 = GameObject.Find("ReleasePoint1");
            releasePoint2 = GameObject.Find("ReleasePoint2");
            releasePoint3 = GameObject.Find("ReleasePoint3");
            releasePoint4 = GameObject.Find("ReleasePoint4");
            ballChild1 = GameObject.Find("BallChild1");
            ballChild2 = GameObject.Find("BallChild2");
            ballChild3 = GameObject.Find("BallChild3");
            childList.Add(ballChild1);
            childList.Add(ballChild2);
            childList.Add(ballChild3);
            rbChild1 = ballChild1.GetComponent<Rigidbody>();
            rbChild2 = ballChild2.GetComponent<Rigidbody>();
            rbChild3 = ballChild3.GetComponent<Rigidbody>();

            ChangeState(BossState.Wander);
        }

        public void changeStateExplode(){
            ChangeState(BossState.Explode);
        }

        #region States

        /// <summary>
        /// ステート: 徘徊
        /// </summary>
        private class StateWander : State<MoveBoss>{
                        
            private float moveDistance;
            private float moveMaxDistance;

            private Vector3 ownerWkPos;

            public StateWander(MoveBoss owner) : base(owner) {}

            public override void Enter(){
                //動く距離・動く拠点を決定
                moveDistance = 0f;
                moveMaxDistance = Random.Range( 4.0f, 8.0f );
                switch(Random.Range(1, 5)){
                    case 1:
                    owner.releasePoint = owner.releasePoint1;
                    break;
                    case 2:
                    owner.releasePoint = owner.releasePoint2;
                    break;
                    case 3:
                    owner.releasePoint = owner.releasePoint3;
                    break;
                    case 4:
                    owner.releasePoint = owner.releasePoint4;
                    break;
                }

                ownerWkPos = owner.transform.position;
                ownerWkPos.y += 3.0f;
                owner.transform.position = ownerWkPos;

                //Debug.Log("releasePoint " + owner.releasePoint.transform.position);
                //Debug.Log("owner.transform.position " + owner.transform.position);
            }

            public override void Execute(){
                //一定距離動いたらAimに遷移
                moveDistance+=Time.deltaTime;
                if(moveDistance>moveMaxDistance){
                    owner.ChangeState(BossState.Aim);
                }
                moveBoss();
                moveChild();
            }

            public override void Exit(){
                //Aimに遷移直後の慣性で動かないようにする
                owner.rbBoss.velocity = Vector3.zero;
                owner.rbChild1.velocity = Vector3.zero;
                owner.rbChild2.velocity = Vector3.zero;
                owner.rbChild3.velocity = Vector3.zero;
            }

            private void moveBoss(){
                // 目標地点の方向を向く
                Quaternion targetRotation = Quaternion.LookRotation(owner.releasePoint.transform.position - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);
                // 前方に進む
                owner.transform.Translate(Vector3.forward * owner.speedWander * Time.deltaTime);
            }

            private void moveChild(){
                float i = 6f;
                foreach (var child in owner.childList){
                    // 目標地点の方向を向く
                    Quaternion targetRotation = Quaternion.LookRotation(owner.transform.position - child.transform.position);
                    child.transform.rotation = Quaternion.Slerp(child.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmoothWanderChild);
                    // 前方に進む
                    child.transform.Translate(i * Vector3.forward * owner.speedWanderChild * Time.deltaTime);
                    i+=2f;
                }
            }

        }

        /// <summary>
        /// ステート: 狙い
        /// </summary>
        private class StateAim : State<MoveBoss>{

            private float moveDistance;
            private float moveMaxDistance;

            public StateAim(MoveBoss owner) : base(owner) {}

            public override void Enter(){
                //動く距離・動く拠点を決定
                moveDistance = 0f;
                moveMaxDistance = Random.Range( 2.0f, 4.0f );
            }
            
            public override void Execute(){
                //一定距離動いたらAttackThrowに遷移
                moveDistance+=Time.deltaTime;
                if(moveDistance>moveMaxDistance){
                   owner.ChangeState(BossState.AttackThrow);
                }
                owner.transform.LookAt (owner.player.position);
                
                moveChild();

            }
            
            public override void Exit(){
                //投げに遷移する直前に子をプレイヤーに向かせる
                foreach (var child in owner.childList){
                    child.transform.LookAt (owner.player.position);
                }
                //LookAt直後にオブジェクトに何か当たる等で回転してしまう現象を抑制
                owner.rbChild1.constraints = RigidbodyConstraints.FreezeRotation;
                owner.rbChild2.constraints = RigidbodyConstraints.FreezeRotation;
                owner.rbChild3.constraints = RigidbodyConstraints.FreezeRotation;
            }

            private void moveChild(){
                float i = 4f;
                foreach (var child in owner.childList){
                    // 目標地点の方向を向く
                    Quaternion targetRotation = Quaternion.LookRotation(owner.transform.position - child.transform.position);
                    child.transform.rotation = Quaternion.Slerp(child.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmoothAimChild);
                    // 前方に進む
                    child.transform.Translate(i * Vector3.forward * owner.speedAimChild * Time.deltaTime);
                    i+=1.5f;
                }
            }

        }

        /// <summary>
        /// ステート: 攻撃[投げ]
        /// </summary>
        private class StateAttackThrow : State<MoveBoss>{

            private float throwInterval;
            Ray ray, chRay;        //レイ
            RaycastHit hit, chHit; //ヒットしたオブジェクト情報

            public StateAttackThrow(MoveBoss owner) : base(owner) {}

            public override void Enter(){
                throwInterval = 0f;
                owner.rbChild1.velocity = Vector3.zero;
                owner.rbChild2.velocity = Vector3.zero;
                owner.rbChild3.velocity = Vector3.zero;
            }
            
            public override void Execute(){
                
                drawRay();
                moveChild();
                
            }
            
            public override void Exit(){
                
            }

            private void drawRay(){
                //レイの設定
                ray = new Ray(owner.transform.position, owner.transform.TransformDirection(Vector3.forward));
                //レイキャスト（原点, 飛ばす方向, 衝突した情報, 長さ）
                if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
                    Debug.DrawRay(owner.transform.position, owner.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                }

                foreach (var child in owner.childList){
                    //レイの設定
                    chRay = new Ray(child.transform.position, child.transform.TransformDirection(Vector3.forward));
                    //レイキャスト（原点, 飛ばす方向, 衝突した情報, 長さ）
                    if (Physics.Raycast(chRay, out chHit, Mathf.Infinity)){
                        Debug.DrawRay(child.transform.position, child.transform.TransformDirection(Vector3.forward) * chHit.distance, Color.red);
                    }
                }
            }

            private void moveChild(){
                throwInterval += Time.deltaTime;
                if(0.5f<throwInterval && throwInterval<0.9f){
                    owner.ballChild1.transform.LookAt (owner.player.position);
                    if(!owner.rbChild1.useGravity){
                        owner.rbChild1.useGravity = true;
                        owner.rbChild1.velocity = owner.rbChild1.transform.forward * owner.speedThrow;
                        owner.rbChild1.constraints = RigidbodyConstraints.None;
                    }
                }else if(0.9f<=throwInterval && throwInterval<1.3f){
                    owner.ballChild2.transform.LookAt (owner.player.position);
                    if(!owner.rbChild2.useGravity){
                        owner.rbChild2.useGravity = true;
                        owner.rbChild2.velocity = owner.rbChild2.transform.forward * owner.speedThrow;
                        owner.rbChild2.constraints = RigidbodyConstraints.None;
                    }
                }else if(1.3f<=throwInterval && throwInterval<1.7f){
                    owner.ballChild3.transform.LookAt (owner.player.position);
                    if(!owner.rbChild3.useGravity){
                        owner.rbChild3.useGravity = true;
                        owner.rbChild3.velocity = owner.rbChild3.transform.forward * owner.speedThrow;
                        owner.rbChild3.constraints = RigidbodyConstraints.None;
                    }
                }else if(3.0f<=throwInterval){
                    owner.ChangeState(BossState.AttackTackle);
                }
            }

        }
        
        /// <summary>
        /// ステート: 攻撃[突撃]
        /// </summary>
        private class StateAttackTackle : State<MoveBoss>{

            private float moveDistance;
            private float moveMaxDistance;
            
            public StateAttackTackle(MoveBoss owner) : base(owner) {}

            public override void Enter(){
                moveDistance = 0f;
                moveMaxDistance = Random.Range( 5.0f, 6.0f );
                owner.rbBoss.useGravity = true;
                // float distance = Vector3.Distance(owner.transform.position, owner.wkPlayerPos); 距離はこれでとれる
            }
            
            public override void Execute(){
                owner.rbBoss.AddForce (owner.localGravity, ForceMode.Acceleration); //穴に落としやすくする
                
                if(moveDistance==0f){
                    owner.transform.LookAt(owner.player.position);
                    owner.rbBoss.velocity = owner.transform.forward * owner.speedTackle;
                }

                //一定距離動いたらWanderに遷移 床にぶつかり一定時間したら
                moveDistance+=Time.deltaTime;
                if(moveDistance>moveMaxDistance){
                    owner.ChangeState(BossState.Release);
                }

            }
            
            public override void Exit(){
                
                //Releaseに遷移直後の慣性で動かないようにする
                owner.rbBoss.velocity = Vector3.zero;
                owner.rbChild1.velocity = Vector3.zero;
                owner.rbChild2.velocity = Vector3.zero;
                owner.rbChild3.velocity = Vector3.zero;
            }

        }


        /// <summary>
        /// ステート: 元に戻る
        /// </summary>
        private class StateRelease : State<MoveBoss>{

            private float releaseInterval;

            public StateRelease(MoveBoss owner) : base(owner) {}

            public override void Enter(){
                //Debug.Log("[Release] in.");
                releaseInterval = 0f;
                owner.rbBoss.constraints = RigidbodyConstraints.FreezeRotation;
                owner.rbChild1.constraints = RigidbodyConstraints.FreezeRotation;
                owner.rbChild2.constraints = RigidbodyConstraints.FreezeRotation;
                owner.rbChild3.constraints = RigidbodyConstraints.FreezeRotation;
            }
            
            public override void Execute(){

                releaseInterval += Time.deltaTime;
                
                if(0f <= releaseInterval && releaseInterval < 1.5f){
                    //owner.transform.position = owner.releasePoint.transform.position + new Vector3(0f,0f,0f);
                }else if(1.5f <= releaseInterval && releaseInterval < 3f){
                    //foreach (var child in owner.childList){
                    //  child.transform.position = owner.transform.position;
                    //}
                }else{
                    owner.ChangeState(BossState.Wander);
                }
            }
            
            public override void Exit(){
                owner.rbBoss.useGravity = false;
                owner.rbChild1.useGravity = false;
                owner.rbChild2.useGravity = false;
                owner.rbChild3.useGravity = false;
            }
        }
        

        /// <summary>
        /// ステート: 爆発
        /// </summary>
        private class StateExplode : State<MoveBoss>{



            public StateExplode(MoveBoss owner) : base(owner) {}

            public override void Enter(){
                Debug.Log("撃破！");
                StageUIManager suim = owner.sceneChanger.GetComponent<StageUIManager>();
                suim.setCurrentScreen(StageUIScreen.GameClear);
                owner.rbBoss.useGravity = true;
                owner.rbChild1.useGravity = true;
                owner.rbChild2.useGravity = true;
                owner.rbChild3.useGravity = true;
            }
            
            public override void Execute(){

            }
            
            public override void Exit(){

            }
        }


        #endregion
    }
}