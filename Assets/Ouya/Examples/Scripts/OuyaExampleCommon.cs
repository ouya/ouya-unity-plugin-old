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

using System;
using UnityEngine;

public class OuyaExampleCommon
{
    /// <summary>
    /// The current selected controller
    /// </summary>
    public static OuyaSDK.OuyaPlayer Player = OuyaSDK.OuyaPlayer.player1;

    /// <summary>
    /// Cache joysticks
    /// </summary>
    public static string[] Joysticks = null;

    /// <summary>
    /// Query joysticks every N seconds
    /// </summary>
    private static DateTime m_timerJoysticks = DateTime.MinValue;

    #region Mapping Helpers

    public static float GetAxis(string ouyaMapping, OuyaSDK.OuyaPlayer player)
    {
        if (null == OuyaExampleCommon.Joysticks)
        {
            return 0f;
        }
        int playerIndex = (int)player - 1;
        if (playerIndex >= OuyaExampleCommon.Joysticks.Length)
        {
            return 0f;
        }

        string joystickName = OuyaExampleCommon.Joysticks[playerIndex];
        if (null == joystickName)
        {
            return 0f;
        }

        bool invert = false;
        string axisName = string.Empty;

        switch (joystickName.ToUpper())
        {
            case "BROADCOM BLUETOOTH HID":
            case "MOGA PRO HID":
#if !UNITY_EDITOR && UNITY_ANDROID
            switch (ouyaMapping)
            {
                case "LX":
                    axisName = string.Format("Joy{0} Axis 1", (int)player);
                    break;
                case "LY":
                    axisName = string.Format("Joy{0} Axis 2", (int)player);
                    invert = true;
                    break;
                case "RX":
                    axisName = string.Format("Joy{0} Axis 3", (int)player);
                    break;
                case "RY":
                    axisName = string.Format("Joy{0} Axis 4", (int)player);
                    break;
                case "LT":
                    axisName = string.Format("Joy{0} Axis 8", (int)player);
                    break;
                case "RT":
                    axisName = string.Format("Joy{0} Axis 7", (int)player);
                    break;
                case "DL":
                    axisName = string.Format("Joy{0} Axis 5", (int)player);
                    break;
                case "DR":
                    axisName = string.Format("Joy{0} Axis 5", (int)player);
                    break;
                case "DU":
                    axisName = string.Format("Joy{0} Axis 6", (int)player);
                    break;
                case "DD":
                    axisName = string.Format("Joy{0} Axis 6", (int)player);
                    break;
                default:
                    return 0f;
            }
#endif
                break;
            case "OUYA GAME CONTROLLER":

#if !UNITY_EDITOR && UNITY_ANDROID
            switch (ouyaMapping)
            {
                case "LX":
                    axisName = string.Format("Joy{0} Axis 1", (int)player);
                    break;
                case "LY":
                    axisName = string.Format("Joy{0} Axis 2", (int)player);
                    invert = true;
                    break;
                case "RX":
                    axisName = string.Format("Joy{0} Axis 3", (int)player);
                    break;
                case "RY":
                    axisName = string.Format("Joy{0} Axis 4", (int)player);
                    invert = true;
                    break;
                case "LT":
                    axisName = string.Format("Joy{0} Axis 5", (int)player);
                    break;
                case "RT":
                    axisName = string.Format("Joy{0} Axis 6", (int)player);
                    break;
                default:
                    return 0f;
            }
#else
                switch (ouyaMapping)
                {
                    case "LX":
                        axisName = string.Format("Joy{0} Axis 1", (int)player);
                        break;
                    case "LY":
                        axisName = string.Format("Joy{0} Axis 2", (int)player);
                        invert = true;
                        break;
                    case "RX":
                        axisName = string.Format("Joy{0} Axis 4", (int)player);
                        invert = true;
                        break;
                    case "RY":
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case "LT":
                        axisName = string.Format("Joy{0} Axis 9", (int)player);
                        break;
                    case "RT":
                        axisName = string.Format("Joy{0} Axis 10", (int)player);
                        break;
                    default:
                        return 0f;
                }
#endif
                break;

            case "XBOX 360 WIRELESS RECEIVER":
            case "CONTROLLER (AFTERGLOW GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (ROCK CANDY GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (XBOX 360 WIRELESS RECEIVER FOR WINDOWS)":
            case "MICROSOFT X-BOX 360 PAD":
            case "CONTROLLER (XBOX 360 FOR WINDOWS)":
            case "CONTROLLER (XBOX360 GAMEPAD)":
            case "XBOX 360 FOR WINDOWS (CONTROLLER)":

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (ouyaMapping)
                {
                    case "LX":
                        axisName = string.Format("Joy{0} Axis 1", (int)player);
                        break;
                    case "LY":
                        axisName = string.Format("Joy{0} Axis 2", (int)player);
                        invert = true;
                        break;
                    case "RX":
                        axisName = string.Format("Joy{0} Axis 3", (int)player);
                        break;
                    case "RY":
                        axisName = string.Format("Joy{0} Axis 4", (int)player);
                        invert = true;
                        break;
                    case "LT":
                        axisName = string.Format("Joy{0} Axis 7", (int)player);
                        break;
                    case "RT":
                        axisName = string.Format("Joy{0} Axis 8", (int)player);
                        break;
                    case "DL":
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case "DR":
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case "DU":
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        break;
                    case "DD":
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        break;
                    default:
                        return 0f;
                }

#else

                switch (ouyaMapping)
                {
                    case "LX":
                        axisName = string.Format("Joy{0} Axis 1", (int)player);
                        break;
                    case "LY":
                        axisName = string.Format("Joy{0} Axis 2", (int)player);
                        invert = true;
                        break;
                    case "RX":
                        axisName = string.Format("Joy{0} Axis 4", (int)player);
                        invert = true;
                        break;
                    case "RY":
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case "LT":
                        axisName = string.Format("Joy{0} Axis 9", (int)player);
                        break;
                    case "RT":
                        axisName = string.Format("Joy{0} Axis 10", (int)player);
                        break;
                    case "DL":
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        invert = true;
                        break;
                    case "DR":
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        invert = true;
                        break;
                    case "DU":
                        axisName = string.Format("Joy{0} Axis 7", (int)player);
                        break;
                    case "DD":
                        axisName = string.Format("Joy{0} Axis 7", (int)player);
                        break;
                    default:
                        return 0f;
                }
#endif

                break;

            case "": //the driver is reporting the controller as blank

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (ouyaMapping)
                {
                    case "LX":
                        axisName = string.Format("Joy{0} Axis 1", (int)player);
                        break;
                    case "LY":
                        axisName = string.Format("Joy{0} Axis 2", (int)player);
                        invert = true;
                        break;
                    case "RX":
                        axisName = string.Format("Joy{0} Axis 3", (int)player);
                        break;
                    case "RY":
                        axisName = string.Format("Joy{0} Axis 4", (int)player);
                        invert = true;
                        break;
                    case "LT":
                        axisName = string.Format("Joy{0} Axis 7", (int)player);
                        break;
                    case "RT":
                        axisName = string.Format("Joy{0} Axis 8", (int)player);
                        break;
                    case "DL":
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case "DR":
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case "DU":
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        break;
                    case "DD":
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        break;
                    default:
                        return 0f;
                }

#else

                switch (ouyaMapping)
                {
                    case "LX":
                        axisName = string.Format("Joy{0} Axis 1", (int)player);
                        break;
                    case "LY":
                        axisName = string.Format("Joy{0} Axis 2", (int)player);
                        invert = true;
                        break;
                    case "RX":
                        axisName = string.Format("Joy{0} Axis 3", (int)player);
                        invert = true;
                        break;
                    case "RY":
                        axisName = string.Format("Joy{0} Axis 4", (int)player);
                        break;
                    case "LT":
                        axisName = string.Format("Joy{0} Axis 9", (int)player);
                        break;
                    case "RT":
                        axisName = string.Format("Joy{0} Axis 10", (int)player);
                        break;
                    case "DL":
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        invert = true;
                        break;
                    case "DR":
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        invert = true;
                        break;
                    case "DU":
                        axisName = string.Format("Joy{0} Axis 7", (int)player);
                        break;
                    case "DD":
                        axisName = string.Format("Joy{0} Axis 7", (int)player);
                        break;
                    default:
                        return 0f;
                }
#endif

                break;
        }
        if (string.IsNullOrEmpty(axisName))
        {
            return 0f;
        }
        if (invert)
        {
            return -Input.GetAxisRaw(axisName);
        }
        else
        {
            return Input.GetAxisRaw(axisName);
        }
    }

