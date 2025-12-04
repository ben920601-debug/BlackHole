using UnityEngine;
using UnityEngine.SceneManagement; // 這是切換場景必備的

public class MenuManager : MonoBehaviour
{
    [Header("UI 物件")]
    public GameObject settingsPanel; // 這是那個黑色的設定視窗

    void Start()
    {
        // 遊戲一開始，先確保設定視窗是關閉的
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // --- 按鈕功能區 ---

    public void OnClickPlay()
    {
        // 載入下一個場景 (請確認您的遊戲場景名字是 GameScene)
        // 如果您的場景叫別的名字，請在這裡修改字串
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickSetting()
    {
        settingsPanel.SetActive(true); // 打開設定
    }

    public void OnClickClose()
    {
        settingsPanel.SetActive(false); // 關閉設定
    }

    // --- 換膚功能區 ---
    // 我們用 PlayerPrefs (手機的記事本) 來記錄玩家的選擇
    // 0 = 星星 (預設), 1 = 討厭鬼

    public void OnSelectSkin_Star()
    {
        PlayerPrefs.SetInt("FoodSkin", 0);
        PlayerPrefs.Save(); // 存檔
        Debug.Log("已選擇：星星");
        // 這裡可以加一點視覺回饋，例如按鈕變色，但我們先求有功能
    }

    public void OnSelectSkin_Hate()
    {
        PlayerPrefs.SetInt("FoodSkin", 1);
        PlayerPrefs.Save(); // 存檔
        Debug.Log("已選擇：討厭鬼");
    }
}

