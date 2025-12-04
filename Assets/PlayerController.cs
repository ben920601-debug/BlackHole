using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class PlayerController : MonoBehaviour
{
    [Header("UI 設定")]
    public GameObject startUI;     
    public GameObject winUI;       
    public GameObject restartBtn;
    public GameObject menuBtn;     // 🔴 新增這個

    [Header("移動設定")]
    public float moveSpeed = 5f;

    [Header("升級設定")]
    public int currentExp = 0;
    public int expToLevelUp = 3;

    private bool isGameStarted = false;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!isGameStarted)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetAxis("Horizontal") != 0)
            {
                StartGame();
            }
            return;
        }
        MoveLogic();
    }

    void MoveLogic()
    {
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
    }

    void AddExperience()
    {
        currentExp++;
        if (currentExp >= expToLevelUp)
        {
            currentExp = 0;
            transform.localScale += new Vector3(0.5f, 0.5f, 0);
        }
    }

    void CheckWinCondition()
    {
        int starsLeft = GameObject.FindGameObjectsWithTag("Food").Length;
        if (starsLeft <= 1) 
        {
            GameWin();
        }
    }

    void GameWin()
    {
        if (winUI != null) winUI.SetActive(true);
        if (restartBtn != null) restartBtn.SetActive(true);
        if (menuBtn != null) menuBtn.SetActive(true); // 🔴 新增：顯示按鈕
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 🔴 新增功能
    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
