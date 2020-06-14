using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Shooting
{
    public class Firing : MonoBehaviour
    {
        [SerializeField] Vector3 recoilFactor = new Vector3(1f, 1f, 1f);
        [SerializeField] float kickSpeed = 5f;
        [SerializeField] float returnSpeed = 3f;
        [SerializeField] float timeBetweenShots = 0.1f;
        [SerializeField] float recoilRotFactor = 1f;
        [SerializeField] float adsSpeed = 5f;
        [SerializeField] Vector3 adsPosition;
        [SerializeField] Quaternion adsRotation = Quaternion.identity;
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] ParticleSystem hitEffect;
        [SerializeField] GameObject shootAudio;
        
        
        Vector3 recoilPos;
        Vector3 origPos;
        Quaternion origRot;
        Quaternion recoilRot;
        float deviationRotY;
        float deviationRotX;
        float timeSinceLastShot = Mathf.Infinity;
        float timeHeldDownLeftMouse = 0f;
        float maxTimeHeldLeftButton = 1f;
        bool isAimingDownSights = false;




        void Awake()
        {
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = 500;
        }
        private void Start()
        {
            origPos = transform.localPosition;
            origRot = transform.localRotation;
            
        }

        private void FixedUpdate()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, origPos, returnSpeed * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, origRot, returnSpeed * Time.fixedDeltaTime);
        }

        private void Update()
        {
            if(Input.GetMouseButton(0))
            {
                if(timeSinceLastShot >= timeBetweenShots)
                {
                    CalculateRecoil();
                    ExecuteRecoil();
                    
                    MuzzleFlashVFX();
                    ShootAudio();
                    ProcessRaycast();

                    timeSinceLastShot = 0;
                }

                timeHeldDownLeftMouse += Time.deltaTime * 1/timeBetweenShots;
                

            }

            else
            {
                timeHeldDownLeftMouse -= Time.deltaTime * 1/(timeBetweenShots * 1.2f);
            }

            

            
            

            timeSinceLastShot += Time.deltaTime;
            timeHeldDownLeftMouse = Mathf.Clamp(timeHeldDownLeftMouse, 0, maxTimeHeldLeftButton * (1/timeBetweenShots));

            
        }

        

        

        private void CalculateRecoil()
        {
            recoilPos = origPos + new Vector3(Random.Range(-recoilFactor.x, recoilFactor.x),
                Random.Range(recoilFactor.y, recoilFactor.y),
                recoilFactor.z);

            

            deviationRotY = Random.Range(-timeHeldDownLeftMouse * recoilRotFactor, timeHeldDownLeftMouse * recoilRotFactor);
            deviationRotX = Random.Range(0, timeHeldDownLeftMouse * recoilRotFactor);

            recoilRot = Quaternion.Euler(origPos.x - deviationRotX * 100f, origPos.y + deviationRotY * 100f, origPos.z);

            
            
        }

        private void ExecuteRecoil()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, recoilPos, kickSpeed * 0.02f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, recoilRot, kickSpeed * 0.02f);
        }

        private void ShootAudio()
        {
            GameObject shootFX = Instantiate(shootAudio, transform.position, Quaternion.identity);
            Destroy(shootFX, 1f);
        }

        private void HitVFX(RaycastHit hit)
        {
            ParticleSystem hitVFX = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitVFX.gameObject, 1.1f);
        }

        private void ProcessRaycast()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, 
                Camera.main.transform.forward + Camera.main.transform.right * deviationRotY + Camera.main.transform.up * deviationRotX, out hit))
            {
                HitVFX(hit);
            }
        }

        private void MuzzleFlashVFX()
        {
            muzzleFlash.Play();
        }

        

    }
}
