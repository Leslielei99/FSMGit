using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMouseLook : MonoBehaviour
{
    public float rotatedSpeed;
    [SerializeField]
    private Transform characterTransform;
    private Transform cameraTransform;
    public Vector2 MAXRotation;
    private float MouseX, MouseY;
    private Vector3 cameraRoatation;
    [Header("后坐力")]
    public AnimationCurve RecoilCurver;
    public Vector2 RecoilRange;
    public float RecoilFadeOutTime;
    private float currentRecoilTime;
    private Vector2 currentRecoil;

    private CameraSpring cameraSpring;

    private void Start()
    {
        cameraTransform = transform;
        cameraSpring = GetComponentInChildren<CameraSpring>();
    }

    private void Update()
    {
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        // Debug.Log("MouseX : " + MouseX + "   ,  MouseY: " + MouseY);
        CalculateRecoilOffset();
        cameraRoatation.x -= MouseY * rotatedSpeed;
        cameraRoatation.y += MouseX * rotatedSpeed;
        Debug.Log(cameraRoatation);
        cameraRoatation.x -= currentRecoil.y;
        cameraRoatation.y += currentRecoil.x;
        cameraRoatation.x = Mathf.Clamp(cameraRoatation.x, MAXRotation.x, MAXRotation.y);
        cameraTransform.rotation = Quaternion.Euler(cameraRoatation.x, cameraRoatation.y, 0);
        characterTransform.rotation = Quaternion.Euler(cameraRoatation.x, cameraRoatation.y, 0);

        //  Debug.Log(currentRecoil);
    }
    /// <summary>
    /// 后坐力偏移
    /// </summary>
    private void CalculateRecoilOffset()
    {
        currentRecoilTime += Time.deltaTime;
        float tmp_RecoilFraction = currentRecoilTime / RecoilFadeOutTime;

        float tmp_RecoilLateFraction = RecoilCurver.Evaluate(tmp_RecoilFraction);

        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilLateFraction);
    }
    public void FiringForTest()
    {
        currentRecoil += RecoilRange;
        cameraSpring.StartCameraSpring();
        currentRecoilTime = 0;
    }
}
