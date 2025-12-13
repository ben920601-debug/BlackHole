using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("UI 設定")]
    public GameObject startUI;
    public GameObject winUI;
    public GameObject loseUI;      // 🔴 新增：失敗文字
    public GameObject restartBtn;
    public GameObject menuBtn;

    [Header("移動設定")]
    public float moveSpeed = 5f;

    [Header("加速道具設定")]
    public float speedMultiplier = 2f; // 加速倍率 (2倍)
    public float speedDuration = 3f;   // 持續時間 (3秒)
    private float originalSpeed;       // 用來記住原本的速度
    private bool isSpeeding = false;   // 是否正在加速中

    [Header("音效設定")]
    public AudioClip eatSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    private AudioSource audioSrc;

    [Header("升級與魔王設定")]
    public int currentExp = 0;
    public int expToLevelUp = 3;
    public GameObject bossPrefab;  // 🔴 新增：魔王印章
    public int levelToSpawnBoss = 2; // 🔴 新增：升級幾次後魔王出現？
    private int currentLevel = 0;    // 紀錄目前等級

    private bool isGameStarted = false;
    private bool isGameOver = false; // 🔴 新增：遊戲結束狀態
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        audioSrc = GetComponent<AudioSource>();
        
        originalSpeed = moveSpeed; // 🔴 記住一開始的速度
    }

    void Update()
    {
        // 如果遊戲還沒開始 或 已經結束，就不能動
        if (!isGameStarted || isGameOver) 
        {
            // 等待開始
            if (!isGameStarted && !isGameOver && (Input.GetMouseButtonDown(0) || Input.GetAxis("Horizontal") != 0))
            {
                StartGame();
            }
            return; 
        }

        MoveLogic();
    }

    // ... (MoveLogic 和 StartGame 維持不變) ...
    void MoveLogic()
    {
        // ... (保留原本的移動邏輯) ...
        // 為了節省篇幅，這裡省略，請保留您原本的 MoveLogic 代碼
        // 簡單來說就是複製您之前的 MoveLogic 內容放這裡
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        bool hasInput = false;

        if (moveX != 0 || moveY != 0)
        {
            transform.Translate(new Vector2(moveX, moveY) * moveSpeed * Time.deltaTime);
            hasInput = true;
        }

        if (!hasInput && Input.GetMouseButton(0))
        {
            Vector3 inputPos = Input.mousePosition;
            inputPos.z = 10; 
            Vector3 worldPos = cam.ScreenToWorldPoint(inputPos);
            worldPos.z = 0; 
            transform.position = Vector3.MoveTowards(transform.position, worldPos, moveSpeed * Time.deltaTime);
        }
    }

    void StartGame()
    {
        isGameStarted = true;
        if (startUI != null) startUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            AddExperience();
            CheckWinCondition();
        }
        else if (other.CompareTag("SpeedItem"))
        {
            Destroy(other.gameObject); // 吃掉道具
            
            // 如果還沒在加速，就開始加速
            // (如果已經在加速，可以選擇重置時間，這裡我們先做簡單版：不重置)
            if (!isSpeeding)
            {
                StartCoroutine(ActivateSpeedBoost());
            }
        }
    }

    void AddExperience()
    {
        currentExp++;
        if (currentExp >= expToLevelUp)
        {
            LevelUp(); // 🔴 把升級邏輯抽出來
        }
    }

    void LevelUp()
    {
        currentExp = 0;
        currentLevel++; // 等級 +1
        transform.localScale += new Vector3(0.5f, 0.5f, 0); // 變大
        
        Debug.Log("升級了！目前等級：" + currentLevel);

        // 🔴 檢查是否要召喚魔王
        if (currentLevel == levelToSpawnBoss)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab != null)
        {
            // 在隨機位置生成魔王 (距離玩家遠一點)
            Vector3 spawnPos = new Vector3(Random.Range(-10, 10), 10, 0); 
            Instantiate(bossPrefab, spawnPos, Quaternion.identity);
            Debug.Log("警告：魔王出現！");
        }
    }

    // ... (CheckWinCondition 維持不變) ...
    void CheckWinCondition()
    {
        int starsLeft = GameObject.FindGameObjectsWithTag("Food").Length;
        if (starsLeft <= 1) 
        {
            GameOver(true); // true 代表贏了
        }
    }

    // 🔴 統一管理的遊戲結束功能 (包含贏和輸)
    public void GameOver(bool isWin)
    {
        isGameOver = true; // 鎖住移動
        isGameStarted = false;

        if (isWin)
        {
            Debug.Log("贏了！");
            if (winUI != null) winUI.SetActive(true);
        }
        else
        {
            Debug.Log("輸了！");
            // 輸的時候，玩家消失 (假裝被吃掉)
            gameObject.SetActive(false); 
            if (loseUI != null) loseUI.SetActive(true);
        }

        // 不管輸贏，都顯示按鈕
        if (restartBtn != null) restartBtn.SetActive(true);
        if (menuBtn != null) menuBtn.SetActive(true);
    }

    

    // ... (RestartGame 和 GoToMenu 維持不變) ...
    public void RestartGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void GoToMenu() { SceneManager.LoadScene("MenuScene"); }

    // 這是一個「協程」，可以讓我們暫停幾秒再繼續執行
    System.Collections.IEnumerator ActivateSpeedBoost()
    {
        isSpeeding = true;
        moveSpeed = originalSpeed * speedMultiplier; // 速度變快！
        Debug.Log("加速開始！飆車啦～");

        // 等待 3 秒 (或是您設定的時間)
        yield return new WaitForSeconds(speedDuration);

        moveSpeed = originalSpeed; // 恢復原本速度
        isSpeeding = false;
        Debug.Log("加速結束，慢下來了。");
    }
}

