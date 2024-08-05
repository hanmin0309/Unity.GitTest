using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
//using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon;

    //움직임 속도
    public float speed;

    //총알,동전,체력,수류탄(필살기)변수 생성
    public int ammo;
    public int coin;
    public int health;
    public int score;
    public int hasGrenandes;
    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxhasGrenades;
    //public AudioClip clip;

    //카메라 변수
    public Camera followCamera;


    //플레이어의 무기관련 배열함수
    public GameObject[] grenades;
    public int hasGrenades;
    public GameObject grenadeObj;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameMng manager;
    //움직임 Horizontal, Vertical
    float hAxis;
    float vAxis;

    //움직임 발동
    bool wDown;

    //점프 발동 변수
    bool jDown;
    bool isJump;

    //회피 발동 변수
    bool isDodge;

    //아이템(획득) 변수
    bool iDown;


    //스왑하는 변수(1,2,3,4번 장비)
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool sDown4;
    bool sDown5;

    //스왑할때 멈추는변수
    bool isSwap;

    //공격하는 변수 선언
    bool fDown;

    //재장전관련변수
    bool rDown;

    //수류탄 변수 선언
    bool gDown;

    //공격준비완료 변수선언
    bool isFireReady = true;

    //재장전 관련 변수 선언
    bool isReload;

    //벽충돌 플래그 번수선언
    bool isBorder;

    //무적타임 변수선언
    bool isDamage;

    //상점 관련 변수
    bool isShop;
    //플레이어 피격 효과
    MeshRenderer[] meshs;

    bool isDead;



    //오브젝트 저장변수
    GameObject nearObject;

    //무기오브젝트 저장할 변수
    public Weapon equipWeapon;
    public Weapon tickDamage;
    public AudioSource jumpSound;
   
    

    //이미 들고있는 무기
    public int equipWeaponIndex = -1;

    //공격딜레이 변수
    float fireDelay;


    Vector3 moveVec;
    Vector3 dodgeVec;


    Rigidbody rb;

    Animator anim;

    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();

       
    }

    void OnEnable()
    {
        //anim.runtimeAnimatorController = animCon[GameMng.instance.PlayerId];
    }

    void Update()
    {
        
        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
        Reload();
        Dodge();
        Interation();
        Swap();
        Grenade();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButton("Fire1");
        //수류탄 눌림
        gDown = Input.GetButtonDown("Fire2");
        rDown = Input.GetButtonDown("Reload");
        iDown = Input.GetButtonDown("Interation");

        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
        sDown4 = Input.GetButtonDown("Swap4");
        sDown5 = Input.GetButtonDown("Swap5");
    }

    void Move()
    {

        //normalized => 모든값이 1로 보정 및 고정됨
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (isDodge)
            moveVec = dodgeVec;

        if (isSwap || isReload || !isFireReady || isDead)
            moveVec = Vector3.zero;

        if (!isBorder)
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;



        //isRun이 발동할거면 방향좌표(0,0,0)이 아닐때
        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        //#회전구현 LookAt == 나아가는 방향으로 바라본다.
        transform.LookAt(transform.position + moveVec);

        //#마우스에 의한 회전
        if (fDown && !isDead)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }

    }

    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge && !isSwap && !isDead)
        {
            rb.AddForce(Vector3.up * 20, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;

            
        }
    }

    void Grenade()
    {
        if (hasGrenades == 0)
            return;

        if (gDown && !isReload && !isSwap && !isDead)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 10;

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
            }


        }
    }

    void Attack()
    {
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isDodge && !isSwap && !isShop && !isJump && !isDead)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }

    void Reload()
    {
        if (equipWeapon == null)
            return;

        if (equipWeapon.type == Weapon.Type.Melee)
            return;

        if (ammo == 0)
            return;

        if (equipWeapon.curAmmo == equipWeapon.maxAmmo)
            return;

        if (rDown && !isJump && !isDodge && !isSwap && isFireReady && !isShop && !isDead)
        {
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 2f);
        }
    }

    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }
    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap && !isShop && !isDead)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;
            Invoke("DodgeOut", 0.5f);
        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;
        if (sDown4 && (!hasWeapons[3] || equipWeaponIndex == 3))
            return;
        if (sDown5 && (!hasWeapons[4] || equipWeaponIndex == 4))
            return;

        int weaponIndex = -1;

        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;
        if (sDown4) weaponIndex = 3;
        if (sDown5) weaponIndex = 4;


        if ((sDown1 || sDown2 || sDown3 || sDown4 || sDown5) && !isJump && !isDodge && !isShop && !isDead)
        {//스왑키 1,2,3 번눌려도 다 동작되겠끔 함수 구성
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            equipWeapon.player = this;

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {

        isSwap = false;


    }

    void Interation()
    {
        if (iDown && nearObject != null && !isJump && !isDodge && !isDead)
        {//아이템줍기 그리고 주변에 nearObject가 있을때 발동하는 조건문 그리고 점프하지않고 구르지않을때 발동함
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);

            }
            else if (nearObject.tag == "Shop") 
            {
                //Debug.Log("상점");
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
                isShop = true;

            }

        }
    }

    void FreezeRotation()
    {
        rb.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;

                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;

                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;

                case Item.Type.Grenade:
                    if (hasGrenades == maxhasGrenades)
                        return;

                    hasGrenades += item.value;

                    grenades[hasGrenades - 1].SetActive(true);


                    if (hasGrenades > maxhasGrenades)
                        hasGrenades = maxhasGrenades;


                    break;
            }
            Destroy(other.gameObject);
        }

        else if (other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;


                bool isBossAttack = other.name == "Boss Melee Area";
                StartCoroutine(OnDamage(isBossAttack));
            }
            if (other.GetComponent<Rigidbody>() != null)
                Destroy(other.gameObject);

        }
    }

    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.red;
        }
        yield return new WaitForSeconds(1f);

        if (isBossAtk)
            rb.AddForce(transform.forward * -25, ForceMode.Impulse);

        if (health <= 0 && !isDead)
            OnDie();

        yield return new WaitForSeconds(1f);

        isDamage = false;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
        if (isBossAtk)
            rb.velocity = Vector3.zero;

        
    }

    void OnDie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
        manager.GameOver();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "Shop")
            nearObject = other.gameObject;

        //Debug.Log(nearObject.name);

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;

        else if (other.tag == "Shop")
        {
            if (nearObject != null && nearObject.TryGetComponent<Shop>(out Shop shop))
            {
                
                //Shop shop = nearObject.GetComponent<Shop>();
                shop.Exit();
                isShop = false;
                nearObject = null;
            }
        }
    }

    public void TickDamage(Weapon damage)
    {
        health -= tickDamage.damage;
    }
}


