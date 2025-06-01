using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pistol : MonoBehaviour
{
    public Animation gunAnimation;
    public ParticleSystem muzzleFlash;
    public GameObject muzzleFlashLight;
    public AudioSource shoot;

    public int maxAmmoInMag = 10;
    public int maxAmmoInStorage = 30;
    public float shootCooldown = 0.5f;
    public float reloadCooldown = 0.5f;
    public float shootRange = 100f;

    public ParticleSystem impactEffect;

    public GameObject ExtraCross;
    public Color enemyCrosshairColor = Color.red;
    private Color defaultCrosshairColor = Color.white;
    private RawImage[] crosshairImages;

    public int currentAmmoInMag;
    public int currentAmmoInStorage;
    public int damager;

    public bool canShoot = false;
    private bool isReloading = false;
    private float shootTimer;

    void Start()
    {
        currentAmmoInMag = maxAmmoInMag;
        currentAmmoInStorage = maxAmmoInStorage;
        muzzleFlashLight.SetActive(false);

        if (ExtraCross != null)
        {
            crosshairImages = ExtraCross.GetComponentsInChildren<RawImage>();
        }

        Debug.Log("Pistol Initialized. CanShoot: " + canShoot);
    }

    void Update()
    {
        // Prevent shooting when the inventory is open
        if (InventoryManager.Instance != null && InventoryManager.Instance.InventoryMenu.activeSelf)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1") && canShoot && !isReloading)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }

        CheckEnemyInCrosshair();
    }


    private int damageBoost = 0; // Default: No extra damage
    public void IncreasePistolDamage(int amount)
    {
        damageBoost += amount; // Increase pistol attack power
        Debug.Log("Pistol damage increased by: " + amount);
    }

    void Shoot()
    {
        if (currentAmmoInMag > 0 && shootTimer <= 0f)
        {
            shoot.Play();
            muzzleFlash.Play();
            muzzleFlashLight.SetActive(true);
            gunAnimation.Play("PistolRecoil");

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootRange))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        int finalDamage = damager + damageBoost; // Apply the boost
                        enemyHealth.TakeDamage(finalDamage);
                        Debug.Log($"Shot enemy! Damage dealt: {finalDamage}");
                    }
                }

                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            StartCoroutine(EndEffects());

            currentAmmoInMag--;
            shootTimer = shootCooldown;

            UIManager.Instance.UpdateAmmo(currentAmmoInMag, currentAmmoInStorage);

            StartCoroutine(EnableShooting());
        }
        else
        {
            Debug.Log("Cannot shoot");
        }
    }


    void Reload()
    {
        if (isReloading || currentAmmoInStorage <= 0 || currentAmmoInMag == maxAmmoInMag)
            return;

        isReloading = true;
        StartCoroutine(ReloadCooldown());
    }

    IEnumerator ReloadCooldown()
    {
        yield return new WaitForSeconds(reloadCooldown);

        int bulletsToReload = maxAmmoInMag - currentAmmoInMag;
        int bulletsAvailable = Mathf.Min(bulletsToReload, currentAmmoInStorage);

        currentAmmoInMag += bulletsAvailable;
        currentAmmoInStorage -= bulletsAvailable;

        UIManager.Instance.UpdateAmmo(currentAmmoInMag, currentAmmoInStorage);

        isReloading = false;
    }

    IEnumerator EndEffects()
    {
        yield return new WaitForSeconds(0.1f);
        muzzleFlashLight.SetActive(false);
    }

    IEnumerator EnableShooting()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
        Debug.Log("Shooting re-enabled. CanShoot: " + canShoot);
    }

    void CheckEnemyInCrosshair()
    {
        if (ExtraCross == null || crosshairImages == null) return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                ChangeCrosshairColor(enemyCrosshairColor);
                return;
            }
        }

        ChangeCrosshairColor(defaultCrosshairColor);
    }

    void ChangeCrosshairColor(Color newColor)
    {
        foreach (RawImage image in crosshairImages)
        {
            image.color = newColor;
        }
    }
    public void IncreaseStoredAmmo(int amount)
    {
        currentAmmoInStorage += amount;
        UIManager.Instance.UpdateAmmo(currentAmmoInMag, currentAmmoInStorage); // Update UI
    }

}
