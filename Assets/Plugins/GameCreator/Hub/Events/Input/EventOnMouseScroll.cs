using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace GameCreator.Runtime.VisualScripting
{
	[Version(0, 0, 5)]
    [Title("On Mouse Wheel Scroll")]
    [Category("Input/On Mouse Wheel Scroll")]
    [Description("Detects when mouse wheel scroll")]

    [Image(typeof(IconRadioOn),ColorTheme.Type.Green)]
    
    [Keywords("Keyboard", "Mouse", "Wheel", "Gamepad", "Controller", "Joystick")]

    [Serializable]
    public class EventOnMouseScroll : Event
    {

        float mouseWheel;
        Vector2 m_Vec;
        private enum myEnum 
	    {
		    m_ScrollUp, 
		    m_ScrollDown
		 };
        [Space]
        [SerializeField] private myEnum mouseScrollDirection;
        
        protected override void OnStart(Trigger trigger)
        {
	        base. OnStart(trigger);
	        
        }
	    protected override void OnDestroy(Trigger trigger)
	    {
	    	base.OnDestroy(this.m_Trigger);
	    	
	    }

        protected override void OnUpdate(Trigger trigger)
        {

            m_Vec = Mouse.current.scroll. ReadValue();
            mouseWheel = m_Vec.y;

           switch (mouseScrollDirection)
           {
           case myEnum.m_ScrollUp:
                if (mouseWheel > 0f)
                {
                _ = this.m_Trigger.Execute(this.Self);
                }
                break;
           case myEnum.m_ScrollDown:
               if (mouseWheel < 0f)
                {
                _ = this.m_Trigger.Execute(this.Self);
                }
                break;
           }
           
        }

    }
}