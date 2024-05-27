using UnityEngine;
using UnityEngine.UI;

public class CanvasMatch : MonoBehaviour
{
    CanvasScaler canvasScaler;
    float referenceAspect;

    void Start()
    {
        canvasScaler = transform.GetComponent<CanvasScaler>();
        referenceAspect = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;

        // 获取屏幕的宽度和高度
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 计算屏幕的宽高比
        float screenAspect = screenWidth / screenHeight;

        // 计算Match值
        float match = referenceAspect / screenAspect;

        // 设置Canvas Scaler的Match值
        canvasScaler.matchWidthOrHeight = match;
    }
}
