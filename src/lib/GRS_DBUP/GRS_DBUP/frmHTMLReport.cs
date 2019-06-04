using System.IO;
using System.Windows.Forms;

namespace GRS_DBUP
{
    public partial class frmHTMLReport : Form
    {
        public frmHTMLReport(string filePath)
        {
            InitializeComponent();

            //var uri = new Uri(filePath);
            //webBrowser1.Navigate(uri);

            //var source = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //webBrowser1.DocumentStream = source;

            var html = File.ReadAllText(filePath);
            webBrowser1.DocumentText = html;
        }
    }
}
