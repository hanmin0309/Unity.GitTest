using Cinemachine;
using System.Collections;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas canvas;
    private RectTransform rectParent;
    private RectTransform rectHp;

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;

    public string virtualCameraName = "PlayerFollowCamera";

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();


        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null)
        {
            uiCamera = brain.OutputCamera;
        }

        if (uiCamera == null)
        {
            uiCamera = canvas.worldCamera;
        }

        if (uiCamera == null)
        {
            Debug.LogError("Camera not assigned.");
        }
    }

    void LateUpdate()
    {
        if (targetTr == null)
        {
            Destroy(gameObject);
            return;
        }

        var screenPos = uiCamera.WorldToScreenPoint(targetTr.position + offset);

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);

        rectHp.localPosition = localPos;
    }
}
