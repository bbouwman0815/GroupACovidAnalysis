using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid19Analysis.Utility
{
    public class FileWriter
    {

        public FileWriter()
        {

        }

        public void initialize()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        }

        private void setSaveFileDialog()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();
        }
    }
}
