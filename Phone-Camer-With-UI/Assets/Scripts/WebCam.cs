using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class WebCam : MonoBehaviour
{

    // first camera delected by default
    int currentCameraIndex = 0;

    WebCamTexture texture;

    public RawImage display;

    public Text startStopText;
    public AspectRatioFitter fit;

    public Quaternion baseRotation;

    public void StopWebCam(){
        display.texture = null;
        texture.Stop(); // stops the camera
        texture = null;
    }

    // two public functions for button handlers
    public void SwapCam_Clicked(){
        // check if there are any device camers
        Debug.Log("THERE ARE " + WebCamTexture.devices.Length + " CAMERAS");
        if(WebCamTexture.devices.Length > 0){
            currentCameraIndex +=1;
            currentCameraIndex %= WebCamTexture.devices.Length; // switches between devices
            // clear texture if camera is already turned on
            if( texture != null){
                StopWebCam();
                StartStopCam_Clicked();
            }
            
        }
    }

    public void StartStopCam_Clicked(){
        //if there is camera, stop the camera, else grab a camera from avaiable devices
        if ( texture != null ){ // if current texture is not null, stop receiving the information from the display and clear texture
            StopWebCam();
            startStopText.text = "Start Camera";

        }else{ // start the camera 
        WebCamDevice device = WebCamTexture.devices[currentCameraIndex];
        texture = new WebCamTexture(device.name);
        display.texture = texture;

        // float antiRotate = -(360 - texture.videoRotationAngle);
        // //Quaternions are used to represent rotations.
        // Quaternion quatRot = new Quaternion();
        // quatRot.eulerAngles = new Vector3(0, 0, antiRotate);

        // display.transform.rotation = quatRot;
        baseRotation = transform.rotation;
        texture.Play(); // starts the camera
        startStopText.text = "Stop Camera";
        }
    }

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if (!texture)
            return;

        // ratio thats going to be used in a aspect fitter
        // it is really important to as float cos we dont want to lose precision
        float ratio = (float)texture.width / (float)texture.height;
    
        fit.aspectRatio = ratio;


        int orient = -texture.videoRotationAngle;
        display.rectTransform.localEulerAngles = new Vector3(0, 0, orient); 
        
        float scaleY = texture.videoVerticallyMirrored ? -2f : 2f;
        display.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        transform.rotation = baseRotation * Quaternion.AngleAxis(texture.videoRotationAngle, Vector3.up);
    }
}
