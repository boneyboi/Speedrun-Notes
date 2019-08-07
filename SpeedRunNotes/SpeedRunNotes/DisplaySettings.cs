using System;
using System.Drawing;
using System.Windows.Forms;
using SpeedRunNotes;
using System.Xml;
using LiveSplit.Model;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace LiveSplit.UI.Components
{
    public partial class DisplaySettings : UserControl
    {
        //these variables are variables to be saved
        public Color DisColor { get; set; }
        public int H { get; set; }
        public int W { get; set; }
        public string cachePath { get; set; }
        public int count { get; set; }

        //need to get the current editor from the component class
        public Editor editor;
        //need to get the current state from the component class
        public LiveSplitState state { get; set; }
        

        //constructor for the displaysettings class
        public DisplaySettings(LiveSplitState state, Editor editor)
        {
            //initializes the user control
            InitializeComponent();

            //initialize values
            this.state = state;
            this.editor = editor; 
            //get the values from the user controls and save them
            DisColor = Color.FromName(SpeedRunNotesComponent.display.BackColor.Name);
            H = SpeedRunNotesComponent.display.Height;
            W = SpeedRunNotesComponent.display.Width;
            

            //add databindings

            //checkBox1.DataBindings.Add(new Binding("Checked", this, "check1", false, DataSourceUpdateMode.OnPropertyChanged));
            //^"object name".-----------------------^"property name", "dataset", "data memeber" -----(the last 2 are always the same, I believe it hel;ps with updating livesplit)
            //property name: just whatever property the object has you want to be binded
            //dataset is always this because these settings need the value
            //data member: the name of the variable being changed
            button1.DataBindings.Add(new Binding("BackColor", this, "DisColor"));
            THeight.DataBindings.Add(new Binding("Text", this, "H"));
            TWidth.DataBindings.Add(new Binding("Text", this, "W"));
            
            //event handlers
            //add each funciton to the event itself
            //checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            button1.BackColorChanged += button1_BackColorchanged;
            button1.Click += button1_Click;
            THeight.TextChanged += THeight_TextChanged;
            TWidth.TextChanged += TWidth_TextChanged;
            Export.Click += Export_Click;
            Import.Click += Import_Click;
            

            
            this.Load += settings_load;

            count = 0;
        }



        //keeps the setting of the inputs saved by reading them from the controls
        private void settings_load(object sender, EventArgs e)
        {
            button1.BackColor = SpeedRunNotesComponent.display.BackColor;
            THeight.Text = SpeedRunNotesComponent.display.Height.ToString();
            TWidth.Text = SpeedRunNotesComponent.display.Width.ToString();

            //checkBox1.Checked = check1;
            //--^ input value ---^ variable value linked to the input
        }


        public XmlNode CreateSettingsNode(XmlDocument xdoc, XmlElement parent)
        {
            return null;
        }

        //gets teh current variables states, puts them in an xml doc and sends it to livesplit to save in the layout file
        public XmlNode GetSettings(XmlDocument document)
        {
            //this adds the node of settings.cs to the document passed in
            //make a parent with the document created element of "Settings" and add nodes to that through AppendChild
            var parent = document.CreateElement("Settings");

            //title!
            //----------------------------------------| this arg must be this.ToString()
            //document.CreateElement(this.ToString()).InnerText = "Speedrun Notes";
            var Element = document.CreateElement(this.ToString());
            Element.InnerText = "Speedrun Notes";
            parent.AppendChild(Element);
            //elements!
            // count the num of elements and input on the settings page, that aswsumption is correct they must be equal
            //document.CreateElement("Checked").InnerText = check1.ToString();
            //------------------------^ Element name ----------^ variable value for the element
            //Wrong lol
            //You have to create an element (like Element), then change its value trhough innertext, and apppend it as a child to the parent
            Element = document.CreateElement("BackColor");
            Element.InnerText = button1.BackColor.Name;
            parent.AppendChild(Element);
            Element = document.CreateElement("Height");
            Element.InnerText = THeight.Text.ToString();
            parent.AppendChild(Element);
            Element = document.CreateElement("Width");
            Element.InnerText = TWidth.Text.ToString();
            parent.AppendChild(Element);
            Element = document.CreateElement("Button");
            Element.InnerText = button2.ToString();
            parent.AppendChild(Element);
            Element = document.CreateElement("Path");
            Element.InnerText = cachePath;
            parent.AppendChild(Element);
            //returns that node that was just created
            return parent;
        }


        //gets the settings from livesplit and sets teh variables here to the values given
        public void SetSettings(XmlNode settings)
        {
            //the settings node is the same as one created in GetSettings
            //The main point of this funciton is to check for changes and then assign those changes to the variables the elements are connected to
            if (settings["BackColor"].InnerText != null)
            {
                DisColor = Color.FromName((settings["BackColor"].InnerText));
                SpeedRunNotesComponent.display.BackColor = DisColor;
            }
            if (settings["Height"] != null)
            {
                H = int.Parse(settings["Height"].InnerText);
                THeight.Text = H.ToString();
                SpeedRunNotesComponent.display.Height = H;
            }
            if (settings["Width"] != null)
            {
                W = int.Parse(settings["Width"].InnerText);
                TWidth.Text = W.ToString();
                SpeedRunNotesComponent.display.Width = W;
            }
            if(settings["Path"] != null && count < 1)
            {
                cachePath = settings["Path"].InnerText;
                importNotes(cachePath);
            }
            count = 1;
        }
        

        //change teh color of the background of the display
        private void button1_Click(object sender, EventArgs e)
        {
            if (BGcolor.ShowDialog() == DialogResult.OK)
            {
                DisColor = BGcolor.Color;
                button1.BackColor = BGcolor.Color;
            }
        }

        //save the value of the color when teh display's color is changed
        private void button1_BackColorchanged(object sender, EventArgs e)
        {
            DisColor = button1.BackColor;
            SpeedRunNotesComponent.display.BackColor = DisColor;
        }

        //changes the height of the display
        private void THeight_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(THeight.Text, out int h))
            {
                H = int.Parse(THeight.Text);
            }
            else if(!THeight.Text.Equals(""))
            {
                THeight.Text = H.ToString();
            }
            SpeedRunNotesComponent.display.Height = H;
        }
        //changes the width of the display
        private void TWidth_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(TWidth.Text, out int w))
            {
                W = int.Parse(TWidth.Text);
            }
            else if(!TWidth.Text.Equals(""))
            {
                TWidth.Text = W.ToString();
            }
            SpeedRunNotesComponent.display.Width = W;
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        //open the note editor
        private void button2_Click(object sender, EventArgs e)
        {
            // this code opens the editor form without creating a new application in the thread
            editor.ShowDialog();
        }
        

        //initiate exporting of the notes
        private void Export_Click(object sender, EventArgs e)
        {
            //show the folder dialog
            if (ExportD.ShowDialog() == DialogResult.OK)
            {
                //summary of this section of code:
                //1. create a new folder to save these files to
                //2. get each of the splits names and their respective memory streams
                //3. format the name of the file to be saved
                //4. export the memorystreams to their respective files in order to preserve the rtf format and the splits names at the same time
                //5. export the images associated with each split
                int listInd = 1;
                int imgInd = 1;

                /*
                foreach(List<PictureBox> pboxes in editor.sheetimgs.Values)
                {
                    if (pboxes.Count > 0)
                    {
                        foreach(PictureBox pbox in pboxes)
                        {
                            Bitmap bmp = new Bitmap(pbox.Name);


                            //string info = "W~" + pbox.Width + " H~" + pbox.Height + " X~" + pbox.Location.X + " Y~" + pbox.Location.Y + " .";
                            bmp.Save("C:\\Users\\dsiglin\\Documents\\tempfolder" + "\\" + pbox.Name + " -"  + imgInd++ + " " + Path.GetFileName(pbox.Name));
                        }
                    }
                }
                */

                /*
                editor.sheetimgsCopy.Clear();
                foreach (Segment s in state.Run)
                {
                    editor.sheetimgsCopy.Add(s.Name, new List<PictureBox>());
                    foreach (PictureBox pbox in editor.sheetimgs[s.Name])
                    {
                        /*
                        PictureBox tempbox = new PictureBox();
                        tempbox.Image = (Image)pbox.Image.Clone();
                        tempbox.Visible = true;
                        */
                        /*
                        editor.sheetimgsCopy[s.Name].Add(pbox);
                        //pbox.Dispose();
                    }
                }
                foreach (List<PictureBox> pboxes in editor.sheetimgs.Values)
                {
                    pboxes.Clear();
                }
                */

                //delete all images so that the list of images can be correctly updated
                foreach (string path in Directory.GetFiles(ExportD.SelectedPath, "*.jpg"))
                {
                    string tpath = Path.GetFileName(path);
                    string sheet = tpath.Substring(0, tpath.IndexOf("-"));
                    bool deletus = true;
                    File.Delete(path);
                    
                }
                foreach (string path in Directory.GetFiles(ExportD.SelectedPath, "*.png"))
                {
                    string tpath = Path.GetFileName(path);
                    string sheet = tpath.Substring(0, tpath.IndexOf("-"));
                    bool deletus = true;
                    File.Delete(path);
                       
                }
                /*
                foreach (Segment s in state.Run)
                {
                    editor.sheetimgs.Add(s.Name, new List<PictureBox>());
                    foreach (PictureBox pbox in editor.sheetimgsCopy[s.Name])
                    {
                        /*
                        PictureBox tempbox = new PictureBox();
                        tempbox.Image = (Image)pbox.Image.Clone();
                        tempbox.Visible = true;
                        */
                        /*
                        editor.sheetimgs[s.Name].Add(pbox);
                        //pbox.Dispose();
                    }
                }
                */

                foreach (string name in editor.splitsList.Keys)
                {
                    string tempname = name;
                    MemoryStream temps = editor.splitsList[name];
                    temps.Position = 0;
                    RichTextBox tempbox = new RichTextBox();
                    tempbox.Visible = false;
                    tempbox.LoadFile(temps, RichTextBoxStreamType.RichText);

                    foreach (char ch in "<>:\"\\/|?*")
                    {
                        while (tempname.IndexOf(ch) >= 0)
                        {
                            tempname = tempname.Replace("" + ch, "");

                        }
                    }

                    
                     try
                     {
                        string path1 = ExportD.SelectedPath + "\\" + listInd++ + " " + tempname + ".rtf";
                        //write over the file
                        File.WriteAllText(path1, tempbox.Rtf);
                        //File.AppendAllText(path1, tempbox.Rtf);
                        foreach (PictureBox pbox in editor.sheetimgs[name])
                        {
                            Bitmap bmp = new Bitmap(pbox.Name);


                            string info = "W~" + pbox.Width + " H~" + pbox.Height + " X~" + pbox.Location.X + " Y~" + pbox.Location.Y + " .";
                            bmp.Save(ExportD.SelectedPath + "\\" + name + "-" + info + imgInd++ + " " + Path.GetFileName(pbox.Name).Substring(0, Path.GetFileName(pbox.Name).Length - 4) + ".png");
                        }

                        cachePath = ExportD.SelectedPath;
                    }
                    catch (IOException er)
                    {
                        if (er.GetType().Name.Equals("DirectoryNotFoundException"))
                        {
                            cachePath = "";
                            MessageBoxButtons button = MessageBoxButtons.OK;
                            DialogResult alert = MessageBox.Show("Try reselecting the folder or making another folder.", "Error loading folder", button);
                            if (alert == DialogResult.OK)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        //initiate the importing of notes
        private void Import_Click(object sender, EventArgs e)
        {
            if (ImportD.ShowDialog() == DialogResult.OK)
            {
                importNotes(ImportD.SelectedPath);
                cachePath = ImportD.SelectedPath;
            }
        }

        //used to import notes
        public void importNotes(string folder)
        {
            //file name should be the folder name
            //then just use the name of each of the splits used from the foreach name loop and get each of the files that way
            //string folder = Path.GetDirectoryName(ExportD.SelectedPath);
            
            

            //summary of this section of code:
            //1. get all of the files from the selected folder
            //2. reformat them to match the current splits names
            //3. copy their rtf information to the current splits' memorystreams
            //4. add the images to the splits' image lists
            if (!folder.Equals(""))
            {
                foreach (List<PictureBox> pboxes in editor.sheetimgs.Values)
                {
                    pboxes.Clear();
                }
                try
                {
                    int listInd = 1;
                    foreach (string name in editor.splitsList.Keys)
                    {
                        string tempname = name;
                        foreach (char ch in "<>:\"\\/|?*")
                        {
                            while (tempname.IndexOf(ch) >= 0)
                            {
                                tempname = tempname.Replace("" + ch, "");
                            }
                        }
                        string path = folder + "\\" + listInd++ + " " + tempname + ".rtf";
                        FileStream f = new FileStream(path, FileMode.Open);
                        editor.splitsList[name].Position = 0;
                        f.CopyTo(editor.splitsList[name]);
                    }

                    foreach (string name in editor.sheetimgs.Keys)
                    {
                        if (editor.sheetimgs[name].Count > 0)
                        {
                            foreach (PictureBox temppic in editor.sheetimgs[name])
                            {
                                temppic.Hide();
                                editor.sheetimgs[name].Remove(temppic);
                                temppic.Dispose();
                                if (editor.sheetimgs[name].Count == 0)
                                {
                                    break;
                                }
                            }
                        }
                    }


                    //TODO: add more image support in case the images are ever exportedc as different file types
                    foreach (string path in Directory.GetFiles(folder, "*.jpg"))
                    {
                        string tpath = Path.GetFileName(path);
                        string sheet = tpath.Substring(0, tpath.IndexOf("-"));

                        string info = tpath.Substring(tpath.IndexOf("-") + 1, tpath.IndexOf(".") - (tpath.IndexOf("-") + 1));

                        int W = Int32.Parse(info.Substring(info.IndexOf("W") + 2, info.IndexOf(" ") - (info.IndexOf("W") + 2)));
                        info = info.Substring(info.IndexOf(" ") + 1);
                        int H = Int32.Parse(info.Substring(info.IndexOf("H") + 2, info.IndexOf(" ") - (info.IndexOf("H") + 2)));
                        info = info.Substring(info.IndexOf(" ") + 1);
                        int X = Int32.Parse(info.Substring(info.IndexOf("X") + 2, info.IndexOf(" ") - (info.IndexOf("X") + 2)));
                        info = info.Substring(info.IndexOf(" ") + 1);
                        int Y = Int32.Parse(info.Substring(info.IndexOf("Y") + 2, info.IndexOf(" ") - (info.IndexOf("Y") + 2)));
                        info = info.Substring(info.IndexOf(" ") + 1);


                        editor.sheetimgs[sheet].Add(new PictureBox());
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Parent = editor.richTextBox1;

                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Location = new Point(X, Y);
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Size = new Size(W, H);

                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].BackColor = Color.Black;

                        var imgstream = File.OpenRead(path);
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Image = Image.FromStream(imgstream);
                        imgstream.Close();
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Visible = false;
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].SizeMode = PictureBoxSizeMode.StretchImage;
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].MouseDown += editor.picbox_OnMouseDown;
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].MouseMove += editor.picbox_OnMouseMove;
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].MouseEnter += new EventHandler(editor.picbox_OnMouseEnter);
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].MouseLeave += new EventHandler(editor.picbox_OnMouseLeave);
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].MouseUp += editor.picbox_OnMouseUp;
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Name = path;

                        editor.imagepaths.Add(editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1], path);
                    }
                    foreach (string path in Directory.GetFiles(folder, "*.png"))
                    {
                        string tpath = Path.GetFileName(path);
                        string sheet = tpath.Substring(0, tpath.IndexOf("-"));

                        string info = tpath.Substring(tpath.IndexOf("-") + 1, tpath.IndexOf(".") - (tpath.IndexOf("-") + 1));

                        int W = Int32.Parse(info.Substring(info.IndexOf("W") + 2, info.IndexOf(" ") - (info.IndexOf("W") + 2)));
                        info = info.Substring(info.IndexOf(" ") + 1);
                        int H = Int32.Parse(info.Substring(info.IndexOf("H") + 2, info.IndexOf(" ") - (info.IndexOf("H") + 2)));
                        info = info.Substring(info.IndexOf(" ") + 1);
                        int X = Int32.Parse(info.Substring(info.IndexOf("X") + 2, info.IndexOf(" ") - (info.IndexOf("X") + 2)));
                        info = info.Substring(info.IndexOf(" ") + 1);
                        int Y = Int32.Parse(info.Substring(info.IndexOf("Y") + 2, info.IndexOf(" ") - (info.IndexOf("Y") + 2)));
                        info = info.Substring(info.IndexOf(" ") + 1);

                        
                        editor.sheetimgs[sheet].Add(new PictureBox());

                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Parent = editor.richTextBox1;

                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Location = new Point(X, Y);
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Size = new Size(W, H);

                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].BackColor = Color.Black;

                        var imgstream = File.OpenRead(path);
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Image = Image.FromStream(imgstream);
                        imgstream.Close();
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Visible = false;
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].SizeMode = PictureBoxSizeMode.StretchImage;
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].MouseDown += editor.picbox_OnMouseDown;
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].MouseMove += editor.picbox_OnMouseMove;
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].MouseUp += editor.picbox_OnMouseUp;
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].MouseEnter += new EventHandler(editor.picbox_OnMouseEnter);
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].MouseLeave += new EventHandler(editor.picbox_OnMouseLeave);
                        editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1].Name = path;

                        editor.imagepaths.Add(editor.sheetimgs[sheet][editor.sheetimgs[sheet].Count - 1], path);
                    }

                }
                catch (IOException er)
                {
                    if (er.GetType().Name.Equals("FileNotFoundException"))
                    {
                        MessageBoxButtons button = MessageBoxButtons.OK;
                        DialogResult alert = MessageBox.Show("One or more of the files could not be found.\nTry loading a different folder or making sure the number of your splits and the name of the splits match the files.", "Speed Run Notes: Error loading folder", button);
                        cachePath = "";
                        if (alert == DialogResult.OK)
                        {
                            //this just makes sure the dialog shows but there is not anything I have to do here
                        }
                    }
                }
            }
        }
    }
}
