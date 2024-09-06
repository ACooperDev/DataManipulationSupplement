using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Cognex.Designer.Core;
using Cognex.Designer.Sequences;
using CsvHelper;
using CsvHelper.Configuration;

namespace DataMaipulation
{
    //Description attribute appears as a tooltip in the Toolbox in Designer
    //Category attribute defines the location of the block in the Toolbox
    [DisplayName("Read CSV")]
    [Description("Work with CSV's")]
    [Category("Data Manipulation.Collection")]
    public class ReadCSV : ConfigurableBlock
    {
        //Define the pins.
        private readonly GenericPin<string> _input1;
        private readonly GenericPin<int> _input2;

        private readonly GenericPin<List<string>> _output1;
        private readonly GenericPin<List<string>> _output2;
        private readonly GenericPin<int> _output3;

        public ReadCSV()
        {
            //Assign pins a name in Designer.
            _input1 = CreateInputPin<string>("filePath");
            _input2 = CreateInputPin<int>("rowIndex");

            _output1 = CreateOutputPin<List<string>>("row");
            _output2 = CreateOutputPin<List<string>>("heading");
            _output3 = CreateOutputPin<int>("rowCount");
        }

        protected override void ExecuteSequence()
        {
            List<Dictionary<string,string>> records = new List<Dictionary<string, string>>();
            List<string> headersList = new List<string>();
            int rowCount = 0;

            //Using CsvHelper to read the CSV file.
            using (StreamReader reader = new StreamReader(_input1.Value))
            using (CsvReader csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
            {
                //Read the first row (header row)
                csv.Read();
                //Prepare to read header values
                csv.ReadHeader();
                //Get the header row
                string[] headers = csv.HeaderRecord;  
                
                headersList.AddRange(headers);

                //Read each row
                while (csv.Read())
                {
                    Dictionary<string, string> row = new Dictionary<string, string>();
                    foreach (var header in headers)
                    {
                        row[header] = csv.GetField(header);

                    }
                    records.Add(row);
                    rowCount++;
                }
            }

            //Search for a specific row and convert to a list.
            Dictionary<string, string> specificRow = records[_input2.Value];
            List<string> list = new List<string>(specificRow.Values);
           
            _output1.Value = list;
            _output2.Value = headersList;
            _output3.Value = rowCount;
        }
    }
}