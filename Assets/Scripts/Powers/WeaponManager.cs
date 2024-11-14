using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Just neatly holds ref to each wep spawner for UpgradeChoices

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance; // Singleton instance for UpgradeChoices to refer to

    public OrbitalWeapon orbitalSpawner;
    public BoomerangSpawner boomerangSpawner;
    public MeteorSpawner meteorSpawner;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OrbitalUpgrade()
    {
        orbitalSpawner?.Upgrade();
    }
    
    public void BoomerangUpgrade()
    {
        boomerangSpawner?.Upgrade();
    }

    public void MeteorUpgrade()
    {
        meteorSpawner?.Upgrade();
    }
}
