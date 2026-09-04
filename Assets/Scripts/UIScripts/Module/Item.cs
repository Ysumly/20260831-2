using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // 道具图片
    [SerializeField] private Image image;
    // 道具按键
    [SerializeField] private TextMeshProUGUI itemKey;
    // 道具价格
    [SerializeField] private TextMeshProUGUI itemPrice;



    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
    // 设置道具信息, 然后显示道具
    public void SetItemInfo(string itemKey, Sprite itemSprite, int itemPrice)
    {
        this.itemKey.text = itemKey;
        this.image.sprite = itemSprite;
        this.itemPrice.text = itemPrice.ToString();
        gameObject.SetActive(true);
    }

}

