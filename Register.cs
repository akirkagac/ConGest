using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class Register : MonoBehaviour {
	public GameObject username;
	
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
    private string Username;
	private string Email;
	private string Password;
	private string ConfPassword;
	private string form;
	private bool EmailValid = false;
	private string[] Characters = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
								   "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
								   "1","2","3","4","5","6","7","8","9","0","_","-"};
	
	public void RegisterButton(){
		bool UN = false;
        Username = username.GetComponent<InputField>().text;
        print(Username);
		if (Username != ""){
			if (!System.IO.File.Exists(@"C:/Users/ArdaKr/Documents/EE491/Users/" + Username+".txt")){
				UN = true;
			} else {
				Debug.LogWarning("Username Taken");
			}
		} else {
			Debug.LogWarning("Username field Empty");
		}
		
		if (UN == true){
						
			form = (Username);
			System.IO.File.WriteAllText(@"C:/Users/ArdaKr/Documents/EE491/Users/" + Username+".txt", form);
			username.GetComponent<InputField>().text = "";
			print ("Registration Complete");
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void button0()
    {
        Username += "0";
        username.GetComponent<InputField>().text = Username;
        print(Username);
    }
    public void button1()
    {
        Username += "1";
        username.GetComponent<InputField>().text = Username;
    }
    public void button2()
    {
        Username += "2";
        username.GetComponent<InputField>().text = Username;
    }
    public void button3()
    {
        Username += "3";
        username.GetComponent<InputField>().text = Username;
    }
    public void button4()
    {
        Username += "4";
        username.GetComponent<InputField>().text = Username;
    }
    public void button5()
    {
        Username += "5";
        username.GetComponent<InputField>().text = Username;
    }
    public void button6()
    {
        Username += "6";
        username.GetComponent<InputField>().text = Username;
    }
    public void button7()
    {
        Username += "7";
        username.GetComponent<InputField>().text = Username;
    }
    public void button8()
    {
        Username += "8";
        username.GetComponent<InputField>().text = Username;
    }
    public void button9()
    {
        Username += "9";
        username.GetComponent<InputField>().text = Username;
    }
    public void bksp()
    {
        if (!Username.Equals(""))
        {
            Username = Username.Substring(0, Username.Length - 1);
            username.GetComponent<InputField>().text = Username;
        }
    }
}
