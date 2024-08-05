using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range};
    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;

    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;
    public Player player;
    public Enemy enemy;

    private void Start()
    {
        
    }
    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }

        else if(type == Type.Range && curAmmo > 0 && player.equipWeaponIndex == 4)
        {
            curAmmo--;
            StopCoroutine("newClear");
            StartCoroutine("newClear");
        }

        
        else if (type == Type.Range && curAmmo >0)
        {
            curAmmo--;
            StopCoroutine("Shot");
            StartCoroutine("Shot");
           
        }
    }

    IEnumerator Swing()
    {
        //yield break;

        //1
        yield return new WaitForSeconds(0.4f); //1������ ��� == 0.1�� ���
        if (player.equipWeaponIndex != 4)
        { 
            meleeArea.enabled = true;
            trailEffect.enabled = true;
        }
        //2�������� �ٽý���
        yield return new WaitForSeconds(0.3f); //1������ ���
        meleeArea.enabled = false;
        

        //3�������� �ٽý���
        yield return new WaitForSeconds(0.3f);//1������ ���
        trailEffect.enabled = false;

    }

    IEnumerator Shot()
    {
        //#1.�Ѿ˹߻�
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        if (player.equipWeaponIndex != 4)
        { 
            Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = bulletPos.forward * 50;
            yield return null;
        
            //#2.ź�ǹ���
            GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
            Rigidbody CaseRigid = intantCase.GetComponent<Rigidbody>();
            Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
            CaseRigid.AddForce(caseVec, ForceMode.Impulse);
            CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

            if(player.equipWeaponIndex == 4)
            {
                Destroy(intantBullet);
            }
        }
        
    }

    IEnumerator newClear()
    {
        //#1.�Ѿ˹߻�
        yield return new WaitForSeconds(3f);
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();

        yield return null;

    }



    //             Use() ���η�ƾ -> Swing()ȣ�� == �����ƾ -> �ٽ� Use()���η�ƾ
    //�ڷ�ƾ�� ���  Use() ���η�ƾ + Swing() �ڷ�ƾ (Co-Op)
}
