using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

//using UnityEngine.AI;
using UnityEngine.UI;

public class Slime : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject activeCharacter; // ����ĳ���� ��ü�� ���� �״� �ϰ� ����� ����
    public enum Type { A, B, C, D, S };
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
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    //public Player player { get; private set; } // ���� Ȱ��ȭ�� ĳ������ Player ������Ʈ
    


    private Canvas uiCanvas;
    private Image hpBarImage;

    public GameObject[] coins;
    public BoxCollider meleeArea;
    public GameObject bullet;
    private Rigidbody rb;
    public BoxCollider bc;
    public MeshRenderer[] meshs;
    //public NavMeshAgent nav;
    public Animator anim;
    private GameObject hpBar;
    public GameObject slimeObject;
    public Player player;
    
    GameObject slime;
    public bool isSplit = true;
    //public float jumpHeight = 5f; // ���� ����
    //public float jumpDistance = 5f; // ���� �Ÿ�
    //public float followDistance = 10f; // �÷��̾���� �Ÿ�
    //public float jumpCooldown = 2f; // ���� ��� �ð�
    //private bool isJumping = false;
    //private float lastJumpTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        //nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        //if (enemyType != Type.D)
        //    Invoke("ChaseStart", 2);

    }
    void Start()
    {
        if (this.manager == null)
            this.manager = GameObject.Find("Game Mng").GetComponent<GameMng>();

        if (enemyType == Type.S)
            SetHpBar();

        manager = GameObject.Find("Game Mng").GetComponent<GameMng>();
        player = GetComponent<Player>();
    }

    void Update()
    {





        if (curHealth <= 0 && isSplit)
        {

            isSplit = false;
            Split();

            manager.Scount(-1); // ��ü ������ ī��Ʈ�� ����
            Destroy(gameObject);

        }
    }
    //void ChaseStart()
    //{
    //    isChase = true;
    //    StartCoroutine(SlimeMove());
    //}

    //IEnumerator SlimeMove()
    //{
    //    while (!isDead)
    //    {
    //        if (isChase)
    //        {
    //            nav.enabled = false; // �׺���̼� ��Ȱ��ȭ
    //            JumpTowardsTarget(); // ������ ����
    //            yield return new WaitForSeconds(2f); // ���� �� ���� �ð� ���
    //        }
    //        //else
    //        //{
    //        //    nav.enabled = true; // �׺���̼� Ȱ��ȭ
    //        //}
    //        yield return new WaitForSeconds(1f); // ���� �������� ��� �ð�
    //    }
    //}

    //private void JumpTowardsTarget()
    //{
    //    if (target == null) return;

    //    isJumping = true;
    //    nav.enabled = false; // NavMeshAgent ��Ȱ��ȭ

    //    Vector3 jumpDirection = (target.position - transform.position).normalized;
    //    jumpDirection.y = 5f; // ���� ���� ����

    //    rb.AddForce(jumpDirection, ForceMode.Impulse); // ���� �� �߰�

    //    StartCoroutine(ResetAgentAfterJump(1f));
    //}

    //private IEnumerator ResetAgentAfterJump(float duration)
    //{
    //    yield return new WaitForSeconds(duration);
    //    nav.enabled = true; // NavMeshAgent �ٽ� Ȱ��ȭ
    //    isJumping = false;
    //}
    // Update is called once per frame

    //private void FollowPlayer()
    //{
    //    if (target == null) return;

    //    // �÷��̾���� �Ÿ� ���
    //    float distanceToPlayer = Vector3.Distance(transform.position, target.position);

    //    // �÷��̾���� �Ÿ��� ������ �Ÿ� �̳��� �� ����
    //    if (distanceToPlayer <= followDistance && !isJumping && Time.time >= lastJumpTime + jumpCooldown)
    //    {
    //        JumpTowardsPlayer();
    //    }
    //    else if (!isJumping)
    //    {
    //        // �������� ���� �� �÷��̾ ���� �̵�
    //        Vector3 direction = (target.position - transform.position).normalized;
    //        rb.MovePosition(transform.position + direction * Time.deltaTime * 2f); // �������� �÷��̾�� ����
    //    }
    //}

    //private void JumpTowardsPlayer()
    //{
    //    isJumping = true;
    //    lastJumpTime = Time.time; // ������ ���� �ð� ���

    //    // �÷��̾� �������� ����
    //    Vector3 jumpDirection = (target.position - transform.position).normalized;
    //    jumpDirection.y = jumpHeight; // ���� �������� ���� ���� ����

    //    // ���� ���� ���� �����Ͽ� ������ ����� ������ ����
    //    Vector3 horizontalForce = jumpDirection * jumpDistance;
    //    horizontalForce.y = 0; // Y�� ���� ����

    //    rb.AddForce(horizontalForce + Vector3.up * jumpHeight, ForceMode.Impulse); // ���� �� �߰�

    //    // ���� �� ���� �ð� �� �ٽ� ���� �������� ����
    //    StartCoroutine(ResetJump());
    //}

    //private IEnumerator ResetJump()
    //{
    //    yield return new WaitForSeconds(0.5f); // ���� �� ��� �ð�

    //}
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
        if (!isDead && enemyType != Type.D)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyType)
            {
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


        }

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            if (enemyType == Type.S)
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
            if (enemyType == Type.S)
            {
                if (hpBarImage != null)
                    hpBarImage.fillAmount = curHealth / maxHealth;
            }
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject, 5f);

            Debug.Log("�Ѿ˸���");

            StartCoroutine(OnDamage(reactVec, false));



        }

    }

    void Split()
    {

        // �������� 4������ �п��˴ϴ�.
        for (int i = 0; i < 4; i++)
        {

            GameObject newSlime = Instantiate(gameObject, transform.position, Quaternion.identity);

            newSlime.transform.localScale = transform.localScale / 2;
            newSlime.GetComponent<Slime>().curHealth = maxHealth / 2; // �� �����ӵ��� ü�� ����
            newSlime.GetComponent<Slime>().manager = manager; // �Ŵ��� ����
            newSlime.GetComponent<Slime>().target = target; // Ÿ�� ����

            manager.Scount(1);

        }
        isSplit = false;
        Destroy(gameObject);
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));

    }

    IEnumerator Attack()
    {
       
            switch (enemyType)
            {
                case Type.S:
                    yield return new WaitForSeconds(0.2f);
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(1f);
                    meleeArea.enabled = false;

                    yield return new WaitForSeconds(1f);

                    break;




            }


    }
    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach (MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;


        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = new Color(167 / 255f, 255 / 255f, 0 / 255f);

            //    reactVec = reactVec.normalized;
            //    reactVec += Vector3.up;

            //    rb.AddForce(reactVec * 5, ForceMode.Impulse);
        }

        else
        {
            gameObject.layer = 12;
            isDead = true;
            isChase = false;
            //nav.enabled = false;

            if (enemyType == Type.S)
                foreach (MeshRenderer mesh in meshs)
                    mesh.material.color = new Color(167 / 255f, 255 / 255f, 0 / 255f);

            if (manager == null)
                manager = GameObject.Find("Game Mng").GetComponent<GameMng>();

            switch (enemyType)
            {

                case Type.S:
                    manager.Scount(-1);
                    break;

            }

            Destroy(gameObject);


            //manager.SetActiveCharacter(0);
            //GameMng player = activeCharacter.GetComponent<Player>();
            //activeCharacter = characters[index];
            //character =  SetActiveCharacter();
            //Player player = gameObject.GetComponent<Player>();


            Player quad = GameObject.Find("Player").GetComponent<Player>();
            Player Ludo = GameObject.Find("Ludo").GetComponent<Player>();


            //player = [quad, Ludo];
            if (player || Ludo)
            {
                player.score += score;
            }
               
        
            //���� ��������
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


    //public void SetActiveCharacter(int index)
    //{
    //    if (index < 0 || index >= characters.Length)
    //    {
    //        Debug.LogError("Invalid character index");
    //        return;
    //    }

    //    if (activeCharacter != null)
    //    {
    //        activeCharacter.SetActive(false);
    //    }

    //    activeCharacter = characters[index];
    //    activeCharacter.SetActive(true);
    //}
}
