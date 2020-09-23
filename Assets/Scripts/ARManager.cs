using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

public class ARManager : MonoBehaviour {

    private enum State : int {
        Idle = 0,
        FindingGround = 1,
        PickMesh = 2     
    }

    // Editor Fields
    public ARSessionOrigin sessionOrigin;
    public ARPointCloudManager pointCloudManager;
    public ARPlaneManager planeManager;
    public GameObject asset;
    public ModelDialog dialog;

    public List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    private State state = State.Idle;

    private void Start() {
        state = State.FindingGround;
    }

    private void OnEnable() {
        SetEvents();
    }

    private void OnDisable() {
        ClearEvents();
    }

    public void Update() {
        if (state == State.FindingGround) {
            RaycastDetectAndPlace();
        }
        else if(state == State.PickMesh){
            RayCastPickMesh();
        }
    }

    private void RaycastDetectAndPlace() {
        if (Input.GetMouseButton(0)) {
            bool collision = sessionOrigin.Raycast(Input.mousePosition, raycastHits, TrackableType.All);
            if (collision) {
                asset.transform.position = raycastHits[0].pose.position;
                state = State.PickMesh;
            }
        }
    }

    private void RayCastPickMesh() {
        if (Input.GetMouseButton(0) && !dialog.isVisible) {

            RaycastHit raycastHit;
            LayerMask layerMask = LayerMask.GetMask(new string[] { "3D Model" });
            Ray ray = sessionOrigin.camera.ScreenPointToRay(Input.mousePosition);
            bool collision = Physics.Raycast(ray, out raycastHit, 10, layerMask);
            if (collision) {
                dialog.Set(raycastHit.collider.name);
            }
        }
    }

    private void SetEvents() {
        pointCloudManager.pointCloudUpdated += OnPointCloudUpdated;
        planeManager.planeAdded += OnPlaneAdded;
        planeManager.planeUpdated += OnPlaneUpdated;
        planeManager.planeRemoved += OnPlaneRemoved;
    }

    private void ClearEvents() {
        pointCloudManager.pointCloudUpdated -= OnPointCloudUpdated;
        planeManager.planeAdded -= OnPlaneAdded;
        planeManager.planeUpdated -= OnPlaneUpdated;
        planeManager.planeRemoved -= OnPlaneRemoved;
    }

    private void OnPointCloudUpdated(ARPointCloudUpdatedEventArgs pEventArgs) {
        //Debug.Log("Point Cloud Updated");
    }

    private void OnPlaneAdded(ARPlaneAddedEventArgs pEventArgs) {
        Debug.Log("Plane Added");
    }

    private void OnPlaneUpdated(ARPlaneUpdatedEventArgs pEventArgs) {
        Debug.Log("Plane Updated");
    }

    private void OnPlaneRemoved(ARPlaneRemovedEventArgs pEvenArgs) {
        Debug.Log("Plane Removed");
    }
}
