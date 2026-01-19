using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackPower { get { return currentWeapon.attackPower; } }
    public WeaponData currentWeapon;
    public float attackDuration { get { return currentWeapon.attackDuration; } }
    public float attackCooldown { get { return currentWeapon.attackCooldown; } }
    public int extraDamage = 0;

    public void AddDamage(int value) { extraDamage += value; }

    public Vector2 swordOffset = new Vector2(1.0f, 0f);
    public HeroController heroController;

    public AudioSource audioSource;
    public AudioClip attackSE;

    private bool inAttack = false;
    private float nextAttackTiam = 0f;
    private GameObject sword;
    private GameObject sword_effect;
    private Transform swordTransform;
    private Transform effectTransform;
    private SpriteRenderer swordSpriteRenderer;

    void Start()
    {
        if (heroController == null) heroController = GetComponent<HeroController>();
        EquipWeapon(currentWeapon);
        SetInitialSwordPosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !inAttack)
        {
            if (Time.time >= nextAttackTiam)
            {
                Attack();
                nextAttackTiam = Time.time + attackCooldown;
            }
        }

        if (heroController != null && swordTransform != null && swordSpriteRenderer != null)
        {
            float angle = heroController.angleZ;
            float displayAngle = 0f;
            float underAngle = 0f;
            // 1. 斜め判定 (45, 135, -45, -135度付近)
            // 先ほど「-45で治った」とのことですので、斜めの時は一律で角度補正を適用します
            // bool isDiagonal = Mathf.Abs(Mathf.Abs(angle) - 45f) < 10f || Mathf.Abs(Mathf.Abs(angle) - 135f) < 10f;
            bool isDiagonal = Mathf.Abs(Mathf.Abs(angle % 90) - 45f) < 10f;

            if (isDiagonal)
            {
                // 斜めの時はFlipをオフにして、移動角度に合わせて回転させる
                swordSpriteRenderer.flipX = false;
                swordSpriteRenderer.flipY = false;
                displayAngle = angle - 45f; // 斜めの基準補正

            }
            else
            {
                //上下左右（4方向）の時はFlipで制御し、回転は0で固定
                if (heroController.direction == 0) // 下
                {
                    swordSpriteRenderer.flipX = false;
                    swordSpriteRenderer.flipY = false;
                    displayAngle = 0f;
                }
                else if (heroController.direction == 1) // 左
                {
                    swordSpriteRenderer.flipX = true;
                    swordSpriteRenderer.flipY = false;
                    displayAngle = 0f;
                }
                else if (heroController.direction == 2) //上
                {
                    swordSpriteRenderer.flipX = true;
                    swordSpriteRenderer.flipY = false;
                    displayAngle = 0f;
                }
                else //右(3)
                {
                    swordSpriteRenderer.flipX = false;
                    swordSpriteRenderer.flipY = false;
                    displayAngle = 0f;
                }
            }


            // 2. 回転の適用
            swordTransform.rotation = Quaternion.Euler(0, 0, displayAngle);

            // 3. 表示順の修正（消えないように値を大きく設定）
            // プレイヤーのSorting Layerが「Default」なら、100以上にすれば確実に前に出ます
            // 上向き(0 < angle < 180)の時だけキャラの背後に回す
            int baseOrder = 100;
            swordSpriteRenderer.sortingOrder = (angle > 10 && angle < 170) ? baseOrder - 1 : baseOrder + 1;

            // 4. 位置の更新
            float angleRad = angle * Mathf.Deg2Rad;
            swordTransform.localPosition = new Vector3(
                Mathf.Cos(angleRad) * (swordOffset.x * 0.5f),
                Mathf.Sin(angleRad) * (swordOffset.x * 0.5f),
                0
            );
        }
    }

    // --- 以下のメソッドは以前と同様 ---
    void Attack()
    {
        if (heroController == null || sword_effect == null || effectTransform == null) return;
        if (audioSource != null && attackSE != null) audioSource.PlayOneShot(attackSE);

        inAttack = true;
        sword_effect.SetActive(true);
        float offsetAngle = 90.0f;
        effectTransform.rotation = Quaternion.Euler(180, 180, heroController.angleZ + offsetAngle);
        float angleRad = heroController.angleZ * Mathf.Deg2Rad;
        Vector3 rotatedOffset = new Vector3(Mathf.Cos(angleRad) * swordOffset.x, Mathf.Sin(angleRad) * swordOffset.x, 0);
        effectTransform.localPosition = rotatedOffset;
        Invoke(nameof(StopAttack), attackDuration);
    }

    void StopAttack() { if (sword_effect != null) sword_effect.SetActive(false); inAttack = false; }

    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;
        if (sword != null) Destroy(sword);
        if (sword_effect != null) Destroy(sword_effect);
        sword = Instantiate(newWeapon.swordPrefab, transform);
        sword_effect = Instantiate(newWeapon.swordEffectPrefab, transform);
        sword.SetActive(true);
        sword_effect.SetActive(false);
        swordTransform = sword.transform;
        effectTransform = sword_effect.transform;
        swordSpriteRenderer = sword.GetComponent<SpriteRenderer>();
        SetInitialSwordPosition();
    }

    void SetInitialSwordPosition()
    {
        if (swordTransform == null || heroController == null) return;
        float angleRad = heroController.angleZ * Mathf.Deg2Rad;
        swordTransform.localPosition = new Vector3(Mathf.Cos(angleRad) * (swordOffset.x * 0.5f), Mathf.Sin(angleRad) * (swordOffset.x * 0.5f), 0);
    }
}