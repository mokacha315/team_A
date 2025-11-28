using System.Collections;
using UnityEngine;


public class WarpPoint : MonoBehaviour
{
    public Transform warpDestination;

    static float warpCooldown = 0.3f;

    static float playerWarpTimer = 0f;

    private void Update()
    {
        //タイマー減少
        if (playerWarpTimer > 0)
        {
            playerWarpTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (playerWarpTimer > 0)
        {
            return;
        }

        //もしPlayerが当たったら
        if (other.CompareTag("Player"))
        {
            StartCoroutine(WarpRoutine(other.transform));
        }
    }

    IEnumerator WarpRoutine(Transform player)
    {
        playerWarpTimer = warpCooldown;

        HeroController hero = player.GetComponent<HeroController>();

        if (hero != null)
        {
            hero.enabled = false;
        }

        //フェードアウト終わるまで待つ
        yield return new WaitForSeconds(warpCooldown);


        //ワープ
        player.position = warpDestination.position;

        //操作ON
        if (hero != null)
        {
            hero.enabled = true;
        }
    }

}