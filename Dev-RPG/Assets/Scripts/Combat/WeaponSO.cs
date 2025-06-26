using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapons/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [field: SerializeField] public GameObject Prefab {get; private set;}
    [field: SerializeField] public AnimatorOverrideController AnimatorOverrideController {get; private set;}
    [field: SerializeField] public float Range {get; private set;}
    [field: SerializeField] public float Damage{get; private set;}

    [SerializeField] private bool isRightHand;
    [field: SerializeField] public Projectile Projectile {get; private set;}
    
    public GameObject Spawn(Transform rightHandPosition, Transform leftHandTransform, Animator animator)
    {
        GameObject temp = null;
        if (Prefab != null)
        {
            temp =Instantiate(Prefab, GetWeaponHand(rightHandPosition, leftHandTransform));
        }

        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

        if (AnimatorOverrideController != null)
        {
            animator.runtimeAnimatorController = AnimatorOverrideController;
        }else if (overrideController != null)
        {
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }
        return temp;
    }
    
    public bool HasProjectile() => Projectile != null;

    public void LaunchProjectile(Transform rightHandPosition, Transform leftHandTransform, Health target)
    {
        var projectile = Instantiate(Projectile, GetWeaponHand(rightHandPosition,leftHandTransform).position, Quaternion.identity);
        projectile.SetTarget(target , Damage);
    }

    private Transform GetWeaponHand(Transform rightHandPosition, Transform leftHandTransform) =>
        isRightHand ? rightHandPosition : leftHandTransform;
}
