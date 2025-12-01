using System.Collections;
using UnityEngine;


public class WarpPoint : MonoBehaviour
{
    public enum WarpType { WarpA, WarpB }
    public WarpType warpType;

    //ワープ先の座標
    public Vector3 warpPosition;
    public Transform warpBDestination;

    //warpAに入ったときの位置を保存
    public static Vector3 warpAEnterPosition;

    static float warpCooldown = 0.3f;
    static float playerWarpTimer = 0f;
    private bool isWarping = false;

    private void Update()
    {
        if (playerWarpTimer > 0)
        {
            playerWarpTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (playerWarpTimer > 0 || isWarping)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        HeroController hero = other.GetComponent<HeroController>();
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Collider2D playerCol = other.GetComponent<Collider2D>();


        if (hero != null)
        {
            hero.enabled = false;
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        if (playerCol != null)
        {
            playerCol.isTrigger = true;
        }

        StartCoroutine(WarpRoutine(other.transform, hero, playerCol));
    }

    IEnumerator WarpRoutine(Transform player, HeroController hero, Collider2D playerCol)
    {
        isWarping = true;
        playerWarpTimer = warpCooldown;

        FadeManager2 fadeManager = FindObjectOfType<FadeManager2>();

        if (fadeManager != null)
        {
            fadeManager.FadeOut();
        }

        yield return new WaitForSeconds(fadeManager != null ? fadeManager.fadeDuration : warpCooldown);

        Vector3 targetPos = warpPosition;

        if (warpType == WarpType.WarpA && warpBDestination != null)
        {
            // WarpAに入ったらプレイヤーの位置を保存
            warpAEnterPosition = player.position;

            //WarpBに移動
            targetPos = warpBDestination.position;
        }
        else if (warpType == WarpType.WarpB)
        {
            // WarpBに入ったらWarpAの位置に移動
            if (WarpPoint.warpAEnterPosition != Vector3.zero)
            {
                targetPos = WarpPoint.warpAEnterPosition;
            }
        }

        //ワープ
        player.position = targetPos + new Vector3(0, 1f, 0);

        //当たり判定戻す
        yield return new WaitForSeconds(0.1f);
        if (playerCol != null)
        {
            playerCol.isTrigger = false;
        }

        if (fadeManager != null)
        {
            fadeManager.FadeIn();
        }

        yield return new WaitForSeconds(fadeManager != null ? fadeManager.fadeDuration : 0.3f);


        if (hero != null)
        {
            hero.enabled = true;
        }

        isWarping = false;
    }

}