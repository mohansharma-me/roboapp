using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShortMessageParser
{
    public class ShortMessage
    {
        
        #region Public Properties
        public string Index
        { get; set; }
        public string Status
        { get; set; }
        public string Sender
        { get; set; }
        public string Alphabet
        { get; set; }
        public string Sent
        { get; set; }
        public string Message
        { get; set; }
        public string DateTime
        { get;set; }
        #endregion

    }

    public class ShortMessageCollection : List<ShortMessage>
    {
        public bool Reading = false;
        public string inputString { get; set; }
    }
}
