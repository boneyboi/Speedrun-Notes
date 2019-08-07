using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using LiveSplit;
using LiveSplit.Model;
using static System.Collections.Specialized.BitVector32;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using SharpDX.XInput;
using SharpDX.Multimedia;
using SharpDX.DirectInput;
using SharpDX.RawInput;


namespace SpeedRunNotes
{

    public partial class Editor : Form
    {

        //countload helps make sure that the load function works correctly only once
        public int countload = 0;

        //timer refreshes the editor for controller input
        private Timer timer = new Timer();

        //livesplit state gets the list of splits and the current state of splitting 
        public LiveSplitState state { get; set; }

        //the splitslist and tempsplitlist variables track the current names of the splits,
        //so that if any names are changed no errors occur and the names are updated
        public Dictionary<string, MemoryStream> splitsList { get; set; }
        public Dictionary<string, MemoryStream> TempsplitsList { get; set; }


        //this keeps a list of buttons keyed by each splits name
        public Dictionary<string, System.Windows.Forms.Button> sheets { get; set; }
        //this keeps a list of pictureboxes keyed to each split
        public Dictionary<string, List<PictureBox>> sheetimgs;
        public Dictionary<PictureBox, string> imagepaths;

        //these variables help with the process of changing the names of splits here when the splits names are changed in the split editor
        public Dictionary<string, MemoryStream> splitsListCopy { get; set; }
        //temp variables are meant for keeping a temporary version of their original so that finalizations/cancelations can be made when clicking ok/cancel or exit
        public Dictionary<string, MemoryStream> TempsplitsListCopy { get; set; }

        //these variables keep the sheets and sheetimgs lists updated when they change
        public Dictionary<string, System.Windows.Forms.Button> sheetsCopy { get; set; }
        public Dictionary<string, List<PictureBox>> sheetimgsCopy;

        //currentsheet keeps track of the current sheet being updated
        public string currentSheet;

        //picboxes is the current images fromt he current sheet
        public List<PictureBox> picboxes;

        //mousedown keeps track of if the leftmouse pressed down or not
        public bool mousedown = false;

        //symbol mode tracks whether symbols should be displayed or not
        public bool symbolMode;
        //control pressed helps with hotkey detection by seeing if the control key is currently pressed
        public bool controlPressed;
        //caps helps reverting capslock when in symbol mode
        public bool caps;
        

        //these variables are for detecting controllers and their input (only works on xbox controllers)
        //gets the state of the controller and saves it
        public State prevgstate;
        //this is the actual controller
        public Controller controller;
        //this keeps a list of buttoins that are being pressed keyed to what the buttons name is
        public Dictionary<string, bool> buttonPressed;
        //this converts the button names into keyboard presses keyed by the name of the button
        public Dictionary<string, string> gametokey;
        //gets and saves the initial deadzones of the controller
        public short LDeadX;
        public short LDeadY;
        public short RDeadX;
        public short RDeadY;
        //radius of the deadzone
        public const short deadzone = 25000;
        //tracks the connection of the controller
        public bool connected;

        //a help form that displays the symbols and which key they are binded to
        public SymbolHelp shelpform;


