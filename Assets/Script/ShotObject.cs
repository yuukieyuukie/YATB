using UnityEngine;
using Status;

public class ShotObject : MonoBehaviour
{
    public float shot_speed;
    protected Vector3 forward;
    protected Quaternion forwardAxis;
    protected Rigidbody rb;
    protected GameObject characterObject;
    public float range;
    private float timer;
    public int damage;

    private GameObject metalImpacts;
    public GameObject metalImpactsPrefab;
    [SerializeField]
    public Vector3 metalImpactsScale;

    void Awake(){

    }

    void Start(){
        rb = this.GetComponent<Rigidbody>();
 
        forward = characterObject.transform.forward;
        timer = 0f;

    }
 
    void Update(){
        rb.velocity = forwardAxis * forward * shot_speed;

        timer += Time.deltaTime;//弾の存在時間カウント
        if(timer >= (range/shot_speed)){//射程限界超えたら
            Destroy(this.gameObject);
        }
    }

    public void SetCharacterObject(GameObject characterObject){
        this.characterObject = characterObject;
    }

    //allway
    public void SetForward4Way(Quaternion axis){
        this.forwardAxis = axis;
    }

    //1way
    public void SetForward(Vector3 axis){
        this.forward = axis;
        this.transform.LookAt(axis);
    }

    public Vector3 GetForward(){
        return this.forward;
    }

    void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponent<EnemyStatus>().TakeDamage(damage);
			Destroy(this.gameObject);
            CallImpactEffect(gameObject.transform);
		}else if(col.gameObject.tag == "Metal") {
            //col.gameObject.GetComponent<MetalImpacts>().CallImpactEffect(gameObject.transform);
            Destroy(this.gameObject,0.5f);
            CallImpactEffect(gameObject.transform);
		}
	}

    public void CallImpactEffect(Transform hitPos){
        metalImpacts = Instantiate(metalImpactsPrefab, transform.position, transform.rotation);
        metalImpacts.transform.localPosition = hitPos.localPosition;
        metalImpacts.transform.localScale = metalImpactsScale;
    }
}
