using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Emgu.CV.Util;

public class ImagesScript : MonoBehaviour
{

    VideoCapture webcam;

    public GameObject target;

    public GameObject target2;

    public GameObject shot;

    Vector3 prevCenter = new Vector3(0, 0, 0);

    double prevArea = 0;
    double prevArea2 = 0;

    Texture2D _textureFrame;
    public UnityEngine.UI.RawImage _rawImage; 

    public int[] hsvMinMax_color1 = { 0, 179, 0, 255, 0, 255 }; //Rouge
    public int[] hsvMinMax_color2 = { 0, 179, 0, 255, 0, 255 }; //Bleu

    
    // Objet permettant la capture d'un flux video

    // Une image est une matrice
    Mat _image;
    Mat _imageHsv;

    // Use this for initialization
    void Start()
    {
        webcam = new VideoCapture(0);

        webcam.ImageGrabbed += new EventHandler(_handlerWebcamGrabFrame);

        _image = new Mat();
        _imageHsv = new Mat();

        _textureFrame = new Texture2D(600, 600);

    }

    void Update()
    {
        if (webcam.IsOpened) webcam.Grab();
        
    }


    // Update is called once per frame
    private void _handlerWebcamGrabFrame(object sender, EventArgs e)
    {

        if (webcam.IsOpened)
        {
            if (webcam.Retrieve(_image))    // On associe à l'image la dernière frame capturée
                if (!_image.IsEmpty)
                {
                    //On converti l'image en Hsv
                    CvInvoke.CvtColor(_image, _imageHsv, ColorConversion.Bgr2Hsv);

                    //Seuillage des image Hsv pour discriminer deux couleurs (rouge, bleu)
                    Mat imgGray = tresholdFeed(_imageHsv, hsvMinMax_color1[0], hsvMinMax_color1[1], hsvMinMax_color1[2], hsvMinMax_color1[3], hsvMinMax_color1[4], hsvMinMax_color1[5]);
                    Mat imgGray2 = tresholdFeed(_imageHsv, hsvMinMax_color2[0], hsvMinMax_color2[1], hsvMinMax_color2[2], hsvMinMax_color2[3], hsvMinMax_color2[4], hsvMinMax_color2[5]);


                    applyHsvAndThreshold(ref imgGray);


                    VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                    Mat hierarchy = new Mat();
                    double biggestContourArea = 0;
                    int biggestContourIndex = 0;
                    VectorOfPoint biggestContour = new VectorOfPoint();

                    VectorOfVectorOfPoint contours2 = new VectorOfVectorOfPoint();
                    Mat hierarchy2 = new Mat();
                    double biggestContourArea2 = 0;
                    int biggestContourIndex2 = 0;
                    VectorOfPoint biggestContour2 = new VectorOfPoint();

                    //Trouver les contoure de l'image seuillé
                    findAndDrawContours(_image, imgGray, new MCvScalar(255, 200, 0), new MCvScalar(225, 0, 0), contours, hierarchy, ref biggestContour, ref biggestContourArea, ref biggestContourIndex);
                    findAndDrawContours(_image, imgGray2, new MCvScalar(255, 200, 0), new MCvScalar(225, 0, 0), contours2, hierarchy2, ref biggestContour2, ref biggestContourArea2, ref biggestContourIndex2);

                    //Afin que le centroïd du plus gros contour
                    Point centroid = findCentroid(biggestContour);
                    Point centroid2 = findCentroid(biggestContour2);

                    //Deplacer les joueuer en fonction du centroid 
                    //target.transform.position = new Vector3(-(centroid.X - 320f) / 32f, -(centroid.Y - 240f) / 24f, 0);
                    //if(biggestContourArea > 1000)
                        target.transform.position = new Vector3(-6,  - (centroid.Y - 240f) / 24f, 0);
                    //if (biggestContourArea2 > 1000)
                        target2.transform.position = new Vector3(6, -(centroid2.Y - 240f) / 24f, 0);

                    float ellAngle = 0f;
                    float ellAngle2 = 0f;

                    //Trouver l'angle d'inclinaison de l'objet
                    saveAngle(ref ellAngle, ref biggestContour);
                    saveAngle(ref ellAngle2, ref biggestContour2);

                    //Apliquer la rotation
                    target.transform.rotation = Quaternion.Euler(0, 0, ellAngle);
                    target2.transform.rotation = Quaternion.Euler(0, 0, ellAngle2);

                    //launchObject(biggestContourArea, centroid, ref prevArea);
                    //launchObject(biggestContourArea2, centroid2, ref prevArea2);

                    //Afficher les retours. 
                    CvInvoke.Imshow("Mon image", _image);
                    //CvInvoke.Imshow("Mon image HSV", _imageHsv);
                    //CvInvoke.Imshow("Mon image HSV avec erosion 1", imgGray);
                    //CvInvoke.Imshow("Mon image HSV avec erosion 2", imgGray2);

                    _rawImage.texture = _convertFromMatToTexture2D(_image, _textureFrame);
                }
                else
                {
                    Debug.Log("Erreur : L'image capté est vide");
                }
        }
        else
        {
            Debug.Log("Erreur : La camera est déconnecté.");
        }

        

    }

