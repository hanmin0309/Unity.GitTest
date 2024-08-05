using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 사용을 위해 추가

public class Follow : MonoBehaviour
{
    public Transform target1; // 첫 번째 타겟
    public Transform target2; // 두 번째 타겟
    public Vector3 offset;

    private Transform currentTarget; // 현재 타겟

    void Start()
    {
        currentTarget = target1; // 초기 타겟 설정
    }

    void Update()
    {
        if (currentTarget != null)
        {
            transform.position = currentTarget.position + offset; // 현재 타겟을 따라감
        }
    }

    // 버튼 클릭 시 호출되는 메소드
    public void SwitchTarget()
    {
        // 현재 타겟을 변경
        currentTarget = (currentTarget == target1) ? target2 : target1;
    }
}
