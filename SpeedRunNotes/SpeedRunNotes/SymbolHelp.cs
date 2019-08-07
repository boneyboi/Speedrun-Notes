using SpeedRunNotes.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeedRunNotes
{
    public partial class SymbolHelp : Form
    {
        public SymbolHelp()
        {
            
            InitializeComponent();
            richTextBox1.Rtf = Resources.SymbolHelp;
            //byte[] symboltext = ;
        }
    }
}
