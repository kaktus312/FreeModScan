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
        public override void Write(char value)
        {
            base.Write(value);
            //_output.Text = value + _output.Text;// Когда символ записывается в поток, добавляем его в textbox.
            //_output.Last() + value;
            //_output.AppendText(value.ToString()); 
        }
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
