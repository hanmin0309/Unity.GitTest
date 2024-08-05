using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : Weapon
{

    public float tickDamagePerSecond = 1f; //�ʴ� ���� ������ 1f
    public float tickDuration = 5f; //~�ʵ���? 5�ʵ���
    public float tickInterval = 1f; //���ݤ��� 1�ʵ���



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
