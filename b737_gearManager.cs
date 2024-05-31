using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class b737_gearManager : MonoBehaviour
{
    [SerializeField] private float speed;
    
    [Header("Front Gear")] 
    [SerializeField] private GameObject frontGear;
    [SerializeField] private GameObject leftGearHatch, rightGearHatch;
    
    [Header("Left Gear")] 
    [SerializeField] private GameObject mainLeftHatch;
    [SerializeField] private GameObject secLeftHatch, mainLeftGear, secLeftGear;
    
    [Header("Right Gear")] 
    [SerializeField] private GameObject mainRightHatch;
    [SerializeField] private GameObject secRightHatch, mainRightGear, secRightGear;

    [SerializeField] private Text[] gearIndicators;

    private List<Gear> _allGear = new();

    void Start()
    {
        _allGear.Add(new FrontGear(this, leftGearHatch, rightGearHatch, frontGear, speed));
        _allGear.Add(new BackGear(true, speed, this, mainLeftHatch, secLeftHatch, mainLeftGear, secLeftGear));
        _allGear.Add(new BackGear(false, speed, this, mainRightHatch, secRightHatch, mainRightGear, secRightGear));
    }

    void Update()
    {
        foreach (Text text in gearIndicators)
        {
            if (AllGearDown()) text.text = "DOWN";
            else if (AllGearUp()) text.text = "UP";
        }
    }

    [ContextMenu("Switch gear")]
    public void SwitchGear()
    {
        if (!OnGearProcess())
            foreach (Gear g in _allGear)
                g.Switch();
    }

    private bool OnGearProcess()
    {
        bool onProcess = false;
        
        foreach (Gear g in _allGear)
            if (g.InSwitchingProcess)
                onProcess = true;

        return onProcess;
    }

    private bool AllGearDown()
    {
        bool opened = true;
        
        foreach (Gear g in _allGear)
            if (!g.IsOpen)
                opened = false;

        return opened;
    }
    
    private bool AllGearUp()
    {
        bool closed = true;
        
        foreach (Gear g in _allGear)
            if (g.IsOpen)
                closed = false;

        return closed;
    }
}

public class Gear
{
    protected readonly MonoBehaviour monoBehaviour;
    protected bool IsGearOpen = true, InProcess;
    protected readonly float _switchSpeed;

    public Gear(float switchSpeed, MonoBehaviour monoBehaviour)
    {
        this.monoBehaviour = monoBehaviour;
        _switchSpeed = switchSpeed;
    }

    public bool IsOpen
    {
        get => IsGearOpen;
        set => IsGearOpen = value;
    }

    public bool InSwitchingProcess => InProcess;

    protected internal virtual void Switch() {}
    
    internal void RoundRotation(GameObject obj)
    {
        Quaternion currentRotation = obj.transform.rotation;

        float x = Mathf.Round(currentRotation.eulerAngles.x);
        float y = Mathf.Round(currentRotation.eulerAngles.y);
        float z = Mathf.Round(currentRotation.eulerAngles.z);

        obj.transform.rotation = Quaternion.Euler(x, y, z);
    }
}

public class FrontGear : Gear
{
    private readonly GameObject _leftHatch, _rightHatch, _gearObject;

    public FrontGear(MonoBehaviour monoBehaviour, GameObject leftHatch, GameObject rightHatch, GameObject gearObject, float speed)
        : base(speed, monoBehaviour)
    {
        _leftHatch = leftHatch;
        _rightHatch = rightHatch;
        _gearObject = gearObject;
    }

    private IEnumerator GearUp()
    {
        float degrees = 0f;
        
        while (Math.Abs(degrees - 110) > 0f)
        {
            float appendValue = (degrees + _switchSpeed < 110)
                ? _switchSpeed : (110 - degrees);
            
            _gearObject.transform.Rotate(appendValue, 0, 0);
            degrees += appendValue;
            yield return new WaitForFixedUpdate();
        }

        degrees = 0f;
        
        while (Math.Abs(degrees - 90) > 0f)
        {
            float appendValue = (degrees + _switchSpeed < 90)
                ? _switchSpeed : (90 - degrees);
            
            _leftHatch.transform.Rotate(0, 0, -appendValue);
            _rightHatch.transform.Rotate(0, 0, appendValue);
            degrees += appendValue;
            yield return new WaitForFixedUpdate();
        }

        IsOpen = false;
        InProcess = false;
        
        RoundRotation(_leftHatch);
        RoundRotation(_rightHatch);
        RoundRotation(_gearObject);
    }
    
