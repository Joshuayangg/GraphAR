/*============================================================================== 
Copyright (c) 2018 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.   
==============================================================================*/

using UnityEngine;
using Vuforia;

public class ProductPlacement : MonoBehaviour
{

    #region PUBLIC_MEMBERS
    public bool IsPlaced { get; private set; }

    [Header("Placement Controls")]
    public GameObject m_TranslationIndicator;
    public GameObject m_RotationIndicator;
    public Transform Floor;

    [Header("Placement Augmentation Size Range")]
    [Range(0.1f, 2.0f)]
    public float ProductSize = 0.65f;
    #endregion // PUBLIC_MEMBERS


    #region PRIVATE_MEMBERS
    Material[] chairMaterials, chairMaterialsTransparent;
    Material ChairShadow, ChairShadowTransparent;
    MeshRenderer chairRenderer;
    [SerializeField]
    MeshRenderer shadowRenderer;

    const string EmulatorGroundPlane = "Emulator Ground Plane";

    GroundPlaneUI m_GroundPlaneUI;
    Camera mainCamera;
    Ray cameraToPlaneRay;
    RaycastHit cameraToPlaneHit;

    float m_PlacementAugmentationScale;
    Vector3 ProductScaleVector;
    #endregion // PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        chairRenderer = GetComponent<MeshRenderer>();

        chairMaterials = new Material[]
        {
            Resources.Load<Material>("ChairBody"),
            Resources.Load<Material>("ChairFrame")
        };

        chairMaterialsTransparent = new Material[]
        {
            Resources.Load<Material>("ChairBodyTransparent"),
            Resources.Load<Material>("ChairFrameTransparent")
        };

        ChairShadow = Resources.Load<Material>("ChairShadow");
        ChairShadowTransparent = Resources.Load<Material>("ChairShadowTransparent");

        m_GroundPlaneUI = FindObjectOfType<GroundPlaneUI>();

        // Enable floor collider if running on device; Disable if running in PlayMode
        Floor.gameObject.SetActive(!VuforiaRuntimeUtilities.IsPlayMode());


        mainCamera = Camera.main;

        m_PlacementAugmentationScale = VuforiaRuntimeUtilities.IsPlayMode() ? 0.1f : ProductSize;

        ProductScaleVector =
            new Vector3(m_PlacementAugmentationScale,
                        m_PlacementAugmentationScale,
                        m_PlacementAugmentationScale);

        gameObject.transform.localScale = ProductScaleVector;
    }


    void Update()
    {
        if (PlaneManager.planeMode == PlaneManager.PlaneMode.PLACEMENT)
        {
            shadowRenderer.enabled = chairRenderer.enabled = (IsPlaced || PlaneManager.GroundPlaneHitReceived);
            EnablePreviewModeTransparency(!IsPlaced);
            if (!IsPlaced)
                UtilityHelper.RotateTowardCamera(gameObject);
        }
        else
        {
            shadowRenderer.enabled = chairRenderer.enabled = IsPlaced;
        }

        if (PlaneManager.planeMode == PlaneManager.PlaneMode.PLACEMENT && IsPlaced)
        {
            m_RotationIndicator.SetActive(Input.touchCount == 2);

            m_TranslationIndicator.SetActive(
                (TouchHandler.IsSingleFingerDragging || TouchHandler.IsSingleFingerStationary) && !m_GroundPlaneUI.IsCanvasButtonPressed());

            if (TouchHandler.IsSingleFingerDragging || (VuforiaRuntimeUtilities.IsPlayMode() && Input.GetMouseButton(0)))
            {
                if (!m_GroundPlaneUI.IsCanvasButtonPressed())
                {
                    cameraToPlaneRay = mainCamera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(cameraToPlaneRay, out cameraToPlaneHit))
                    {
                        if (cameraToPlaneHit.collider.gameObject.name ==
                            (VuforiaRuntimeUtilities.IsPlayMode() ? EmulatorGroundPlane : Floor.name))
                        {
                            gameObject.PositionAt(cameraToPlaneHit.point);
                        }
                    }
                }
            }
        }
        else
        {
            m_RotationIndicator.SetActive(false);
            m_TranslationIndicator.SetActive(false);
        }

    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    public void Reset()
    {
        transform.position = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        transform.localScale = ProductScaleVector;
    }

    public void SetProductAnchor(Transform transform)
    {
        if (transform)
        {
            IsPlaced = true;
            gameObject.transform.SetParent(transform);
            gameObject.transform.localPosition = Vector3.zero;
            UtilityHelper.RotateTowardCamera(gameObject);
        }
        else
        {
            IsPlaced = false;
            gameObject.transform.SetParent(null);
        }
    }
    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS
    void EnablePreviewModeTransparency(bool previewEnabled)
    {
        chairRenderer.materials = previewEnabled ? chairMaterialsTransparent : chairMaterials;
        shadowRenderer.material = previewEnabled ? ChairShadowTransparent : ChairShadow;
    }
    #endregion // PRIVATE_METHODS

}
