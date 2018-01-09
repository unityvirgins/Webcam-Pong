using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.IO;

public class EmptyScript : MonoBehaviour {

    // Objet permettant la capture d'un flux video
    VideoCapture webcam;

    VideoWriter writer;

    // Une image est une matrice
    Mat image;


	// Use this for initialization
	void Start () {

        // Webcam interne : VideoCapture(0)
        // Sinon, path d'un fichier vidéo
        webcam = new VideoCapture("D:/Projects/Unity/Interfaces/InterfaceProject/Interface Project/Assets/Videos/videotest2.mp4");

        writer = new VideoWriter("D:/Projects/Unity/Interfaces/InterfaceProject/Interface Project/Assets/Videos/writtenvideo.mp4", 120, new Size(960,540), true);
        CvInvoke.NamedWindow("FrameWindow");
        CvInvoke.WaitKey(0);


    }
	
	// Update is called once per frame
	void Update () {

        // On associe à l'image la dernière frame capturée
        image = webcam.QueryFrame();
        // On affiche l'image dans une fenêtre
        CvInvoke.CvtColor(image, image, ColorConversion.Bgr2Xyz);
        CvInvoke.Imshow("Mon image", image);

        CvInvoke.Resize(image, image, new Size(960,540));
        //CvInvoke.Flip(image, image, FlipType.Vertical);
        writer.Write(image);

	}

    void OnDestroy()
    {
        // On ferme toutes les fenêtres ouvertes avec OpenCV
        CvInvoke.DestroyAllWindows();
    }
}
