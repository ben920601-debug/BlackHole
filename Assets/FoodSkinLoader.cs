using UnityEngine;

public class FoodSkinLoader : MonoBehaviour
{
    // 這是一個欄位，讓您把「討厭鬼」的圖片拖進來
    public Sprite hateSprite; 

    void Start()
    {
        // 1. 去讀取手機存檔，看看玩家選了什麼？ (預設是 0:星星)
        int skinID = PlayerPrefs.GetInt("FoodSkin", 0);

        // 2. 如果玩家選的是 1 (討厭鬼)
        if (skinID == 1)
        {
            // 3. 抓取身上的圖片組件 (SpriteRenderer)
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            
            // 4. 如果有抓到，就把圖片換掉！
            if (sr != null && hateSprite != null)
            {
                sr.sprite = hateSprite;
                
                // (選用) 如果討厭鬼圖片不是圓的，可以重設碰撞大小
                // GetComponent<CircleCollider2D>().radius = 0.5f; 
            }
        }
        // 如果是 0，就什麼都不做，維持原本的星星樣子
    }
}
