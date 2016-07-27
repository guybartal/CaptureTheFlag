using UnityEngine;
using System.Collections;
using System;

public class ScreenClicker : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Clicked(ray);
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                var ray = Camera.main.ScreenPointToRay(touch.position);
                Clicked(ray);
            }
        }
    }

    private void Clicked(Ray ray)
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            var clickable = hit.collider.gameObject.GetComponent<IClickable>();
            if (clickable != null)
            {
                clickable.OnClick(hit);
            }
        }

    }
}
