using UnityEngine;
using UnityEngine.UI;

public class autoPilotManager : MonoBehaviour
{
    [Header("Autopilot settings")]
    public float headingAngle = 0;
    public float verticalAngle = 0, speedKnots = 200f;

    [Header("Autopilot enabling")]
    public bool headingOP;
    public bool verticalOP, speedOP;

    private Rigidbody rb;
    private float currientX = 0f, currientY = 0f, currientZ = 0f;
    private float manualZrot = 0f, manualYrot = 0f, manualXrot;
    private float verticalManualMultiplyer = 0f, horizontalManualMultiplyer = 0f;

    [Header("Speed values")]
    [Range(0.0f, 400.0f)]
    public float speed = 0f;
    public float verticalSpeed = 0f;

    [Header("Outputs")]
    public Text headingAP;
    public Text verticalAP, speedAP, headingIsOn, verticalIsOn, speedIsOn, realHeading, realVertical, realSpeed;
    public Text realTimeSpeed, realTimeHeading, realTimeAltitude;
    public GameObject verticalWarning, headingWarning, taxiPanel;

    public Button speedPlus, speedMinus, verticalPlus, verticalMinus;
    public onGroundDetector gr;
    public surfaceTouchMonitor monitor;

    [Header("Movable parts")]
    public engineRotorRotater[] engines;
    public GameObject[] allWheels;
    public GameObject[] smokes;

    [Header("Input UI")]
    public Scrollbar powerToggle;
    public Joystick sidestick;

    [Header("Audio")]
    public AudioSource outside;
    public AudioSource inside;

    private bool taxiGas, taxiBrake, taxiLeft, taxiRight;
    private bool airBrake, groundBrake;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        manageAP();
        if (taxiPanel.activeSelf && !speedOP && !verticalOP && !headingOP && speed <= 25)
            manageTaxi();

