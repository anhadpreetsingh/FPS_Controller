using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.Control;

namespace FPS.Shooting
{
    public class RecoilHandler : MonoBehaviour
    {
        [Header("CamRecoilStats")]
        public float camRecoilReturnSpeed = 5f;
        public float yRecoil = 0.5f;
        public float xRecoil = 0.5f;
        public static float xMovedRot = 3f;
        public static float yMovedRot = 1f;
        

        [Header("Current Recoil Stats")]
        public Vector3 currentPos;
        public Quaternion currentRot;
        public Vector3 currentRecoilFactor = new Vector3(1f, 1f, 1f);
        public float currentReturnSpeed;
        public float currentKickSpeed;
        public float currentRecoilRotFactor;
        

        [Header("Hip Fire Recoil Stats")]
        public Vector3 origPos;
        public Quaternion origRot;
        public float origReturnSpeed = 3f;
        public float origKickSpeed = 5f;
        public float origRecoilRotFactor = 0.01f;
        

        [Header("ADS Recoil Stats")]
        public Vector3 adsPosition;
        public Vector3 adsRotation;
        public float adsReturnSpeed = 5f;
        public float adsKickSpeed = 10f;
        public float adsRecoilRotFactor = 0.005f;
        




        Vector3 recoilPos;
        Quaternion recoilRot;
        public float deviationRotY;
        public float deviationRotX;

        public float timeHeldDownLeftMouse = 0f;

        [SerializeField] Transform player;

        private void Start()
        {
            origPos = transform.localPosition;
            origRot = transform.localRotation;
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                ProcessADS();
            }
            else
            {
                ProcessHipFireAim();
            }

            
        }

        private void FixedUpdate()
        {
            if (!Firing.assistAdjust) // todo fix this using interfaces
            {
                LookAround.testEulerAngles = Vector3.Lerp(LookAround.testEulerAngles, new Vector3(LookAround.assistEulerAngles.x, 0f, 0f), camRecoilReturnSpeed * Time.fixedDeltaTime);
                LookAround.playerEulerAngles = Vector3.Lerp(LookAround.playerEulerAngles, new Vector3(0f, LookAround.assistEulerAngles.y, 0f), camRecoilReturnSpeed * Time.fixedDeltaTime);
            }

            

            transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, currentReturnSpeed * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, currentRot, currentReturnSpeed * Time.fixedDeltaTime);
        }





        public void Recoil()
        {
            CalculateRecoil();
            ExecuteRecoil();
            CamRecoil();
        }





        public void ProcessHipFireAim()
        {
            currentPos = origPos;
            currentRot = origRot;

            currentReturnSpeed = origReturnSpeed;
            currentKickSpeed = origKickSpeed;
            currentRecoilRotFactor = origRecoilRotFactor;
        }

        public void ProcessADS()
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
            deviationRotX = Random.Range((-timeHeldDownLeftMouse * currentRecoilRotFactor) / 2f, timeHeldDownLeftMouse * currentRecoilRotFactor);

            recoilRot = Quaternion.Euler(currentPos.x - deviationRotX * 100f, currentPos.y + deviationRotY * 100f, currentPos.z);


        }

        private void ExecuteRecoil()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, recoilPos, currentKickSpeed * 0.02f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, recoilRot, currentKickSpeed * 0.02f);
        }

        private void CamRecoil()
        {
            LookAround.testEulerAngles += new Vector3(-xRecoil, 0f, 0f);
            LookAround.playerEulerAngles += new Vector3(0f, yRecoil, 0f);
        }






    }
}

