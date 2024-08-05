using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
   

    public enum Type {A,B,C,D,S};
    public Type enemyType;
    public int maxHealth;
    public float curHealth;
    public int score;
    public GameMng manager;
    public Transform target;
    public bool isChase;
    public bool isAttack;
    public bool isDead;
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0, 2.2f,0);

    private Canvas uiCanvas;
    private Image hpBarImage;
    
    public GameObject[] coins;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public Rigidbody rb;
    public BoxCollider bc;
    public MeshRenderer[] meshs;
    public NavMeshAgent nav;
    public Animator anim;
    private GameObject hpBar;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();   
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
       
         if (enemyType != Type.D)
            Invoke("ChaseStart", 2);

    }

    void Start()
    {

        if (enemyType == Type.A || enemyType == Type.B || enemyType == Type.C)
            SetHpBar();
    }
    void ChaseStart()
    {
        isChase = true;
        if (enemyType == Type.A || enemyType == Type.B || enemyType == Type.C )
        anim.SetBool("isWalk", true);
    }
    void Update()
    {
       

        if (isDead)
        {
            StopAllCoroutines();
            return;
        }

        if (nav.enabled && enemyType != Type.D)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
        
    }
    void SetHpBar()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        var _hpbar = hpBar.GetComponent<EnemyHpBar>();
        _hpbar.targetTr = this.gameObject.transform;
        _hpbar.offset = hpBarOffset;
    }

    public void FreezeVelocity()
    {
        if (isChase) 
        { 
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
      
        FreezeVelocity();

        Targeting();

    }

    void Targeting()
    {
        if(!isDead && enemyType != Type.D)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;

                case Type.B:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;

                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;

                case Type.S:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;

            }

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position,
                                     targetRadius,
                                     transform.forward,
                                     targetRange,
                                     LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                if (enemyType == Type.A || enemyType == Type.B || enemyType == Type.C )
                    StartCoroutine(Attack());
            }
        }
        
    }
    //16분 33초
    IEnumerator Attack()
    {
        if (enemyType == Type.A || enemyType == Type.B || enemyType == Type.C ) 
        { 
            isChase = false;
            isAttack = true;
            //Debug.Log("Starting attack animation");
            anim.SetBool("isAttack", true);
        }


        

        switch (enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);

                break;

            case Type.B:
                yield return new WaitForSeconds(0.1f);
                rb.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rb.velocity = Vector3.zero;
                meleeArea.enabled = false;
                break;

            case Type.C:
                yield return new WaitForSeconds(0.4f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rbBullet = instantBullet.GetComponent<Rigidbody>();
                rbBullet.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;

           
        }

        isChase = true;
        isAttack = false;
        Debug.Log("Ending attack animation");
        anim.SetBool("isAttack", false);

    

        

    }

    void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            if (enemyType == Type.A || enemyType == Type.B || enemyType == Type.C || enemyType == Type.S)
            {
                if (hpBarImage != null)
                    hpBarImage.fillAmount = curHealth / maxHealth;
            }
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, false));
            
        }
        else if (other.tag == "Bullet" || other.tag == "newClear")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            if (enemyType == Type.A || enemyType == Type.B || enemyType == Type.C || enemyType == Type.S)
            {
                if(hpBarImage!= null)
                    hpBarImage.fillAmount = curHealth / maxHealth;
            }
            Vector3 reactVec = transform.position - other.transform.position;
            
            if(other.tag == "Bullet")
            {
                Destroy(other.gameObject);
            }
            
            else if (other.tag == "newClear")
            {
                 Destroy(other.gameObject , 5f);
            }
            Debug.Log("총알명중");
            StartCoroutine(OnDamage(reactVec, false));

        }
       
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec , true));

    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach(MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;


        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;

        //    reactVec = reactVec.normalized;
        //    reactVec += Vector3.up;

        //    rb.AddForce(reactVec * 5, ForceMode.Impulse);
        }

        else
        {

            foreach (MeshRenderer mesh in meshs)
                if (enemyType == Type.A || enemyType == Type.B || enemyType == Type.C)
                    mesh.material.color = Color.gray;

            gameObject.layer = 12;
            isDead = true;
            isChase = false;
            nav.enabled = false;
            if(enemyType == Type.A || enemyType == Type.B || enemyType == Type.C)
                anim.SetTrigger("doDie");
            else if(enemyType == Type.S)
                foreach (MeshRenderer mesh in meshs)
                    mesh.material.color = Color.green;

            if (manager == null)
                manager = GameObject.Find("Game Mng").GetComponent<GameMng>();
            switch (enemyType)
            {
                case Type.A:
                    manager.enemyCntA--;
                    break;
                case Type.B:
                    manager.enemyCntB--;
                    break;
                case Type.C:
                    manager.enemyCntC--;
                    break;
                
                case Type.D:
                    manager.enemyCntD--;
                    break;
            }

            Destroy(gameObject);

            Player player = GameObject.Find("Player").GetComponent<Player>();
            Player Ludo = GameObject.Find("Ludo").GetComponent<Player>();

            if (player || Ludo)
            {
                player.score += score;
            }
            //코인 생성구간
            int ranCoin = Random.Range(0, 3);
            Instantiate(coins[ranCoin], transform.position, Quaternion.identity);
           

            if (isGrenade) 
            { 
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3.0f;

                rb.freezeRotation = false;
                rb.AddForce(reactVec * 5, ForceMode.Impulse);
                rb.AddTorque(reactVec * 15, ForceMode.Impulse);
                reactVec += -transform.forward;
            }
            Quaternion jumpRotation = Quaternion.LookRotation(-transform.forward + Vector3.up * 2.0f);
            rb.rotation = jumpRotation;

            rb.AddForce(reactVec * 5, ForceMode.Impulse);

            Destroy(gameObject, 4);

            
        }
    }

   public void TakeDamage(float damage)
    {
        curHealth -= damage;
    }

    public void ApplyTickDamage(float damagePerTick, float duration, float tickInterval)
    {
        StartCoroutine(TickDamageCoroutine(damagePerTick, duration, tickInterval));
    }

    private IEnumerator TickDamageCoroutine(float damagePerTick, float duration, float tickInterval)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
            elapsedTime += tickInterval;
        }
    }

}
