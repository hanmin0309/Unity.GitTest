using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;
    public bool isRock;
    public bool isNewclear;
    private void OnCollisionEnter(Collision collision)
    {
        

        
        //Debug.Log(collision.gameObject.tag);
        if (!isNewclear&&!isRock && collision.gameObject.tag == "Floor")
        {
            
            Destroy(gameObject, 5f);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Wall" && !isMelee && !isNewclear )
        {
            Destroy(gameObject , 5f);
        }
    }

    
}
