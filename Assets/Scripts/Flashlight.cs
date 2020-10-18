
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : BaseObject
{
    [SerializeField] private Light flashlightLight;
    private float timeout = 10000;
    private float currTime = 0;
    private float currReloadTime = 0;
    
    private KeyCode control = KeyCode.F;
    private KeyCode picker = KeyCode.P;

    private Transform TMCam;

    private float charge = 100;
    private float rechargeRate = 0.5f;
    private float unchargeRate = 1f;

    private Text chargeText;

    protected override void Awake()
    {
        base.Awake();
        flashlightLight = GetComponentInChildren<Light>();
        chargeText = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<Text>();
        TMCam = Camera.main.transform; 
    }

    private void ActiveFlashLight(bool val)
    {
        flashlightLight.enabled = val;
    }

    void Update()
    {
        if (Input.GetKeyDown(control) && !flashlightLight.enabled)
        {
            ActiveFlashLight(true);
        }
        else if(Input.GetKeyDown(control) && flashlightLight.enabled)
        {
            ActiveFlashLight(false);
        }

        if (flashlightLight.enabled)
        {
            currTime += Time.deltaTime;
            if(currTime > timeout)
            {
                currTime = 0;
                ActiveFlashLight(false);
            }
        }

        if (flashlightLight.enabled && charge > 0)
        {
            charge -= unchargeRate * Time.deltaTime;
            if (charge < 0)
                charge = 0;
        }
        else if (!flashlightLight.enabled && charge < 100 )
        {
            charge += rechargeRate * Time.deltaTime;
            if(charge>100)
                charge = 100;
        }

        if (charge < 0.01)
        {
            ActiveFlashLight(false);
        }
        else if(charge < 10)
        {
            flashlightLight.color = new Color(0.7f, 0.7f, 0.5f);
        }
        else if( charge > 10)
        {
            flashlightLight.color = new Color(1f, 1f, 1f);
        }
        chargeText.text = ((int)charge).ToString();

        if (Input.GetKeyDown(picker))
        {
            RaycastHit hit;

            Ray ray = new Ray(TMCam.position, TMCam.forward);

            if (Physics.Raycast(ray, out hit, 5.0f))
            {
                Debug.Log(hit.collider.name);
            }
        }


    }
}
