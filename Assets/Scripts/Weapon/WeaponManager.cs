using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Weapon;
public class WeaponManager : MonoBehaviour
{
    public Firearms MainWeapon;
    public Firearms SecondaryWeapon;

    private Firearms currentCarriedWeapon;

    private void Start()
    {
        currentCarriedWeapon = MainWeapon;
        Debug.Log("当前武器是否为空：" + currentCarriedWeapon == null);

    }
    private void Update()
    {
        if (!currentCarriedWeapon) return;
        SwapWeapon();
        if (Input.GetMouseButton(0))
        {
            //扣动扳机
            currentCarriedWeapon.HoldTrigger();
        }
        if (Input.GetMouseButtonUp(0))
        {
            //松开扳机
            currentCarriedWeapon.RealseTrigger();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //换弹
            currentCarriedWeapon.ReloadAmmo();
        }
        if (Input.GetMouseButtonDown(1))
        {
            //瞄准
            currentCarriedWeapon.Aiming(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            //取消瞄准
            currentCarriedWeapon.Aiming(false);
        }
    }
    private void SwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentCarriedWeapon.gameObject.SetActive(false);
            currentCarriedWeapon = MainWeapon;
            currentCarriedWeapon.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentCarriedWeapon.gameObject.SetActive(false);
            currentCarriedWeapon = SecondaryWeapon;
            currentCarriedWeapon.gameObject.SetActive(true);
        }
    }
}
