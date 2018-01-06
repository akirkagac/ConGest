using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class Login : MonoBehaviour {
	public GameObject LogUsNm;
    public static string UsernameL;
    public static Boolean guest;
    public Button b1;
    public Button b2;
    public Button b3;
    public Button b4;
    public Button b5;
    public Button b6;
    public Button b7;
    public Button b8;
    public Button b9;
    public Button b0;
    public Button baksp;
    public Text warning;  
   

   
	
	private String[] Lines;
	private string DecryptedPass;
    
    public void LoginButton(){
       
        bool UN = false;
		UsernameL = LogUsNm.GetComponent<InputField>().text;
        if (UsernameL != ""){
			if(System.IO.File.Exists(@"C:/Users/ArdaKr/Documents/EE491/Users/" + UsernameL+".txt")){
				UN = true;
				Lines = System.IO.File.ReadAllLines(@"C:/Users/ArdaKr/Documents/EE491/Users/" + UsernameL+".txt");
			} else {
               
                Debug.LogWarning("ID Invalid");
                warning.text = "ID not valid";
                LogUsNm.GetComponent<InputField>().text = "";
            }
		} else {
           
            Debug.LogWarning("ID Field Empty");
            LogUsNm.GetComponent<InputField>().text = "";
            warning.text = "ID field empty";

        }
		
		if (UN == true){
			
			
			print ("Login Sucessful");
            print(UsernameL);
			Application.LoadLevel("MainScreen");

		}
	}

    public void guestButton()
    {

        guest = true;
        Application.LoadLevel("MainScreen");

       
    }
    // Update is called once per frame
    void Update () {
		
		UsernameL = LogUsNm.GetComponent<InputField>().text;
			
	}

    public void button0()
    {
        UsernameL += "0";
        LogUsNm.GetComponent<InputField>().text = UsernameL;
    }
    public void button1()
    {
        UsernameL += "1";
        LogUsNm.GetComponent<InputField>().text = UsernameL;
    }
    public void button2()
    {
        UsernameL += "2";
        LogUsNm.GetComponent<InputField>().text = UsernameL;
    }
    public void button3()
    {
        UsernameL += "3";
        LogUsNm.GetComponent<InputField>().text = UsernameL;
    }
    public void button4()
    {
        UsernameL += "4";
        LogUsNm.GetComponent<InputField>().text = UsernameL;
    }
    public void button5()
    {
        UsernameL += "5";
        LogUsNm.GetComponent<InputField>().text = UsernameL;
    }
    public void button6()
    {
        UsernameL += "6";
        LogUsNm.GetComponent<InputField>().text = UsernameL;
    }
    public void button7()
    {
        UsernameL += "7";
        LogUsNm.GetComponent<InputField>().text = UsernameL;
    }
    public void button8()
    {
        UsernameL += "8";
        LogUsNm.GetComponent<InputField>().text = UsernameL;
    }
    public void button9()
    {
        UsernameL += "9";
        LogUsNm.GetComponent<InputField>().text = UsernameL;
    }
    public void bksp()
    {
        if (!UsernameL.Equals(""))
        {
            UsernameL = UsernameL.Substring(0, UsernameL.Length - 1);
            LogUsNm.GetComponent<InputField>().text = UsernameL;
        }
    }
}
