using UnityEngine;
using UnityEngine.UI; // UIコンポーネントを使うために必要

public class ItemDisplayManager : MonoBehaviour
{
    // Inspectorから設定できるように、ImageコンポーネントとSpriteを定義
    [SerializeField]
    private Image itemDisplayImage;

    [SerializeField]
    private Sprite busterSwordSprite; // バスタードソードの画像を設定

    [SerializeField]
    private Sprite blackSwordSprite;  // 黒剣の画像を設定

    // 初期状態では画像を非表示にしておく
    private void Start()
    {
        // Imageコンポーネント自体（GameObject）を非アクティブにする
        itemDisplayImage.gameObject.SetActive(false);
    }

    // バスタードソードを拾ったときに呼び出す関数
    public void DisplayBusterSwordInfo()
    {
        // 表示するSpriteを切り替え
        itemDisplayImage.sprite = busterSwordSprite;

        // Imageオブジェクトを表示（アクティブ化）
        itemDisplayImage.gameObject.SetActive(true);

        // 必要に応じて、一定時間後に非表示にする処理などを追加
         Invoke("HideItemInfo", 5f);//5秒後に非表示にする
    }

    // 黒剣を拾ったときに呼び出す関数
    public void DisplayBlackSwordInfo()
    {
        itemDisplayImage.sprite = blackSwordSprite;
        itemDisplayImage.gameObject.SetActive(true);
        // Invoke("HideItemInfo", 5f);
    }

    // 情報表示を非表示にする関数
    public void HideItemInfo()
    {
        itemDisplayImage.gameObject.SetActive(false);
    }
}