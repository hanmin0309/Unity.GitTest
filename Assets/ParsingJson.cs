using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ParsingJson : MonoBehaviour
{
    [Serializable]
    public class Lotto
    {
        public int id;
        public string date;
        public int[] number;
        public int bonus;

        public void printNumbers()
        {
            string str = "numbers : ";
            for (int i=0; i<6; i++)
                str += number[i] +" ";

            Debug.Log(str);
            Debug.Log("bonus : " + bonus);
        }
    }
}
