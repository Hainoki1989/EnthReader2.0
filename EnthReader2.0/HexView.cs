using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnthReader2._0
{
    public partial class HexView : Form
    {
        public HexView()
        {
            InitializeComponent();
            t_hexView.ScrollBars = ScrollBars.Vertical; 
        }

        public void DisplayHexData(byte[] data, long StartAddress)
        {
            if (data != null && data.Length > 0)
            {
                StringBuilder hexStringBuilder = new StringBuilder();

                hexStringBuilder.Append($"THE START ADDRESS FOR THIS BLOCK IS :{StartAddress.ToString("X")}");
                hexStringBuilder.AppendLine();

                for (int i = 0; i < data.Length; i++)
                {
                    hexStringBuilder.Append(data[i].ToString("X2"));

                    if ((i + 1) % 16 == 0)
                        hexStringBuilder.AppendLine();
                    else if ((i + 1) % 4 == 0)
                    {
                        hexStringBuilder.Append("  ");
                    }
                    else
                        hexStringBuilder.Append(" ");


                }

                t_hexView.Text = hexStringBuilder.ToString();
                t_hexView.Font = new Font("Courier New", 12);
            }
        }

        private void t_hexView_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
