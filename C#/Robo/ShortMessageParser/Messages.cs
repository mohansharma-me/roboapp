using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShortMessageParser
{
    public class Messages
    {
        public static ShortMessageCollection ParseMessages(String input)
        {
            ShortMessageCollection messages = new ShortMessageCollection();
            try
            {
                Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""\r\n(.+)");//\r\n
                Match m = r.Match(input);
                while (m.Success)
                {
                    ShortMessage msg = new ShortMessage();
                    //msg.Index = int.Parse(m.Groups[1].Value);
                    msg.Index = m.Groups[1].Value;
                    msg.Status = m.Groups[2].Value;
                    msg.Sender = m.Groups[3].Value;
                    msg.Alphabet = m.Groups[4].Value;
                    msg.Sent = m.Groups[5].Value;
                    msg.Message = m.Groups[6].Value;
                    messages.Add(msg);

                    m = m.NextMatch();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return messages;
        }
    }
}