        //constructor for the editor class
        public Editor(LiveSplitState state)
        {
            //if anything is left unitialized and then used later the component will not load

            //implement the help form
            shelpform = new SymbolHelp();

            
            //initialize the controller and detect the first controller
            controller = new Controller(UserIndex.One);

            //initalize the buttonPressed dicitionary
            buttonPressed = new Dictionary<string, bool>();
            //initialize all the keys of the buttons I want to listen to set their value to false ton detect them when they are pressed
            buttonPressed.Add("A", false);
            buttonPressed.Add("B", false);
            buttonPressed.Add("X", false);
            buttonPressed.Add("Y", false);
            buttonPressed.Add("DPadUp", false);
            buttonPressed.Add("DPadDown", false);
            buttonPressed.Add("DPadLeft", false);
            buttonPressed.Add("DPadRight", false);
            buttonPressed.Add("Back", false);
            buttonPressed.Add("Start", false);
            buttonPressed.Add("LStick", false);
            buttonPressed.Add("RStick", false);
            buttonPressed.Add("LeftThumb", false);
            buttonPressed.Add("RightThumb", false);
            buttonPressed.Add("LeftShoulder", false);
            buttonPressed.Add("RightShoulder", false);
            buttonPressed.Add("LUp", false);
            buttonPressed.Add("LDown", false);
            buttonPressed.Add("LLeft", false);
            buttonPressed.Add("LRight", false);
            buttonPressed.Add("RUp", false);
            buttonPressed.Add("RDown", false);
            buttonPressed.Add("RLeft", false);
            buttonPressed.Add("RRight", false);
            buttonPressed.Add("LeftTrigger", false);
            buttonPressed.Add("RightTrigger", false);


            //initialize the gametokey dictionary
            gametokey = new Dictionary<string, string>();
            //initialize all the button bindings by keying the button name and the value to the key supposed to be pressed
            gametokey.Add("A", "z");
            gametokey.Add("B", "x");
            gametokey.Add("X", "c");
            gametokey.Add("Y", "v");
            gametokey.Add("DPadUp", "y");
            gametokey.Add("DPadDown", "i");
            gametokey.Add("DPadLeft", "o");
            gametokey.Add("DPadRight", "u");
            gametokey.Add("Back", "n");
            gametokey.Add("Start", "b");
            gametokey.Add("LStick", "");
            gametokey.Add("RStick", "");
            gametokey.Add("LeftThumb", "m");
            gametokey.Add("RightThumb", "f");
            gametokey.Add("LeftShoulder", "q");
            gametokey.Add("RightShoulder", "e");
            gametokey.Add("LUp", "w");
            gametokey.Add("LDown", "s");
            gametokey.Add("LLeft", "a");
            gametokey.Add("LRight", "d");
            gametokey.Add("RUp", "g");
            gametokey.Add("RDown", "j");
            gametokey.Add("RLeft", "k");
            gametokey.Add("RRight", "h");
            gametokey.Add("LeftTrigger", "r");
            gametokey.Add("RightTrigger", "t");


            //if the controller is already connected then get it's state and set the deadzones
            if (controller.IsConnected)
            {
                prevgstate = controller.GetState();
                LDeadX = controller.GetState().Gamepad.LeftThumbX;
                LDeadY = controller.GetState().Gamepad.LeftThumbY;
                RDeadX = controller.GetState().Gamepad.RightThumbX;
                RDeadY = controller.GetState().Gamepad.RightThumbY;
                connected = true;
            }
            


            //initialize the splitslist
            this.splitsList = new Dictionary<string, MemoryStream>();
            //initialize the sheets
            this.sheets = new Dictionary<string, System.Windows.Forms.Button>();
            //initialize the sheetimgs
            sheetimgs = new Dictionary<string, List<PictureBox>>();
            imagepaths = new Dictionary<PictureBox, string>();

            //gets teh state passed in from the component class
            this.state = state;

            //starts the timer, so that updating can start
            timer.Start();
            timer.Tick += new EventHandler(timer_OnTick);

            //built-in method that initilaizes the form
            InitializeComponent();
            //built-in


            //add methods to the events of the editor's main rich text box
            richTextBox1.SelectionChanged += richTextBox1_OnSelectionChanged;
            richTextBox1.TextChanged += richTextBox1_OnTextChanged;

            //add methods to the events of the font change button
            FontColor.Click += FontColor_Click;
            FontChange.Click += FontChange_Click;

            //add methods to this form's event handlers to keep track of key presses for reasons outside of just text editing
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(keypressed);
            this.KeyUp += new KeyEventHandler(keyup);
            //add methods to the resizing event in order to scale the form when resized
            this.SizeChanged += Form_SizeChanged;

            //necessary event handler in order to load things that can't be here because the form hasn't loaded yet
            this.Load += editor_Load;
            //add a form close method to the event for this form to dispose of objects and handle some things
            this.FormClosed += Form_closed;

            //these variables determine the size and position of buttons
            int sheetW = 50;
            int pad = 0;
            int sheetH = 20;
            int posX = 0;
            int posY = 0;
            
            //get each split from the current run and initialize each button, image list, and memory stream keyed to each split
            foreach (Segment s in state.Run)
            {
                //this conditional just ensures that if the form is opened again nothing weird happens
                if (countload == 0)
                {
                    //initialize and save what is currently in the RTB to the memorystream
                    splitsList.Add(s.Name, new MemoryStream());
                    //always set the position of the memorystream to 0 before using it, or else it will not load or save information correctly
                    splitsList[s.Name].Position = 0;
                    //this method saves the information stored in the richtextbox into the memorystream
                    richTextBox1.SaveFile(splitsList[s.Name], RichTextBoxStreamType.RichText);

                    //ensures the currentsheet is set to the first split seen here
                    if (posX == 0 && countload == 0)
                    {
                        currentSheet = s.Name;
                    }

                    //initialize the button for this split and give it it's attributes based off how many splits the loop has gone through
                    sheets.Add(s.Name, new System.Windows.Forms.Button());
                    sheets[s.Name].Name = s.Name;
                    sheets[s.Name].Text = s.Name;
                    sheets[s.Name].Location = new System.Drawing.Point(posX, posY);
                    sheets[s.Name].Size = new Size(sheetW, sheetH);
                    sheets[s.Name].Visible = true;
                    sheets[s.Name].Click += Sheet_OnClick;
                    //adds the element to the editor and puts it into panel 1
                    sheets[s.Name].Parent = panel1;
                    sheets[s.Name].Show();

                    posX += sheetW + pad;
                    //initializes a list of picture boxes for this split
                    sheetimgs.Add(s.Name, new List<PictureBox>());
                }
            }
            //change the countload to make sure the condition becomes false
            countload++;

            //these variables help with scaling the form's controls when the form is resized
            richtextpadX = (this.Width - this.richTextBox1.Width);
            richtextpadY = (this.Height - this.richTextBox1.Height);
            panelpad = (this.Width - this.panel1.Width);
            originalY = this.Height;
            originalX = this.Width;

            //initialize the temporary tempsplits list and then copy the splitslist memory streams to them
            TempsplitsList = new Dictionary<string, MemoryStream>();
            foreach (KeyValuePair<String, MemoryStream> pair in splitsList)
            {
                MemoryStream tempmem = new MemoryStream();
                tempmem.Position = 0;
                pair.Value.Position = 0;
                pair.Value.CopyTo(tempmem);
                TempsplitsList.Add(pair.Key, tempmem);
            }


            //initializecopies for splitslisttemp and splitslist
            splitsListCopy = new Dictionary<string, MemoryStream>();
            TempsplitsListCopy = new Dictionary<string, MemoryStream>();

            //initialize copies of shets and sheetimgs
            sheetsCopy = new Dictionary<string, System.Windows.Forms.Button>();
            sheetimgsCopy = new Dictionary<string, List<PictureBox>>();

            //set symbolmode and caps to false since neither should be on
            symbolMode = false;
            //TODO: make caps match the current state of caps lock rather than assume
            caps = false;
        }

        

