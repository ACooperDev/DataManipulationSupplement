using System;
using System.Collections.Generic;
using System.IO;
using Cognex.Designer.Core;
using Cognex.Designer.Sequences;

namespace DataMaipulation
{
    //Description attribute appears as a tooltip in the Toolbox in Designer
    //Category attribute defines the location of the block in the Toolbox
    [DisplayName("Write CSV")]
    [Description("Work with CSV's")]
    [Category("Data Manipulation.Collection")]
    public class WriteCSV : ConfigurableBlock
    {
        //Define the pins.
        private readonly GenericPin<string> _input1;
        private readonly GenericPin<List<string>> _input2;
        private readonly GenericPin<Boolean> _input3;
        
        public WriteCSV()
        {
            //Assign pins a name in Designer.
            _input1 = CreateInputPin<string>("filePath");
            _input2 = CreateInputPin<List<string>>("writeData");
            _input3 = CreateInputPin<Boolean>("append");
        }

        //Block properties can be configured in various ways.
        //Here, it is configured as an editable property accessed from the property editor. 
        //[DisplayName("My Input Index")]
       // [Category("Common")]
        //[BlockProperty(ConfigurationOption.Input, ConfigurationOption.Editable)]
       // public int myIndex { get; set; }

        protected override void ExecuteSequence()
        {
            using (StreamWriter writer = new StreamWriter(_input1.Value, append: _input3.Value)) // Change append to false to overwrite
            {
                string row = string.Join(",", _input2.Value); // Combine all list items into a CSV row
                writer.WriteLine(row);
            }

        }
    }
}