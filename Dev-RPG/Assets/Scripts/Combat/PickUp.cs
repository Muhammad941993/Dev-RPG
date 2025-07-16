using System.Collections;
using RPG.Combat;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSo;
    [SerializeField] private float disableTime = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<Fighter>().EquipWeapon(weaponSo);

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
}