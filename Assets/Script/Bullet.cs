using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public float damage = 20f; 

    void OnCollisionEnter(Collision collision)
    {
        
        SlimeFSM slime = collision.gameObject.GetComponent<SlimeFSM>();
        if (slime != null)
        {
            slime.TakeDamage(damage); 
        }

        
        Destroy(gameObject);
    }
}

