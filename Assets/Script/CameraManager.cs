using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject playerFollowCamera;
    public GameObject playerHandCamera;

    private static CameraManager instance = null;
    void Awake()
    {
        if (null == instance)
        {            
            instance = this;          
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {           
            Destroy(this.gameObject);
        }
    }

    //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ���� ȣ���� �� �ִ�.
    public static CameraManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
}
