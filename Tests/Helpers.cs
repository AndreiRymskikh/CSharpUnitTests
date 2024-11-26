using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class Helpers
    {
        public StringWriter stringWriter;

        public void ConsoleSetOut()
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