        //the load method that gets called when the form is open and loads
        public void editor_Load(Object senderO, EventArgs e)
        {
            //check if the controller is connected and get it's state and set the deadzones
            if (controller.IsConnected && !connected)
            {
                prevgstate = controller.GetState();
                LDeadX = controller.GetState().Gamepad.LeftThumbX;
                LDeadY = controller.GetState().Gamepad.LeftThumbY;
                RDeadX = controller.GetState().Gamepad.RightThumbX;
                RDeadY = controller.GetState().Gamepad.RightThumbY;
                connected = true;
            }

            

            
            

            //keep track of which split list keys for later use
            int ind = 0;
            List<string> tempNames = splitsList.Keys.ToList();



            ////////this section of code copies the lists information into their respective copies and then///////
            ////////reloads the copies information into the original lists under the current splits' names////////

            //copy the original lists' info to their copies
            sheetimgsCopy.Clear();
            foreach (Segment s in state.Run)
            {
                //if there are existing splitslist and templist memorystreams, then copy them to the copies, if not: create new memorystreams/buttons/picturebox list
                splitsListCopy.Add(s.Name, splitsList[tempNames[ind]] != null ? splitsList[tempNames[ind]] : new MemoryStream());
                TempsplitsListCopy.Add(s.Name, TempsplitsList[tempNames[ind]] != null ? TempsplitsList[tempNames[ind]] : new MemoryStream());
                sheetsCopy.Add(s.Name, sheets[tempNames[ind]]);
                sheetimgsCopy.Add(s.Name, new List<PictureBox>());
                if(sheetimgs[tempNames[ind]] != null)
                {
                    if(sheetimgs[tempNames[ind]].Count > 0)
                    {
                        foreach(PictureBox pbox in sheetimgs[tempNames[ind]])
                        {
                            PictureBox tempbox = pbox;
                            sheetimgsCopy[s.Name].Add(tempbox);
                        }
                    }
                }
                ind++;
            }
            //reset the ind to 0 for next time
            ind = 0;

            //clear the lists so that the copies can load the information back in correctly
            splitsList.Clear();
            TempsplitsList.Clear();
            sheets.Clear();
            sheetimgs.Clear();

            //reload the copied information under the current names of the splits
            foreach (Segment s in state.Run)
            {
                splitsList.Add(s.Name, splitsListCopy[s.Name]);
                TempsplitsList.Add(s.Name, TempsplitsListCopy[s.Name]);
                sheets.Add(s.Name, sheetsCopy[s.Name]);
                sheets[s.Name].Name = s.Name;
                sheets[s.Name].Text = s.Name;
                sheetimgs.Add(s.Name, new List<PictureBox>());
                foreach (PictureBox pbox in sheetimgsCopy[s.Name])
                {
                    sheetimgs[s.Name].Add(pbox);
                    //pbox.Dispose();
                }
                ind++;
            }

            //clear the copies so that they can be used properly next time
            splitsListCopy.Clear();
            TempsplitsListCopy.Clear();
            sheetsCopy.Clear();
            /*
            foreach(string sheet in sheetimgsCopy.Keys)
            {
                foreach(PictureBox pbox in sheetimgsCopy[sheet])
                {
                    sheetimgsCopy[sheet].Remove(pbox);
                }
            }
            */
            //sheetimgsCopy.Clear();

            //set the default colors for the color button and the colordialog
            FColor.Color = defaultColor;
            FontColor.BackColor = FColor.Color;
            //set the default fonts from the font dialog and the RTB
            defaultFont = richTextBox1.Font;
            tempFont = defaultFont;


            //load the first split and its memorystream into the RTB
            int c = 0;
            foreach (Segment s in state.Run)
            {
                if (c > 0) { break; }
                else
                {
                    currentSheet = s.Name;
                    splitsList[s.Name].Position = 0;
                    richTextBox1.LoadFile(splitsList[s.Name], RichTextBoxStreamType.RichText);
                    sheets[currentSheet].BackColor = System.Drawing.Color.Gray;
                    if (sheetimgs[currentSheet].Count > 0)
                    {
                        foreach (PictureBox pbox in sheetimgs[currentSheet])
                        {
                            pbox.Visible = true;
                        }
                    }
                }
                c++;
            }
            
            
        }
        
