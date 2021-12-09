using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts._3DPlacing
{
    public class EditModelCanvas : BtnBase, IPointerDownHandler, IDragHandler
    {
        [SerializeField]
        private Camera ARCamera;

        public void OnDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log(eventData.position);

            RaycastHit hit;
            Physics.Raycast(ARCamera.ScreenPointToRay(eventData.position), out hit);
            Debug.Log(hit.collider.gameObject);
        }
    }
}
