using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCheeseAndFish : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private GameObject chesePrefab;
    [SerializeField]
    private GameObject fishPrefab;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (Input.GetKey("c"))
            {
                //var position = cam.ScreenToWorldPoint(Input.mousePosition);
                //position.z = 0;

                //GameObject sam = GameObject.Instantiate(chesePrefab);
                //sam.transform.position = position;

                Vector3 click = Input.mousePosition;
                Vector3 wantedPosition = Camera.main.ScreenToWorldPoint(new Vector3(click.x, click.y, 1f));
                wantedPosition.z = transform.position.z;
                chesePrefab.transform.position = wantedPosition;
            }
            else if (Input.GetKey("b"))
            {
                var position = cam.ScreenToWorldPoint(Input.mousePosition);
                position.z = 0;

                GameObject fish = GameObject.Instantiate(fishPrefab);
                fish.transform.position = position;
            }
        }
    }
}
