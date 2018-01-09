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

public class HaarScript : MonoBehaviour {

    VideoCapture webcam;

    Mat image = new Mat();
    Mat imageGray = new Mat();

    CascadeClassifier FFCascade;
    string FFCascadePath = "D:/Projects/Unity/Interfaces/InterfaceProject/Interface Project/Assets/xml/haarcascade_frontalface_default.xml";
    Rectangle[] frontFaces;

    CascadeClassifier Cascade2;
    string Cascade2Path = "D:/Projects/Unity/Interfaces/InterfaceProject/Interface Project/Assets/xml/haarcascade_righteye_2splits.xml";
    Rectangle[] facesList2;

    CascadeClassifier Cascade3;
    string Cascade3Path = "D:/Projects/Unity/Interfaces/InterfaceProject/Interface Project/Assets/xml/haarcascade_lefteye_2splits.xml";
    Rectangle[] facesList3;

    int MIN_FACE_SIZE = 50;
    int MAX_FACE_SIZE = 200;

    // Use this for initialization
    void Start () {
        webcam = new VideoCapture(0);

        FFCascade = new CascadeClassifier(FFCascadePath);
        Cascade2 = new CascadeClassifier(Cascade2Path);
        Cascade3 = new CascadeClassifier(Cascade3Path);

        webcam.ImageGrabbed += new EventHandler(handleWebcamQueryFrame);
    }

    // Update is called once per frame
    void Update () {

        if (webcam.IsOpened)
        {
            webcam.Grab();
        }

        findCascade(image, imageGray, FFCascade, frontFaces, new MCvScalar(0, 255, 0));
        findCascade(image, imageGray, Cascade2, facesList2, new MCvScalar(255, 100, 0));
        findCascade(image, imageGray, Cascade3, facesList3, new MCvScalar(255, 0, 100));

        CvInvoke.Imshow("Mon image", image);
        CvInvoke.Imshow("Mon image grise", imageGray);

    }

    void handleWebcamQueryFrame(object sender, EventArgs e)
    {
        webcam.Retrieve(image);
        CvInvoke.CvtColor(image, imageGray, ColorConversion.Bgr2Gray);
    }

    void findCascade(Mat image, Mat imageGray, CascadeClassifier classifier, Rectangle[] rectangleList, MCvScalar color)
    {
        rectangleList = classifier.DetectMultiScale(imageGray, 1.1, 5, new Size(MIN_FACE_SIZE, MIN_FACE_SIZE), new Size(MAX_FACE_SIZE, MAX_FACE_SIZE));

        foreach (Rectangle r in rectangleList)
        {
            CvInvoke.Rectangle(image, r, color, 5);
            CvInvoke.Rectangle(imageGray, r, color, 5);
        }
    }

    void OnDestroy()
    {
        // On ferme toutes les fenêtres ouvertes avec OpenCV
        CvInvoke.DestroyAllWindows();
    }
}
