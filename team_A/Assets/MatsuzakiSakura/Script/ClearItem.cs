using UnityEngine;

public class ClearItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //UIG‚ê‚½‚çƒQ[ƒ€ƒNƒŠƒA
            UIManager ui = FindAnyObjectByType<UIManager>();

            if (ui != null)
            {
                ui.GameClear();
            }

        }
    }
}
