using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 10f;
    public float jumpCooldown = 2f;

    private Transform target;
    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(JumpRoutine());
    }

    void Update()
    {
        UpdateTarget();
    }

    void UpdateTarget()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject Ludo = GameObject.FindWithTag("Ludo");
        if (player != null)
        {
            target = player.transform;
        }
        else
            target = Ludo.transform;
    }

    IEnumerator JumpRoutine()
    {
        while (true)
        {
            if (isGrounded)
            {
                MoveTowardsTarget();
                Jump();
            }
            yield return new WaitForSeconds(jumpCooldown);
        }
    }

    void MoveTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        //direction.y = 0; // 수평 이동만 고려

        rb.AddForce(direction * moveSpeed, ForceMode.Impulse);
    }

    void Jump()
    {
        if (!isGrounded || target == null) return;

        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}