        //method that handles the form close event
        private void Form_closed(object sender, FormClosedEventArgs e)
        {
            //set the images of the current sheet to be invisible, so that the first sheet shows correctly when reloaded
            if (sheetimgs[currentSheet].Count > 0)
            {
                foreach (PictureBox pbox in sheetimgs[currentSheet])
                {
                    pbox.Visible = false;
                }
            }
            //unselects the currentsheet button
            sheets[currentSheet].BackColor = SystemColors.Control;
            if (symbolMode)
            {
                symbolmodechanged();
            }
        }


        //method for when the time ticks
        public void timer_OnTick(Object sender, EventArgs e)
        {
            //whenever the timer ticks, update
            update();
        }

        private void update()
        {
            //always check the connection status of the controller
            if (controller.IsConnected && !connected)
            {
                prevgstate = controller.GetState();
                LDeadX = controller.GetState().Gamepad.LeftThumbX;
                LDeadY = controller.GetState().Gamepad.LeftThumbY;
                RDeadX = controller.GetState().Gamepad.RightThumbX;
                RDeadY = controller.GetState().Gamepad.RightThumbY;
                connected = true;
            }

            //check for a disconnection
            if (!controller.IsConnected)
            {
                connected = false;
            }


            //update the controller if it's state has changed
            if (connected)
            {
                if (!prevgstate.Equals(controller.GetState()))
                {
                    //get the buttons being pressed
                    string buttons = "" + controller.GetState().Gamepad.Buttons;
                    List<string> buttonlist = new List<string>();

                    //unpack the string returned by gamepad.buttons
                    do
                    {
                        if (buttons.Contains(","))
                        {
                            buttonlist.Add(buttons.Substring(0, buttons.IndexOf(",")));
                            buttons = buttons.Substring(buttons.IndexOf(" ") + 1);
                        }
                        if (!buttons.Contains(","))
                        {
                            buttonlist.Add(buttons);
                        }
                    } while (buttons.Contains(","));

                    //print out or do things based on what buttons were pressed
                    if (buttonlist[0] != "None")
                    {
                        foreach (string s in buttonlist)
                        {
                            GamePadPress(controller, s);
                        }
                    }

                    //initialize a new list that ikeeps track of which buttons are pressed
                    //this list is for handling non-analog input
                    Dictionary<string, bool> buttonPressedtemp = new Dictionary<string, bool>();

                    //copy the keys from the buttonpressed keys up to the analog inputs, setting their value to false
                    foreach (string s in buttonPressed.Keys)
                    {
                        buttonPressedtemp.Add(s, false);
                        if (s.Equals("RightShoulder")) { break; }
                    }
                    //if a button is currently pressed then set its press value to true
                    foreach (string s in buttonPressed.Keys)
                    {
                        if (buttonlist.Contains(s))
                        {
                            buttonPressedtemp[s] = true;
                        }
                        if (s.Equals("RightShoulder")) { break; }
                    }
                    //set the original list values to the temp values
                    foreach (string s in buttonPressedtemp.Keys)
                    {
                        buttonPressed[s] = buttonPressedtemp[s];
                        if (s.Equals("RightShoulder")) { break; }
                    }


                    //for analog stick input
                    //doing Math.Sqrt(Math.Pow((double)(- LDeadX + controller.GetState().Gamepad.LeftThumbX), 2)) >= deadzone ensures a circular radius is checked rather than a box
                    //and it also means I can tell which direction is being held by getting the polarity of the current position of the stick
                    if (Math.Sqrt(Math.Pow((double)(- LDeadX + controller.GetState().Gamepad.LeftThumbX), 2)) >= deadzone && controller.GetState().Gamepad.LeftThumbX >= 0)
                    {
                        GamePadPress(controller, "LRight");
                        buttonPressed["LUp"] = false;
                        buttonPressed["LDown"] = false;
                        buttonPressed["LLeft"] = false;
                    } else if(Math.Sqrt(Math.Pow((double)(- LDeadX + controller.GetState().Gamepad.LeftThumbX), 2)) >= deadzone && controller.GetState().Gamepad.LeftThumbX < 0)
                    {
                        GamePadPress(controller, "LLeft");
                        buttonPressed["LUp"] = false;
                        buttonPressed["LDown"] = false;
                        buttonPressed["LRight"] = false;
                    } else if(Math.Sqrt(Math.Pow((double)(- LDeadY + controller.GetState().Gamepad.LeftThumbY), 2)) >= deadzone && controller.GetState().Gamepad.LeftThumbY >= 0)
                    {
                        GamePadPress(controller, "LUp");
                        buttonPressed["LDown"] = false;
                        buttonPressed["LLeft"] = false;
                        buttonPressed["LRight"] = false;
                    } else if (Math.Sqrt(Math.Pow((double)(- LDeadY + controller.GetState().Gamepad.LeftThumbY), 2)) >= deadzone && controller.GetState().Gamepad.LeftThumbY < 0)
                    {
                        GamePadPress(controller, "LDown");
                        buttonPressed["LUp"] = false;
                        buttonPressed["LLeft"] = false;
                        buttonPressed["LRight"] = false;
                    } else
                    {
                        buttonPressed["LUp"] = false;
                        buttonPressed["LDown"] = false;
                        buttonPressed["LLeft"] = false;
                        buttonPressed["LRight"] = false;
                    }

                    if (Math.Sqrt(Math.Pow((double)(-RDeadX + controller.GetState().Gamepad.RightThumbX), 2)) >= deadzone && controller.GetState().Gamepad.RightThumbX >= 0)
                    {
                        GamePadPress(controller, "RRight");
                        buttonPressed["RUp"] = false;
                        buttonPressed["RDown"] = false;
                        buttonPressed["RLeft"] = false;
                    }
                    else if (Math.Sqrt(Math.Pow((double)(-RDeadX + controller.GetState().Gamepad.RightThumbX), 2)) >= deadzone && controller.GetState().Gamepad.RightThumbX < 0)
                    {
                        GamePadPress(controller, "RLeft");
                        buttonPressed["RUp"] = false;
                        buttonPressed["RDown"] = false;
                        buttonPressed["RRight"] = false;
                    }
                    else if (Math.Sqrt(Math.Pow((double)(-RDeadY + controller.GetState().Gamepad.RightThumbY), 2)) >= deadzone && controller.GetState().Gamepad.RightThumbY >= 0)
                    {
                        GamePadPress(controller, "RUp");
                        buttonPressed["RDown"] = false;
                        buttonPressed["RLeft"] = false;
                        buttonPressed["RRight"] = false;
                    }
                    else if (Math.Sqrt(Math.Pow((double)(-RDeadY + controller.GetState().Gamepad.RightThumbY), 2)) >= deadzone && controller.GetState().Gamepad.RightThumbY < 0)
                    {
                        GamePadPress(controller, "RDown");
                        buttonPressed["RUp"] = false;
                        buttonPressed["RLeft"] = false;
                        buttonPressed["RRight"] = false;
                    }
                    else
                    {
                        buttonPressed["RUp"] = false;
                        buttonPressed["RDown"] = false;
                        buttonPressed["RLeft"] = false;
                        buttonPressed["RRight"] = false;
                    }

                    //get the input from the analog triggers
                    if (controller.GetState().Gamepad.LeftTrigger > 50)
                    {
                        GamePadPress(controller, "LeftTrigger");
                    } else
                    {
                        buttonPressed["LeftTrigger"] = false;
                    }
                    if (controller.GetState().Gamepad.RightTrigger > 50)
                    {
                        GamePadPress(controller, "RightTrigger");
                    }
                    else
                    {
                        buttonPressed["RightTrigger"] = false;
                    }


                    //update the state
                    prevgstate = controller.GetState();
                }
            }

            //update the color of the font and the variable that saves that
            if (this.Visible)
            {
                if (tempColor != richTextBox1.SelectionColor)
                {
                    tempColor = richTextBox1.SelectionColor;
                    FontColor.BackColor = tempColor;
                }
            }
            

            //resets this variable for use again when the next update comes around
            caps = false;

            
        }

