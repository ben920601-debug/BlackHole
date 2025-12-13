using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 記得要有這行才能控制 Image

public class MenuManager : MonoBehaviour
{
    [Header("UI 面板")]
    public GameObject settingsPanel;

    [Header("按鈕外觀 (請把按鈕拖進來)")]
    public Image btnSkinDefault; // 預設食物按鈕的圖
    public Image btnSkinHate;    // 討厭鬼按鈕的圖

    [Header("顏色設定")]
    public Color selectedColor = Color.green; // 選中時變綠色
    public Color normalColor = Color.white;   // 沒選中時白色

    void Start()
    {
        // 1. 初始化面板
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // 2. 讀取目前的選擇，並更新按鈕顏色
        UpdateButtonVisuals();
    }

    // --- 內部功能：更新按鈕顏色 ---
    void UpdateButtonVisuals()
    {
        // 讀取存檔 (0=預設, 1=討厭鬼)
        int currentSkin = PlayerPrefs.GetInt("FoodSkin", 0);

        if (btnSkinDefault != null && btnSkinHate != null)
        {
            if (currentSkin == 0)
            {
                // 選的是預設：預設亮綠燈，討厭鬼變白
                btnSkinDefault.color = selectedColor;
                btnSkinHate.color = normalColor;
            }
            else
            {
                // 選的是討厭鬼：討厭鬼亮綠燈，預設變白
                btnSkinDefault.color = normalColor;
                btnSkinHate.color = selectedColor;
            }
        }
    }

    // --- 按鈕功能區 ---

    public void OnClickPlay()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickSetting()
    {
        settingsPanel.SetActive(true);
        UpdateButtonVisuals(); // 打開設定時，也要更新一下顏色
    }

    public void OnClickClose()
    {
        settingsPanel.SetActive(false);
    }

    public void OnSelectSkin_Default() // 改個名字比較清楚
    {
        PlayerPrefs.SetInt("FoodSkin", 0);
        PlayerPrefs.Save();
        UpdateButtonVisuals(); // 按下後立刻變色
        Debug.Log("已選擇：預設食物");
    }

    public void OnSelectSkin_Hate()
    {
        PlayerPrefs.SetInt("FoodSkin", 1);
        PlayerPrefs.Save();
        UpdateButtonVisuals(); // 按下後立刻變色
        Debug.Log("已選擇：討厭鬼");
    }
}