    float getAngle(Point a, Point b)
    {
        float deltaY = b.Y - a.Y;
        float deltaX = b.X - a.X;
        float angle = Mathf.Atan(deltaY/deltaX) * 180 / (float)Math.PI;
        return angle;
    }

    void saveAngle(ref float ellAngle, ref VectorOfPoint biggestContour)
    {
        if (biggestContour != null && biggestContour.Size > 5)
        {
            RotatedRect ell = CvInvoke.FitEllipse(biggestContour);
            ellAngle = ell.Angle;
        }
    }

    void applyHsvAndThreshold(ref Mat imgGray)
    {

        int operationSize = 1;
        Mat structElement = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(2 * operationSize + 1, 2 * operationSize + 1), new Point(operationSize, operationSize));

        CvInvoke.Erode(imgGray, imgGray, structElement, new Point(-1, 1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
        CvInvoke.Dilate(imgGray, imgGray, structElement, new Point(-1, 1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
    }

    void findAndDrawContours(Mat image, Mat imgGray, MCvScalar color, MCvScalar color2, VectorOfVectorOfPoint contours, Mat hierarchy, ref VectorOfPoint biggestContour, ref double biggestContourArea, ref int biggestContourIndex)
    {
        CvInvoke.FindContours(imgGray, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxNone);
        CvInvoke.DrawContours(image, contours, -1, color, 5);

        for (int i = 0; i < contours.Size; i++)
        {
            if (CvInvoke.ContourArea(contours[i]) > biggestContourArea)
            {
                biggestContour = contours[i];
                biggestContourIndex = i;
                biggestContourArea = CvInvoke.ContourArea(contours[i]);
            }
        }

        CvInvoke.DrawContours(image, contours, biggestContourIndex, color2, 5);
    }

    Point findCentroid(VectorOfPoint biggestContour)
    {
        var moments = CvInvoke.Moments(biggestContour);
        int cx = (int)(moments.M10 / moments.M00);
        int cy = (int)(moments.M01 / moments.M00);
        Point centroid = new Point(cx, cy);

        CvInvoke.Circle(_image, centroid, 1, new MCvScalar(0, 0, 225), 5);

        return centroid;
    }

     Mat tresholdFeed(Mat image, int hmin, int hmax, int smin, int smax, int vmin, int vmax)
        {
            Image<Hsv, byte> imageHSV = image.ToImage<Hsv, byte>();
            Hsv lower = new Hsv(hmin, smin, vmin);
            Hsv upper = new Hsv(hmax, smax, vmax);
            Mat imgGray = imageHSV.InRange(lower, upper).Mat;
            return imgGray;
        }

    void blurFeed(Mat src, Mat dst, int size, int type)
    {
        switch (type)
        {
            case 0: CvInvoke.MedianBlur(src, dst, size); break;
            case 1: CvInvoke.GaussianBlur(src, dst, new Size(size, size), 0); break;
            case 2: CvInvoke.BilateralFilter(src, dst, 9, size, size); break;
            default: break;
        }
    }

    void launchObject(double biggestContourArea, Point centroid, ref double prevArea)
    {
        if (biggestContourArea > prevArea * 2)
        {
            GameObject spawn = Instantiate(shot);
            spawn.transform.position = new Vector3(-(centroid.X - 320f) / 32f, -(centroid.Y - 240f) / 24f, 0);
            spawn.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 10);
        }

        prevArea = biggestContourArea;
    }


    private Texture2D _convertFromMatToTexture2D(Mat matImage, Texture2D texture)
    {
        MemoryStream memstream = new MemoryStream();
        CvInvoke.Flip(matImage, matImage, Emgu.CV.CvEnum.FlipType.Horizontal);
        matImage.ToImage<Bgr, byte>().ToBitmap().Save(stream: memstream, format: matImage.ToImage<Bgr, Byte>().ToBitmap().RawFormat );
        texture.LoadImage(memstream.ToArray());
        return texture;
    }

    void OnDestroy()
    {
        // On ferme toutes les fenêtres ouvertes avec OpenCV
        CvInvoke.DestroyAllWindows();
    }
}
