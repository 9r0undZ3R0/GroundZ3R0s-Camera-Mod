using BepInEx;
using Cinemachine;
using GorillaNetworking;
using Libraries;
using Photon.Pun;
using System;
using System.Diagnostics;
using UnityEngine;

namespace GroundZ3R0s_Camera_Mod
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        bool showGUI = false;
        bool headcam = false;
        bool bodycam = false;
        bool rotatecam = false;
        bool canmove = false;
        bool freecam = false;
        public static GameObject Camera = null;
        public static Vector3 cameraSize = new Vector3(0.25f, 0.2f, 0.02f);
        public static Vector3 cameraPos = new Vector3(-65f, 12f, -82f);
        Vector3 previousMousePosition;
        GameObject ThirdPersonCameraGO = null;
        float FOVSLIDER = 60f;

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }
        void __CameraInit__()
        {
            Camera = ObjLIB.CreateObj("GroundZ3R0's Camera", PrimitiveType.Cube, "GorillaTag/UberShader", false, cameraSize, cameraPos, new Color(125f, 75f, 125f));
            Camera.transform.parent = null;

            var CMVirtualCameraGO = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera/CM vcam1");
            var CMVirtualCamera = CMVirtualCameraGO.GetComponent<CinemachineVirtualCamera>();

            CMVirtualCamera.enabled = false;
            ThirdPersonCameraGO.transform.SetParent(Camera.transform, true);
            ThirdPersonCameraGO.transform.position = Camera.transform.position;
            ThirdPersonCameraGO.transform.rotation = Camera.transform.rotation;
        }

        void Update()
        {
            if (ThirdPersonCameraGO == null && GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera") != null)
            {
                ThirdPersonCameraGO = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera");
                __CameraInit__();
            }
            if (UnityInput.Current.GetKeyDown(KeyCode.F3))
            {
                showGUI = !showGUI;
            }
            if (headcam)
            {
                Camera.transform.position = GorillaLocomotion.Player.Instance.headCollider.transform.position;
                Camera.transform.rotation = GorillaLocomotion.Player.Instance.headCollider.transform.rotation;
                Camera.GetComponent<Renderer>().enabled = false;
            }
            if ((!headcam && !bodycam) && !Camera.GetComponent<Renderer>().enabled)
            {
                Camera.GetComponent<Renderer>().enabled = true;
            }
            if (bodycam)
            {
                Camera.transform.position = GorillaLocomotion.Player.Instance.bodyCollider.transform.position;
                Camera.transform.rotation = GorillaLocomotion.Player.Instance.bodyCollider.transform.rotation;
                Camera.GetComponent<Renderer>().enabled = false;
            }
            if (rotatecam)
            {
                Camera.transform.position = GorillaLocomotion.Player.Instance.headCollider.transform.position + new Vector3(0f, 0.7f, 0f);
                Camera.transform.Rotate(0f, 0.4f, 0f);
            }
            if (canmove)
            {
                if (ControllerInputPoller.instance.rightGrab && ControllerInputPoller.instance.leftGrab)
                {
                    if (Camera.transform.parent == null)
                    {
                        Camera.transform.parent = GorillaLocomotion.Player.Instance.rightControllerTransform;
                    }
                    else if (Camera.transform.parent == GorillaLocomotion.Player.Instance.rightControllerTransform)
                    {
                        Camera.transform.parent = null;
                    }
                }
            }
            if (freecam)
            {
                FreeCam();
            }
            SetFOV(FOVSLIDER);
        }
        void OnGUI()
        {
            if (showGUI)
            {
                GUI.backgroundColor = new Color(125f, 75f, 125f);
                GUI.contentColor = Color.white;
                GUI.Box(new Rect(475f, 280f, 800f, 600f), "GroundZ3R0's Camera Mod Panel");
                bool FLIPCAMBUTTON = GUI.Button(new Rect(475f, 300f, 800f, 40f), "Flip Camera");
                bool ROTATECAMBUTTON = GUI.Button(new Rect(475f, 340f, 800f, 40f), "Rotate Cam");
                bool HEADCAMBUTTON = GUI.Button(new Rect(475f, 380f, 800f, 40f), "Head Cam");
                bool BODYCAMBUTTON = GUI.Button(new Rect(475f, 420f, 800f, 40f), "Body Cam");
                bool RESETBUTTON = GUI.Button(new Rect(475f, 460f, 800f, 40f), "Reset Camera");
                bool MOVEMENTBUTTON = GUI.Button(new Rect(475f, 500f, 800f, 40f), "Toggle Camera Movement");
                bool FREECAMBUTTON = GUI.Button(new Rect(475f, 540f, 800f, 40f), "FreeCam");
                bool SIXTYFOVBUTTON = GUI.Button(new Rect(475f, 580f, 800f, 40f), "60 Fov");
                bool NINETYFOVBUTTON = GUI.Button(new Rect(475f, 620f, 800f, 40f), "90 Fov");
                bool ONETWENTYFOVBUTTON = GUI.Button(new Rect(475f, 660f, 800f, 40f), "120 Fov");
                bool ONEFIFTYFOVBUTTON = GUI.Button(new Rect(475f, 700f, 800f, 40f), "150 Fov");
                GUI.Label(new Rect(475f, 740f, 800f, 40f), "FOV Slider:");
                FOVSLIDER = GUI.HorizontalSlider(new Rect(475f, 760f, 800f, 40f), FOVSLIDER, 60f, 160f);

                if (HEADCAMBUTTON)
                {
                    HeadCam();
                }
                if (BODYCAMBUTTON)
                {
                    BodyCam();
                }
                if (RESETBUTTON)
                {
                    ResetCamera();
                }
                if (MOVEMENTBUTTON)
                {
                    canmove = !canmove;
                }
                if (ROTATECAMBUTTON)
                {
                    RotateCam();
                }
                if (FREECAMBUTTON)
                {
                    freecam = !freecam;
                }
                if (SIXTYFOVBUTTON)
                {
                    SetFOV(60f);
                    FOVSLIDER = 60f;
                }
                if (NINETYFOVBUTTON)
                {
                    SetFOV(90f);
                    FOVSLIDER = 90f;
                }
                if (ONETWENTYFOVBUTTON)
                {
                    SetFOV(120f);
                    FOVSLIDER = 120f;
                }
                if (ONEFIFTYFOVBUTTON)
                {
                    SetFOV(150f);
                    FOVSLIDER = 150f;
                }
                if (FLIPCAMBUTTON)
                {
                    FlipCamera();
                }
            }
        }
        void FlipCamera()
        {
            Camera.transform.Rotate(0f, 180f, 0f);
        }
        void SetFOV(float fov)
        {
            ThirdPersonCameraGO.GetComponent<Camera>().fieldOfView = fov;
        }
        void FreeCam()
        {
            rotatecam = false;
            headcam = false;
            bodycam = false;
            if (UnityInput.Current.GetKey(KeyCode.W))
            {
                Camera.transform.position += Camera.transform.forward * Time.deltaTime * 6.5f;
            }
            if (UnityInput.Current.GetKey(KeyCode.S))
            {
                Camera.transform.position -= Camera.transform.forward * Time.deltaTime * 6.5f;
            }
            if (UnityInput.Current.GetKey(KeyCode.A))
            {
                Camera.transform.position -= Camera.transform.right * Time.deltaTime * 6.5f;
            }
            if (UnityInput.Current.GetKey(KeyCode.D))
            {
                Camera.transform.position += Camera.transform.right * Time.deltaTime * 6.5f;
            }
            if (UnityInput.Current.GetMouseButton(1))
            {
                Vector3 val = UnityInput.Current.mousePosition - previousMousePosition;
                float num2 = Camera.transform.localEulerAngles.y + val.x * 0.3f;
                float num3 = Camera.transform.localEulerAngles.x - val.y * 0.3f;
                Camera.transform.localEulerAngles = new Vector3(num3, num2, 0f);
            }
            previousMousePosition = UnityInput.Current.mousePosition;
        }
        void RotateCam()
        {
            Camera.transform.localEulerAngles = Vector3.zero;
            rotatecam = true;
            headcam = false;
            bodycam = false;
            freecam = false;
        }
        void HeadCam()
        {
            headcam = true;
            bodycam = false;
            rotatecam = false;
            freecam = false;
        }
        void BodyCam()
        {
            bodycam = true;
            headcam = false;
            rotatecam = false;
            freecam = false;
        }
        void ResetCamera()
        {
            cameraPos = new Vector3(-65f, 12f, -82f);
            Camera.transform.position = cameraPos;
            Camera.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            bodycam = false;
            headcam = false;
            rotatecam = false;
            freecam = false;
        }
    }
}
