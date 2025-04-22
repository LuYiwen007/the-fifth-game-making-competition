using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // 引用 Cinemachine Virtual Camera
    public float zoomedSize; // 放大后的相机大小（Orthographic Size 或 Field of View）
    public float zoomDuration; // 放大动画的持续时间
    private float originalSize; // 记录原始相机大小
    private bool isZooming = false; // 防止重复触发

    private void Start()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("未设置 Cinemachine Virtual Camera！");
            return;
        }

        // 获取相机的原始大小
        if (virtualCamera.m_Lens.Orthographic)
        {
            originalSize = virtualCamera.m_Lens.OrthographicSize;
        }
        else
        {
            originalSize = virtualCamera.m_Lens.FieldOfView;
        }
    }

    public void StartZoomIn()
    {
        if (!isZooming)
        {
            StartCoroutine(ZoomCamera(zoomedSize));
        }
    }

    public void StartZoomOut()
    {
        if (!isZooming)
        {
            StartCoroutine(ZoomCamera(originalSize));
        }
    }

    private IEnumerator ZoomCamera(float targetSize)
    {
        isZooming = true;
        float startSize = virtualCamera.m_Lens.Orthographic ? virtualCamera.m_Lens.OrthographicSize : virtualCamera.m_Lens.FieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            float newSize = Mathf.Lerp(startSize, targetSize, elapsedTime / zoomDuration);

            if (virtualCamera.m_Lens.Orthographic)
            {
                virtualCamera.m_Lens.OrthographicSize = newSize;
            }
            else
            {
                virtualCamera.m_Lens.FieldOfView = newSize;
            }

            yield return null;
        }

        if (virtualCamera.m_Lens.Orthographic)
        {
            virtualCamera.m_Lens.OrthographicSize = targetSize;
        }
        else
        {
            virtualCamera.m_Lens.FieldOfView = targetSize;
        }

        isZooming = false;
    }
}
