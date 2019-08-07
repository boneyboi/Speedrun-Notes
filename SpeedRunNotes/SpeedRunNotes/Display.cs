using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeedRunNotes
{
    public partial class Display : Form
    {
        //create an editor so that when one is passed in we can keep it in this variable and use it's attributes
        public Editor editor;
        //create an RTB to get the text to display
        public RichTextBox displaybox;


        //use a timer to refresh and update the display
        public Timer timer;
        //keeps track of if the editor is open or not
        public bool editing = false;
        //gets the sheet that should be currently displayed
        public string displaySheet;
        //gets the state of livepsplit
        public LiveSplitState state;

        //this helps make it so that it isn't updating everytime a tick happens because that is unneccessary
        //TODO: do i need this???
        public int ticks = 5;

        //this is used for text display later
        public int defaultoffsetY;


        //constructor for the display class
        //initializes the display form component
        public Display(LiveSplitState state, Editor editor)
        {
            //set the editor to the one created in the speedrunnotescomponent class
            this.editor = editor;
            //gets the state from the speedrunnotescomponent class
            this.state = state;

            //these eventhandlers handle when livesplit starts a run, pauses and changes splits
            this.state.OnStart += state_OnStart;
            this.state.OnSplit += state_OnSplit;
            this.state.OnPause += state_OnPause;

            //creates a paint event for this form
            this.Paint += new PaintEventHandler(Form_Paint);

            //initialize the component
            InitializeComponent();


            //lets the window be resizable
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            //creates a new richtextbox, and since it will only be used to get the rich text I make it invisible
            this.displaybox = new RichTextBox();
            this.displaybox.Visible = false;
            displaySheet = editor.currentSheet;

            //create a new timer and start it
            timer = new Timer();
            timer.Start();
            timer.Tick += timer_tick;
        }

        //this method handles the start of splitting in livesplit and ensures the display displays the current split's sheet
        private void state_OnStart(object sender, EventArgs e)
        {
            if (!this.state.CurrentSplit.Name.Equals(null))
            {
                displaySheet = this.state.CurrentSplit.Name;
            }
        }
        //this method handles the change of a split in livesplit and ensures the display displays the current split's sheet
        private void state_OnSplit(object sender, EventArgs e)
        {
            if (!this.state.CurrentSplit.Name.Equals(null))
            {
                displaySheet = this.state.CurrentSplit.Name;
            }
        }
        //TODO: is there a need for a pause method?
        private void state_OnPause(object sender, EventArgs e)
        {
            //displaySheet = editor.currentSheet;
        }

        //handles everytick by updating
        private void timer_tick(object sender, EventArgs e)
        {
            update();
        }

        //updates the display with the text and images associated witht hte current split's sheet
        public void update()
        {
            


            //changes the value of editing if the editor is open or not
            if (editor.Visible)
            {
                editing = true;
            } else
            {
                editing = false;
            }

            //changes and updates the displaybox and display sheet for display
            //displays the current sheet being edited
            editor.splitsList[displaySheet].Position = 0;
            this.displaybox.LoadFile(editor.splitsList[displaySheet], RichTextBoxStreamType.RichText);
            if (editing) ///if the note editor is open
            {
                displaySheet = editor.currentSheet;
                editor.TempsplitsList[displaySheet].Position = 0;
                this.displaybox.LoadFile(editor.TempsplitsList[displaySheet], RichTextBoxStreamType.RichText);
            }

            this.Refresh();
        }

        //this method handles the paint event, and is clled when refresh is called
        public void Form_Paint(object sender, PaintEventArgs pe)
        {
            //create a new graphics object in order to draw things to the displpay
            Graphics g = pe.Graphics;
            
            //to start drawing one must make a new "Pen" so g knows how to draw anything
            //Pen takes arguments System.Drawing.Color color and a Float/int width
            SolidBrush brush = new SolidBrush(Color.Black);

            

            ////////this displays the text from the split's or sheet's rich text box////////////
            //these variables keep track of where a charachter should be placed
            //includes defaultOffsetY
            float offsetx = 0;
            float offsety = 0;
            float tempoffsety = 0;
            //the string format helps tell the measure method what kind of formatting my text has
            StringFormat stf = new StringFormat();
            stf.Alignment = StringAlignment.Near;
            stf.LineAlignment = StringAlignment.Near;
            
            //this loop goes through all of the charachters, measures them and then displays them correctly on the display
            for (int i = 0; i < displaybox.Text.Length; i++)
            {
                displaybox.Select(i, 1);
                brush.Color = displaybox.SelectionColor;
                string tempch = this.displaybox.Text.Substring(i, 1);
                g.DrawString(tempch, displaybox.SelectionFont, brush, offsetx, offsety);



                //checking what kind of charachter it is to determine what to do with it when it is drawn
                if(tempch.Equals(" "))
                {
                    
                        offsetx += g.MeasureString(tempch, displaybox.SelectionFont).Width;
                        if (tempoffsety < g.MeasureString(tempch, displaybox.SelectionFont).Height)
                        {
                            tempoffsety = g.MeasureString(tempch, displaybox.SelectionFont).Height;
                        }
                        if (offsetx + g.MeasureString(tempch, displaybox.SelectionFont).Width * 2 >= this.Width)
                        {
                            offsetx = 0;
                            offsety += tempoffsety;
                            tempoffsety = 0;
                        }
                        
                }
                else if(tempch.Equals("\n"))
                {
                    if(tempoffsety == 0)
                    {
                        offsetx = 0;
                        defaultoffsetY = displaybox.SelectionFont.Height;
                        offsety += defaultoffsetY;
                    }
                    offsetx = 0;
                    offsety += tempoffsety;
                    tempoffsety = 0;

                    
                }
                else
                {
                    //setting the charachter range is necessary so that the measure ranges method actually returns an array of regions based off how many chracter ranges I make in this variable
                    List<CharacterRange> chr = new List<CharacterRange>();
                    chr.Add(new CharacterRange(0, 1));
                    stf.SetMeasurableCharacterRanges(chr.ToArray());

                    Region[] regions = g.MeasureCharacterRanges(tempch, displaybox.SelectionFont, new RectangleF(0, 0, this.Width, this.Height), stf);
                    offsetx += regions[0].GetBounds(g).Size.Width;
                    if (tempoffsety < regions[0].GetBounds(g).Size.Height)
                    {
                        tempoffsety = regions[0].GetBounds(g).Size.Height;
                    }
                    if (offsetx + regions[0].GetBounds(g).Size.Width*2 >= this.Width)
                    {
                        offsetx = 0;
                        offsety += tempoffsety;
                        tempoffsety = 0;
                    }
                    
                }
                
            }
            //////////////end of text display//////////////////

            ////image dispaly////
            //go through all the images associated with the split and display each
            if(editor.sheetimgs[displaySheet].Count > 0)
            {
                foreach(PictureBox pbox in editor.sheetimgs[displaySheet])
                {
                    g.DrawImage(pbox.Image, pbox.Location.X, pbox.Location.Y, pbox.Width, pbox.Height);
                }
            }
            
            
        }
       

        //this method handles the load event of the display
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //trying to get the border to not show up without locking the form
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;


            //TODO:make sure this goes in the update method in ordere to update this value and teh display's layer
            if (state.LayoutSettings.AlwaysOnTop)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }

            //ensures that the dorm is always on top like LiveSplit
            //TODO: mkae sure it changes to be the same as livesplit's settings
            //this.TopMost = true;
        }


        //variables that help with resizing and moving the form
        private bool mousedown = false;
        private bool resizingH = false;
        private bool resizingV = false;
        private bool resizingHV = false;
        private int x, y;
        private int grip = 3;
        //handles mouse presses and resizes the form when the mouse is pressed over the grip
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                mousedown = true;
                x = e.X;
                y = e.Y;
            }
            if ((e.Y >= this.Height - grip * 2 && e.X >= this.Width - grip * 2))
            {
                this.resizingHV = true;
            }
            else if ( e.Y >= this.Height - grip)
            {
                this.resizingV = true;
            }
            else if (e.X >= this.Width - grip)
            {
                this.resizingH = true;
            }
            
        }
        //handles mouse movement for when the mouse moves when it grabbed the grip for resizing or moves the form when not over the grip
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            //e.Y is the cursors position within the form
            if((e.Y >= this.Height - grip*2 && e.X >= this.Width - grip*2))
            {
                this.Cursor = Cursors.SizeNWSE;
            }
            else if (e.Y >= this.Height - grip)
            {
                this.Cursor = Cursors.SizeNS;
            }
            else if (e.X >= this.Width - grip)
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
                    int dx = Location.X + (x1 - x);
                    int dy = Location.Y + (y1 - y);
                    Location = new Point(dx, dy);
                }
                else if(resizingHV)
                {
                    this.Height += (y1 - y);
                    this.Width += (x1 - x);
                    x = x1;
                    y = y1;
                }
                else if (resizingV)
                {
                    this.Height += (y1 - y);
                    y = y1;
                }
                else if (resizingH)
                {
                    this.Width += (x1 - x);
                    x = x1;
                }
                
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //apparently the problem was 2 fold, needed to use show dialog, not show
            // and then needed to use this. to make sure it references the right object
           this.editor.ShowDialog();
        }

        //this method handles the mouse up event and resets variables associated with pressing buttons on the mouse
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mousedown = false;
            this.resizingHV = false;
            this.resizingH = false;
            this.resizingV = false;
        }
    }
}
