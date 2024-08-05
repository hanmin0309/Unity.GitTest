using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;


public class Select : MonoBehaviour
{
    public GameObject creat; //player이름을 입력하는 창이 뜨겠금 하는 변수.
    public Text[] slotText;
    public Text newPlayerName;

    bool[] savefile = new bool[3];

    void Start()
    {
        for (int i =0; i<3; i++) 
        { 
        //slot별로 저장된 데이터가 존재하는지 판단
            if(File.Exists(DataManager.Instance.path + $"{i}"))
            {
                savefile[i] = true;
                DataManager.Instance.nowSlot = i;
                DataManager.Instance.LoadData();
                slotText[i].text = DataManager.Instance.nowPlayer.name;
                
            }
            else
            {
                slotText[i].text = "비어있음";
            }
        }
        DataManager.Instance.DataClear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //slot 3ea how do that?
    public void slot(int number)
    {
        DataManager.Instance.nowSlot = number;
        
        if (savefile[number]) // save파일 있을경우 데이터를 이어서 사용
        {
            DataManager.Instance.LoadData();
            GoGame();
        }
        else // 없을경우 데이터를 새로 생성
        {
            Creat();
        }
        
        //2.저장된 데이터가 있을때 => 불러오기해서 게임씬으로 넘어가기
        
       
    }

    public void Creat()
    {
        creat.gameObject.SetActive(true);
    }

    public void GoGame()
    {
        if (!savefile[DataManager.Instance.nowSlot])
        {
            DataManager.Instance.nowPlayer.name = newPlayerName.text;
            DataManager.Instance.SaveData();
        }
        
        //위 코드처럼 진행하게 되면 원래nowPlayer에 있던 데이터가 날아가고 newPlayer로 덮어지는 오류발생

        SceneManager.LoadScene(1);
    }
}
