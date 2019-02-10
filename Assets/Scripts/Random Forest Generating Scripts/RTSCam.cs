//very basic RTS cam. It is not perfect, but it will get you moving, rotating, and zooming around the scene. 
//the main issue with the cam is that stops orbitating around the centre of the screen when the player zooms in and out.
//stay tuned! when I learn how to fix it, I'll make a video. 

using UnityEngine;
using System.Collections;

public class RTSCam : MonoBehaviour 
{
	//camera move speed
	public int cameraSpeed;
    public float fakeDelta;

    private void Start()
    {
        fakeDelta = 0.02f; 
    }

	void Update () 
	{
		//translating on the x, z and Y axes using WASD
		if(Input.GetKey("w"))
		{
            transform.Translate(Vector3.forward * cameraSpeed * fakeDelta);// * Time.deltaTime ); 
		}
		
		if(Input.GetKey("s"))
		{
            transform.Translate(Vector3.back * cameraSpeed * fakeDelta);// * Time.deltaTime ); 
		}
		
		if(Input.GetKey("d"))
		{
            transform.Translate(Vector3.right * cameraSpeed * fakeDelta);// * Time.deltaTime); 
		}
		
		if(Input.GetKey("a"))
		{
            transform.Translate(Vector3.left * cameraSpeed * fakeDelta);// * Time.deltaTime ); 
		}

	
		//zooming up and down with the scrollWheel

		if(Input.GetAxis("Mouse ScrollWheel") < 0  && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
		{
            transform.Translate((Vector3.up * 5) * cameraSpeed * fakeDelta);// * Time.deltaTime ); 
		}

		if(Input.GetAxis("Mouse ScrollWheel") > 0 && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
		{
            transform.Translate((Vector3.down * 5) * cameraSpeed * fakeDelta);// * Time.deltaTime ); 
		}

		//rotate

		if(Input.GetKey("e"))
		{
            transform.Rotate(Vector3.up * cameraSpeed * 10 * fakeDelta);// * Time.deltaTime); 
		}
		
		if(Input.GetKey ("q"))
		{
            transform.Rotate(Vector3.down * cameraSpeed * 10 * fakeDelta);// * Time.deltaTime); 
		}
		
	}
	
}





