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
            if (_output.InvokeRequired)//для доступа к RichTextBox не из того потока в котором он был создан
                _output.Invoke(new Action<string>((s) => _output.Text = s+ _output.Text), tmp);
            else
                _output.Text = tmp + _output.Text;// Когда символ записывается в поток, добавляем его в textbox/RichTextBox.
            //_output.Text = tmp + _output.Text;
        }
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
