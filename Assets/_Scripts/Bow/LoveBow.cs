using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LoveBow : MonoBehaviour
{
    XRGrabInteractable grab;
    [SerializeField]GameObject bowString;
    private Transform interactor;
    
    static public Love shootedLove = null;
    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(GrabBow);
        grab.selectExited.AddListener(ReleaseBow);
    }

    // Update is called once per frame

    private void GrabBow(SelectEnterEventArgs arg0)
    {
        interactor = arg0.interactorObject.transform;
        bowString.SetActive(true);
        float rotZ =(interactor.tag=="Right")?-90f:(interactor.tag=="Left")?90f:bowString.transform.localRotation.z;
        bowString.transform.localEulerAngles = new Vector3(bowString.transform.localEulerAngles.x,bowString.transform.localEulerAngles.y,rotZ);
    }
        private void ReleaseBow(SelectExitEventArgs arg0)
    {
        interactor = arg0.interactorObject.transform;
        bowString.SetActive(false);
    }
}
