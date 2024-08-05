using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //input,output 어딘가로 데이터를 넣거나 빼올때 사용함.

//slot별로 다르게 저장해야함.

//저장하는 방법
//1. 저장할 데이터가 존재
//2. 데이터를 제이슨으로 변환해줄 작업
//3. 제이슨을 외부에 저장

// 불러오는 방법
//1. 외부에 저장된 제이슨 가져옴
//2. 제이슨을 데이터형태로 변환
//3. 불러온 데이터를 사용

public class PlayerData
{
    //이름, 레벨, 코인, 착용중인무기(추가적으로 넣어야될것 : 몬스터(A,B,C,D),스테이지,시간,총알수) == 1.내부데이터생성
    public string name;
    public int level;
    public int coin;
    public int item;

}
public class DataManager : MonoBehaviour
{
    //싱글톤
    public static DataManager Instance;

    public PlayerData nowPlayer = new PlayerData();
    //경로를 뽑아내기위한 변수
    public string path;
    
    public int nowSlot;

    private void Awake()
    {
        #region 싱글톤
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

    //저장기능 함수 SaveData
    public void SaveData()
    {
        //2.내부 데이터 Json화 하기
        string data = JsonUtility.ToJson(nowPlayer);

        //3.외부 데이터저장
        File.WriteAllText(path + nowSlot.ToString(), data);
    }
    //불러오기기능 함수 LoadData
    public void LoadData()
    {
        //외부에 저장된 Json파일 데이터를 끌고옴
        string date = File.ReadAllText(path + nowSlot.ToString());
        nowPlayer = JsonUtility.FromJson<PlayerData>(date);
    }

    public void DataClear()
    {
        nowSlot = -1;
        nowPlayer = new PlayerData();
    }
}
