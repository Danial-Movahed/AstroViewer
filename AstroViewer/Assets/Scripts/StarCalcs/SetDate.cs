﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDate : MonoBehaviour
{
    public StarCalc sun;
    public int year = 2020;

    [Range(1, 12)]
    public int month = 1;
    [Range(0, 31)]
    public int day = 1;

    private void OnValidate()
    {
        try{
            DateTime d = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,DateTime.Now.Hour,DateTime.Now.Minute,0);
            Debug.Log(d);
            if(sun) sun.SetDate(d);
        }
        catch(System.ArgumentOutOfRangeException /*e*/){
            Debug.LogWarning("bad date");
        }
    }

}