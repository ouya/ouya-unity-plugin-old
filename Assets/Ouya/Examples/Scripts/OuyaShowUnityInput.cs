/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;

public class OuyaShowUnityInput : MonoBehaviour,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IMenuAppearingListener
{
    #region Model reference fields

    public Material ControllerMaterial;

    public MeshRenderer RendererAxisLeft = null;
    public MeshRenderer RendererAxisRight = null;
    public MeshRenderer RendererButtonA = null;
    public MeshRenderer RendererButtonO = null;
    public MeshRenderer RendererButtonU = null;
    public MeshRenderer RendererButtonY = null;
    public MeshRenderer RendererDpadDown = null;
    public MeshRenderer RendererDpadLeft = null;
    public MeshRenderer RendererDpadRight = null;
    public MeshRenderer RendererDpadUp = null;
    public MeshRenderer RendererLB = null;
    public MeshRenderer RendererLT = null;
    public MeshRenderer RendererRB = null;
    public MeshRenderer RendererRT = null;

    #endregion

    #region Thumbstick plots

    public Camera ThumbstickPlotCamera = null;

    #endregion

    void Awake()
    {
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);
    }

    void OnDestroy()
    {
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);
    }

    private void Start()
    {
        UpdatePlayerButtons();
        Input.ResetInputAxes();
    }

    public void SetPlayer1()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player1;
        UpdatePlayerButtons();
    }
    public void SetPlayer2()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player2;
        UpdatePlayerButtons();
    }
    public void SetPlayer3()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player3;
        UpdatePlayerButtons();
    }
    public void SetPlayer4()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player4;
        UpdatePlayerButtons();
    }
    public void SetPlayer5()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player5;
        UpdatePlayerButtons();
    }
    public void SetPlayer6()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player6;
        UpdatePlayerButtons();
    }
    public void SetPlayer7()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player7;
        UpdatePlayerButtons();
    }
    public void SetPlayer8()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player8;
        UpdatePlayerButtons();
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

    #region Presentation

    private void UpdateLabels()
    {
        try
        {
            OuyaNGUIHandler nguiHandler = GameObject.Find("_NGUIHandler").GetComponent<OuyaNGUIHandler>();
            if (nguiHandler != null &&
                null != OuyaExampleCommon.Joysticks)
            {
                for (int i = 0; i < OuyaExampleCommon.Joysticks.Length || i < 8; i++)
                {
                    string joyName = "N/A";
                    if (i < OuyaExampleCommon.Joysticks.Length)
                    {
                        joyName = OuyaExampleCommon.Joysticks[i];
                    }
                    switch (i)
                    {
                        case 0:
                            nguiHandler.controller1.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player1 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 1:
                            nguiHandler.controller2.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player2 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 2:
                            nguiHandler.controller3.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player3 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 3:
                            nguiHandler.controller4.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player4 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 4:
                            nguiHandler.controller5.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player5 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 5:
                            nguiHandler.controller6.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player6 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 6:
                            nguiHandler.controller7.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player7 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 7:
                            nguiHandler.controller8.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player8 ? string.Format("*{0}", joyName) : joyName;
                            break;

                    }
                }

                nguiHandler.button1.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 0).ToString();
                nguiHandler.button2.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 1).ToString();
                nguiHandler.button3.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 2).ToString();
                nguiHandler.button4.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 3).ToString();
                nguiHandler.button5.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 4).ToString();
                nguiHandler.button6.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 5).ToString();
                nguiHandler.button7.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 6).ToString();
                nguiHandler.button8.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 7).ToString();
                nguiHandler.button9.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 8).ToString();
                nguiHandler.button10.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 9).ToString();
                nguiHandler.button11.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 10).ToString();
                nguiHandler.button12.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 11).ToString();
                nguiHandler.button13.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 12).ToString();
                nguiHandler.button14.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 13).ToString();
                nguiHandler.button15.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 14).ToString();
                nguiHandler.button16.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 15).ToString();
                nguiHandler.button17.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 16).ToString();
                nguiHandler.button18.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 17).ToString();
                nguiHandler.button19.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 18).ToString();
                nguiHandler.button20.text = OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, 19).ToString();

                //nguiHandler.rawOut.text = OuyaGameObject.InputData;
                nguiHandler.device.text = SystemInfo.deviceModel;
            }
        }
        catch (System.Exception)
        {
        }
    }

    void Update()
    {
        OuyaNGUIHandler nguiHandler = GameObject.Find("_NGUIHandler").GetComponent<OuyaNGUIHandler>();
        if (nguiHandler != null)
        {
            // Input.GetAxis("Joy1 Axis1");
            nguiHandler.axis1.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 1", (int) OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis2.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 2", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis3.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 3", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis4.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 4", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis5.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 5", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis6.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 6", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis7.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 7", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis8.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 8", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis9.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 9", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis10.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 10", (int)OuyaExampleCommon.Player)).ToString("F5");
        }
    }

    void FixedUpdate()
    {
        UpdateController();
        UpdateLabels();

        OuyaExampleCommon.UpdateJoysticks();
    }

    void OnGUI()
    {
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.Space(600);
        for (int index = 1; index <= 4; ++index)
        {
            if (GUILayout.Button(string.Format("JOY{0}", index), GUILayout.MaxHeight(40)))
            {
                OuyaExampleCommon.Player = (OuyaSDK.OuyaPlayer)index;
                UpdatePlayerButtons();
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(600);
        for (int index = 5; index <= 8; ++index)
        {
            if (GUILayout.Button(string.Format("JOY{0}", index), GUILayout.MaxHeight(40)))
            {
                OuyaExampleCommon.Player = (OuyaSDK.OuyaPlayer)index;
                UpdatePlayerButtons();
            }
        }
        GUILayout.EndHorizontal();
    }

    void UpdatePlayerButtons()
    {
        if (ThumbstickPlotCamera)
        {
            foreach (OuyaPlotMeshThumbstick plot in ThumbstickPlotCamera.GetComponents<OuyaPlotMeshThumbstick>())
            {
                plot.Player = OuyaExampleCommon.Player;
            }
        }

        OuyaNGUIHandler nguiHandler = GameObject.Find("_NGUIHandler").GetComponent<OuyaNGUIHandler>();
        if (nguiHandler != null)
        {
            nguiHandler.player1.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player1);
            nguiHandler.player2.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player2);
            nguiHandler.player3.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player3);
            nguiHandler.player4.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player4);
            nguiHandler.player5.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player5);
            nguiHandler.player6.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player6);
            nguiHandler.player7.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player7);
            nguiHandler.player8.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player8);

        }
    }

    #endregion

    #region Extra

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
        #region Axis Code

        UpdateHighlight(RendererAxisLeft, Mathf.Abs(OuyaExampleCommon.GetAxis("LX", OuyaExampleCommon.Player)) > 0.25f ||
            Mathf.Abs(OuyaExampleCommon.GetAxis("LY", OuyaExampleCommon.Player)) > 0.25f);

        RendererAxisLeft.transform.localRotation = Quaternion.Euler(OuyaExampleCommon.GetAxis("LY", OuyaExampleCommon.Player) * 15, 0, OuyaExampleCommon.GetAxis("LX", OuyaExampleCommon.Player) * 15);

        UpdateHighlight(RendererAxisRight, Mathf.Abs(OuyaExampleCommon.GetAxis("RX", OuyaExampleCommon.Player)) > 0.25f ||
            Mathf.Abs(OuyaExampleCommon.GetAxis("RY", OuyaExampleCommon.Player)) > 0.25f);

        RendererAxisRight.transform.localRotation = Quaternion.Euler(OuyaExampleCommon.GetAxis("RY", OuyaExampleCommon.Player) * 15, 0, OuyaExampleCommon.GetAxis("RX", OuyaExampleCommon.Player) * 15);

        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_L3))
        {
            RendererAxisLeft.transform.localPosition = Vector3.Lerp(RendererAxisLeft.transform.localPosition,
                                                                     new Vector3(5.503977f, 0.75f, -3.344945f), Time.fixedDeltaTime);
        }
        else
        {
            RendererAxisLeft.transform.localPosition = Vector3.Lerp(RendererAxisLeft.transform.localPosition,
                                                                     new Vector3(5.503977f, 1.127527f, -3.344945f), Time.fixedDeltaTime);
        }

        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_R3))
        {
            RendererAxisRight.transform.localPosition = Vector3.Lerp(RendererAxisRight.transform.localPosition,
                                                                     new Vector3(-2.707688f, 0.75f, -1.354063f), Time.fixedDeltaTime);
        }
        else
        {
            RendererAxisRight.transform.localPosition = Vector3.Lerp(RendererAxisRight.transform.localPosition,
                                                                     new Vector3(-2.707688f, 1.11295f, -1.354063f), Time.fixedDeltaTime);
        }

        #endregion


        #region Button Code

        #region BUTTONS O - A
        //Check O button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_O))
        {
            UpdateHighlight(RendererButtonO, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonO, false, true);
        }

        //Check U button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_U))
        {
            UpdateHighlight(RendererButtonU, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonU, false, true);
        }

        //Check Y button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_Y))
        {
            UpdateHighlight(RendererButtonY, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonY, false, true);
        }

        //Check A button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_A))
        {
            UpdateHighlight(RendererButtonA, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonA, false, true);
        }

        //Check L3 button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_L3))
        {
            UpdateHighlight(RendererAxisLeft, true, true);
        }
        else
        {
            UpdateHighlight(RendererAxisLeft, false, true);
        }

        //Check R3 button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_R3))
        {
            UpdateHighlight(RendererAxisRight, true, true);
        }
        else
        {
            UpdateHighlight(RendererAxisRight, false, true);
        }
        #endregion

        #region Bumpers
        //Check LB button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_LB))
        {
            UpdateHighlight(RendererLB, true, true);
        }
        else
        {
            UpdateHighlight(RendererLB, false, true);
        }

        //Check RB button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_RB))
        {
            UpdateHighlight(RendererRB, true, true);
        }
        else
        {
            UpdateHighlight(RendererRB, false, true);
        }
        #endregion

        #region triggers
        //Check LT button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_LT))
        {
            UpdateHighlight(RendererLT, true, true);
        }
        else
        {
            UpdateHighlight(RendererLT, false, true);
        }

        //Check RT button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_RT))
        {
            UpdateHighlight(RendererRT, true, true);
        }
        else
        {
            UpdateHighlight(RendererRT, false, true);
        }
        #endregion

        #region DPAD
        //Check DPAD UP button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_DPAD_UP))
        {
            UpdateHighlight(RendererDpadUp, true, true);
        }
        else
        {
            UpdateHighlight(RendererDpadUp, false, true);
        }

        //Check DPAD DOWN button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN))
        {
            UpdateHighlight(RendererDpadDown, true, true);
        }
        else
        {
            UpdateHighlight(RendererDpadDown, false, true);
        }

        //Check DPAD LEFT button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT))
        {
            UpdateHighlight(RendererDpadLeft, true, true);
        }
        else
        {
            UpdateHighlight(RendererDpadLeft, false, true);
        }

        //Check DPAD RIGHT button for down state
        if (OuyaExampleCommon.GetButton(OuyaExampleCommon.Player, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT))
        {
            UpdateHighlight(RendererDpadRight, true, true);
        }
        else
        {
            UpdateHighlight(RendererDpadRight, false, true);
        }
        #endregion

        #endregion
    }

    #endregion
}