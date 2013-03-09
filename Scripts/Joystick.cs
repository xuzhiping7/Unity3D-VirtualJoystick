/*
 * Coded by ZhipingXu  xuzhiping7@qq.com 
 * 
 * "Virtual Joystick" for Android and Windows
 * Now it just suitable for Overlook 
 * 
 * To Do:1.Suitable for all viewing angles 
 *       2.Suitable for IOS
*/


using UnityEngine;
using System.Collections;

public class Joystick : MonoBehaviour
{

    //Virtual Area and Joystick gameobjec
    public GameObject go_VirtualArea;
    public GameObject go_Joystick;

    //Control Range 
    public float f_ControlRadius = 2.0f;
    public float f_ControlCheckArea = 4.0f;

    private Vector3 vec3_JoystickOriPoint;
    private Vector2 vec2_ControlResult;

    private bool b_Controling = false;

    //Keep the static refenrece
    private static Joystick m_Joystick;

    void Start()
    {
        m_Joystick = this;
        
        if (!go_VirtualArea || !go_Joystick)
        {
            Debug.LogError("No Pic For Joystick , Please add it to the component !");
            return;
        }


        //Get the joystick original center point 

        vec3_JoystickOriPoint = go_Joystick.transform.position;
    }


    //void OnGUI()
    //{
    //    GUI.Label(new Rect(10, 30, 500, 20), "CheckInVirtualArea:" + Input.mousePosition);
    //}

    void Update()
    {

        //Catch the mouse input
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 0.0f));

                if (CheckInVirtualArea(worldPosition))
                {
                    b_Controling = true;
                }


                if (b_Controling)
                {
                    Vector3 vec3 = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x,Input.touches[0].position.y,0.0f));

                    vec2_ControlResult = new Vector2(vec3.x - vec3_JoystickOriPoint.x, vec3.z - vec3_JoystickOriPoint.z) / f_ControlRadius;
                    
                    if (vec2_ControlResult.magnitude > 1) vec2_ControlResult = vec2_ControlResult.normalized;

                    JoystickMoving();
                }
            }
            else
            {
                go_Joystick.transform.position = vec3_JoystickOriPoint;
                b_Controling = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (CheckInVirtualArea(worldPosition))
                {
                    b_Controling = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                go_Joystick.transform.position = vec3_JoystickOriPoint;
                b_Controling = false;
            }

            if (b_Controling && Input.GetMouseButton(0))
            {

                Vector3 vec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                vec2_ControlResult = new Vector2(vec3.x - vec3_JoystickOriPoint.x, vec3.z - vec3_JoystickOriPoint.z) / f_ControlRadius;

                if (vec2_ControlResult.magnitude > 1) vec2_ControlResult = vec2_ControlResult.normalized;

                JoystickMoving();

            }
        }

    }
    
    private bool CheckInVirtualArea(Vector3 worldPosition)
    {
        if (new Vector3(worldPosition.x - vec3_JoystickOriPoint.x, 0.0f, worldPosition.z - vec3_JoystickOriPoint.z).magnitude <= f_ControlCheckArea)
        {
            return true;
        }
        
        return false;
    }

    private void JoystickMoving()
    {
        go_Joystick.transform.position = vec3_JoystickOriPoint + new Vector3(vec2_ControlResult.x * f_ControlRadius, 0.0f, vec2_ControlResult.y * f_ControlRadius);
    }

    public Vector2 GetControlVec()
    {
        if (b_Controling)
        {
            return vec2_ControlResult;
        }
        else
        {
            return Vector2.zero;
        }
    }


    public static Joystick GetThis()
    {
        return m_Joystick;
    }
}
