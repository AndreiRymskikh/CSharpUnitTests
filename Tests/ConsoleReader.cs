using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class ConsoleReader
    {
        public StringWriter stringWriter;

        public ConsoleReader()
        {
            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
        }

        public string ConsoleOutput()
        {
            return stringWriter.ToString().Trim();
        }
    }
}
