using System;
using System.Globalization;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class NixieTubePack : MonoBehaviour {

    public GameObject NixieTube0;
    public GameObject NixieTube1;
    public GameObject NixieTube2;
    public GameObject NixieTube3;
    public GameObject NixieTube4;
    public GameObject NixieTube5;
    public GameObject NixieTube6;
    public GameObject NixieTube7;
    public GameObject NixieTube8;
    public GameObject NixieTube9;

    public int Value;

	// Use this for initialization
    void Start()
    {

	}
	
	// Update is called once per frame
    void Update()
    {
        //Value = Random.Range(0, 9999);

        if (NixieTube0 != null) NixieTube0.SendMessage("SetValue", GetDigitFromNumber(Value, 0));
        if (NixieTube1 != null) NixieTube1.SendMessage("SetValue", GetDigitFromNumber(Value, 1));
        if (NixieTube2 != null) NixieTube2.SendMessage("SetValue", GetDigitFromNumber(Value, 2));
        if (NixieTube3 != null) NixieTube3.SendMessage("SetValue", GetDigitFromNumber(Value, 3));
        if (NixieTube4 != null) NixieTube4.SendMessage("SetValue", GetDigitFromNumber(Value, 4));
        if (NixieTube5 != null) NixieTube5.SendMessage("SetValue", GetDigitFromNumber(Value, 5));
        if (NixieTube6 != null) NixieTube6.SendMessage("SetValue", GetDigitFromNumber(Value, 6));
        if (NixieTube7 != null) NixieTube7.SendMessage("SetValue", GetDigitFromNumber(Value, 7));
        if (NixieTube8 != null) NixieTube8.SendMessage("SetValue", GetDigitFromNumber(Value, 8));
        if (NixieTube9 != null) NixieTube9.SendMessage("SetValue", GetDigitFromNumber(Value, 9));
	}

    private int GetDigitFromNumber(int param_number, int param_position)
    {
        return (int)((param_number % Math.Pow(10, param_position + 1)) / Math.Pow(10, param_position));
    }

    public void SetValue(int param_value)
    {
        Value = param_value;
    }
}
