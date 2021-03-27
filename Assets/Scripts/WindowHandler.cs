using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PepeHelper
{
    public class WindowHandler : MonoBehaviour
    {
        private static IntPtr _activeWindow;

        private struct Margins
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        [DllImport("Dwmapi.dll")]
        private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins margins);

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll")]
        private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);


        private const int GWL_EXSTYLE = -20;

        private const uint WS_EX_LAYERED = 0x00080000;
        private const uint WS_EX_TRANSPARENT = 0x00000020;

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        private const uint LWA_COLORKEY = 0x00000001;

        private void Awake()
        {
#if !UNITY_EDITOR
            _activeWindow = GetActiveWindow();

            Margins margins = new Margins() {cxLeftWidth = -1};
            DwmExtendFrameIntoClientArea(_activeWindow, ref margins);

            SetWindowLong(_activeWindow, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
            //SetLayeredWindowAttributes(_activeWindow, 0, 0, LWA_COLORKEY);
            SetWindowPos(_activeWindow, HWND_TOPMOST, 0, 0, 0, 0, 0);
#endif
        }

        private void Update()
        {
            SetClickthrough(!IsPointerOverUI());
        }

        private void SetClickthrough(bool clickthrough)
        {
            if (clickthrough)
            {
                SetWindowLong(_activeWindow, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
            }
            else
            {
                SetWindowLong(_activeWindow, GWL_EXSTYLE, WS_EX_LAYERED);
            }
        }

        private bool IsPointerOverUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            else
            {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);
                return hits.Count > 0;
            }
        }
    }
}
