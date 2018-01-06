using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System;
using WindowsInput;

public class EE491Handler : MonoBehaviour {

    //Image reference
    public Boolean OnOff = false;
    public Boolean performToggle;
    
    Transform image;
    public Toggle TrainTest;
    public Text testtext;
    public Text gestureText;
    public Text costComparison;
    public Text selectedGesture;
    public Text trainText;
    public Text trainedGestures;
    public Dropdown dd;
    public Button perform;
    public Button remove;
    public Button logout;
    SerialPort stream = new SerialPort("COM6", 9600);
    double[] in1 = { 0, 0, 0 };
    double[] input = { 0, 0, 0 };
    double[][] buffer = new double[3][];
    double[][] temp = new double[3][];
    double[][] tempG = new double[3][];
    double[] x;
    double[] y;
    double[,] f;
    double sum;
    double[,] distance;
    public String crg;
    public String gestLine;
    public String totalLine;
    
    int windowSize = 20;

    ArrayList gestures;
    ArrayList gestureNames;
    String ptread;

    int update_number = 0;
    int dtw_update_number = 0;
    
    void Start () {

        
        perform.interactable = false;
        remove.interactable = false;
        TrainTest.interactable = false;
        dd.interactable = false;
       
        PopulateList();
        CreateArrayStructures();
        gestureNames = new ArrayList();
        gestures = new ArrayList();
       
        stream.Open();      
       
        image = transform.GetChild(0).GetChild(0);
        image.gameObject.SetActive(true);

        if(System.IO.File.Exists(@"C:/Users/ArdaKr/Documents/EE491/Users/" + Login.UsernameL + "-saved.txt"))
        {
            string[] Lines = System.IO.File.ReadAllLines(@"C:/Users/ArdaKr/Documents/EE491/Users/" + Login.UsernameL + "-saved.txt");
            for (int k = 0; k< Lines.Length; k++)
            {
                tempG = new double[3][];
                tempG[0] = new double[windowSize];
                tempG[1] = new double[windowSize];
                tempG[2] = new double[windowSize];
                string gestureR = Lines[k];
                print(gestureR);
                gestureNames.Add(gestureR);
                string[] gestureLines = System.IO.File.ReadAllLines(@"C:/Users/ArdaKr/Documents/EE491/Users/" + Login.UsernameL + "-"+gestureR+".txt");
                for (int g = 0; g < gestureLines.Length; g++)
                {

                    if (!gestureLines[g].Equals(""))
                    {
                        print(gestureLines[g]);
                        string[] vec3G = gestureLines[g].Split(',');

                        double yawG = System.Convert.ToDouble(vec3G[0]);
                        double pitchG = System.Convert.ToDouble(vec3G[1]);
                        double rollG = System.Convert.ToDouble(vec3G[2]);

                        tempG[0][g] = yawG;
                        tempG[1][g] = pitchG;
                        tempG[2][g] = rollG;
                    }

                }
                    double[][] temp2G = new double[3][];
                    temp2G[0] = new double[windowSize];
                    temp2G[1] = new double[windowSize];
                    temp2G[2] = new double[windowSize];

                    for (int a = 0; a < 3; a++)
                    {
                        for (int b = 0; b < windowSize; b++)
                        {
                            temp2G[a][b] = tempG[a][b];

                        }
                    }
                    tempG = null;
                    gestures.Add(temp2G);
                
            }
        }


    }