    private IEnumerator GearDown()
    {
        float degrees = 0f;
        
        while (Math.Abs(degrees - 90) > 0f)
        {
            float appendValue = (degrees + _switchSpeed < 90)
                ? _switchSpeed : (90 - degrees);
            
            _leftHatch.transform.Rotate(0, 0, appendValue);
            _rightHatch.transform.Rotate(0, 0, -appendValue);
            degrees += appendValue;
            yield return new WaitForFixedUpdate();
        }
        
        degrees = 0f;
        
        while (Math.Abs(degrees - 110) > 0f)
        {
            float appendValue = (degrees + _switchSpeed < 110)
                ? _switchSpeed : (110 - degrees);
            
            _gearObject.transform.Rotate(-appendValue, 0, 0);
            degrees += appendValue;
            yield return new WaitForFixedUpdate();
        }
        
        IsOpen = true;
        InProcess = false;
        
        RoundRotation(_leftHatch);
        RoundRotation(_rightHatch);
        RoundRotation(_gearObject);
    }

    protected internal override void Switch()
    {
        InProcess = true;
        monoBehaviour.StartCoroutine(IsOpen ? GearUp() : GearDown());
    }
}

public class BackGear : Gear
{
    private readonly GameObject _mainHatch, _secHatch, _mainGear, _secGear;
    private readonly bool _isLeft;

    public BackGear(bool isLeft, float switchSpeed, MonoBehaviour monoBehaviour, GameObject mainHatch, GameObject secHatch, GameObject mainGear, GameObject secGear) : base(switchSpeed, monoBehaviour)
    {
        _mainHatch = mainHatch;
        _secHatch = secHatch;
        _mainGear = mainGear;
        _secGear = secGear;
        _isLeft = isLeft;
    }

    private IEnumerator GearUp()
    {
        float degrees = 0f;
        
        while (Math.Abs(degrees - 90) > 0f)
        {
            float appendValue = (degrees + _switchSpeed < 90)
                ? _switchSpeed : (90 - degrees);
            
            if (degrees < 78)
            {
                if (_isLeft) _mainGear.transform.Rotate(0, 0, -((degrees + appendValue < 78) ? appendValue : (78 - degrees)));
                else _mainGear.transform.Rotate(0, 0, (degrees + appendValue < 78) ? appendValue : (78 - degrees));
            }
            
            _secGear.transform.Rotate(0, 0, appendValue * 116/90);
            _mainHatch.transform.Rotate(0, 0, (_isLeft) ? -appendValue : appendValue);
            _secHatch.transform.Rotate(0, 0, (_isLeft) ? -appendValue/3 : appendValue/3);
            
            degrees += appendValue;
            yield return new WaitForFixedUpdate();
        }
        
        IsOpen = false;
        InProcess = false;
        
        RoundRotation(_mainHatch);
        RoundRotation(_secHatch);
        RoundRotation(_mainGear);
        RoundRotation(_secGear);
    }
    
    private IEnumerator GearDown()
    {
        float degrees = 0f;
        
        while (Math.Abs(degrees - 90) > 0f)
        {
            float appendValue = (degrees + _switchSpeed < 90)
                ? _switchSpeed : (90 - degrees);

            if (degrees < 78)
            {
                if (_isLeft) _mainGear.transform.Rotate(0, 0, (degrees + appendValue < 78) ? appendValue : (78 - degrees));
                else _mainGear.transform.Rotate(0, 0, -((degrees + appendValue < 78) ? appendValue : (78 - degrees)));
            }
            
            _secGear.transform.Rotate(0, 0, -appendValue * 116/90);
            _mainHatch.transform.Rotate(0, 0, (_isLeft) ? appendValue : -appendValue);
            _secHatch.transform.Rotate(0, 0, (_isLeft) ? appendValue/3 : -appendValue/3);
            
            degrees += appendValue;
            yield return new WaitForFixedUpdate();
        }
        
        IsOpen = true;
        InProcess = false;
        
        RoundRotation(_mainHatch);
        RoundRotation(_secHatch);
        RoundRotation(_mainGear);
        RoundRotation(_secGear);
    }
    
    protected internal override void Switch()
    {
        InProcess = true;
        monoBehaviour.StartCoroutine(IsOpen ? GearUp() : GearDown());
    }
}
