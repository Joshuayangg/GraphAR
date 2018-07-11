/*==============================================================================
Copyright (c) 2018 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
==============================================================================*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GroundPlaneUI : MonoBehaviour
{
    #region PUBLIC_MEMBERS
    [Header("UI Elements")]
    public Text m_Title;
    public Text m_TrackerStatus;
    public Text m_Instructions;
    public CanvasGroup m_ScreenReticle;

    [Header("UI Buttons")]
    public Button m_ResetButton;
    public Toggle m_PlacementToggle, m_GroundToggle, m_MidAirToggle;
    #endregion // PUBLIC_MEMBERS


    #region PRIVATE_MEMBERS
    const string TITLE_PLACEMENT = "Product Placement";
    const string TITLE_GROUNDPLANE = "Ground Plane";
    const string TITLE_MIDAIR = "Mid-Air";

    GraphicRaycaster m_GraphicRayCaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    ProductPlacement m_ProductPlacement;
    TouchHandler m_TouchHandler;

    Image m_TrackerStatusImage;
    #endregion // PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        m_ResetButton.interactable = m_MidAirToggle.interactable =
            m_GroundToggle.interactable = m_PlacementToggle.interactable = false;

        m_Title.text = TITLE_PLACEMENT;
        m_TrackerStatus.text = "";
        m_TrackerStatusImage = m_TrackerStatus.GetComponentInParent<Image>();

        m_ProductPlacement = FindObjectOfType<ProductPlacement>();
        m_TouchHandler = FindObjectOfType<TouchHandler>();

        m_GraphicRayCaster = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem = FindObjectOfType<EventSystem>();

        Vuforia.DeviceTrackerARController.Instance.RegisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
    }

    void Update()
    {
        if (m_ProductPlacement.IsPlaced || PlaneManager.AstronautIsPlaced)
        {
            m_ResetButton.interactable = m_MidAirToggle.interactable = true;
        }

        m_TrackerStatusImage.enabled = !string.IsNullOrEmpty(m_TrackerStatus.text);
    }

    void LateUpdate()
    {
        if (PlaneManager.GroundPlaneHitReceived)
        {
            // We got an automatic hit test this frame

            // Hide the onscreen reticle when we get a hit test
            m_ScreenReticle.alpha = 0;

            m_Instructions.transform.parent.gameObject.SetActive(true);
            m_Instructions.enabled = true;

            if (PlaneManager.planeMode == PlaneManager.PlaneMode.GROUND)
            {
                m_Instructions.text = "Tap to place Astronaut";
            }
            else if (PlaneManager.planeMode == PlaneManager.PlaneMode.PLACEMENT)
            {
                m_Instructions.text = (m_ProductPlacement.IsPlaced) ?
                    "• Touch and drag to move Chair" +
                    "\n• Two fingers to rotate" +
                    ((m_TouchHandler.enablePinchScaling) ? " or pinch to scale" : "") +
                    "\n• Double-tap to reset Anchor location"
                    :
                    "Tap to place Chair";
            }
        }
        else
        {
            // No automatic hit test, so set alpha based on which plane mode is active
            m_ScreenReticle.alpha =
                (PlaneManager.planeMode == PlaneManager.PlaneMode.GROUND ||
                PlaneManager.planeMode == PlaneManager.PlaneMode.PLACEMENT) ? 1 : 0;

            m_Instructions.transform.parent.gameObject.SetActive(true);
            m_Instructions.enabled = true;

            if (PlaneManager.planeMode == PlaneManager.PlaneMode.GROUND ||
                PlaneManager.planeMode == PlaneManager.PlaneMode.PLACEMENT)
            {
                m_Instructions.text = "Point device towards ground";
            }
            else if (PlaneManager.planeMode == PlaneManager.PlaneMode.MIDAIR)
            {
                m_Instructions.text = "Tap to place Drone";
            }
        }
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy() called.");

        Vuforia.DeviceTrackerARController.Instance.UnregisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    public void Reset()
    {
        m_ResetButton.interactable = m_MidAirToggle.interactable = false;

        m_PlacementToggle.isOn = true;
    }

    public void UpdateTitle()
    {
        switch (PlaneManager.planeMode)
        {
            case PlaneManager.PlaneMode.GROUND:
                m_Title.text = TITLE_GROUNDPLANE;
                break;
            case PlaneManager.PlaneMode.MIDAIR:
                m_Title.text = TITLE_MIDAIR;
                break;
            case PlaneManager.PlaneMode.PLACEMENT:
                m_Title.text = TITLE_PLACEMENT;
                break;
        }
    }

    public bool InitializeUI()
    {
        // Runs only once after first successful Automatic hit test
        m_PlacementToggle.interactable = true;
        m_GroundToggle.interactable = true;

        if (Vuforia.VuforiaRuntimeUtilities.IsPlayMode())
        {
            m_MidAirToggle.interactable = true;
            m_ResetButton.interactable = true;
        }

        // Make the PlacementToggle active
        m_PlacementToggle.isOn = true;

        return true;
    }

    public bool IsCanvasButtonPressed()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        m_GraphicRayCaster.Raycast(m_PointerEventData, results);

        bool resultIsButton = false;
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponentInParent<Toggle>() ||
                result.gameObject.GetComponent<Button>())
            {
                resultIsButton = true;
                break;
            }
        }
        return resultIsButton;
    }
    #endregion // PUBLIC_METHODS


    #region VUFORIA_CALLBACKS
    void OnDevicePoseStatusChanged(Vuforia.TrackableBehaviour.Status status, Vuforia.TrackableBehaviour.StatusInfo statusInfo)
    {
        Debug.Log("OnDevicePoseStatusChanged(" + status + ", " + statusInfo + ")");

        switch (statusInfo)
        {
            case Vuforia.TrackableBehaviour.StatusInfo.INITIALIZING:
                m_TrackerStatus.text = "Tracker Initializing";
                break;
            case Vuforia.TrackableBehaviour.StatusInfo.EXCESSIVE_MOTION:
                m_TrackerStatus.text = "Excessive Motion";
                break;
            case Vuforia.TrackableBehaviour.StatusInfo.INSUFFICIENT_FEATURES:
                m_TrackerStatus.text = "Insufficient Features";
                break;
            default:
                m_TrackerStatus.text = "";
                break;
        }

    }
    #endregion // VUFORIA_CALLBACKS

}
