using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // ���� Cinemachine Virtual Camera
    public float zoomedSize; // �Ŵ��������С��Orthographic Size �� Field of View��
    public float zoomDuration; // �Ŵ󶯻��ĳ���ʱ��
    private float originalSize; // ��¼ԭʼ�����С
    private bool isZooming = false; // ��ֹ�ظ�����

    private void Start()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("δ���� Cinemachine Virtual Camera��");
            return;
        }

        // ��ȡ�����ԭʼ��С
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
