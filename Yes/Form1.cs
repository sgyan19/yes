using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //makeTables();
            CChessboard board = new CChessboard();
            board.makeTables(16);
            if (board.check())
            {
                board.log();
            }
            InitializeComponent();
        }
    }
}
