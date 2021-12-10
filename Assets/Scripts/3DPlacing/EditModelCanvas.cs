using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts._3DPlacing
{
    public class EditModelCanvas : BtnBase, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private Camera ARCamera;
        [SerializeField] private GameObject EditPanel;
        [SerializeField] private Canvas FloatingCanvas;

        private ModelScript _model; // current model
        private bool _dragging;

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragging)
            {
                ARCursor.Instance.DrawStrategy.PlacingMove(eventData.position);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            RaycastHit hit;
            Physics.Raycast(ARCamera.ScreenPointToRay(eventData.position), out hit);

            if (hit.collider != null)
            {
                _model = hit.collider.gameObject.GetComponent<ModelScript>();
                if (_model.EditPanel == null)
                {
                    Clicked();
                    var editPanel = Instantiate(EditPanel);
                    editPanel.transform.SetParent(FloatingCanvas.transform, false);
                    editPanel.transform.localScale = Vector3.one;
                    editPanel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    editPanel.GetComponent<EditModelPanelUI>().model = _model;

                    var lookAt = editPanel.GetComponent<EditPanelLookAt>();
                    lookAt.lookAt = _model.transform;
                    lookAt.cam = ARCamera;
                    _model.EditPanel = editPanel;
                }
                else _model.EditPanel.gameObject.SetActive(true);

                _dragging = true;
                ARCursor.Instance.DrawStrategy.PlacingStarted(eventData.position, _model);
            }
            else Clicked();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _dragging = false;
            ARCursor.Instance.DrawStrategy.PlacingEnded();
        }
    }
}
