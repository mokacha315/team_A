using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    public Transform warpDestination;

    static float warpCooldown = 0.5f;

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
        if (playerWarpTimer > 0) return;

        //もしPlayerが当たったら
        if (other.CompareTag("Player"))
        {
            //プレイヤーを移動させる
            other.transform.position = warpDestination.position;

            playerWarpTimer = warpCooldown;
        }
    }
}
