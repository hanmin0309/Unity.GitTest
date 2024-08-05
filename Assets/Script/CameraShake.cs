using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform target; // 타겟 참조
    public Vector3 offset; // 카메라와 타겟 사이의 거리
    public CinemachineVirtualCamera virtualCamera; // 가상 카메라 참조

    private bool isShaking = false;
    private CinemachineBasicMultiChannelPerlin noise;

    void Start()
    {
        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        trackPlayer();
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        if (noise == null) yield break;

        isShaking = true;
        float elapsed = 0.0f;

        noise.m_AmplitudeGain = magnitude;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        noise.m_AmplitudeGain = 0f;
        isShaking = false;
    }

    private void LateUpdate()
    {
        trackPlayer();
    }

    public void TriggerShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    public void targetset(Transform newTarget)
    {
        target = newTarget;
        trackPlayer();
    }

    public void trackPlayer()
    {
        if (target == null) return;

        Transform currentTarget = target;
        virtualCamera.transform.position = currentTarget.position + offset;
    }
}
