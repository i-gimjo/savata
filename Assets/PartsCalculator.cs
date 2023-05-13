using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PartsCalculator : MonoBehaviour
{
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
        // 코 파츠
        float nose = 0; // 초기화, 긴 코: 0, 짧은 코: 1
        float face_length = land[8, 1] - land[71, 1];
        float nose_length = land[30, 1] - land[27, 1];
        if (nose_length >= face_length / 3.75)
        {
            nose = 0;
        }
        else
        {
            nose = 1;
        }
        Debug.Log(nose);
    }

    void EyeCal(float[,] land)
    {
        // 눈 파츠: 큰 눈: 0, 작은 눈: 1
        int eye = 0;
        float eye_left_height = land[41, 1] - land[37, 1];
        float eye_right_height = land[40, 1] - land[38, 1];
        float eye_width = land[39, 0] - land[36, 0];
        if ((eye_right_height + eye_right_height) / 2 >= eye_width / 3)
        {
            eye = 0;
        }
        else
        {
            eye = 1;
        }
        Debug.Log(eye);
    }

    void EyebrowCal(float[,] land)
    {
        // 0, "일자눈썹" / 1, "아치형 눈썹"
        float eyebrow = 0;
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
            eyebrow = 0;
        }
        else
        {
            eyebrow = 1;
        }
        Debug.Log(eyebrow);
    }

    // 입술형
    // 0: "두껍고 짧은 입술 파츠", 1: "두껍고 긴 입술 파츠", 2: "얇고 짧은 입술 파츠", 3: "얇고 긴 입술 파츠"
    void MouthCal(float[,] land)
    {
        float mouth = 0;
        // 필요한 랜드마크 번호: 51, 57 (상하), 48, 54 (좌우)
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
        Debug.Log(mouth);
    }

    // 얼굴형
    // 0: "round face", 1: "square", 2: "diamond", 3: "oval", 4: "rectangle"
    void FaceCal(float[,] land)
    {
        float face = 0;
        float d1 = land[14, 0] - land[2, 0]; // 귀 아래
        float d2 = land[79, 0] - land[75, 0]; // 이마
        float d3 = land[8, 1] - land[71, 1]; // 얼굴 위아래 길이
        float d10 = land[11, 0] - land[5, 0];

        float r2 = (float)d3 / d1;
        float r1 = (float)d2 / d1;

        // 판별
        if (r2 <= 1.25)
        {
            if (r1 <= 0.8)
            {
                face = 0; // "round face"
            }
            else
            {
                face = 1; // "square"
            }
        }
        else
        {
            if (r1 <= 0.7)
            {
                face = 2; // "diamond"
            }
            else if (r1 > 0.7 && r1 < 0.75)
            {
                face = 3; // "oval"
            }
            else
            {
                face = 4; // "rectangle"
            }
        }
        Debug.Log(face);
    }

}
