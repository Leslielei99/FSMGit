using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    public abstract class Firearms : MonoBehaviour, IWeapon//抽象为枪械类
    {
        #region 变量属性
        [Header("-----枪口、弹壳-----")]
        public Transform MuzzlePoint;//枪口位置
        public Transform CasingPoint;//弹壳抛出位置

        public ImpackAudioData ImpackAudioData;
        public float FireRate;
        protected float LastFireTime;
        public ParticleSystem MuzzleParticle;//枪焰
        public ParticleSystem CasingParticle;//抛弹壳
        [Header("-----弹夹-----")]
        public int AmmoInMag = 30;//弹夹容量
        public int MaxAmmoCarried = 120;//最大携带数

        protected int CurrentAmmo;//
        protected int CurrentMaxAmmoCarried;//

        protected Animator GunAnimator;//开枪动画
        protected AnimatorStateInfo GunStateInfo;//当前状态
        [Header("-----瞄准镜-----")]
        public Camera EyesCamera;//瞄准镜头大小
        protected float OriginFOV;
        protected bool isAimming;
        [Header("-----子弹-----")]
        public GameObject BulletPrefab;
        public GameObject BulletImpactPrefab;//击中
        public AudioSource FirearmsShootingAudioSource;
        public AudioSource FirearmsReloadAudioSource;
        public FirearmsAudioData FirearmsAudioData;
        [Header("-----子弹散射-----")]
        public float SpreadAngle;


        private IEnumerator doAimCoroutine;
        protected bool IsHoldingTrigger;
        #endregion

        #region 方法
        protected virtual void Start()
        {
            doAimCoroutine = DoAim();
            CurrentAmmo = AmmoInMag;
            OriginFOV = EyesCamera.fieldOfView;
            CurrentMaxAmmoCarried = MaxAmmoCarried;
            GunAnimator = GetComponent<Animator>();
            Debug.Log(GunAnimator.isActiveAndEnabled);
        }

        public void DoAttack()
        {
            Shooting();
        }
        protected abstract void Shooting();//射击
        protected abstract void Reload();//装子弹
        //protected abstract void Aim();//瞄准
        protected bool IsAllowShooting()//射击间隔
        {
            return Time.time - LastFireTime > 1 / FireRate;
        }
        /// <summary>
        /// 子弹散射
        /// </summary>
        protected Vector3 CalculateSpreadOffset()
        {
            float tmp_SpreadPercent = SpreadAngle / EyesCamera.fieldOfView;
            return tmp_SpreadPercent * UnityEngine.Random.insideUnitCircle;
        }

        protected IEnumerator CheckReloadAmmoAnimationEnd()
        {
            while (true)
            {
                yield return null;
                GunStateInfo = GunAnimator.GetCurrentAnimatorStateInfo(2);
                if (GunStateInfo.IsTag("ReloadAmmo"))
                {
                    if (GunStateInfo.normalizedTime >= 0.9f)
                    {
                        int tmpAmmo = AmmoInMag - CurrentAmmo;//差的
                        CurrentAmmo += CurrentMaxAmmoCarried > tmpAmmo ? tmpAmmo : CurrentMaxAmmoCarried;
                        CurrentMaxAmmoCarried -= tmpAmmo;
                        CurrentMaxAmmoCarried = Mathf.Clamp(CurrentMaxAmmoCarried, 0, CurrentMaxAmmoCarried);
                        yield break;
                    }
                }

            }

        }
        protected IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;
                float tmp_CurrentFOV = 0f;
                EyesCamera.fieldOfView = Mathf.SmoothDamp(EyesCamera.fieldOfView,
                isAimming ? 26 : OriginFOV,
                ref tmp_CurrentFOV,
                Time.deltaTime * 2);
            }
        }
        internal void Aiming(bool _isAiming)
        {
            isAimming = _isAiming;
            GunAnimator.SetBool("Aim", isAimming);
            if (doAimCoroutine == null)
            {
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
            else
            {
                StopCoroutine(doAimCoroutine);
                doAimCoroutine = null;
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
        }
        internal void HoldTrigger()
        {
            DoAttack();
            IsHoldingTrigger = true;
        }
        internal void RealseTrigger()
        {
            IsHoldingTrigger = false;
        }
        internal void ReloadAmmo()
        {
            Reload();
        }

        #endregion


    }
}

