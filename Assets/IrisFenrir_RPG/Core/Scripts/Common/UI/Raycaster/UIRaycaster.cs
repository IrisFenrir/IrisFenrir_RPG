using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IrisFenrir
{
    public class UIRaycaster
    {
        private static PointerEventData m_pointerData;

        public static bool Raycast(GraphicRaycaster raycaster, out List<RaycastResult> results, Vector2 screenPos)
        {
            results = null;
            if (raycaster == null) return false;
            m_pointerData ??= new PointerEventData(EventSystem.current);
            m_pointerData.position = screenPos;
            results = new List<RaycastResult>();
            raycaster.Raycast(m_pointerData, results);
            return results.Count > 0; 
        }

        public static bool RaycastFromMouse(GraphicRaycaster raycaster, out List<RaycastResult> results)
        {
            return Raycast(raycaster, out results, Input.mousePosition);
        }

        public static bool RaycastWithClick(GraphicRaycaster raycaster, out List<RaycastResult> results, int mouseButton = 0)
        {
            results = null;
            return Input.GetMouseButtonDown(mouseButton) && RaycastFromMouse(raycaster, out results);
        }
    }
}