        rotateWheels();
        manageSmoke();
        applyMovementData();
        setEnginesPower();
        checkButtonsMax();
        setDataToText();
    }

    void Update()
    {
        setWarnings();
        calibrateHeading();
    }

    public void loadCurrientStats()
    {
        currientX = normalizeAngle(transform.eulerAngles.x);
        currientY = normalizeAngle(transform.eulerAngles.y);
        currientZ = normalizeAngle(transform.eulerAngles.z);
    }

    public float getSpeed() => speed;
    public bool onGround() => gr.fullyGrounded;
    public float getAltitude() => transform.position.y;

    public void switchHeading() => headingOP = !headingOP;
    public void switchVertical() => verticalOP = !verticalOP;
    public void switchSpeed() => speedOP = !speedOP;

    public void appendHeading(float value) => headingAngle += value;
    public void appendVertical(float value) => verticalAngle += value;
    public void appendSpeed(float value) => speedKnots += value;

    public void brakeDown() => taxiBrake = true;
    public void brakeUp() => taxiBrake = false;

    public void gasDown() => taxiGas = true;
    public void gasUp() => taxiGas = false;

    public void leftDown() => taxiLeft = true;
    public void leftUp() => taxiLeft = false;

    public void rightDown() => taxiRight = true;
    public void rightUp() => taxiRight = false;

    public void airBrakeDown() => airBrake = true;
    public void airBrakeUp() => airBrake = false;

    public void groundBrakeDown() => groundBrake = true;
    public void groundBrakeUp() => groundBrake = false;

    private void setDataToText()
    {
        headingAP.text = Mathf.Round(headingAngle).ToString();
        verticalAP.text = Mathf.Round(-verticalAngle).ToString();
        speedAP.text = Mathf.Round(speedKnots).ToString();

        headingIsOn.text = headingOP ? "on" : "off";
        verticalIsOn.text = verticalOP ? "on" : "off";
        speedIsOn.text = speedOP ? "on" : "off";

        realHeading.text = Mathf.Round(currientY).ToString();
        realVertical.text = Mathf.Round(-currientX).ToString();
        realSpeed.text = Mathf.Round(speed).ToString();

        realTimeSpeed.text = Mathf.Round(GetLocalZSpeed()).ToString();
        realTimeHeading.text = Mathf.Round(transform.eulerAngles.y).ToString();
        realTimeAltitude.text = Mathf.Round(transform.position.y).ToString();
    }

    private void setEnginesPower()
    {
        float pitch = 0.5f + speed / 300;
        inside.pitch = pitch;
        outside.pitch = pitch;

        foreach (var engine in engines)
        {
            engine.rotationSpeed = 3f + speed / 150;
        }
    }

    private void checkButtonsMax()
    {
        speedMinus.interactable = speedKnots > 200;
        speedPlus.interactable = speedKnots < 350;
        verticalMinus.interactable = verticalAngle < 20;
        verticalPlus.interactable = verticalAngle > -20;
    }

    private void manageTaxi()
    {
        if (GetLocalZSpeed() >= 0 && taxiGas)
            rb.AddRelativeForce(Vector3.forward * 9000000f / (1 + GetLocalZSpeed() * 1f));

        if (taxiBrake)
        {
            //
        }

        if (GetLocalZSpeed() >= 0 && taxiLeft)
            rb.AddRelativeTorque(-Vector3.up * 10000000f * GetLocalZSpeed() / (1 + GetYawRate() * 1f));
        else if (GetLocalZSpeed() >= 0 && taxiRight)
            rb.AddRelativeTorque(Vector3.up * 10000000f * GetLocalZSpeed() / (1 + GetYawRate() * 1f));
    }

    private void manageAP()
    {
        if (speedOP)
            speed = Mathf.MoveTowards(speed, speedKnots, 0.1f);

        if (verticalOP)
            currientX = Mathf.MoveTowards(currientX, verticalAngle, 0.1f);

        if (headingOP)
        {
            currientY = Mathf.MoveTowards(currientY, headingAngle, 0.2f);
            if (currientY < headingAngle)
                currientZ = Mathf.MoveTowards(currientZ, -20, 0.3f);
            else if (currientY > headingAngle)
                currientZ = Mathf.MoveTowards(currientZ, 20, 0.3f);

            currientZ = Mathf.MoveTowards(currientZ, 0, 0.1f);
            manualYrot = currientY;
        }
    }

    private void applyMovementData()
    {
        rb.AddRelativeForce(Vector3.forward * powerToggle.value * 80000000f / (1 + GetLocalZSpeed() * 1f));
        if (GetLocalZSpeed() > 0) rb.AddForce(Vector3.up * GetLocalZSpeed() * 5000f / (1 + GetYSpeed() * 1f));
        
        rb.AddRelativeTorque(Vector3.right * sidestick.Vertical * GetLocalZSpeed() * 50000f / (1 + GetPitchRate() * 1f));
        
        rb.AddRelativeTorque(Vector3.forward * sidestick.Horizontal * -GetLocalZSpeed() * 2000f / (1 + GetRollRate() * 1f));
        rb.AddRelativeTorque(Vector3.up * GetRollAngle() * -GetLocalZSpeed() * 100f / (1 + GetYawRate() * 1f));
        
        // Компенсируем боковые силы
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        rb.AddRelativeForce(-localVelocity.x * rb.mass, 0, 0);
    }

    private void setWarnings()
    {
        verticalWarning.SetActive(speed < 150);
        if (speed < 150) verticalOP = false;

        headingWarning.SetActive(speed < 180);
        if (speed < 180) headingOP = false;
    }

    private void calibrateHeading()
    {
        headingAngle = normalizeAngle(headingAngle);
    }

    private float normalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
    
    public Vector3 GetAngularVelocity()
    {
        Vector3 worldAngularVelocity = rb.angularVelocity;
        Vector3 localAngularVelocity = transform.InverseTransformDirection(worldAngularVelocity);

        return localAngularVelocity;
    }

    public float GetPitchRate()
    {
        return GetAngularVelocity().x;
    }

    public float GetYawRate()
    {
        return GetAngularVelocity().y;
    }
    
    public float GetRollRate()
    {
        return GetAngularVelocity().z;
    }
    
    public float GetRollAngle()
    {
        float rollAngle = transform.eulerAngles.z;
        if (rollAngle > 180)
        {
            rollAngle -= 360;
        }
        return rollAngle;
    }
    
    public float GetLocalZSpeed()
    {
        Vector3 worldVelocity = rb.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(worldVelocity);
        return localVelocity.z;
    }
    
    public float GetYSpeed()
    {
        return rb.velocity.y;
    }

    private void rotateWheels()
    {
        if (gr.fullyGrounded)
        {
            foreach (var wheel in allWheels)
                wheel.transform.Rotate(speed * 1f, 0, 0);
        }
    }

    private void manageSmoke()
    {
        foreach (var smoke in smokes)
        {
            smoke.SetActive(transform.position.y >= 1000 && speed >= 200);
        }
    }
}
