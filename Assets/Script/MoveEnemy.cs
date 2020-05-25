using UnityEngine;
using System.Collections;
using Effekseer;

namespace StateMachineSample{
    public enum EnemyState{
        Wander,
        Pursuit,
        Attack,
        Explode,
    }

    public class MoveEnemy : StatefulObjectBase<MoveEnemy, EnemyState>{
        // public Transform turret;
        // public Transform muzzle;
        // public GameObject bulletPrefab;
        public GameObject explosion;

        private Transform player;

        private float wanderSpeed = 6f;
        private float rotationSmooth = 2f;
        //private float turretRotationSmooth = 0.8f;
        //private float attackInterval = 2f;
        private float pursuitSpeed = 8f;
        private float attackSpeed = 30f;

        private float pursuitSqrDistance = 700f;
        private float attackSqrDistance = 400f;
        private float margin = 50f;

        private float changeTargetSqrDistance = 1000f;

        private Rigidbody rb;
        private GameObject prefabManager;
        private Vector3 localGravity = new Vector3(0f, -10f, 0f);
        
        [SerializeField]
        private GameObject flameEffect;
        private int dribbleFlameTime = 0;
        private bool dribbleFlameTimeFlg = false;

        [SerializeField]
        private AudioClip explodeAudio;
        private AudioSource explodeAudioSource;
        [SerializeField]
        private AudioClip hitAudio;
        private AudioSource hitAudioSource;

        private void Start(){
            Initialize();
        }

        private void FixedUpdate(){
            if(dribbleFlameTime==0 && dribbleFlameTimeFlg){
                flameEffect.SetActive(true);
                dribbleFlameTime++;
            }
            else if(dribbleFlameTime!=0 && dribbleFlameTimeFlg){
                dribbleFlameTime++;
                if(dribbleFlameTime>90){
                    flameEffect.SetActive(false);
                    dribbleFlameTimeFlg = false;
                    dribbleFlameTime = 0;
                }
            }

            rb.AddForce (localGravity, ForceMode.Acceleration);
        }

        public void Initialize(){
            explodeAudioSource = GetComponent<AudioSource>();
            hitAudioSource = GetComponent<AudioSource>();
            // 始めにプレイヤーの位置を取得できるようにする
            player = GameObject.FindWithTag("Player").transform;

            // ステートマシンの初期設定
            stateList.Add(new StateWander(this));
            stateList.Add(new StatePursuit(this));
            stateList.Add(new StateAttack(this));
            stateList.Add(new StateExplode(this));

            stateMachine = new StateMachine<MoveEnemy>();
            rb = GetComponent<Rigidbody>();
            prefabManager = GameObject.Find("PrefabSpawn");

            ChangeState(EnemyState.Wander);
        }

        public void changeStateExplode(){
            ChangeState(EnemyState.Explode);
        }

        #region States

        /// <summary>
        /// ステート: 徘徊
        /// </summary>
        private class StateWander : State<MoveEnemy>{
            private Vector3 targetPosition;

            public StateWander(MoveEnemy owner) : base(owner) {}

            public override void Enter(){
                targetPosition = GetRandomPositionOnLevel();// 始めの目標地点を設定する
            }

            public override void Execute(){
                
                // プレイヤーとの距離が小さければ、追跡ステートに遷移
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer <  owner.pursuitSqrDistance - owner.margin){
                    // Debug.Log("Change -> Pursuit");
                    owner.ChangeState(EnemyState.Pursuit);
                }

                // 目標地点との距離が小さければ、次のランダムな目標地点を設定する
                float sqrDistanceToTarget = Vector3.SqrMagnitude(owner.transform.position - targetPosition);
                if (sqrDistanceToTarget < owner.changeTargetSqrDistance){
                    targetPosition = GetRandomPositionOnLevel();
                }

                // 目標地点の方向を向く
                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

                // 前方に進む
                owner.transform.Translate(Vector3.forward * owner.wanderSpeed * Time.deltaTime);
            }

