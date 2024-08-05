using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;


public class Select : MonoBehaviour
{
    public GameObject creat; //player�̸��� �Է��ϴ� â�� �߰ڱ� �ϴ� ����.
    public Text[] slotText;
    public Text newPlayerName;

    bool[] savefile = new bool[3];

    void Start()
    {
        for (int i =0; i<3; i++) 
        { 
        //slot���� ����� �����Ͱ� �����ϴ��� �Ǵ�
            if(File.Exists(DataManager.Instance.path + $"{i}"))
            {
                savefile[i] = true;
                DataManager.Instance.nowSlot = i;
                DataManager.Instance.LoadData();
                slotText[i].text = DataManager.Instance.nowPlayer.name;
                
            }
            else
            {
                slotText[i].text = "�������";
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
        
        if (savefile[number]) // save���� ������� �����͸� �̾ ���
        {
            DataManager.Instance.LoadData();
            GoGame();
        }
        else // ������� �����͸� ���� ����
        {
            Creat();
        }
        
        //2.����� �����Ͱ� ������ => �ҷ������ؼ� ���Ӿ����� �Ѿ��
        
       
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
        
        //�� �ڵ�ó�� �����ϰ� �Ǹ� ����nowPlayer�� �ִ� �����Ͱ� ���ư��� newPlayer�� �������� �����߻�

        SceneManager.LoadScene(1);
    }
}
