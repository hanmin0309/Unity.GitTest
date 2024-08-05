using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //input,output ��򰡷� �����͸� �ְų� ���ö� �����.

//slot���� �ٸ��� �����ؾ���.

//�����ϴ� ���
//1. ������ �����Ͱ� ����
//2. �����͸� ���̽����� ��ȯ���� �۾�
//3. ���̽��� �ܺο� ����

// �ҷ����� ���
//1. �ܺο� ����� ���̽� ������
//2. ���̽��� ���������·� ��ȯ
//3. �ҷ��� �����͸� ���

public class PlayerData
{
    //�̸�, ����, ����, �������ι���(�߰������� �־�ߵɰ� : ����(A,B,C,D),��������,�ð�,�Ѿ˼�) == 1.���ε����ͻ���
    public string name;
    public int level;
    public int coin;
    public int item;

}
public class DataManager : MonoBehaviour
{
    //�̱���
    public static DataManager Instance;

    public PlayerData nowPlayer = new PlayerData();
    //��θ� �̾Ƴ������� ����
    public string path;
    
    public int nowSlot;

    private void Awake()
    {
        #region �̱���
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        DontDestroyOnLoad(Instance);
        #endregion

        //
        path = Application.persistentDataPath + "/save";
    }
    void Start()
    {
     
        //print(path);
        
    }

    //������ �Լ� SaveData
    public void SaveData()
    {
        //2.���� ������ Jsonȭ �ϱ�
        string data = JsonUtility.ToJson(nowPlayer);

        //3.�ܺ� ����������
        File.WriteAllText(path + nowSlot.ToString(), data);
    }
    //�ҷ������� �Լ� LoadData
    public void LoadData()
    {
        //�ܺο� ����� Json���� �����͸� �����
        string date = File.ReadAllText(path + nowSlot.ToString());
        nowPlayer = JsonUtility.FromJson<PlayerData>(date);
    }

    public void DataClear()
    {
        nowSlot = -1;
        nowPlayer = new PlayerData();
    }
}
