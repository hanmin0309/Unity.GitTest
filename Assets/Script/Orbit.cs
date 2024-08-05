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
        //offset 은 즉 수류탄 - 주인공의 거리 = 사이의 간격
    }

    
    void Update()
    {
        if(target == null) return;
        transform.position = target.position + offset;
        //장소는 타켓의포지션 + 오프셋 즉 계속 그 간격만큼 업데이트해준다는소리.
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
