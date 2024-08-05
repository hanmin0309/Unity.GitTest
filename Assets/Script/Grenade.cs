using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rb;
    private CameraShake cameraShake;

    void Start()
    {
        //cameraShake = Camera.main.GetComponent<CameraShake>();
        cameraShake = CameraManager.Instance.playerFollowCamera.GetComponent<CameraShake>();
        StartCoroutine(Explosion());
    }


    IEnumerator Explosion()
    {
        Debug.Log("수류탄 투척");
        yield return new WaitForSeconds(3f);
        //이동속도
        rb.velocity = Vector3.zero;
        //회전속도
        rb.angularVelocity = Vector3.zero;

        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 
                                                     15, 
                                                     Vector3.up, 
                                                     0f, 
                                                     LayerMask.GetMask("Enemy"));

        foreach(RaycastHit hitObj in rayHits)
        {
            Debug.Log(hitObj.transform.name);

            try
            {
                // Attempt to get the Enemy component and call HitByGrenade if it exists
                Enemy enemy = hitObj.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.HitByGrenade(transform.position);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to process Enemy: {e.Message}");
            }

            try
            {
                // Attempt to get the Slime component and call HitByGrenade if it exists
                Slime slime = hitObj.transform.GetComponent<Slime>();
                if (slime != null)
                {
                    slime.HitByGrenade(transform.position);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to process Slime: {e.Message}");
            }
            //hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
            //hitObj.transform.GetComponent<Slime>().HitByGrenade(transform.position);
        }

       

        Destroy(gameObject, 5);

        if (cameraShake != null)
        {
            Debug.Log("카메라 움직임");
            StartCoroutine(cameraShake.Shake(0.5f, 5.0f));
        }
    }

  
}