    public static string GetKeyCode(OuyaSDK.OuyaPlayer player, int buttonNum)
    {
        switch (player)
        {
            case OuyaSDK.OuyaPlayer.none:
                return string.Format("JoystickButton{0}", buttonNum);
            default:
                return string.Format("Joystick{0}Button{1}", ((int)player), buttonNum);
        }
    }

    public static bool GetButton(OuyaSDK.OuyaPlayer player, int buttonNum)
    {
        string keyCode = GetKeyCode(player, buttonNum);
        if (string.IsNullOrEmpty(keyCode))
        {
            return false;
        }
        OuyaKeyCode key = (OuyaKeyCode)Enum.Parse(typeof(OuyaKeyCode), keyCode);
        return Input.GetKey((KeyCode)(int)key);
    }

    public static bool GetButton(OuyaSDK.OuyaPlayer player, OuyaSDK.KeyEnum keyCode)
    {
        if (null == OuyaExampleCommon.Joysticks)
        {
            return false;
        }
        int playerIndex = (int) player - 1;
        if (playerIndex >= OuyaExampleCommon.Joysticks.Length)
        {
            return false;
        }

        string joystickName = OuyaExampleCommon.Joysticks[playerIndex];
        if (null == joystickName)
        {
            return false;
        }

        switch (joystickName.ToUpper())
        {
            case "BROADCOM BLUETOOTH HID":
            case "MOGA PRO HID":
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(player, 6);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(player, 7);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(player, 0);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(player, 3);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(player, 4);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(player, 1);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(player, 13);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(player, 14);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetAxis("DU", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetAxis("DD", player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetAxis("DL", player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetAxis("DR", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetAxis("LT", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetAxis("RT", player) > 0f;
                    default:
                        return false;
                }

            case "OUYA GAME CONTROLLER":

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(player, 4);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(player, 5);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(player, 0);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(player, 1);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(player, 2);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(player, 3);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(player, 6);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(player, 7);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetButton(player, 8);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetButton(player, 9);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetButton(player, 10);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetButton(player, 11);
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetButton(player, 12);
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetButton(player, 13);
                    default:
                        return false;
                }

            case "XBOX 360 WIRELESS RECEIVER":
            case "CONTROLLER (AFTERGLOW GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (ROCK CANDY GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (XBOX 360 WIRELESS RECEIVER FOR WINDOWS)":
            case "MICROSOFT X-BOX 360 PAD":
            case "CONTROLLER (XBOX 360 FOR WINDOWS)":
            case "CONTROLLER (XBOX360 GAMEPAD)":
            case "XBOX 360 FOR WINDOWS (CONTROLLER)":

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(player, 6);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(player, 7);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(player, 0);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(player, 3);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(player, 4);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(player, 1);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(player, 13);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(player, 14);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetAxis("DU", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetAxis("DD", player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetAxis("DL", player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetAxis("DR", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetAxis("LT", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetAxis("RT", player) > 0f;
                    default:
                        return false;
                }
#else
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(player, 4);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(player, 5);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(player, 0);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(player, 2);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(player, 3);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(player, 1);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(player, 8);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(player, 9);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetAxis("DU", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetAxis("DD", player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetAxis("DL", player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetAxis("DR", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetAxis("LT", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetAxis("RT", player) > 0f;
                    default:
                        return false;
                }
#endif

            case "": //the driver is reporting the controller as blank

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(player, 6);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(player, 7);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(player, 0);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(player, 3);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(player, 4);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(player, 1);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(player, 13);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(player, 14);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetAxis("DU", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetAxis("DD", player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetAxis("DL", player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetAxis("DR", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetAxis("LT", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetAxis("RT", player) > 0f;
                    default:
                        return false;
                }
#else
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(player, 4);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(player, 5);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(player, 0);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(player, 2);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(player, 3);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(player, 1);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(player, 8);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(player, 9);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetAxis("DU", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetAxis("DD", player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetAxis("DL", player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetAxis("DR", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetAxis("LT", player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetAxis("RT", player) > 0f;
                    default:
                        return false;
                }
#endif
        }

        return false;
    }

    #endregion

    public static void UpdateJoysticks()
    {
        if (m_timerJoysticks < DateTime.Now)
        {
            //check for new joysticks every N seconds
            m_timerJoysticks = DateTime.Now + TimeSpan.FromSeconds(3);

            Joysticks = Input.GetJoystickNames();
        }
    }
}