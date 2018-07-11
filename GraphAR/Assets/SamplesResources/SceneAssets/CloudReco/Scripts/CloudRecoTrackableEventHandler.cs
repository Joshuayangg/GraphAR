/*===============================================================================
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.
 
Copyright (c) 2010-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;

public class CloudRecoTrackableEventHandler : DefaultTrackableEventHandler
{
    #region PRIVATE_MEMBERS
    private CloudRecoEventHandler m_CloudRecoEventHandler;
    #endregion // PRIVATE_MEMBERS


    #region PROTECTED_METHODS

    protected override void Start()
    {
        base.Start();

        m_CloudRecoEventHandler = FindObjectOfType<CloudRecoEventHandler>();
    }

    protected override void OnTrackingFound()
    {
        Debug.Log("<color=blue>OnTrackingFound()</color>");

        base.OnTrackingFound();

        if (m_CloudRecoEventHandler != null)
        {
            m_CloudRecoEventHandler.TrackingFound();
        }
    }

    protected override void OnTrackingLost()
    {
        Debug.Log("<color=blue>OnTrackingLost()</color>");

        base.OnTrackingLost();

        if (m_CloudRecoEventHandler != null)
        {
            m_CloudRecoEventHandler.TrackingLost();
        }
    }

    #endregion //PROTECTED_METHODS
}
