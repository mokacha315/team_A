using System.Collections;
using UnityEngine;


public class WarpPoint : MonoBehaviour
{
    //ƒ[ƒvæ‚ÌÀ•W
    public Vector3 warpPosition;

    public Transform warpDestination;
    static float warpCooldown = 0.3f;
    static float playerWarpTimer = 0f;
    private bool isWarping = false;

    private void Update()
    {
        //ï¿½^ï¿½Cï¿½}ï¿½[ï¿½ï¿½ï¿½ï¿½
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

        //ï¿½ï¿½ï¿½ï¿½Playerï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
        if (!other.CompareTag("Player"))
        {
            return;
        }

        //ï¿½ï¿½lï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½~
        HeroController hero = other.GetComponent<HeroController>();
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Collider2D playerCol = other.GetComponent<Collider2D>();


        if (hero != null)
        {
            hero.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
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

        //ï¿½tï¿½Fï¿½[ï¿½hï¿½Aï¿½Eï¿½gï¿½Iï¿½ï¿½ï¿½Ü‚Å‘Ò‚ï¿½
        yield return new WaitForSeconds(fadeManager != null ? fadeManager.fadeDuration : warpCooldown); ;


<<<<<<< HEAD
        //ï¿½ï¿½ï¿½[ï¿½v (ï¿½vï¿½ï¿½ï¿½Cï¿½ï¿½ï¿½[ï¿½Ê’uï¿½ï¿½ï¿½ç‚·)
        player.position = warpDestination.position + new Vector3(0, 1.0f, 0);
=======
        //ƒ[ƒv (ƒvƒŒƒCƒ„[ˆÊ’u‚¸‚ç‚·)
        player.position = warpPosition + new Vector3(0, 1.0f, 0);
>>>>>>> f6d4043bfb7e15666470e26b0c539322b3920e32


        //ï¿½ï¿½ï¿½ï¿½ï¿½è”»ï¿½ï¿½ï¿½ß‚ï¿½
        yield return new WaitForSeconds(0.1f);
        if (playerCol != null)
        {
            playerCol.isTrigger = false;
        }

        //ï¿½tï¿½Fï¿½[ï¿½hï¿½Cï¿½ï¿½
        if (fadeManager != null)
        {
            fadeManager.FadeIn();
        }

        yield return new WaitForSeconds(fadeManager != null ? fadeManager.fadeDuration : 0.3f);


        if (hero != null)
        {
            hero.enabled = true;
        }

        //ï¿½tï¿½ï¿½ï¿½Oï¿½Í‚ï¿½ï¿½ï¿½
        isWarping = false;
    }

}