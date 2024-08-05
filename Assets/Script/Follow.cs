using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ����� ���� �߰�

public class Follow : MonoBehaviour
{
    public Transform target1; // ù ��° Ÿ��
    public Transform target2; // �� ��° Ÿ��
    public Vector3 offset;

    private Transform currentTarget; // ���� Ÿ��

    void Start()
    {
        currentTarget = target1; // �ʱ� Ÿ�� ����
    }

    void Update()
    {
        if (currentTarget != null)
        {
            transform.position = currentTarget.position + offset; // ���� Ÿ���� ����
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼ҵ�
    public void SwitchTarget()
    {
        // ���� Ÿ���� ����
        currentTarget = (currentTarget == target1) ? target2 : target1;
    }
}
