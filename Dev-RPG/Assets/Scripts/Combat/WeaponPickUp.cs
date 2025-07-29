using System.Collections;
using RPG.Attribute;
using RPG.Combat;
using RPG.Control;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour ,IRayCastable
{
    [SerializeField] private WeaponSO weaponSo;
    [SerializeField] private float disableTime = 5;
    [SerializeField] private float healthToRestore;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PickUp(other.gameObject);
    }

    private void PickUp(GameObject subject)
    {
        if (weaponSo != null)
        {
            subject.GetComponent<Fighter>().EquipWeapon(weaponSo);
        }

        if (healthToRestore > 0)
        {
            subject.GetComponent<Health>().Heal(healthToRestore);
        }

        StartCoroutine(DisableForSecond(disableTime));
    }

    private IEnumerator DisableForSecond(float time)
    {
        EnablePickUp(false);
        yield return new WaitForSeconds(time);
        EnablePickUp(true);
    }

    private void EnablePickUp(bool enable)
    {
        GetComponent<BoxCollider>().enabled = enable;

        foreach (Transform trans in transform) trans.gameObject.SetActive(enable);
    }

    public CursorType GetCursorType()
    {
        return CursorType.PickUp;
    }

    public bool HandleRayCast(PlayerController player)
    {
        if (Input.GetMouseButtonDown(0))
        {
            PickUp(player.gameObject);
        }
        return true;
    }
}