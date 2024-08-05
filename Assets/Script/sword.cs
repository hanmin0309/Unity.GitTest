using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : Weapon
{

    public float tickDamagePerSecond = 1f; //초당 들어가는 데미지 1f
    public float tickDuration = 5f; //~초동안? 5초동안
    public float tickInterval = 1f; //간격ㄱ은 1초동안



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Immediate damage
                enemy.ApplyTickDamage(tickDamagePerSecond, tickDuration, tickInterval); // Apply tick damage
            }
        }
    }


}
