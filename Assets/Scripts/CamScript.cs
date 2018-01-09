using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.IO;
using Emgu.CV.Util;

public class CamScript : MonoBehaviour {

    VideoCapture webcam;

    public static Mat image;
    public static Mat imageCopy;

    // Use this for initialization
    void Start () {
        webcam = new VideoCapture(0);
    }
	
	// Update is called once per frame
	void Update () {
        image = webcam.QueryFrame();
        imageCopy = webcam.QueryFrame();
    }

}
