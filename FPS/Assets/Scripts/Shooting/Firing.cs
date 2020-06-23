using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.Control;


namespace FPS.Shooting
{
    public class Firing : MonoBehaviour
    {
        [SerializeField] Transform parentForTemp;

        [Header("Constant Recoil Stats")]
        [SerializeField] float timeBetweenShots = 0.1f;
        [SerializeField] GameObject bulletHolePrefab;

        [Header("FX")]
        public ParticleSystem muzzleFlash;
        public  ParticleSystem hitEffect;
        public GameObject shootAudio;

        

        public static bool assistAdjust = false;


        RecoilHandler recoilHandler;
        float timeSinceLastShot = Mathf.Infinity;
        float maxTimeHeldLeftButton = 1f;
        


        void Awake()
        {
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = 500;
        }
        private void Start()
        {
            recoilHandler = GetComponent<RecoilHandler>();
        }

        

        private void Update()
        {
            if(Input.GetMouseButton(0))
            {
                if(timeSinceLastShot >= timeBetweenShots)
                {
                    recoilHandler.Recoil();
                    ProcessFX();
                    ProcessRaycast();

                    assistAdjust = true;

                    timeSinceLastShot = 0;
                }


                recoilHandler.timeHeldDownLeftMouse += Time.deltaTime * 1/timeBetweenShots;
                
            }

            else
            {
                assistAdjust = false;
                recoilHandler.timeHeldDownLeftMouse -= Time.deltaTime * 1/(timeBetweenShots * 1.2f);
            }

            timeSinceLastShot += Time.deltaTime;
            recoilHandler.timeHeldDownLeftMouse = Mathf.Clamp(recoilHandler.timeHeldDownLeftMouse, 0, maxTimeHeldLeftButton * (1/timeBetweenShots));



            
        }

        private void ProcessFX()
        {
            MuzzleFlashVFX();
            ShootAudio();
        }


        private void ShootAudio()
        {
            GameObject shootFX = Instantiate(shootAudio, transform.position, Quaternion.identity);
            shootFX.transform.parent = parentForTemp;
            Destroy(shootFX, 1f);
        }

        private void HitVFX(RaycastHit hit)
        {
            ParticleSystem hitVFX = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            hitVFX.transform.parent = parentForTemp;
            Destroy(hitVFX.gameObject, 1.1f);
        }

        private void ProcessRaycast()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, 
                Camera.main.transform.forward + Camera.main.transform.right * recoilHandler.deviationRotY + Camera.main.transform.up * recoilHandler.deviationRotX, out hit))
            {
                HitVFX(hit);
                GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point - transform.forward * 0.01f, Quaternion.LookRotation(hit.normal));
                bulletHole.transform.parent = parentForTemp;
                Destroy(bulletHole, 30f);
            }
        }

        private void MuzzleFlashVFX()
        {
            muzzleFlash.Play();
        }

    }
}
