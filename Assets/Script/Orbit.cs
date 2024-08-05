using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Orbit : MonoBehaviour
{
    public Transform target;
    public float orbitSpeed;
    Vector3 offset;
    //public GameMng GM;
   
    void Start()
    {
       SetOffset();
        //offset �� �� ����ź - ���ΰ��� �Ÿ� = ������ ����
    }

    
    void Update()
    {
        if(target == null) return;
        transform.position = target.position + offset;
        //��Ҵ� Ÿ���������� + ������ �� ��� �� ���ݸ�ŭ ������Ʈ���شٴ¼Ҹ�.
        transform.RotateAround(target.position, 
                               Vector3.up, 
                               orbitSpeed*Time.deltaTime);

        offset = transform.position - target.position;
    }

    public void targetset(Transform newtarget)
    {
        target = newtarget;
        SetOffset();
    }

    public void SetOffset()
    {
        if (target == null) return;
        offset = transform.position - target.position;
    }
}
