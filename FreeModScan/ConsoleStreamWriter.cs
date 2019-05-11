using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreeModScan
{
    class ConsoleStreamWriter : TextWriter
    {
        RichTextBox _output = null;

        public ConsoleStreamWriter(RichTextBox output)
        {
            _output = output;
        }
        public override void Write(string value)
        {
            string tmp = "** " + DateTime.Now.ToString() + " - " + value + " **" + "\n";
            base.Write(tmp);
            _output.Text = tmp + _output.Text;// Когда символ записывается в поток, добавляем его в textbox/RichTextBox.
        }
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