            public override void Exit() {}

            public Vector3 GetRandomPositionOnLevel(){
                return owner.prefabManager.GetComponent<PrefabManager>().getEnemyPosStage3(int.Parse(owner.gameObject.name.Replace("enemy","")));
            }
        }

        /// <summary>
        /// ステート: 追跡
        /// </summary>
        private class StatePursuit : State<MoveEnemy>{
            public StatePursuit(MoveEnemy owner) : base(owner) {}

            public override void Enter() {}

            public override void Execute(){
                // プレイヤーとの距離が小さければ、攻撃ステートに遷移
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer < owner.attackSqrDistance - owner.margin){
                    owner.dribbleFlameTimeFlg = true; //条件：敵種Toroll
                    // Debug.Log("Change -> Attack");
                    owner.ChangeState(EnemyState.Attack);
                }

                // プレイヤーとの距離が大きければ、徘徊ステートに遷移
                if (sqrDistanceToPlayer > owner.pursuitSqrDistance + owner.margin){
                    // Debug.Log("Change -> Wander");
                    owner.ChangeState(EnemyState.Wander);
                }

                // プレイヤーの方向を向く
                Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

                // 前方に進む
                owner.transform.Translate(Vector3.forward * owner.pursuitSpeed * Time.deltaTime);
            }

            public override void Exit() {}
        }

        /// <summary>
        /// ステート: 攻撃
        /// </summary>
        private class StateAttack : State<MoveEnemy>{
            private float lastAttackTime;

            public StateAttack(MoveEnemy owner) : base(owner) { }

            public override void Enter(){
                // プレイヤーの方向を向く
                Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);
                // 前方に進む
                owner.rb.AddForce(targetRotation * Vector3.forward * owner.attackSpeed, ForceMode.Impulse);
            }

            public override void Execute(){
                //プレイヤーとの距離が大きければ、追跡ステートに遷移
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer > owner.attackSqrDistance + owner.margin){
                    // Debug.Log("Change -> Pursuit");
                    owner.ChangeState(EnemyState.Pursuit);
                }
                
                // プレイヤーの方向を向く
                Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

                // 前方に進む
                //owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
                //owner.rb.velocity = targetRotation * Vector3.forward * owner.attackSpeed;

                // // 砲台をプレイヤーの方向に向ける
                // Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.turret.position);
                // owner.turret.rotation = Quaternion.Slerp(owner.turret.rotation, targetRotation, Time.deltaTime * owner.turretRotationSmooth);

                // // 一定間隔で弾丸を発射する
                // if (Time.time > lastAttackTime + owner.attackInterval)
                // {
                //     Instantiate(owner.bulletPrefab, owner.muzzle.position, owner.muzzle.rotation);
                //     lastAttackTime = Time.time;
                // }
            }

            public override void Exit() {}
        }

        /// <summary>
        /// ステート: 爆発
        /// </summary>
        private class StateExplode : State<MoveEnemy>{
            public StateExplode(MoveEnemy owner) : base(owner) {}

            public override void Enter(){
                owner.explodeAudioSource.PlayOneShot(owner.explodeAudio);
                // ランダムな吹き飛ぶ力を加える
                Vector3 force = (owner.transform.position - owner.player.position) * 200f + Random.insideUnitSphere * 300f;
                owner.GetComponent<Rigidbody>().AddForce(force);

                // // ランダムに吹き飛ぶ回転力を加える
                Vector3 torque = new Vector3(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
                owner.GetComponent<Rigidbody>().AddTorque(torque);
                owner.explosion.GetComponent<EffekseerEmitter>().Play();
                // 1秒後に自身を消去する
                Destroy(owner.gameObject, 1.0f);
                
            }

            public override void Execute() {}

            public override void Exit() {}
        }

        void OnCollisionEnter(Collision col){
            if(col.gameObject.tag == "Shot"){ //当たった
                hitAudioSource.PlayOneShot(hitAudio);
            }
        }

        #endregion

    }
}