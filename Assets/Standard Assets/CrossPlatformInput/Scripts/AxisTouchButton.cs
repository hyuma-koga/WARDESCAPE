// 修正済み AxisTouchButton.cs（Unity 2023+ 互換性対応）
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class AxisTouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public string axisName = "Horizontal";
        public float axisValue = 1;
        public float responseSpeed = 3;
        public float returnToCentreSpeed = 3;

        AxisTouchButton m_PairedWith;
        CrossPlatformInputManager.VirtualAxis m_Axis;

        void OnEnable()
        {
            if (!CrossPlatformInputManager.AxisExists(axisName))
            {
                m_Axis = new CrossPlatformInputManager.VirtualAxis(axisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_Axis);
            }
            else
            {
                m_Axis = CrossPlatformInputManager.VirtualAxisReference(axisName);
            }
            FindPairedButton();
        }

        void FindPairedButton()
        {
            // Unity 2023+ 推奨の FindObjectsByType を使用
            var otherAxisButtons = FindObjectsByType<AxisTouchButton>(FindObjectsSortMode.None);

            if (otherAxisButtons != null)
            {
                foreach (var button in otherAxisButtons)
                {
                    if (button.axisName == axisName && button != this)
                    {
                        m_PairedWith = button;
                    }
                }
            }
        }

        void OnDisable()
        {
            m_Axis.Remove();
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (m_PairedWith == null)
            {
                FindPairedButton();
            }
            m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, axisValue, responseSpeed * Time.deltaTime));
        }

        public void OnPointerUp(PointerEventData data)
        {
            m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, 0, responseSpeed * Time.deltaTime));
        }
    }
}
