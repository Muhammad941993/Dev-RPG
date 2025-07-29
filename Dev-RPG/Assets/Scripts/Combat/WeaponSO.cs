using RPG.Attribute;
using RPG.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapons/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [field: SerializeField] public Weapon Prefab { get; private set; }
    [field: SerializeField] public AnimatorOverrideController AnimatorOverrideController { get; private set; }
    [field: SerializeField] public float Range { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float PercentageBonus { get; private set; }


    [SerializeField] private bool isRightHand;
    [field: SerializeField] public Projectile Projectile { get; private set; }

    public Weapon Spawn(Transform rightHandPosition, Transform leftHandTransform, Animator animator)
    {
        Weapon temp = null;
        if (Prefab != null) temp = Instantiate(Prefab, GetWeaponHand(rightHandPosition, leftHandTransform));
        
        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

        if (AnimatorOverrideController != null)
            animator.runtimeAnimatorController = AnimatorOverrideController;
        else if (overrideController != null)
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;

        return temp;
    }

    public bool HasProjectile()
    {
        return Projectile != null;
    }

    public void LaunchProjectile(GameObject instigator, Transform rightHandPosition, Transform leftHandTransform, Health target ,float damage)
    {
        var projectile = Instantiate(Projectile, GetWeaponHand(rightHandPosition, leftHandTransform).position,
            Quaternion.identity);
        projectile.SetTarget(instigator,target, damage);
    }

    private Transform GetWeaponHand(Transform rightHandPosition, Transform leftHandTransform)
    {
        return isRightHand ? rightHandPosition : leftHandTransform;
    }
}