        //this function handles the conversion of game pad button presses into key presses
        private void GamePadPress(Controller g, String name)
        {
            //TODO: if other controllers end up being added, handle different keybpoard settings and symbol settings here and then enter the correct symbol
            if (!buttonPressed[name])
            {
                SendKeys.Send(gametokey[name]);
                buttonPressed[name] = true;
            }

        }


        //these variables help with scaling and resizing when the form is resized
        private int richtextpadX;
        private int richtextpadY;
        private int panelpad;
        private int originalY;
        private int originalX;
        //method that handles the form resized event
        private void Form_SizeChanged(object sender, EventArgs e)
        {
            Form f = sender as Form;
            //this condition handles changes in the width of the form
            if (f.Size.Width > richTextBox1.Size.Width || f.Size.Width < richTextBox1.Size.Width)
            {
                richTextBox1.Width = f.Width - richtextpadX;
                panel1.Width = f.Width - panelpad;
                OKbutton.Location = new System.Drawing.Point(OKbutton.Location.X - (originalX - f.Width), OKbutton.Location.Y);
                CancelButton.Location = new System.Drawing.Point(CancelButton.Location.X - (originalX - f.Width), CancelButton.Location.Y);
                originalX = f.Width;
            }
            //this condition handles changes in the height of the form
            if (f.Size.Height > originalY || f.Size.Height < originalY)
            {
                richTextBox1.Height = f.Height - richtextpadY;
                panel1.Location = new System.Drawing.Point(panel1.Location.X, panel1.Location.Y - (originalY - f.Height));
                OKbutton.Location = new System.Drawing.Point(OKbutton.Location.X, OKbutton.Location.Y - (originalY - f.Height));
                CancelButton.Location = new System.Drawing.Point(CancelButton.Location.X, CancelButton.Location.Y - (originalY - f.Height));
                originalY = f.Height;
            }

        }

