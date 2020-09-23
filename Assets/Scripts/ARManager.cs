using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour {

    private enum State {
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

            Ray touchRay = sessionOrigin.camera.ScreenPointToRay(Input.mousePosition);

            NativeArray<XRRaycastHit> collisions = planeManager.Raycast(touchRay, TrackableType.All, Allocator.Temp);

            if (collisions.Length > 0) {
                asset.transform.position = collisions[0].pose.position;
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
        pointCloudManager.pointCloudsChanged += OnPointCloudChanged;
        planeManager.planesChanged += OnPlaneChanged;
    }

    private void ClearEvents() {
        pointCloudManager.pointCloudsChanged -= OnPointCloudChanged;
        planeManager.planesChanged -= OnPlaneChanged;
    }

    private void OnPointCloudChanged(ARPointCloudChangedEventArgs pEventArgs) {
        //Debug.Log("Point Cloud Updated");
    }

    private void OnPlaneChanged(ARPlanesChangedEventArgs pEventArgs) {
        Debug.Log("Plane Changed");
    }
}
