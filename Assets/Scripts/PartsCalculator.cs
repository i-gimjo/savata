using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PartsCalculator : MonoBehaviour
{
    public int nose;
    public int eye;
    public int eyebrow;
    public int mouth;
    public int face;


    public void CalculateParts(float[,] land)
    {
        NoseCal(land);
        EyeCal(land);
        EyebrowCal(land);
        MouthCal(land);
        FaceCal(land);
    }

    void NoseCal(float[,] land)
    {
        //nose parts
        float face_length = land[8, 1] - land[71, 1];
        float nose_length = land[30, 1] - land[27, 1];
        float nose_ratio = face_length / nose_length;
        if (nose_ratio<=4.8)
        {
            nose = 0;//long nose
        }
        else
        {
            nose = 1;//short nose
        }
    }

    void EyeCal(float[,] land)
    {
        //eye parts
        float eye_width = land[39, 0] - land[36, 0]; //eye width
        float eye_left = land[41, 1] - land[37, 1];
        float eye_right = land[40, 1] - land[38, 1];
        float eye_height = (eye_left + eye_right) / 2;  //eye height
        float nose_ear = land[27, 0] - land[0, 0];  //귀 위부터 가로로 코위까지 수평 길이

        float longness = eye_width / nose_ear;  //눈의 짧고 긺

        float eye_center = land[39, 1];
        float tail = land[36, 1];

        float size = eye_height / eye_width;  // 눈의 크기 - 크고 작음.

        float dif = eye_center - tail; //눈 중심과 눈 꼬리의 y값 차이
        float shape = dif / eye_height; // 눈 처진 정도

        //eye width : short
        if (longness < 0.35)
        {
            //eye height : short
            if (size < 0.31)
            {
                //눈꼬리 : 처짐
                if (shape < 0.25)
                {
                    eye = 0;
                }
                //눈꼬리 : 보통
                else if (shape >= 0.25 && shape < 0.31)
                {
                    eye = 1;
                }
                //눈꼬리 : 올라감
                else
                {
                    eye = 2;
                }
            }
            //eye height : long
            else
            {
                //눈꼬리 : 처짐
                if (shape < 0.21)
                {
                    eye = 3;
                }
                //눈꼬리 : 보통
                else if (shape >= 0.21 && shape < 0.27)
                {
                    eye = 4;
                }
                //눈꼬리 : 올라감
                else
                {
                    eye = 5;
                }
            }
        }
        //eye width : long
        else
        {
            //eye height : short
            if (size < 0.32)
            {
                //눈꼬리 : 처짐
                if (shape < 0.29)
                {
                    eye = 6;
                }
                //눈꼬리 : 보통
                else if (shape >= 0.29 && shape < 0.37)
                {
                    eye = 7;
                }
                //눈꼬리 : 올라감
                else
                {
                    eye = 8;
                }
            }
            //eye height : long
            else
            {
                //눈꼬리 : 처짐
                if (shape < 0.26)
                {
                    eye = 9;
                }
                //눈꼬리 : 보통
                else if (shape >= 0.26 && shape < 0.28)
                {
                    eye = 10;
                }
                //눈꼬리 : 올라감
                else
                {
                    eye = 11;
                }
            }
        }

    }

    void EyebrowCal(float[,] land)
    {
        // eyebrows parts
        float eb_leftx = land[17, 0];
        float eb_lefty = land[17, 1];
        float eb_midx = land[19, 0];
        float eb_midy = land[19, 1];
        float rad = Mathf.Atan2(eb_midy - eb_lefty, eb_midx - eb_leftx);
        float PI = Mathf.PI; // 라디안을 디그리로 바꿈
        float deg_left = (rad * 180) / PI;
        float deg1 = 90 + deg_left; // 양수로 바꾸고 90에서 빼기

        float eb_rightx = land[21, 0];
        float eb_righty = land[21, 1];
        rad = Mathf.Atan2(eb_righty - eb_midy, eb_rightx - eb_midx);

        PI = Mathf.PI;
        float deg_right = (rad * 180) / PI;

        float deg2 = 90 - deg_right;

        float eb_degree = deg1 + deg2;

        // 눈썹 판별
        if (141 <= eb_degree && eb_degree <= 180)
        {
            eyebrow = 0; //straight eyebrows
        }
        else
        {
            eyebrow = 1; //arched eyebrows
        }
    }

    // 입술형
    // 0: "두껍고 짧은 입술 파츠", 1: "두껍고 긴 입술 파츠", 2: "얇고 짧은 입술 파츠", 3: "얇고 긴 입술 파츠"
    void MouthCal(float[,] land)
    {
        //mouth parts
        float mouthHeight = land[57, 1] - land[51, 1];
        float mouthWidth = land[54, 0] - land[48, 0];

        float noseJaw = land[8, 1] - land[33, 1];
        float thickness = (float)mouthHeight / noseJaw;
        float mouthRatio = (float)mouthHeight / mouthWidth;

        // 판별
        if (thickness >= 1.0 / 3.0)
        { // 두꺼운 입술
            if (mouthRatio >= 0.6)
            {
                mouth = 0; //"두껍고 짧은 입술 파츠"
            }
            else
            {
                mouth = 1; //"두껍고 긴 입술 파츠"
            }
        }
        else
        { // 얇은 입술
            if (mouthRatio >= 0.4)
            {
                mouth = 2; //"얇고 짧은 입술 파츠"
            }
            else
            {
                mouth = 3; //"얇고 긴 입술 파츠"
            }
        }
    }

    // 얼굴형
    // 0: "diamond", 1: "round", 2: "square", 3: "oval", 4: "oblong"
    void FaceCal(float[,] land)
    {
        float d1 = land[14, 0] - land[2, 0]; // 귀 아래
        float d2 = land[79, 0] - land[75, 0]; // 이마
        float d3 = land[8, 1] - land[71, 1]; // 얼굴 위아래 길이
        float d10 = land[11, 0] - land[5, 0];

        float r2 = (float)d3 / d1;
        float r1 = (float)d2 / d1;

        // 판별
        if (r2 <= 1.25)
        {
            if (r1 <= 0.68)
            {
                face = 0; // "diamond"
            }
            else if (r1 > 0.68 && r1 < 0.7)
            {
                face = 1; // "round"
            }
            else
            {
                face = 2; // "square"
            }
        }
        else
        {
            if (r1 <= 0.73)
            {
                face = 3; // "oval"
            }
            else
            {
                face = 4; // "oblong"
            }
        }
    }

}
