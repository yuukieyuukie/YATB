using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyinput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        
        Animator anim = GetComponent<Animator>();
        if(Input.GetKey("up"))
        {
            transform.position += transform.forward * 0.2f;
            anim.SetBool("is_running", true);
        }
        else
        {
            anim.SetBool("is_running", false);
        }
        
        if (Input.GetKey("right"))
        {
            transform.Rotate(0, 2.0f, 0 ,Space.World);
        }
        
        if (Input.GetKey("left"))
        {
            transform.Rotate(0, -2.0f, 0, Space.World);
        }
    }
}