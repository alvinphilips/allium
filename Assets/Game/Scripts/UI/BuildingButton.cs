using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Camera mainCamera;
    [SerializeField] LayerMask floorMask;

    [SerializeField] GameObject spawnObject;

    //Preview while placing can be a seperate object that the actual prefab
    GameObject previewObject;

    private void Start()
    {
        mainCamera = Camera.main;

        //Fetch object detail (price / etc)

    }

    private void Update()
    {
        if(previewObject != null)
        {
            UpdatePreview();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");

        previewObject = Instantiate(spawnObject);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.touches[0].position);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask)) 
        {
            //Replace preview with actual object (optional if we are directly using og objects for preview)
        }

        previewObject = null;
    }

    void UpdatePreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.touches[0].position);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask))
        {
            previewObject.transform.position = hit.point;
        }
    }

}
