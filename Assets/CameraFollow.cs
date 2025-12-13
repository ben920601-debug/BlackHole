using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("跟隨目標")]
    public Transform target;        // 主角 (黑洞)
    public float smoothSpeed = 5f;  // 跟隨的平滑度

    [Header("變焦設定 (Zoom)")]
    public float baseSize = 5f;     // 初始鏡頭大小 (預設是 5)
    public float zoomFactor = 0.8f; // 變焦倍率 (數字越小，鏡頭拉遠越慢；數字越大，拉遠越快)

    private Vector3 offset;         // 保持 Z 軸距離用
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (target != null)
        {
            // 記住一開始的相對距離 (主要是 Z 軸 -10)
            offset = transform.position - target.position;
        }
    }

   void LateUpdate()
    {
        if (target == null) return;

        // 只跟隨目標的 X 和 Y，但是 Z 軸永遠固定在 -10
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, -10f);

        // 使用 Lerp 讓移動滑順
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);


        // --- 2. 動態變焦 (這部分維持不變) ---
        float targetSize = baseSize * target.localScale.x * zoomFactor;
        if (targetSize < baseSize) targetSize = baseSize;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothSpeed * Time.deltaTime);
    }
}