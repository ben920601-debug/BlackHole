using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("生成設定")]
    public GameObject foodPrefab;   // 我們要複製的那個星星印章
    public int spawnCount = 20;     // 一開始要生幾顆？
    
    [Header("生成範圍 (矩形)")]
    public float width = 15f;       // 左右寬度範圍
    public float height = 10f;      // 上下高度範圍

    void Start()
    {
        SpawnFood();
    }

    void SpawnFood()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // 1. 隨機決定一個座標 (X: -寬~寬, Y: -高~高)
            float randomX = Random.Range(-width, width);
            float randomY = Random.Range(-height, height);
            Vector3 spawnPos = new Vector3(randomX, randomY, 0);

            // 2. 生成星星 (Instantiate)
            Instantiate(foodPrefab, spawnPos, Quaternion.identity);
        }
    }

    // (選用) 畫出範圍框框，讓你在編輯器裡看得到範圍
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(width * 2, height * 2, 0));
    }
}