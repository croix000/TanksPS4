using UnityEngine;
using UnityEngine.UI;
using InControl;
using System.Collections;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;

    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;                


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce;

        if (m_PlayerNumber == 1 && InputManager.ActiveDevice.Action1.IsPressed && !m_Fired){
            // we released the button, having not fired yet
           
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }

        if(m_PlayerNumber==2 &&InputManager.ActiveDevice.Action2.IsPressed && !m_Fired)
            {
                // we released the button, having not fired yet

                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }
    }


    private void Fire()
    {
        Handheld.Vibrate();
        // Instantiate and launch the shell.
        m_Fired = true;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
        
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        
        m_CurrentLaunchForce = m_MinLaunchForce;
        StartCoroutine(resetFire());
    }

    IEnumerator resetFire()
    {

        yield return new WaitForSeconds(0.5f);

        m_Fired = false;

    }
}