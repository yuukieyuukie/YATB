using UnityEngine;


public class SearchingBehavior : MonoBehaviour{
    private GameObject target;
    public event System.Action<GameObject>  onFound = ( obj ) => {};
    public event System.Action<GameObject>  onLost  = ( obj ) => {};
    public event System.Action<GameObject[]>  onStay  = ( obj ) => {};

    GameObject nearestEnemy = null;

    GameObject[] enemys;

    private GameObject messageUI;

    private const float LOCK_ON_DISTANCE = 30f;

    void Start(){
        messageUI = GameObject.Find("MessageUI");
    }

    void Update(){

    }

    private void OnTriggerEnter( Collider i_other ){
        if (i_other.gameObject.CompareTag("Enemy") || i_other.gameObject.CompareTag("Boss")){
            GameObject enterObject    = i_other.gameObject;
            onFound( enterObject );
            this.target = enterObject;
            MessageUIManager muim = messageUI.GetComponent<MessageUIManager>();
            muim.checkPlayerColType(PlayerColType.EnemyNear);
        }
    }

    private void OnTriggerExit( Collider i_other ){
        GameObject exitObject   = i_other.gameObject;
        onLost( exitObject );
        this.target = null;
        nearestEnemy = null;
        //Debug.Log("exit: "+exitObject.name);
    }
    
    private void OnTriggerStay( Collider i_other ){
        float minDis = LOCK_ON_DISTANCE;
        
        enemys = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemys!=null){
            foreach(GameObject enemy in enemys){
                float dis = Vector3.Distance(transform.position, enemy.transform.position);
                if(dis<minDis){
                    minDis=dis;
                    nearestEnemy=enemy;
                }
            }
        }
        
        enemys = GameObject.FindGameObjectsWithTag("Boss");

        if(enemys!=null){
            foreach(GameObject enemy in enemys){
                float dis = Vector3.Distance(transform.position, enemy.transform.position);
                if(dis<minDis){
                    minDis=dis;
                    nearestEnemy=enemy;
                }
            }
        }

        onStay( enemys );
    }

    public GameObject getTarget(){
        return this.nearestEnemy;
    }

}
