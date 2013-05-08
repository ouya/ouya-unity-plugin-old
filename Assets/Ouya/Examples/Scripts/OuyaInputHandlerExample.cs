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
using System.Collections.Generic;

public class OuyaInputHandlerExample : MonoBehaviour,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IMenuAppearingListener
{
    [SerializeField]
    public OuyaSDK.OuyaPlayer player;
    public float speed = 6.0F;
    public float gravity = 20.0F;
    public float turnSpeed = 60f;
    private Vector3 moveDirection = Vector3.zero;

    //private const string DEVELOPER_ID = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0";

    private CharacterController controller;
    private bool isBattleStance = false; 

    void Awake()
    {
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);

        OuyaInputManager.OuyaButtonEvent.addButtonEventListener(HandleButtonEvent);
        
        //Get our character controller;
        controller = GetComponent<CharacterController>();

#if UNITY_EDITOR
        
        //If you find that your controller does not work properly in the Unity Editor, You can add a custom controller mapping as seen below.
        /* Here is some code to find keycodes for devices that are mapped to your InputManager.asset
        List<OuyaGameObject.Device> devices = OuyaInputManager.GetDevices();
        for (int iPlayerJoystick = 1; iPlayerJoystick <= devices.Count; iPlayerJoystick++)
        {
            OuyaGameObject.Device device = devices.Find(delegate(OuyaGameObject.Device d) { return (null == d || null == devices) ? false : (d.id == devices[iPlayerJoystick - 1].id); });
            for (int i = 0; i < 13; i++)
            {
                //Controller Name:
                string fireBtnName = string.Format("Joystick{0}Button{1}", iPlayerJoystick, i);
                KeyCode keycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), fireBtnName);
                Debug.Log(string.Format("Button Name:{0}, Keycode:{1}",keycode,int(keycode));
            }
        }
        */

        //Custom Controller Mapping.
        List<ButtonMap> buttonMap = new List<ButtonMap>();
        ControllerType xbox360 = new ControllerType();
		xbox360.name="xbox360";
		xbox360.leftAnalogH = 1;
		xbox360.leftAnalogV = 2;
		xbox360.rightAnalogH = 4;
		xbox360.rightAnalogV = 5;
		xbox360.triggers = 3;
		xbox360.dpadH = 6;
		xbox360.dpadV = 7;
        JoystickType joystickType = JoystickType.xbox;
		
        //Start XBOX 360 ButtonMap ( only has 13 buttons )
        buttonMap.Add(new ButtonMap(joystickType, 370, OuyaSDK.KeyEnum.BUTTON_O));
        buttonMap.Add(new ButtonMap(joystickType, 371, OuyaSDK.KeyEnum.BUTTON_A));
        buttonMap.Add(new ButtonMap(joystickType, 372, OuyaSDK.KeyEnum.BUTTON_U));
        buttonMap.Add(new ButtonMap(joystickType, 373, OuyaSDK.KeyEnum.BUTTON_Y));

        buttonMap.Add(new ButtonMap(joystickType, 374, OuyaSDK.KeyEnum.BUTTON_LB));
        buttonMap.Add(new ButtonMap(joystickType, 375, OuyaSDK.KeyEnum.BUTTON_RB));
        buttonMap.Add(new ButtonMap(joystickType, 376, OuyaSDK.KeyEnum.BUTTON_SELECT));
        buttonMap.Add(new ButtonMap(joystickType, 377, OuyaSDK.KeyEnum.BUTTON_START));
        buttonMap.Add(new ButtonMap(joystickType, 378, OuyaSDK.KeyEnum.BUTTON_L3));
        buttonMap.Add(new ButtonMap(joystickType, 379, OuyaSDK.KeyEnum.BUTTON_R3));
        buttonMap.Add(new ButtonMap(joystickType, 380, OuyaSDK.KeyEnum.BUTTON_LT)); //Doesn't Show up
        buttonMap.Add(new ButtonMap(joystickType, 381, OuyaSDK.KeyEnum.BUTTON_RT)); //Doesn't Show up
        buttonMap.Add(new ButtonMap(joystickType, 382, OuyaSDK.KeyEnum.BUTTON_SYSTEM)); //Dowsn't Show up

