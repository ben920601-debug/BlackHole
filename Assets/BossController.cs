using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 2.0f; // 魔王移動速度 (不要太快，給玩家逃跑機會)
    private Transform player;      // 目標

    void Start()
    {
        // 1. 遊戲一開始，魔王就搜尋 "Player" 標籤的物件
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
    }

    void Update()
    {
        // 2. 如果玩家還活著，就追過去！
        if (player != null)
        {
            // MoveTowards: 從 A 移動到 B
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    // 3. 碰撞檢測
    void OnTriggerEnter2D(Collider2D other)
    {
        // 如果撞到的是玩家
        if (other.CompareTag("Player"))
        {
            // 呼叫玩家身上的 "GameOver" 功能 (失敗版)
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.GameOver(false); // false 代表輸了
            }
        }
    }
}