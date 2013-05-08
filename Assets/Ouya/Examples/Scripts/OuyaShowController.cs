using System;
using System.Collections.Generic;
using UnityEngine;

public class OuyaShowController : MonoBehaviour,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IMenuAppearingListener
{
    /// <summary>
    /// This is your assigned developer id
    /// </summary>
    //private const string DEVELOPER_ID = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0";
    private const OuyaSDK.OuyaPlayer player = OuyaSDK.OuyaPlayer.player1;

    private OuyaSDK.InputAction m_inputAction = OuyaSDK.InputAction.None;
    private OuyaSDK.KeyEnum m_keyEnum = OuyaSDK.KeyEnum.NONE;

    #region Transition fields

    /// <summary>
    /// Meta reference for camera positions
    /// </summary>
    public Transform[] CameraPositions = null;

    /// <summary>
    /// The camera to transition to
    /// </summary>
    private int m_transitionId = 0;

    /// <summary>
    /// The previous transition id
    /// </summary>
    private int m_oldTransitionId = 0;

    /// <summary>
    /// A timer for transitions
    /// </summary>
    private DateTime m_transitionTimer = DateTime.MinValue;

    #endregion

    #region Model reference fields

    public Material ControllerMaterial;

    public MeshRenderer RendererAxisLeft;
    public MeshRenderer RendererAxisRight;
    public MeshRenderer RendererButtonA;
    public MeshRenderer RendererButtonO;
    public MeshRenderer RendererButtonU;
    public MeshRenderer RendererButtonY;
    public MeshRenderer RendererDpadDown;
    public MeshRenderer RendererDpadLeft;
    public MeshRenderer RendererDpadRight;
    public MeshRenderer RendererDpadUp;
    public MeshRenderer RendererLB;
    public MeshRenderer RendererLT;
    public MeshRenderer RendererRB;
    public MeshRenderer RendererRT;

    #endregion

    void Awake()
    {
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);