        /*
        //4 button dpad mappings
        buttonMap.Add(new ButtonMap(joystickType, 13, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT));
        buttonMap.Add(new ButtonMap(joystickType, 14, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT));
        buttonMap.Add(new ButtonMap(joystickType, 15, OuyaSDK.KeyEnum.BUTTON_DPAD_UP));
        buttonMap.Add(new ButtonMap(joystickType, 16, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN));
        */

		//controllers.Add(xbox360.name,xbox360)
        //OuyaControllerMapping.RegisterCustomControllerMapping(xbox360, buttonMap); //duplicate of existing
#endif

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

    void Update()
    {

        //Get Joystick points and calculate deadzone.
        Vector2 point = convertRawJoystickCoordinates(OuyaInputManager.GetAxis("LX", player), OuyaInputManager.GetAxis("LY", player), .25f);

        //Debug.Log(string.Format("Player:{0}, Axis:{1}",player,point));

        //Rotate character to where it is looking.
        transform.Rotate(0f, point.x * turnSpeed * Time.deltaTime, 0f);

        //only move if we are grounded.
        if (controller.isGrounded)
        {
            //get the move direction from the controller axis;
            moveDirection = transform.forward * point.y;

            //Set the speed
            moveDirection *= speed;
        }

        //Apply any gravity factors
        moveDirection.y -= gravity * Time.deltaTime;

        //Move the character;
        controller.Move(moveDirection * Time.deltaTime);

        //Handle animation if we are moving.
        if (point.x != 0f && point.y != 0f)
        {
            //If axis is not 0 then play run animation.
            this.animation.Play("run");
        }

        //if no animation is playing, then play the idle animation.
        if (!this.animation.isPlaying)
        {
            string anim = "idle";
            if (isBattleStance)
            {
                anim = "waitingforbattle";
            }
            this.animation.Play(anim);
        }
    }
    
    void HandleButtonEvent(OuyaSDK.OuyaPlayer p, OuyaSDK.KeyEnum b, OuyaSDK.InputAction bs)
    {
        if (!player.Equals(p)) { return; }

        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_O) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            this.animation.Play("attack");
            Debug.Log("Button O Down Event was triggered on Player" + player);
        }

        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_O) && bs.Equals(OuyaSDK.InputAction.KeyUp))
        {
            Debug.Log("Button O Up Event was triggered on Player" + player);
        }

        //BATTLE STANCE:
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_Y) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            if (!isBattleStance)
            {
                //Set Battle stance.
                this.animation.Play("waitingforbattle");
                isBattleStance = true;
            }
            else
            {
                if (this.animation.IsPlaying("waitingforbattle"))
                {
                    this.animation.Play("idle");
                    isBattleStance = false;
                }
            }
        }

        //DANCE:
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_U) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            this.animation.Play("dance");
        }


        //FAKE Death:
        if (b.Equals(OuyaSDK.KeyEnum.BUTTON_A) && bs.Equals(OuyaSDK.InputAction.KeyDown))
        {
            this.animation.Play("die");
        }
    }

    private Vector2 convertRawJoystickCoordinates(float x, float y, float deadzoneRadius)
    {

        Vector2 result = new Vector2(x, y); // a class with just two members, int x and int y
        bool isInDeadzone = testIfRawCoordinatesAreInDeadzone(x, y, deadzoneRadius);
        if (isInDeadzone)
        {
            result.x = 0f;
            result.y = 0f;
        }
        return result;
    }

    private bool testIfRawCoordinatesAreInDeadzone(float x, float y, float radius)
    {
        float distance = Mathf.Sqrt((x * x) + (y * y));
        return distance < radius;
    }

}