	// Update is called once per frame
	void Update () {
        if (!OnOff)
        {
			try{
				stream.ReadLine();
			}
			catch (System.Exception){
				
			}
        }

        else
        {
            
            if (TrainTest.isOn)
            {
                
				String ptread = stream.ReadLine();
				
                dtw_update_number = 0;
                perform.interactable = true;
                remove.interactable = true;
                dd.interactable = true;

                if (performToggle)
                {
                   
                    update_number++;
                    print(update_number);
                    if (update_number == -29)
                    {
                        trainText.text = "You have 2s to start.";
                    }

                    if (update_number == -15)
                    {
                        trainText.text = "You have 1s to start.";
                    }

                    if (update_number == -1)
                    {
                        
                        trainText.text = "Perform!";
                    }
                       
                        string[] vec3 = ptread.Split(',');

                        double yaw = System.Convert.ToDouble(vec3[0]);
                        double pitch = System.Convert.ToDouble(vec3[1]);
                        double roll = System.Convert.ToDouble(vec3[2]);

                        double[] in2 = { yaw, pitch, roll };
                        
                        for (int i = 0; i < 3; i++)
                        { 
                            input[i] = in2[i] - in1[i];

                        }

                        in1[0] = yaw;
                        in1[1] = pitch;
                        in1[2] = roll;
                    image.transform.Rotate(new Vector3((float)input[0], (float)input[1], (float)input[2]));
                    
                    if (update_number > 0 && update_number < windowSize+1)
                    {
                        temp[0][update_number-1] = input[0];
                        temp[1][update_number-1] = input[1];
                        temp[2][update_number-1] = input[2];
                        

                    }

                    if(update_number == windowSize)
                    {
                        double[][] temp2 = new double[3][];
                        temp2[0] = new double[windowSize];
                        temp2[1] = new double[windowSize];
                        temp2[2] = new double[windowSize];

                        for (int a = 0; a < 3; a++) {
                            for (int b = 0; b < windowSize; b++) {
                                temp2[a][b] = temp[a][b];

                            } }
                        temp = null; 
                        gestures.Add(temp2);
                        print(gestures.Count);
                        
                        gestureNames.Add(crg);
                        crg = "";
                        performToggle = false;
                        trainedGestures.text = ToString(gestureNames);
                        trainText.text = "Done.";
                    }

                    
                }
                
                
            }

            else
            {
                              
				String ptread = stream.ReadLine();
				
                perform.interactable = false;
                remove.interactable = false;
                dtw_update_number++;  
                if(dtw_update_number >0 && dtw_update_number < 15)
                {
                    
                    trainText.text = "Prepare to control!";
                }
                else {
                    trainText.text = "Go for it!";
                    
                    string[] vec3 = ptread.Split(',');

                    double yaw = System.Convert.ToDouble(vec3[0]);
                    double pitch = System.Convert.ToDouble(vec3[1]);
                    double roll = System.Convert.ToDouble(vec3[2]);

                    double[] in2 = { yaw, pitch, roll };

                    for (int i = 0; i < 3; i++)
                    {
                        input[i] = in2[i] - in1[i];

                    }

                    in1[0] = yaw;
                    in1[1] = pitch;
                    in1[2] = roll;


                    shift_and_fill(buffer, input);
                    image.transform.Rotate(new Vector3((float)input[0], (float)input[1], (float)input[2]));


                    if (dtw_update_number == 30)
                    {

                        dtw_update_number = 0;

                        double[,] dtw_costs = new double[gestures.Count, 3];
                        double[] gestureCosts = new double[gestures.Count];
                        int gesture = 20;

                        double min = double.PositiveInfinity;

                        for (int i = 0; i < gestures.Count; i++)
                        {
                            double[][] currentGesture = (double[][])gestures[i];
                           
                            for (int j = 0; j < 3; j++)
                            {
                                DTWO(buffer[j], currentGesture[j]);
                                
                                dtw_costs[i, j] = computeDTW();
                   
                                gestureCosts[i] += dtw_costs[i, j];
                                Math.Round(gestureCosts[i], 2);
                                
                            }

                            if (gestureCosts[i] < min)
                            {
                                min = gestureCosts[i];
                                gesture = i;
                            }
                            
                        }
                        
                        String performedGesture = (String)gestureNames[gesture];
                        testtext.text = performedGesture;
                        switch (performedGesture)
                        {

                            case "Idle":
                               
                                break;
                            case "Previous":
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.LEFT); break;
                            case "Next":
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.RIGHT); break;
                            case "PlayPause":
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.MEDIA_PLAY_PAUSE);
                                break;
                            case "Previous Track":
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.MEDIA_PREV_TRACK); break;

                            case "Next Track":
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.MEDIA_NEXT_TRACK);
                                break;

                            case "Volume Up":
                               
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.VOLUME_UP);
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.VOLUME_UP);
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.VOLUME_UP);
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.VOLUME_UP);
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.VOLUME_UP);
                                break;
                            case "Volume Down":
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.VOLUME_DOWN);
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.VOLUME_DOWN);
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.VOLUME_DOWN);
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.VOLUME_DOWN);
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.VOLUME_DOWN);
                                break;
                            case "Mute":
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.VOLUME_MUTE); break;

                            default: break;

                        }
                    }
                }
                
            }          

        }

    }

    void PopulateList()
    {
        List<String> gestures = new List<String>() {"Idle", "Previous", "Next", "PlayPause", "Previous Track", "Next Track" , "Volume Up" , "Volume Down" , "Mute" };
        dd.AddOptions(gestures);
    }

    void CreateArrayStructures()
    {
        buffer[0] = new double[windowSize];
        buffer[1] = new double[windowSize];
        buffer[2] = new double[windowSize];
        tempG[0] = new double[windowSize];
        tempG[1] = new double[windowSize];
        tempG[2] = new double[windowSize];
    }

    public void PerformG()
    {
        print(dd.options[dd.value].text);
        PerformGesture(dd.options[dd.value].text);
    }


    public void RemoveG()
    {
        RemoveGesture(dd.options[dd.value].text);
    }

    public void Logout()
    {
        SaveCurrentGestures(gestureNames, gestures);
        Application.LoadLevel("Login Menu");        
        Login.guest = false;
    }

    public void RemoveGesture (string text)
    {
        foreach (var i in gestureNames)
        {
            if (text.Equals((string)i)) {
               int idx = gestureNames.IndexOf(i);
                gestureNames.Remove(i);
                gestures.RemoveAt(idx);
                trainText.text = text + " removed.";
            }
        }       

    }

    public void SaveCurrentGestures(ArrayList gestureNames, ArrayList gestures)
    {
        string names = "";
        if (!Login.guest) { 
        foreach (var j in gestureNames)
        {
            int idxS = gestureNames.IndexOf(j);
            double[][] gestS = (double[][])gestures[idxS];
            for (int i = 0; i < windowSize; i++)
            {
                gestLine = (gestS[0][i] + "," + gestS[1][i] + "," + gestS[2][i] + Environment.NewLine);
                totalLine = totalLine + gestLine;
            }
            System.IO.File.WriteAllText(@"C:/Users/ArdaKr/Documents/EE491/Users/" + Login.UsernameL + "-" + (string)j + ".txt", totalLine);
                names = (names + (string)j + Environment.NewLine);
                totalLine = "";
            }
            System.IO.File.WriteAllText(@"C:/Users/ArdaKr/Documents/EE491/Users/" + Login.UsernameL + "-saved.txt", names);
        }
    }



    public void PerformGesture(string text)
    {
        temp = new double[3][];
        temp[0] = new double[windowSize];
        temp[1] = new double[windowSize];
        temp[2] = new double[windowSize];
        crg = text;            
        update_number = -30;
        performToggle = true;
    }

    public void onoff()
    {
        if (!OnOff)
        {
            OnOff = true;
            perform.interactable = true;
            remove.interactable = true;
            TrainTest.interactable = true;
            dd.interactable = true;
            
        }
        else
        {
            OnOff = false;
            perform.interactable = false;
            remove.interactable = false;
            TrainTest.interactable = false;
            dd.interactable = false;
           
        }
     }


    private void DTWO(double[] _x, double[] _y)
    {
        x = _x;
        y = _y;
        distance = new double[x.Length, y.Length];
        f = new double[x.Length + 1, y.Length + 1];

        for (int i = 0; i < x.Length; ++i)
        {
            for (int j = 0; j < y.Length; ++j)
            {
                distance[i, j] = Math.Abs(x[i] - y[j]);
            }
        }

        for (int i = 0; i <= x.Length; ++i)
        {
            for (int j = 0; j <= y.Length; ++j)
            {
                f[i, j] = -1.0;
            }
        }

        for (int i = 1; i <= x.Length; ++i)
        {
            f[i, 0] = double.PositiveInfinity;
        }
        for (int j = 1; j <= y.Length; ++j)
        {
            f[0, j] = double.PositiveInfinity;
        }

        f[0, 0] = 0.0;
        sum = 0.0;


    }

    public double computeFBackward(int i, int j)
    {
        if (!(f[i, j] < 0.0))
        {
            return f[i, j];
        }
        else
        {
            if (computeFBackward(i - 1, j) <= computeFBackward(i, j - 1) && computeFBackward(i - 1, j) <= computeFBackward(i - 1, j - 1)
                && computeFBackward(i - 1, j) < double.PositiveInfinity)
            {
                f[i, j] = distance[i - 1, j - 1] + computeFBackward(i - 1, j);
            }
            else if (computeFBackward(i, j - 1) <= computeFBackward(i - 1, j) && computeFBackward(i, j - 1) <= computeFBackward(i - 1, j - 1)
                && computeFBackward(i, j - 1) < double.PositiveInfinity)
            {
                f[i, j] = distance[i - 1, j - 1] + computeFBackward(i, j - 1);
            }
            else if (computeFBackward(i - 1, j - 1) <= computeFBackward(i - 1, j) && computeFBackward(i - 1, j - 1) <= computeFBackward(i, j - 1)
                && computeFBackward(i - 1, j - 1) < double.PositiveInfinity)
            {
                f[i, j] = distance[i - 1, j - 1] + computeFBackward(i - 1, j - 1);
            }
        }
        return f[i, j];
    }

    public double computeDTW()
    {
        sum = computeFBackward(x.Length, y.Length);
        return sum;
        //sum = computeFForward();
    }

    public string ToString(ArrayList list)
    {
        string temp = "";
        foreach (string str in list)
        {
            temp += str + ", "; 
        }
        return temp;
    }

    void shift_and_fill(double[][] buffer, double[] input)
        //shifts the array one to left and adds the input to the end 
    {
        int length = windowSize;
            for (int k = 0; k < 3; k++){
            for (int l = 0; l < length-1; l++) {

                    buffer[k][l] = buffer[k][l+1];
            }
        }
        buffer[0][length-1] = input[0];
        buffer[1][length-1] = input[1];
        buffer[2][length-1] = input[2];
    }

}

       