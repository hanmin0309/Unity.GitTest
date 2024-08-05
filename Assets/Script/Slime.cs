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
    public GameObject activeCharacter; // 게임캐릭터 자체를 껐다 켰다 하게 만드는 변수
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
    //public Player player { get; private set; } // 현재 활성화된 캐릭터의 Player 컴포넌트
    


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
    //public float jumpHeight = 5f; // 점프 높이
    //public float jumpDistance = 5f; // 점프 거리
    //public float followDistance = 10f; // 플레이어와의 거리
    //public float jumpCooldown = 2f; // 점프 대기 시간
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

            manager.Scount(-1); // 본체 슬라임 카운트를 감소
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
    //            nav.enabled = false; // 네비게이션 비활성화
    //            JumpTowardsTarget(); // 슬라임 점프
    //            yield return new WaitForSeconds(2f); // 점프 후 착지 시간 대기
    //        }
    //        //else
    //        //{
    //        //    nav.enabled = true; // 네비게이션 활성화
    //        //}
    //        yield return new WaitForSeconds(1f); // 다음 점프까지 대기 시간
    //    }
    //}

    //private void JumpTowardsTarget()
    //{
    //    if (target == null) return;

    //    isJumping = true;
    //    nav.enabled = false; // NavMeshAgent 비활성화

    //    Vector3 jumpDirection = (target.position - transform.position).normalized;
    //    jumpDirection.y = 5f; // 점프 방향 설정

    //    rb.AddForce(jumpDirection, ForceMode.Impulse); // 점프 힘 추가

    //    StartCoroutine(ResetAgentAfterJump(1f));
    //}

    //private IEnumerator ResetAgentAfterJump(float duration)
    //{
    //    yield return new WaitForSeconds(duration);
    //    nav.enabled = true; // NavMeshAgent 다시 활성화
    //    isJumping = false;
    //}
    // Update is called once per frame

    //private void FollowPlayer()
    //{
    //    if (target == null) return;

    //    // 플레이어와의 거리 계산
    //    float distanceToPlayer = Vector3.Distance(transform.position, target.position);

    //    // 플레이어와의 거리가 설정된 거리 이내일 때 점프
    //    if (distanceToPlayer <= followDistance && !isJumping && Time.time >= lastJumpTime + jumpCooldown)
    //    {
    //        JumpTowardsPlayer();
    //    }
    //    else if (!isJumping)
    //    {
    //        // 점프하지 않을 때 플레이어를 향해 이동
    //        Vector3 direction = (target.position - transform.position).normalized;
    //        rb.MovePosition(transform.position + direction * Time.deltaTime * 2f); // 슬라임이 플레이어에게 접근
    //    }
    //}

    //private void JumpTowardsPlayer()
    //{
    //    isJumping = true;
    //    lastJumpTime = Time.time; // 마지막 점프 시간 기록

    //    // 플레이어 방향으로 점프
    //    Vector3 jumpDirection = (target.position - transform.position).normalized;
    //    jumpDirection.y = jumpHeight; // 수직 방향으로 점프 높이 설정

    //    // 수평 방향 힘을 조정하여 포물선 모양의 점프를 구현
    //    Vector3 horizontalForce = jumpDirection * jumpDistance;
    //    horizontalForce.y = 0; // Y축 성분 제거

    //    rb.AddForce(horizontalForce + Vector3.up * jumpHeight, ForceMode.Impulse); // 점프 힘 추가

    //    // 점프 후 일정 시간 후 다시 점프 가능으로 설정
    //    StartCoroutine(ResetJump());
    //}

    //private IEnumerator ResetJump()
    //{
    //    yield return new WaitForSeconds(0.5f); // 점프 후 대기 시간

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

            Debug.Log("총알명중");

            StartCoroutine(OnDamage(reactVec, false));



        }

    }

    void Split()
    {

        // 슬라임이 4마리로 분열됩니다.
        for (int i = 0; i < 4; i++)
        {

            GameObject newSlime = Instantiate(gameObject, transform.position, Quaternion.identity);

            newSlime.transform.localScale = transform.localScale / 2;
            newSlime.GetComponent<Slime>().curHealth = maxHealth / 2; // 새 슬라임들의 체력 설정
            newSlime.GetComponent<Slime>().manager = manager; // 매니저 설정
            newSlime.GetComponent<Slime>().target = target; // 타겟 설정

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