        //these variables keep track of the color of the font, color dialog, and color button
        private System.Drawing.Color defaultColor = System.Drawing.Color.Black;
        private System.Drawing.Color tempColor = System.Drawing.Color.Red;
        private string tempSelection;

        //method that handles the color button being clicked by opening a color dialog and switchuing the selected font color to the one chosen or setting the current font color to the color chosen
        private void FontColor_Click(object sender, EventArgs e)
        {
            if (FColor.ShowDialog() == DialogResult.OK)
            {
                if (tempSelection != null)
                {
                    if (tempSelection.Equals(""))
                    {
                        FontColor.BackColor = FColor.Color;
                        defaultColor = FColor.Color;
                    }
                    else
                    {
                        tempColor = FColor.Color;
                        richTextBox1.SelectionColor = FColor.Color;
                        FontColor.BackColor = defaultColor;
                    }

                    tempColor = defaultColor;
                    FColor.Color = defaultColor;
                }
                else
                {
                    FontColor.BackColor = FColor.Color;
                    defaultColor = FColor.Color;
                    richTextBox1.SelectionColor = defaultColor;
                }
            }
        }


        //variables that track and determine the font family and size of the selected text
        private static float currentSize = 12;
        private static FontStyle currentStyle = new FontStyle();
        private Font defaultFont = new System.Drawing.Font("Times New Roman", currentSize, currentStyle);
        private Font tempFont = new System.Drawing.Font("Times New Roman", currentSize, currentStyle);

        //method that handles the clicking of the font change button by opening a font dialog that allows the user to change the
        //selected font type to the one chosen or set the default to the chosen font
        private void FontChange_Click(object sender, EventArgs e)
        {
            if (FFont.ShowDialog() == DialogResult.OK)
            {
                if (tempSelection != null)
                {
                    if (tempSelection.Equals(""))
                    {
                        defaultFont = FFont.Font;
                    }
                    else
                    {
                        tempFont = FFont.Font;
                        richTextBox1.SelectionFont = FFont.Font;
                    }

                    tempFont = defaultFont;
                    FFont.Font = defaultFont;
                }
                else
                {
                    defaultFont = FFont.Font;
                    richTextBox1.SelectionFont = defaultFont;
                }
                if (symbolMode)
                {
                    symbolmodechanged();
                }
            }
        }

        //handles when a sheet button is clicked
        //changes the current sheet loaded into the richtextbox to the one clicked
        //changes the currentsheet to the one clicked
        //highlights the currentsheet button
        //displays the image associated with the sheet chosen
        //hides the images that were on the previously shown sheet
        public void Sheet_OnClick(object sender, EventArgs e)
        {
            TempsplitsList[currentSheet].Position = 0;
            splitsList[currentSheet].Position = 0;
            TempsplitsList[currentSheet].CopyTo(splitsList[currentSheet]);
            Font tempselectfont = richTextBox1.SelectionFont;
            System.Windows.Forms.Button tempb = (System.Windows.Forms.Button)sender;
            currentSheet = tempb.Name;
            splitsList[tempb.Name].Position = 0;
            richTextBox1.LoadFile(splitsList[tempb.Name], RichTextBoxStreamType.RichText);
            richTextBox1.SelectionFont = tempselectfont;
            foreach (System.Windows.Forms.Button b in sheets.Values)
            {
                if (b.Name.Equals(currentSheet))
                {
                    b.BackColor = System.Drawing.Color.Gray;
                }
                else
                {
                    b.BackColor = SystemColors.Control;
                }
            }


            foreach (string sheet in sheetimgs.Keys)
            {
                if (sheet.Equals(currentSheet))
                {
                    if (sheetimgs[sheet].Count > 0)
                    {
                        foreach (PictureBox pbox in sheetimgs[sheet])
                        {
                            pbox.Visible = true;
                        }
                    }
                }
                else
                {
                    if (sheetimgs[sheet].Count > 0)
                    {
                        foreach (PictureBox pbox in sheetimgs[sheet])
                        {
                            pbox.Visible = false;
                        }
                    }
                }
            }

        }

        //this methiod handles the text of the main RTB changing by saving the changes to a splitslist memorystream
        public void richTextBox1_OnTextChanged(object sender, EventArgs e)
        {
            TempsplitsList[currentSheet].Position = 0;
            richTextBox1.SaveFile(TempsplitsList[currentSheet], RichTextBoxStreamType.RichText);
        }
        

        private void richTextBox1_OnSelectionChanged(object sender, EventArgs e)
        {
            if (!richTextBox1.SelectedText.Equals(""))
            {
                tempSelection = richTextBox1.SelectedText;
            }
        }


        private void InsertButton_Click(object sender, EventArgs e)
        {

            if (ImageInsert.ShowDialog() == DialogResult.OK)
            {
                ImageInsert.Multiselect = false;
                sheetimgs[currentSheet].Add(new PictureBox());
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].Parent = richTextBox1;
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].BackColor = System.Drawing.Color.Black;


