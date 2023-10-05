using System.Collections;
using UnityEngine;
namespace Scripts.Weapon
{
    public class AssaultRifle : Firearms
    {
        private IEnumerator tmpCoroutine;
        // private IEnumerator doAimCoroutine;
        private FPSMouseLook MouseLook;
        protected override void Start()
        {
            base.Start();
            tmpCoroutine = CheckReloadAmmoAnimationEnd();
            // doAimCoroutine = DoAim();
            MouseLook = FindObjectOfType<FPSMouseLook>();
        }
        protected override void Reload()
        {
            GunAnimator.SetLayerWeight(2, 1);
            GunAnimator.SetTrigger(CurrentAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");

            FirearmsReloadAudioSource.clip = CurrentAmmo > 0 ?
            FirearmsAudioData.ReloadLeft : FirearmsAudioData.ReloadOutOf;

            FirearmsReloadAudioSource.Play();
            StartCoroutine(CheckReloadAmmoAnimationEnd());
            if (tmpCoroutine == null)
            {
                tmpCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(tmpCoroutine);
            }
            else
            {
                StopCoroutine(tmpCoroutine);
                tmpCoroutine = null;
                tmpCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(tmpCoroutine);
            }

        }

        protected override void Shooting()
        {
            if (CurrentAmmo <= 0) return;
            if (!IsAllowShooting()) return;
            MuzzleParticle.Play();
            FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudio;
            FirearmsReloadAudioSource.Play();
            GunAnimator.Play("Fire", isAimming ? 1 : 0, 0);
            CurrentAmmo -= 1;
            CreatBullet();
            MouseLook.FiringForTest();
            CasingParticle.Play();
            LastFireTime = Time.time;
        }
        // private void Update()
        // {
        //     if (Input.GetMouseButton(0))
        //     {
        //         DoAttack();
        //     }
        //     if (Input.GetKeyDown(KeyCode.R))
        //     {
        //         Reload();
        //     }
        //     if (Input.GetMouseButtonDown(1))
        //     {
        //         isAimming = !isAimming;
        //         Aim();
        //     }
        // }
        protected void CreatBullet()
        {
            GameObject tmp_Bullet = Instantiate(BulletPrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            tmp_Bullet.transform.eulerAngles += CalculateSpreadOffset();
            //tmp_Bullet.AddComponent<Rigidbody>();
            var tmp_BulletScript = tmp_Bullet.AddComponent<Bullet>();
            tmp_BulletScript.ImpactPrefab = BulletImpactPrefab;
            tmp_BulletScript.ImpackAudioData = ImpackAudioData;
            tmp_BulletScript.BulletSpeed = 100f;

        }
        // private IEnumerator CheckReloadAmmoAnimationEnd()
        // {
        //     while (true)
        //     {
        //         yield return null;
        //         GunStateInfo = GunAnimator.GetCurrentAnimatorStateInfo(2);
        //         if (GunStateInfo.IsTag("ReloadAmmo"))
        //         {
        //             if (GunStateInfo.normalizedTime >= 0.9f)
        //             {
        //                 int tmpAmmo = AmmoInMag - CurrentAmmo;//差的
        //                 CurrentAmmo += CurrentMaxAmmoCarried > tmpAmmo ? tmpAmmo : CurrentMaxAmmoCarried;
        //                 CurrentMaxAmmoCarried -= tmpAmmo;
        //                 CurrentMaxAmmoCarried = Mathf.Clamp(CurrentMaxAmmoCarried, 0, CurrentMaxAmmoCarried);
        //                 yield break;
        //             }
        //         }

        //     }

        // }
        // private IEnumerator DoAim()
        // {
        //     while (true)
        //     {
        //         yield return null;
        //         float tmp_CurrentFOV = 0f;
        //         EyesCamera.fieldOfView = Mathf.SmoothDamp(EyesCamera.fieldOfView,
        //         isAimming ? 26 : OriginFOV,
        //         ref tmp_CurrentFOV,
        //         Time.deltaTime * 2);
        //     }
        // }
        // protected override void Aim()
        // {
        //     GunAnimator.SetBool("Aim", isAimming);
        //     if (doAimCoroutine == null)
        //     {
        //         doAimCoroutine = DoAim();
        //         StartCoroutine(doAimCoroutine);
        //     }
        //     else
        //     {
        //         StopCoroutine(doAimCoroutine);
        //         doAimCoroutine = null;
        //         doAimCoroutine = DoAim();
        //         StartCoroutine(doAimCoroutine);
        //     }
        // }
    }
}

