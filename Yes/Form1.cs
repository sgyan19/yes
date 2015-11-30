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
            makeTables(8);
            InitializeComponent();
        }

        public void makeTables(int squareCount)
        {
            Btable = new Hashtable();
            Atable = new Hashtable();
            count = squareCount;
            max = 1 << squareCount;
            
            temp = new int[squareCount];
            data = new ArrayList[squareCount + 1][];
            for (int i = 0; i < squareCount; i++)
            {
                temp[i] = i;
            }
            for (int i = 0; i < squareCount + 1; i++)
            {
                data[i] = new ArrayList[squareCount + 1];
                for (int j = 0; j < max; j++)
                {
                    data[i][j] = new ArrayList();
                }
            }
            
            for (int i = 0; i < count + 1; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    if(get1Count(j) == i)
                    {
                        Console.WriteLine("line:" + i + ",num =" + j);
                        filp(j);
                    }
                }
            }
        }

        public void filp(int baseNumber)
        {
            Hashtable filpSonTable;
            if (!Atable.ContainsKey(baseNumber))
            {
                filpSonTable = new Hashtable();
                Atable.Add(baseNumber, filpSonTable);
            }
            else
            {
                filpSonTable = (Hashtable) Atable[baseNumber];
            }
            
            ArrayList exsits = new ArrayList();
            
            for (int i = 0; i < count; i++)
            {
                int add = 1 << i;
                int newNumber = baseNumber ^ add;

                int n = get1Count(newNumber);
                ArrayList list = data[n][newNumber];
                if (Btable.ContainsKey(newNumber))
                {
                    int exsit = (int)Btable[newNumber];
                    filpSonTable.Add(add, exsit);
                    exsits.Add(exsit);
                    continue;
                }
                for (int j = 0; j < count; j++)
                {
                    if(exsits.Contains(j) || list.Contains(j))
                    {
                        continue;
                    }
                    Btable.Add(newNumber, j);
                    filpSonTable.Add(add, j);
                    list.Add(j);
                    exsits.Add(j);
                    break;
                }

            }
        }

        public int get1Count(int number)
        {
            int c = 0;
            for (c = 0; number != 0; ++c)
            {
                number &= (number - 1); // 清除最低位的1
            }
            return c;
        }
        

        Hashtable Btable; // 朋友的结果表
        Hashtable Atable; // 我的翻子表
        int count;
        int max;

        int[] temp;
        ArrayList[][] data;
    }
}