                var imgstream = File.OpenRead(ImageInsert.FileName);
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].Image = Image.FromStream(imgstream);
                imgstream.Close();
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].Visible = true;
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].SizeMode = PictureBoxSizeMode.StretchImage;
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].MouseDown += picbox_OnMouseDown;
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].MouseMove += picbox_OnMouseMove;
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].MouseUp += picbox_OnMouseUp;
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].MouseEnter += new EventHandler(picbox_OnMouseEnter);
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].MouseLeave += new EventHandler(picbox_OnMouseLeave);
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].Name = ImageInsert.FileName;
                sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1].BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);

                imagepaths.Add(sheetimgs[currentSheet][sheetimgs[currentSheet].Count - 1], ImageInsert.FileName);
            }
        }





        //variables that help with resizing and moving the form
        //private bool mousedown = false;
        private bool resizingH = false;
        private bool resizingV = false;
        private bool resizingHV = false;
        private int px, py;
        private int grip = 3;
        public void picbox_OnMouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            PictureBox tempbox = sender as PictureBox;
            if (e.Button == MouseButtons.Left)
            {
                mousedown = true;
                px = e.X;
                py = e.Y;
            }
            if ((e.Y >= tempbox.Height - grip * 2 && e.X >= tempbox.Width - grip * 2))
            {
                this.resizingHV = true;
            }
            else if (e.Y >= tempbox.Height - grip)
            {
                this.resizingV = true;
            }
            else if (e.X >= tempbox.Width - grip)
            {
                this.resizingH = true;
            }
        }
        public void picbox_OnMouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            PictureBox tempbox = sender as PictureBox;
            //e.Y is the cursors position within the form
            if ((e.Y >= tempbox.Height - grip * 2 && e.X >= tempbox.Width - grip * 2))
            {
                this.Cursor = Cursors.SizeNWSE;
            }
            else if (e.Y >= tempbox.Height - grip)
            {
                this.Cursor = Cursors.SizeNS;
            }
            else if (e.X >= tempbox.Width - grip)
            {
                this.Cursor = Cursors.SizeWE;
            }
            else
            {
                this.Cursor = Cursors.SizeAll;
            }
            //when the user has the mouse pressed
            if (mousedown)
            {
                int x1 = e.X, y1 = e.Y;
                if (!resizingHV && !resizingV && !resizingH)
                {
                    int dx = tempbox.Location.X + (x1 - px);
                    int dy = tempbox.Location.Y + (y1 - py);
                    tempbox.Location = new System.Drawing.Point(dx, dy);
                }
                else if (resizingHV)
                {
                    tempbox.Height += (y1 - py);
                    tempbox.Width += (x1 - px);
                    px = x1;
                    py = y1;
                }
                else if (resizingV)
                {
                    tempbox.Height += (y1 - py);
                    py = y1;
                }
                else if (resizingH)
                {
                    tempbox.Width += (x1 - px);
                    px = x1;
                }

            }

        }


        //these variables and methods serve to delete images when the mouse is over them, or resize them when the mouse is over the grip
        public bool mousehoverimg = false;
        public PictureBox currentimg;
        public void picbox_OnMouseEnter(object sender, EventArgs e)
        {
            currentimg = sender as PictureBox;
            mousehoverimg = true;
        }
        public void picbox_OnMouseLeave(object sender, EventArgs e)
        {
            mousehoverimg = false;
            currentimg = null;
            this.Cursor = Cursors.Default;
        }
        public void picbox_OnMouseUp(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mousedown = false;
            this.resizingHV = false;
            this.resizingH = false;
            this.resizingV = false;
        }

        
        //this method handles any keypresses
        private void keypressed(Object sender, KeyEventArgs e)
        {
            
            //this keeps a record of if the control key is pressed for hotkey purposes
            if (e.KeyCode == Keys.ControlKey)
            {
                controlPressed = true;
            }
            if (e.KeyCode == Keys.T && controlPressed)
            {
                //hotkey toggle the symbol mode
                symbolmodechanged();
            }

            //deletes the image the mouse is hovered hover if esc is pressed
            if (mousehoverimg)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    foreach (PictureBox temppic in sheetimgs[currentSheet])
                    {
                        if (temppic.Equals(currentimg))
                        {
                            imagepaths.Remove(temppic);
                            temppic.Hide();
                            sheetimgs[currentSheet].Remove(temppic);
                            sheetimgsCopy[currentSheet].Remove(temppic);
                            if (sheetimgs[currentSheet].Count == 0)
                            {
                                sheetimgs[currentSheet].Clear();
                                sheetimgsCopy[currentSheet].Clear();
                            }
                            temppic.Dispose();

                            //sheetimgs[currentSheet][sheetimgs[currentSheet].IndexOf(temppic)].Dispose();
                            /*
                            sheetimgsCopy[currentSheet][sheetimgsCopy[currentSheet].IndexOf(temppic)].Dispose();
                            sheetimgsCopy[currentSheet].Remove(temppic);
                            temppic.Dispose();
                            */
                            currentimg.Dispose();
                            currentimg = null;
                            e.Handled = true;
                            break;
                        }
                    }
                }
            }

            //converts keypresses to the corresponding symbol when in symbol mode
            if (symbolMode)
            {
                //if any special keys are pressed: add their charachter
                //if any keys with values are pressed: add the charachter
                //if other keys are pressed: treat them as handled and do nothing (maybe leave some keys open like numbers)
                if (e.KeyCode == Keys.Left)
                {
                    e.Handled = true;
                    richTextBox1.SelectedText += "a";
                }
                else if (e.KeyCode == Keys.Up)
                {
                    e.Handled = true;
                    richTextBox1.SelectedText += "w";
                }
                else if (e.KeyCode == Keys.Right)
                {
                    e.Handled = true;
                    richTextBox1.SelectedText += "d";
                }
                else if (e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                    richTextBox1.SelectedText += "s";
                }
                else if (e.KeyCode == Keys.Alt)
                {
                    e.Handled = true;
                    richTextBox1.SelectedText += "b";
                }
                else if (e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                    richTextBox1.SelectedText += "c";
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    e.Handled = true;
                    SendKeys.Send("f");
                }
                else if (e.KeyCode == Keys.CapsLock)
                {
                    e.Handled = true;
                    if (!caps)
                    {
                        SendKeys.Send("{CAPSLOCK}");
                        richTextBox1.SelectedText += "g";
                        caps = true;
                    }
                }
                else if (e.KeyCode == Keys.ControlKey)
                {
                    e.Handled = true;
                    richTextBox1.SelectedText += "m";
                }
                else if (e.KeyCode == Keys.ShiftKey)
                {
                    e.Handled = true;
                    richTextBox1.SelectedText += "n";
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    e.Handled = true;
                    richTextBox1.SelectedText += "v";
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    richTextBox1.SelectedText += "x";
                }
                else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin)
                {
                    e.Handled = true;
                    SendKeys.Send("s");
                }
                else if (!(e.KeyCode == Keys.A ||
                    e.KeyCode == Keys.B ||
                    e.KeyCode == Keys.C ||
                    e.KeyCode == Keys.D ||
                    e.KeyCode == Keys.F ||
                    e.KeyCode == Keys.G ||
                    e.KeyCode == Keys.M ||
                    e.KeyCode == Keys.N ||
                    e.KeyCode == Keys.S ||
                    e.KeyCode == Keys.V ||
                    e.KeyCode == Keys.W ||
                    e.KeyCode == Keys.X ||
                    e.KeyCode == Keys.Z))
                {
                    e.Handled = true;
                }
            }
        }
        //keeps track of the control key for hotkey purposes
        private void keyup(object sender, KeyEventArgs e)
        {
            if (symbolMode)
            {
                if (e.KeyCode == Keys.ControlKey)
                {
                    controlPressed = false;
                }
            }
        }

        //finalizes the temporary changes by saving temp variables to the originals
        private void OKButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, MemoryStream> pair in TempsplitsList)
            {
                TempsplitsList[pair.Key].Position = 0;
                splitsList[pair.Key].Position = 0;
                TempsplitsList[pair.Key].CopyTo(splitsList[pair.Key]);
            }
            if (symbolMode)
            {
                symbolmodechanged();
            }
            this.Close();
        }

        //doesn't save any changes
        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (symbolMode)
            {
                symbolmodechanged();
            }
            this.Close();
        }


        //handles the suymbol change button click by changing the symbol mode
        private void SymbolButton_Click(object sender, EventArgs e)
        {
            symbolmodechanged();
        }

        //displays a help form that shows the symbols and their corresponding keys
        private void SymbolHelp_Click(object sender, EventArgs e)
        {
            //show new form lol
            shelpform = new SymbolHelp();
            shelpform.Show();
        }

        
        //changes the symbol mode and some UI changes that indicate that
        public void symbolmodechanged()
        {
            symbolMode = !symbolMode;
            if (symbolMode)
            {
                SymbolButton.Text = "Turn Off Symbols";
                if(controllerType.Text.Equals("Xbox") || controllerType.Text.Equals("Keyboard") || controllerType.Text.Equals("Nintendo") || controllerType.Text.Equals("Playstation"))
                {
                    richTextBox1.SelectionFont = new Font(controllerType.Text, richTextBox1.SelectionFont.Size);
                } else
                {
                    richTextBox1.SelectionFont = new Font("Keyboard", richTextBox1.SelectionFont.Size);
                }
                controllerType.Enabled = true;
            }
            else
            {
                SymbolButton.Text = "Turn On Symbols";
                richTextBox1.SelectionFont = tempFont;
                controllerType.Enabled = false;
            }
        }

        //handles when the combo box has it's value changed and makes the font family reflect which controller type is chosen
        private void controllerTypeChanged(object sender, EventArgs e)
        {
            if (controllerType.Text.Equals("Xbox") || controllerType.Text.Equals("Keyboard") || controllerType.Text.Equals("Nintendo") || controllerType.Text.Equals("Playstation"))
            {
                richTextBox1.SelectionFont = new Font(controllerType.Text, 24);
            }
        }
    }
}

