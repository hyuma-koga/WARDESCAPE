using System;
using UnityEngine;
using UnityEngine.UI; // UI.Text—p‚Ì–¼‘O‹óŠÔ‚ð’Ç‰Á

namespace UnityStandardAssets.Utility
{
    public class SimpleActivatorMenu : MonoBehaviour
    {
        // UI.Text ‚É•ÏX
        public Text camSwitchButton;
        public GameObject[] objects;

        private int m_CurrentActiveObject;

        private void OnEnable()
        {
            m_CurrentActiveObject = 0;
            if (camSwitchButton != null)
                camSwitchButton.text = objects[m_CurrentActiveObject].name;
        }

        public void NextCamera()
        {
            int nextactiveobject = m_CurrentActiveObject + 1 >= objects.Length ? 0 : m_CurrentActiveObject + 1;

            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(i == nextactiveobject);
            }

            m_CurrentActiveObject = nextactiveobject;
            if (camSwitchButton != null)
                camSwitchButton.text = objects[m_CurrentActiveObject].name;
        }
    }
}
