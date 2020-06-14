using UnityEngine;
using System.Collections;

public class TestSway : MonoBehaviour
{
    public float amount = 0.02f;
    public float maxamount = 0.03f;
    public float smooth = 3;
    private Quaternion def;
    private bool Paused = false;

    void Start()
    {
        def = transform.localRotation;
    }

    void FixedUpdate()
    {
        float factorX = (Input.GetAxis("Mouse Y")) * amount;
        float factorY = -(Input.GetAxis("Mouse X")) * amount;
        float factorZ = -Input.GetAxis("Vertical") * amount;
        factorZ = 0 * amount;

        if (!Paused)
        {
            if (factorX > maxamount)
                factorX = maxamount;

            if (factorX < -maxamount)
                factorX = -maxamount;

            if (factorY > maxamount)
                factorY = maxamount;

            if (factorY < -maxamount)
                factorY = -maxamount;

            if (factorZ > maxamount)
                factorZ = maxamount;

            if (factorZ < -maxamount)
                factorZ = -maxamount;

            Quaternion Final = Quaternion.Euler(def.x + factorX, def.y + factorZ, def.z + factorY);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Final, (Time.time * smooth));
        }
    }
}
