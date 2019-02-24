using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CalibratePosition : MonoBehaviour {

    [SerializeField]
    private Canvas CalibrationUI;

    [SerializeField]
    private GameObject OVRCameraObject;
    private Vector3 OVRInitialPosition;
    private Quaternion OVRInitialQaurt;
    private Vector3 OVRInitialRotation;

    // Calibration Saves
    private float ForwardCal;
    private float RightCal;
    private float UpCal;
    private float YawCal;
    private float RollCal;
    private float PitchCal;

    // UI
    [SerializeField]
    private InputField InputLinear;
    [SerializeField]
    private InputField InputStrafe;
    [SerializeField]
    private InputField InputRise;

    [SerializeField]
    private InputField InputPitch;
    [SerializeField]
    private InputField InputYaw;
    [SerializeField]
    private InputField InputRoll;


    // Use this for initialization
    void Start () {
        OVRInitialPosition = !OVRCameraObject ? Vector3.zero
            : OVRCameraObject.transform.localPosition;
        OVRInitialRotation = !OVRCameraObject ? Vector3.zero
             : OVRCameraObject.transform.localEulerAngles;
        OVRInitialQaurt = !OVRCameraObject ? Quaternion.identity
            : OVRCameraObject.transform.localRotation;

        // Get old calibration
        ForwardCal = PlayerPrefs.GetFloat("FC", 0f);
        RightCal = PlayerPrefs.GetFloat("RiC", 0f);
        UpCal = PlayerPrefs.GetFloat("UC", 0f);
        YawCal = PlayerPrefs.GetFloat("YC", 0f);
        RollCal = PlayerPrefs.GetFloat("RoC", 0f);
        PitchCal = PlayerPrefs.GetFloat("PC", 0f);

        // Set calibration
        SetCalibration();

        // UI


        if (!(SceneManager.GetActiveScene().name == "Ship Calibration"))
            CalibrationUI.gameObject.SetActive(false);
    }

    public void SetForwardCal(string f)
    {
        ForwardCal = float.Parse(f);
        SetForwardCal(ForwardCal);
    }

    public void SetForwardCal(float f)
    {
        ForwardCal = f;
        PlayerPrefs.SetFloat("FC", ForwardCal);
        InputLinear.text = "" + ForwardCal;
        SetCalibration();
    }

    public void SetRightCal(string f)
    {
        RightCal = float.Parse(f);
        SetRightCal(RightCal);
    }

    public void SetRightCal(float f)
    {
        RightCal = f;
        PlayerPrefs.SetFloat("RiC", RightCal);
        InputStrafe.text = "" + RightCal;
        SetCalibration();
    }

    public void SetUpCal(string f)
    {
        UpCal = float.Parse(f);
        SetUpCal(UpCal);
    }

    public void SetUpCal(float f)
    {
        UpCal = f;
        PlayerPrefs.SetFloat("UC", UpCal);
        InputRise.text = "" + UpCal;
        SetCalibration();
    }

    public void SetYawCal(string f)
    {
        YawCal = float.Parse(f);
        SetYawCal(YawCal);
    }

    public void SetYawCal(float f)
    {
        YawCal = f;
        PlayerPrefs.SetFloat("YC", YawCal);
        InputYaw.text = "" + YawCal;
        SetCalibration();
    }

    public void SetRollCal(string f)
    {
        RollCal = float.Parse(f);
        SetRollCal(RollCal);
    }

    public void SetRollCal(float f)
    {
        RollCal = f;
        PlayerPrefs.SetFloat("RoC", RollCal);
        InputRoll.text = "" + RollCal;
        SetCalibration();
    }

    public void SetPitchCal(string f)
    {
        PitchCal = float.Parse(f);
        SetPitchCal(PitchCal);
    }

    public void SetPitchCal(float f)
    {
        PitchCal = f;
        PlayerPrefs.SetFloat("PC", PitchCal);
        InputPitch.text = "" + PitchCal;
        SetCalibration();
    }

    public void IncrementForward(float f)
    {
        ForwardCal += f;
        SetForwardCal(ForwardCal);
    }
    public void IncrementUp(float f)
    {
        UpCal += f;
        SetUpCal(UpCal);
    }
    public void IncrementRight(float f)
    {
        RightCal += f;
        SetRightCal(RightCal);
    }
    public void IncrementPitch(float f)
    {
        PitchCal += f;
        SetPitchCal(PitchCal);
    }
    public void IncrementYaw(float f)
    {
        YawCal += f;
        SetYawCal(YawCal);
    }
    public void IncrementRoll(float f)
    {
        RollCal += f;
        SetRollCal(RollCal);
    }

    private void SetCalibration()
    {
        // Set back to initial
        OVRCameraObject.transform.localPosition = OVRInitialPosition;
        OVRCameraObject.transform.localRotation = OVRInitialQaurt;

        // Set Changes
        OVRCameraObject.transform.localPosition += new Vector3(RightCal, UpCal, ForwardCal);
        OVRCameraObject.transform.localRotation = Quaternion.Euler(PitchCal, YawCal, RollCal);
    }
}
