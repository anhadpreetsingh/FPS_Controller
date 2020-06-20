using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.Control;


namespace FPS.Shooting
{
    public class Firing : MonoBehaviour
    {
        [Header("CamRecoilStats")]
        public float camRecoilReturnSpeed = 5f;
        public float yRecoil = 0.5f;
        public float xRecoil = 0.5f;
        
        [Header("Current Recoil Stats")]
        public Vector3 currentPos;
        public Quaternion currentRot;        
        public Vector3 currentRecoilFactor = new Vector3(1f, 1f, 1f);
        public float currentReturnSpeed;
        public float currentKickSpeed;
        public float currentRecoilRotFactor;
        public Vector3 currentCamRecoil;

        [Header("Hip Fire Recoil Stats")]
        public Vector3 origPos;
        public Quaternion origRot;
        public float origReturnSpeed = 3f;
        public float origKickSpeed = 5f;
        public float origRecoilRotFactor = 0.01f;
        public Vector3 origCamRecoil;

        [Header("ADS Recoil Stats")]
        public Vector3 adsPosition;
        public Vector3 adsRotation;
        public float adsReturnSpeed = 5f;
        public float adsKickSpeed = 10f;
        public float adsRecoilRotFactor = 0.005f;
        public Vector3 adsCamRecoil;

        [Header("Constant Recoil Stats")]

        [SerializeField] float timeBetweenShots = 0.1f;
        [SerializeField] GameObject bulletHolePrefab;
        
        
        
        


        [Header("FX")]
        public ParticleSystem muzzleFlash;
        public  ParticleSystem hitEffect;
        public GameObject shootAudio;

        Vector3 recoilPos;
        Quaternion recoilRot;
        float deviationRotY;
        float deviationRotX;
        float timeSinceLastShot = Mathf.Infinity;
        float timeHeldDownLeftMouse = 0f;
        float maxTimeHeldLeftButton = 1f;
        bool isAimingDownSights = false;

        public static bool isShooting = false;




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
            if(!isShooting)
            {
                LookAround.testEulerAngles = Vector3.Lerp(LookAround.testEulerAngles, LookAround.assistEulerAngles, camRecoilReturnSpeed * Time.fixedDeltaTime);
            }
            
            transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, currentReturnSpeed * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, currentRot, currentReturnSpeed * Time.fixedDeltaTime);
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

                    CamRecoil();


                    isShooting = true;

                    timeSinceLastShot = 0;
                }
                

                timeHeldDownLeftMouse += Time.deltaTime * 1/timeBetweenShots;
                
            }

            else
            {
                isShooting = false;
                timeHeldDownLeftMouse -= Time.deltaTime * 1/(timeBetweenShots * 1.2f);
            }

            if(Input.GetMouseButton(1))
            {
                isAimingDownSights = true;
            }
            else
            {
                isAimingDownSights = false;
            }


            if(isAimingDownSights)
            {
                ProcessADS();
            }
            else
            {
                ProcessHipFireAim();
            }


            timeSinceLastShot += Time.deltaTime;
            timeHeldDownLeftMouse = Mathf.Clamp(timeHeldDownLeftMouse, 0, maxTimeHeldLeftButton * (1/timeBetweenShots));



            print(LookAround.assistEulerAngles);
        }

        private void ProcessHipFireAim()
        {
            currentPos = origPos;
            currentRot = origRot;

            currentReturnSpeed = origReturnSpeed;
            currentKickSpeed = origKickSpeed;
            currentRecoilRotFactor = origRecoilRotFactor;
        }

        private void ProcessADS()
        {
            currentPos = adsPosition;
            currentRot.eulerAngles = adsRotation;

            currentReturnSpeed = adsReturnSpeed;
            currentKickSpeed = adsKickSpeed;
            currentRecoilRotFactor = adsRecoilRotFactor;
        }

        private void CalculateRecoil()
        {
            recoilPos = currentPos + new Vector3(Random.Range(-currentRecoilFactor.x, currentRecoilFactor.x),
                Random.Range(currentRecoilFactor.y, currentRecoilFactor.y),
                currentRecoilFactor.z);

            

            deviationRotY = Random.Range(-timeHeldDownLeftMouse * currentRecoilRotFactor, timeHeldDownLeftMouse * currentRecoilRotFactor);
            deviationRotX = Random.Range((-timeHeldDownLeftMouse * currentRecoilRotFactor)/2f, timeHeldDownLeftMouse * currentRecoilRotFactor);

            recoilRot = Quaternion.Euler(currentPos.x - deviationRotX * 100f, currentPos.y + deviationRotY * 100f, currentPos.z);

           
        }

        private void ExecuteRecoil()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, recoilPos, currentKickSpeed * 0.02f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, recoilRot, currentKickSpeed * 0.02f);
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
                Instantiate(bulletHolePrefab, hit.point - transform.forward * 0.01f, Quaternion.LookRotation(hit.normal));
            }
        }

        private void MuzzleFlashVFX()
        {
            muzzleFlash.Play();
        }

        private void CamRecoil()
        {
            LookAround.testEulerAngles += new Vector3(-xRecoil, yRecoil, 0);
        }

        

    }
}
