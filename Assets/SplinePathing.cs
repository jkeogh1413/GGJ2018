﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Interpolation between points with a Catmull-Rom spline
public class SplinePathing : MonoBehaviour
{
    //Has to be at least 4 points
    public Transform[] controlPointsList;
    //Are we making a line or a loop?
    public bool isLooping = false;
	public float alpha = 0.2f;
	public List<Vector3> PosArray = new List<Vector3>();


    //Display without having to press play
    void OnDrawGizmos()
    {
		PosArray.Clear();
        Gizmos.color = Color.white;

        //Draw the Catmull-Rom spline between the points
        for (int i = 0; i < controlPointsList.Length; i++)
        {
            //Cant draw between the endpoints
            //Neither do we need to draw from the second to the last endpoint
            //...if we are not making a looping line
            if ((i == 0 || i == controlPointsList.Length - 2 || i == controlPointsList.Length - 1) && !isLooping)
            {
                continue;
            }
			if (i == 1) {
				Vector3 p1 = controlPointsList [i].position;
				PosArray.Add (p1);
			}
			  
            DisplayCatmullRomSpline(i);
        }
    }

    //Display a spline between 2 points derived with the Catmull-Rom spline algorithm
    void DisplayCatmullRomSpline(int pos)
    {
        //The 4 points we need to form a spline between p1 and p2
        Vector3 p0 = controlPointsList[ClampListPos(pos - 1)].position;
        Vector3 p1 = controlPointsList[pos].position;
        Vector3 p2 = controlPointsList[ClampListPos(pos + 1)].position;
        Vector3 p3 = controlPointsList[ClampListPos(pos + 2)].position;

        //The start position of the line
		Vector3 lastPos = p1;

        //How many times should we loop?
        int loops = Mathf.FloorToInt(1f / alpha);
//		PosArray.Add(p1);

        for (int i = 1; i <= loops; i++)
        {
            //Which t position are we at?
            float t = i * alpha;

            //Find the coordinate between the end points with a Catmull-Rom spline
            Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);

            //Draw this line segment
            Gizmos.DrawLine(lastPos, newPos);

			PosArray.Add (newPos);
            //Save this pos so we can draw the next line segment
            lastPos = newPos;
        }
    }

    //Clamp the list positions to allow looping
    int ClampListPos(int pos)
    {
        if (pos < 0)
        {
            pos = controlPointsList.Length - 1;
        }

        if (pos > controlPointsList.Length)
        {
            pos = 1;
        }
        else if (pos > controlPointsList.Length - 1)
        {
            pos = 0;
        }

        return pos;
    }

    //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
    //http://www.iquilezles.org/www/articles/minispline/minispline.htm
    Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }
}