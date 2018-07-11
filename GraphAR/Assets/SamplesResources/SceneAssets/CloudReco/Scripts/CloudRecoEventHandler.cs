/*===============================================================================
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.

Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;

/// <summary>
/// This MonoBehaviour implements the Cloud Reco Event handling for this sample.
/// It registers itself at the CloudRecoBehaviour and is notified of new search results as well as error messages
/// The current state is visualized and new results are enabled using the TargetFinder API.
/// </summary>
public class CloudRecoEventHandler : MonoBehaviour, ICloudRecoEventHandler
{
    #region PRIVATE_MEMBERS
    CloudRecoBehaviour m_CloudRecoBehaviour;
    ObjectTracker m_ObjectTracker;
    TargetFinder m_TargetFinder;
    CloudRecoContentManager m_CloudRecoContentManager;
    TrackableSettings m_TrackableSettings;
    bool isTargetFinderScanning;
    #endregion // PRIVATE_MEMBERS


    #region PUBLIC_MEMBERS
    /// <summary>
    /// Can be set in the Unity inspector to reference a ImageTargetBehaviour that is used for
    /// augmentations of new cloud reco results.
    /// </summary>
    [Tooltip("Here you can set the ImageTargetBehaviour from the scene that will be used to " +
             "augment new cloud reco search results.")]
    public ImageTargetBehaviour m_ImageTargetBehaviour;
    /// <summary>
    /// The scan-line rendered in overlay when Cloud Reco is in scanning mode.
    /// </summary>
    public ScanLine m_ScanLine;
    public UnityEngine.UI.Image m_CloudActivityIcon;
    public UnityEngine.UI.Button m_ResetCloudTrackables;
    #endregion //PUBLIC_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    /// <summary>
    /// register for events at the CloudRecoBehaviour
    /// </summary>
    void Start()
    {
        m_ScanLine = FindObjectOfType<ScanLine>();
        m_CloudRecoContentManager = FindObjectOfType<CloudRecoContentManager>();
        m_TrackableSettings = FindObjectOfType<TrackableSettings>();

        // register this event handler at the cloud reco behaviour
        m_CloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        if (m_CloudRecoBehaviour)
        {
            m_CloudRecoBehaviour.RegisterEventHandler(this);
        }
    }

    void Update()
    {
        if (m_CloudRecoBehaviour.CloudRecoInitialized)
        {
            SetCloudActivityIconVisible(m_ObjectTracker.TargetFinder.IsRequesting());
        }

        if (m_TrackableSettings != null)
        {
            EnableResetTrackablesButton(m_TrackableSettings.m_DeviceTrackerEnabled);
        }
    }
    #endregion //MONOBEHAVIOUR_METHODS
    

    #region INTERFACE_IMPLEMENTATION_ICloudRecoEventHandler
    /// <summary>
    /// Called when TargetFinder has been initialized successfully
    /// </summary>
    public void OnInitialized()
    {
        Debug.Log("Cloud Reco initialized successfully.");

        // get a reference to the Object Tracker, remember it
        m_ObjectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        m_TargetFinder = m_ObjectTracker.TargetFinder;
    }

    
    // Error callback methods implemented in CloudErrorHandler
    public void OnInitError(TargetFinder.InitState initError) {}
    public void OnUpdateError(TargetFinder.UpdateState updateError) {}


    /// <summary>
    /// when we start scanning, unregister Trackable from the ImageTargetTemplate, then delete all trackables
    /// </summary>
    public void OnStateChanged(bool scanning)
    {

        Debug.Log("<color=blue>OnStateChanged(): </color>" + scanning);

        isTargetFinderScanning = scanning;
        
        if (scanning)
        {
            // clear all known trackables
            m_TargetFinder.ClearTrackables(false);

        }

        m_ScanLine.ShowScanLine(scanning);
    }

    /// <summary>
    /// Handles new search results
    /// </summary>
    /// <param name="targetSearchResult"></param>
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {
        Debug.Log("<color=blue>OnNewSearchResult(): </color>" + targetSearchResult.TargetName);

        // This code demonstrates how to reuse an ImageTargetBehaviour for new search results
        // and modifying it according to the metadata. Depending on your application, it can
        // make more sense to duplicate the ImageTargetBehaviour using Instantiate() or to
        // create a new ImageTargetBehaviour for each new result. Vuforia will return a new
        // object with the right script automatically if you use:
        // TargetFinder.EnableTracking(TargetSearchResult result, string gameObjectName)

        m_CloudRecoContentManager.HandleTargetFinderResult(targetSearchResult);


        //Check if the metadata isn't null
        if (targetSearchResult.MetaData == null)
        {
            Debug.Log("Target metadata not available.");
        }
        else
        {
            Debug.Log("MetaData: " + targetSearchResult.MetaData);
            Debug.Log("TargetName: " + targetSearchResult.TargetName);
            Debug.Log("Pointer: " + targetSearchResult.TargetSearchResultPtr);
            Debug.Log("TargetSize: " + targetSearchResult.TargetSize);
            Debug.Log("TrackingRating: " + targetSearchResult.TrackingRating);
            Debug.Log("UniqueTargetId: " + targetSearchResult.UniqueTargetId);
        }

        // First clear all trackables
        m_TargetFinder.ClearTrackables(false);

        // enable the new result with the same ImageTargetBehaviour:
        m_TargetFinder.EnableTracking(targetSearchResult, m_ImageTargetBehaviour.gameObject);
    }
    #endregion // INTERFACE_IMPLEMENTATION_ICloudRecoEventHandler


    #region PUBLIC_METHODS
    

    public void TrackingLost()
    {
        TrackableWasFound(false);
    }

    public void TrackingFound()
    {
        TrackableWasFound(true);
    }
    
    #endregion // PUBLIC_METHODS

        
    #region PRIVATE_METHODS
    void SetCloudActivityIconVisible(bool visible)
    {
        if (!m_CloudActivityIcon) return;

        m_CloudActivityIcon.enabled = visible;
    }

    void EnableResetTrackablesButton(bool enable)
    {
        m_ResetCloudTrackables.image.enabled = enable;
        m_ResetCloudTrackables.interactable = enable && !isTargetFinderScanning;
        m_ResetCloudTrackables.enabled = enable;
    }
    
    void TrackableWasFound(bool isTrackingFound)
    {
        if (m_CloudRecoContentManager)
        {
            m_CloudRecoContentManager.ShowTargetInfo(isTrackingFound);
        }

        if (m_TargetFinder != null)
        {
            if (isTrackingFound)
            {
                // Start Target Finder again if we lost the current trackable
                isTargetFinderScanning = !m_TargetFinder.Stop();   
            }
            else
            {
                // Stop Target Finder since we have now a result
                // Target Finder will be restarted again when we lose track of the result
                m_TargetFinder.ClearTrackables(false);
                isTargetFinderScanning = m_TargetFinder.StartRecognition();
            }
            
            if (m_ScanLine)
            {
                // Start or Stop showing the scan-line
                m_ScanLine.ShowScanLine(isTargetFinderScanning);
            }
        }
    }

    #endregion // PRIVATE_METHODS
    
    
    #region BUTTON_METHODS
    public void ResetTrackables()
    {
        if (m_TrackableSettings.m_DeviceTrackerEnabled)
        {
            // When the Reset button is clicked during Extended Tracking, manually
            // trigger a lost event to hide the augmentations since the
            // TrackableEventHandler will not have received a normal automatic trackable event.
            FindObjectOfType<CloudRecoTrackableEventHandler>().OnTrackableStateChanged(
                TrackableBehaviour.Status.EXTENDED_TRACKED, TrackableBehaviour.Status.NO_POSE);
        }
        
        TrackingLost();
    }
    #endregion // BUTTON_METHODS
}