        //Register method listener to handle button events.
        OuyaInputManager.OuyaButtonEvent.addButtonEventListener(HandleButtonEvent);
    }

    void OnDestroy()
    {
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);

        OuyaInputManager.OuyaButtonEvent.removeButtonEventListener(HandleButtonEvent);
        OuyaInputManager.initKeyStates();
    }

    public void OuyaMenuAppearing()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public void OuyaOnPause()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public void OuyaOnResume()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    //Handles Button Events.
    private void HandleButtonEvent(OuyaSDK.OuyaPlayer p, OuyaSDK.KeyEnum b, OuyaSDK.InputAction bs)
    {
        //Debug.Log(string.Format("Player:{0} | Button:{1} | InputAction:{2}", p, b, bs));
        m_inputAction = bs;
        m_keyEnum = b;

        //If this event was not meant for us then do not handle it.
        if (!player.Equals(p)) { return; }

        //NOTE: This Button event handler only handles events for Player 1, because of the above statement.

        #region BUTTONS O - A
        //Check O button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_O) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererButtonO, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_O) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererButtonO, false, true);
        }

        //Check U button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_U) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererButtonU, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_U) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererButtonU, false, true);
        }

        //Check Y button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_Y) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererButtonY, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_Y) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererButtonY, false, true);
        }

        //Check A button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_A) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererButtonA, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_A) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererButtonA, false, true);
        }

        //Check L3 button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_L3) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererAxisLeft, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_L3) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererAxisLeft, false, true);
        }

        //Check R3 button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_R3) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererAxisRight, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_R3) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererAxisRight, false, true);
        }
        #endregion

        #region Bumpers
        //Check LB button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_LB) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererLB, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_LB) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererLB, false, true);
        }

        //Check RB button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_RB) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererRB, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_RB) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererRB, false, true);
        }
        #endregion

        #region triggers
        //Check LT button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_LT) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererLT, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_LT) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererLT, false, true);
        }

        //Check RT button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_RT) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererRT, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_RT) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererRT, false, true);
        }
        #endregion

        #region DPAD
        //Check DPAD UP button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_DPAD_UP) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererDpadUp, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_DPAD_UP) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererDpadUp, false, true);
        }

        //Check DPAD DOWN button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererDpadDown, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererDpadDown, false, true);
        }

        //Check DPAD LEFT button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererDpadLeft, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererDpadLeft, false, true);
        }

        //Check DPAD RIGHT button for down state
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            UpdateHighlight(RendererDpadRight, true, true);
        }
        else if (b.Equals(OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            UpdateHighlight(RendererDpadRight, false, true);
        }
        #endregion

    }

    #region Presentation

    private void OnGUI()
    {
        try
        {
           // UpdateCamera();
            OuyaNGUIHandler nguiHandler = GameObject.Find("_NGUIHandler").GetComponent<OuyaNGUIHandler>();
            if (nguiHandler != null)
            {
                string[] joystickNames = Input.GetJoystickNames();
                //foreach (string joystick in Input.GetJoystickNames())
                for (int i = 0; i < joystickNames.Length && i < 4; i++)
                {
                    if (i == 1) { nguiHandler.controller1.text = joystickNames[i]; }
                    if (i == 2) { nguiHandler.controller1.text = joystickNames[i]; }
                    if (i == 3) { nguiHandler.controller1.text = joystickNames[i]; }
                    if (i == 4) { nguiHandler.controller1.text = joystickNames[i]; }
                }

                nguiHandler.genericEvent.text = m_inputAction + " : " + m_keyEnum;
                nguiHandler.joystickLeft.text = string.Format("{0},{1}", OuyaInputManager.GetAxis("LX", player), OuyaInputManager.GetAxis("LY", player));
                nguiHandler.joystickRight.text = string.Format("{0},{1}", OuyaInputManager.GetAxis("RX", player), OuyaInputManager.GetAxis("RY", player));
                nguiHandler.trigger.text = string.Format("{0},{1}", OuyaInputManager.GetAxis("LT", player), OuyaInputManager.GetAxis("RT", player));
                nguiHandler.rawOut.text = OuyaGameObject.InputData;
            }
        }
        catch (System.Exception)
        {
        }
    }

    void FixedUpdate()
    {
        UpdateController();
    }

    #endregion

    #region Extra
    private const float TRANSITION_RATE = 4.0f;
    private void UpdateCamera()
    {
        //move the camera around
        float t = 0f;
        if (m_transitionTimer < DateTime.Now)
        {
            m_transitionTimer = DateTime.Now + TimeSpan.FromSeconds(TRANSITION_RATE);
            m_oldTransitionId = m_transitionId;
            m_transitionId = UnityEngine.Random.Range(0, CameraPositions.Length);
        }
        else
        {
            t = 1f - (float) ((m_transitionTimer - DateTime.Now).TotalSeconds)/TRANSITION_RATE;
        }

        Camera.main.transform.position = Vector3.Lerp(CameraPositions[m_oldTransitionId].position,
                                                      CameraPositions[m_transitionId].position, t);
        Camera.main.transform.rotation = Quaternion.Lerp(CameraPositions[m_oldTransitionId].rotation,
                                                         CameraPositions[m_transitionId].rotation, t);
    }

    private void UpdateHighlight(MeshRenderer mr, bool highlight, bool instant = false)
    {
        float time = Time.deltaTime * 10;
        if (instant) { time = 1000; }

        if (highlight)
        {
            Color c = new Color(0, 10, 0, 1);
            mr.material.color = Color.Lerp(mr.material.color, c, time);
        }
        else
        {
            mr.material.color = Color.Lerp(mr.material.color, Color.white, time);
        }
    }


    private void UpdateController()
    {
        UpdateHighlight(RendererAxisLeft, Mathf.Abs(OuyaInputManager.GetAxis("LX",player)) > 0.25f ||
            Mathf.Abs(OuyaInputManager.GetAxis("LY", player)) > 0.25f);

        RendererAxisLeft.transform.localRotation = Quaternion.Euler(OuyaInputManager.GetAxis("LY", player) * 15, 0, OuyaInputManager.GetAxis("LX", player) * 15);

        UpdateHighlight(RendererAxisRight, Mathf.Abs(OuyaInputManager.GetAxis("RX", player)) > 0.25f ||
            Mathf.Abs(OuyaInputManager.GetAxis("RY", player)) > 0.25f);

        RendererAxisRight.transform.localRotation = Quaternion.Euler(OuyaInputManager.GetAxis("RY", player) * 15, 0, OuyaInputManager.GetAxis("RX", player) * 15);

        if (OuyaInputManager.GetButtonDown("L3", player))
        {
            RendererAxisLeft.transform.localPosition = Vector3.Lerp(RendererAxisLeft.transform.localPosition,
                                                                     new Vector3(5.503977f, 0.75f, -3.344945f), Time.fixedDeltaTime);
        }
        else
        {
            RendererAxisLeft.transform.localPosition = Vector3.Lerp(RendererAxisLeft.transform.localPosition,
                                                                     new Vector3(5.503977f, 1.127527f, -3.344945f), Time.fixedDeltaTime);
        }

        if (OuyaInputManager.GetButtonDown("R3", player))
        {
            RendererAxisRight.transform.localPosition = Vector3.Lerp(RendererAxisRight.transform.localPosition,
                                                                     new Vector3(-2.707688f, 0.75f, -1.354063f), Time.fixedDeltaTime);
        }
        else
        {
            RendererAxisRight.transform.localPosition = Vector3.Lerp(RendererAxisRight.transform.localPosition,
                                                                     new Vector3(-2.707688f, 1.11295f, -1.354063f), Time.fixedDeltaTime);
        }
    }

    #endregion

}