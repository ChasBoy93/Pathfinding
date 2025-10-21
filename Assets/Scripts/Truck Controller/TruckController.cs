using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TruckController : MonoBehaviour
{
    [Header("Engine Settings")]
    public float engineIdleRPM = 800f;
    public float engineMaxRPM = 2500f;
    public float torque = 400f;
    public float brakeTorque = 5000f;
    public bool useAutomatic = true;

    [Header("Suspension Settings")]
    public float suspensionDistance = 0.3f;
    public float suspensionSpring = 35000f;
    public float suspensionDamper = 4500f;
    public float wheelMass = 100f;

    [Header("Wheel Colliders")]
    public WheelCollider[] driveWheels;
    public WheelCollider[] steerWheels;

    [Header("Driver Settings")]
    public Transform driverSeat;
    private GameObject currentDriver;

    [Header("Engine Sounds")]
    public AudioSource startSound;
    public AudioSource shutdownSound;
    public AudioSource idleSound;
    public AudioSource lowRPMSound;
    public AudioSource midRPMSound;
    public AudioSource highRPMSound;

    private Rigidbody rb;
    private float currentRPM;
    private bool engineOn = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down * 0.5f; // stability
        currentRPM = engineIdleRPM;
        SetupWheelColliders();
    }

    private void Update()
    {
        HandleEnterExit();
        if (engineOn && currentDriver != null)
        {
            HandleEngine();
            HandleSteering();
            HandleBrakes();
            UpdateEngineSounds();
        }
    }

    private void SetupWheelColliders()
    {
        foreach (WheelCollider wc in driveWheels)
        {
            wc.suspensionDistance = suspensionDistance;
            JointSpring spring = wc.suspensionSpring;
            spring.spring = suspensionSpring;
            spring.damper = suspensionDamper;
            spring.targetPosition = 0.5f;
            wc.suspensionSpring = spring;
            wc.mass = wheelMass;
        }

        foreach (WheelCollider wc in steerWheels)
        {
            wc.suspensionDistance = suspensionDistance;
            JointSpring spring = wc.suspensionSpring;
            spring.spring = suspensionSpring;
            spring.damper = suspensionDamper;
            spring.targetPosition = 0.5f;
            wc.suspensionSpring = spring;
            wc.mass = wheelMass;
        }
    }

    private void HandleEngine()
    {
        float accel = Input.GetAxis("Vertical");
        currentRPM = Mathf.Lerp(engineIdleRPM, engineMaxRPM, Mathf.Abs(accel));

        foreach (WheelCollider wheel in driveWheels)
        {
            wheel.motorTorque = accel * torque;
        }
    }

    private void HandleSteering()
    {
        float steer = Input.GetAxis("Horizontal");
        foreach (WheelCollider wheel in steerWheels)
        {
            wheel.steerAngle = steer * 30f;
        }
    }

    private void HandleBrakes()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (WheelCollider wheel in driveWheels)
                wheel.brakeTorque = brakeTorque;
        }
        else
        {
            foreach (WheelCollider wheel in driveWheels)
                wheel.brakeTorque = 0f;
        }
    }

    private void UpdateEngineSounds()
    {
        float accel = Mathf.Abs(Input.GetAxis("Vertical"));

        if (idleSound) idleSound.volume = Mathf.Clamp01(1 - accel);
        if (lowRPMSound) lowRPMSound.volume = Mathf.Clamp01(accel * 0.5f);
        if (midRPMSound) midRPMSound.volume = Mathf.Clamp01(Mathf.Max(0, accel - 0.5f));
        if (highRPMSound) highRPMSound.volume = Mathf.Clamp01(Mathf.Max(0, accel - 0.8f));
    }

    private void HandleEnterExit()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentDriver == null)
            {
                // Enter
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = driverSeat.position;
                    player.transform.rotation = driverSeat.rotation;
                    player.transform.SetParent(driverSeat);
                    player.GetComponent<CharacterController>().enabled = false;
                    currentDriver = player;
                    StartEngine();
                }
            }
            else
            {
                // Exit
                StopEngine();
                currentDriver.transform.SetParent(null);
                currentDriver.GetComponent<CharacterController>().enabled = true;
                currentDriver = null;
            }
        }
    }

    private void StartEngine()
    {
        if (!engineOn && startSound)
        {
            startSound.Play();
        }
        engineOn = true;
    }

    private void StopEngine()
    {
        if (engineOn && shutdownSound)
        {
            shutdownSound.Play();
        }
        engineOn = false;
        foreach (WheelCollider wheel in driveWheels)
            wheel.motorTorque = 0f;
    }
}
