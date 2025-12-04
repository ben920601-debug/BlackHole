using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("跟隨目標")]
    public Transform target;        // 您的黑洞
    public float smoothSpeed = 0.125f;
    public Vector3 offset;          // 保持的距離 (Z應該是 -10)

    [Header("動態變焦 (Dynamic Zoom)")]
    public float zoomFactor = 1.5f; // 縮放係數 (數字越大，鏡頭拉越遠)
    public float zoomSpeed = 2f;    // 鏡頭變焦的反應速度
    
    private Camera cam;             // 攝影機組件
    private float initialSize;      // 攝影機一開始的大小

    void Start()
    {
        cam = GetComponent<Camera>(); // 抓取攝影機組件
        initialSize = cam.orthographicSize; // 記住一開始的大小 (通常是 5)
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // --- 1. 位置跟隨 (原本的功能) ---
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // --- 2. 自動變焦 (新功能！) ---
            // 數學公式：目標視野 = 原本視野 + (黑洞現在的大小 - 黑洞原本的大小) * 係數
            // 這樣黑洞變大時，視野就會跟著等比例變大
            float currentScale = target.localScale.x; // 假設黑洞是圓的，取 X 軸大小即可
            
            // 這裡減 1 是假設黑洞初始大小是 1 (如果您初始設為 3，這裡可以改成 3)
            // 但為了通用，我們用動態計算增量的方式比較簡單：
            float sizeIncrease = (currentScale - 1f) * zoomFactor; 
            float targetSize = initialSize + sizeIncrease;

            // 確保視野不會小於原本的大小
            if (targetSize < initialSize) targetSize = initialSize;

            // 使用 Lerp 讓縮放變得很滑順，不會頭暈
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        }
    }
}


