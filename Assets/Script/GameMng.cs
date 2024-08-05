using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameMng : MonoBehaviour
{
    //========= 이부분 게임 선택스크립트 =======//
    public GameObject[] characters; // 배열로 게임캐릭터를 안에 넣을수 있게 만듦
    public GameObject activeCharacter; // 게임캐릭터 자체를 껐다 켰다 하게 만드는 변수
    public Orbit[] orbits;
    public CameraShake cameraShake;

    //public int playerId; // 시작화면에서 캐릭터 선택할때 필요한 부분 = 변수

    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startZone;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntS;
    public int enemyCntD;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;

    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weapon4Img;
    public Image weapon5Img;
    public Image weaponRImg;

    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;
    public Text enemySTxt;

    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;
    public Text curScoreText;
    public Text bestText;

    void Awake()
    {
        enemyList = new List<int>();
        //maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));

        if (PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);
    }

    void Start()
    {
        // Ensure all characters are initially inactive
   
    }

    public void GameStart(int id)
    {
       

        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        //player.gameObject.SetActive(true);
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }

        // Optionally activate the first character
        if (characters.Length >= 0)
        {
            SetActiveCharacter(0);
        }
     
    }

    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreText.text = scoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if(player.score > maxScore)
        {
            bestText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void StageStart()
    {
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZone.SetActive(false);

        foreach(Transform zone in enemyZones)
           zone.gameObject.SetActive(true);
        

        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageOver()
    {
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZone.SetActive(false);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(true);


        isBattle = true; // 
        StartCoroutine(InBattle());
    }

    IEnumerator InBattle()
    {   
        if(stage % 5 == 0)
        {
            enemyCntD++;
            GameObject instanteEnemy = Instantiate(enemies[4], enemyZones[0].position, enemyZones[0].rotation);
            Enemy enemy = instanteEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.manager = this;
            boss = instanteEnemy.GetComponent<Boss>();
        }
        else
        {
            //for문은 몬스터 ran으로 1,2,3 번째중 무엇을 소환할지 랜덤가챠
            for (int index = 0; index < stage; index++)
            {
                int ran = Random.Range(0, 4);
               
                enemyList.Add(ran);

                switch (ran)
                {
                    case 0:
                        enemyCntA++;
                        break;
                    case 1:
                        enemyCntB++;
                        break;
                    case 2:
                        enemyCntC++;
                        break;
                    case 3:
                        Scount(1);
                        break;
                    case 4:
                        enemyCntD++;
                        break;
                }
            }

            //적이 섬멸될때까지 동작 : 
            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 4);
                if(enemyList.Count > 0) 
                {  
                    GameObject instanteEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
                    if (instanteEnemy.GetComponent<Enemy>()!=null)
                    {
                        Enemy enemy = instanteEnemy.GetComponent<Enemy>();
                        enemy.target = player.transform;
                        enemy.manager = this;
                        enemyList.RemoveAt(0);
                        
                    }
                    else
                    {
                        Slime enemy = instanteEnemy.GetComponent<Slime>();
                        enemy.target = player.transform;
                        enemy.manager = this;
                        enemyList.RemoveAt(0);
                       //Debug.Log("asdf");
                    }

                    yield return new WaitForSeconds(4f);
                }
              
            }



        }

        while (enemyCntA + enemyCntB + enemyCntC + enemyCntD + enemyCntS > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(4f);
        boss = null;
        StageEnd();
    }

    public void StageEnd()
    {
        player.transform.position = Vector3.up * 0.8f;

        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZone.SetActive(true);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(false);

        isBattle = false;
        enemyCntA = 0;
        enemyCntB = 0;
        enemyCntC = 0;
        enemyCntS = 0;
        Debug.Log("스테이지 종료");

    stage++;
    }



    void Update()
    {
        if (isBattle)
        {
            playTime += Time.deltaTime;
        }   
    }

    void LateUpdate()
    {
        if (player == null) return;  // player가 설정되지 않으면 UI 갱신 안 함

        //상단 UI
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "STAGE " + stage;
        playTimeTxt.text = "";

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);

        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);

        //플레이어 UI
        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);

        if (player.equipWeapon == null)
            playerAmmoTxt.text = " - / " + player.ammo;

        else if (player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoTxt.text = " - / " + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;

        //무기 UI
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weapon4Img.color = new Color(1, 1, 1, player.hasWeapons[3] ? 1 : 0);
        weapon5Img.color = new Color(1, 1, 1, player.hasWeapons[4] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        //몬스터 UI
        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();
        enemySTxt.text = enemyCntS.ToString();

        //보스 UI
        if (boss != null)
        {
            bossHealthGroup.anchoredPosition = Vector3.down * 30;
            bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
        }
        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 200;
        }
    }
    public void Scount(int count)
    {
       enemyCntS += count;
    }

    public void SetActiveCharacter(int index)
    {
        if (index < 0 || index >= characters.Length)
        {
            Debug.LogError("Invalid character index");
            return;
        }

        if (activeCharacter != null)
        {
            activeCharacter.SetActive(false);
        }

        activeCharacter = characters[index];
        activeCharacter.SetActive(true);

        // player 변수에 현재 활성화된 캐릭터 설정
        player = activeCharacter.GetComponent<Player>();

        foreach (Orbit orbit in orbits)
        {
            orbit.targetset(characters[index].transform);
        }

        if (cameraShake != null)
        {
            cameraShake.targetset(characters[index].transform);
        }
    }

}
