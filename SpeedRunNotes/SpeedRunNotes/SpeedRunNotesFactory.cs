using LiveSplit.SpeedRunNotes;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: ComponentFactory(typeof(SpeedRunNotesFactory))]

namespace LiveSplit.SpeedRunNotes
{
    public class SpeedRunNotesFactory : IComponentFactory
    {

        public ComponentCategory Category
        {
            //determines what category the component displays under
            get { return ComponentCategory.Other; }
        }

        public string ComponentName
        {
            //name the component dislplays as
            get { return "Speedrun Notes"; }
        }

        public IComponent Create(Model.LiveSplitState state)
        {
            //cretes a new instance of the component class
            return new SpeedRunNotesComponent(state);
        }

        public string Description
        {
            //description that displays with the component name
            get { return "This is a component that allows for high customuzation of notes for speedrunning that goes along with each split."; }
        }

        public string UpdateName
        {
            get { return ComponentName; }
        }

        public string UpdateURL
        {
            //put github page here maybe, maybe not so nothing weird happens
            get { return ""; }
        }

        public Version Version
        {
            get { return Version.Parse("1.0"); }
        }

        public string XMLURL
        {
            get { return UpdateURL + ""; }
        }
    }
}