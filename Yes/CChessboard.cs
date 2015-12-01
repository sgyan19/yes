using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yes
{
    class CChessboard
    {
        Hashtable friendTable;      // 朋友的结果表
        Hashtable myTable;          // 我的翻子表
        HashSet<int>[] columnLists; // 同列表，对于索引结果i，columnLists[i] 表示会和他处于同列的结果
        int count;                  // 位数
        int max;                    // 结果的2进制最大值

        /// <summary>
        /// 获取2进制数1的个数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public int get1Count(int number)
        {
            int c = 0;
            for (c = 0; number != 0; ++c)
            {
                number &= (number - 1); // 清除最低位的1
            }
            return c;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="squareCount"></param>
        public void makeTables(int squareCount)
        {
            friendTable = new Hashtable();
            myTable = new Hashtable();
            count = squareCount;
            max = 1 << squareCount;

            makeColumnLists();

            for (int i = 0; i < max; i++)
            {
                filp(i);
            }
        }
        /// <summary>
        /// 翻动动作，居于当前情况，进行一次翻动。
        /// </summary>
        /// <param name="baseNumber"></param>
        public void filp(int baseNumber)
        {
            ArrayList exsits = new ArrayList();         // 已经映射过的结果。
            Hashtable filpSonTable = new Hashtable();   // 本次翻动动作和结果的映射
            for (int i = 0; i < count; i++)
            {
                int action = 1 << i;                       // 翻动动作值
                int newNumber = baseNumber ^ action;       // 翻动后新值
                if (friendTable.ContainsKey(newNumber))      // 检查是否已经映射过本次结果，如果已经映射过，就记录下翻动动作，不必重新分配映射
                {
                    int exsit = (int)friendTable[newNumber];
                    filpSonTable.Add(action, exsit);
                    exsits.Add(exsit);
                    continue;
                }


                ArrayList exsits2 = new ArrayList();    // 同列检查表，同列元素如果已经映射，不能再映射他们
                HashSet<int> column = columnLists[newNumber];   // 取出当前结果的同列。
                foreach (int item in column)            // 遍历同列元素，查看他们是否已经做过映射
                {
                    if (friendTable.ContainsKey(item))
                    {
                        exsits2.Add(friendTable[item]);      // 已经映射
                    }
                }

                for (int j = 0; j < count; j++)
                {
                    if (exsits.Contains(j) || exsits2.Contains(j))  // 排除同列已经映射的值
                    {
                        continue;
                    }
                    friendTable.Add(newNumber, j);
                    filpSonTable.Add(action, j);
                    exsits.Add(j);
                    break;
                }

            }
            myTable.Add(baseNumber, filpSonTable);
        }


        /// <summary>
        /// 构建同列表，对每一个结果值，记录他会和其他哪些结果同列。同列者不能映射同一个结果
        /// </summary>
        /// <param name="squareCount"></param>
        private void makeColumnLists()
        {
            columnLists = new HashSet<int>[max];
            // 初始化结构
            for (int i = 0; i < max; i++)
            {
                columnLists[i] = new HashSet<int>();
            }

            for (int i = 0; i < max; i++)
            {
                // 对于本次基础值 i，翻出所有新的情况 newN。则对这个基础值翻出的所有新情况为同列。
                ArrayList temp = new ArrayList(); // 暂存已经出现的新结果
                for (int j = 0; j < count; j++)
                {
                    int action = 1 << j;
                    int newN = i ^ action; // 新结果
                    // 遍历已经存在的新结果
                    foreach (int item in temp)
                    {
                        columnLists[item].Add(newN); // 最新的结果放入同列表。
                        columnLists[newN].Add(item); // 
                    }
                    temp.Add(newN);
                }
            }
        }

        /// <summary>
        /// 检查对于每一种情况的所有翻动映射，是否有重复
        /// </summary>
        /// <returns></returns>
        public bool check()
        {
            foreach (Hashtable item in myTable.Values)
            {
                ArrayList list = new ArrayList();
                foreach (int i in item.Values)
                {
                    foreach (int hasCheck in list)
                    {
                        if (i == hasCheck)
                            return false;
                    }
                    list.Add(i);
                }
            }


            return true;
        }

        public void log()
        {
            FileStream firendTableStream = new FileStream("0x"+count+"firendTable.txt", FileMode.OpenOrCreate);
            StreamWriter writer1 = new StreamWriter(firendTableStream);
            for (int i = 0; i < max; i++)
            {
                String key = Convert.ToString(i, 2).PadLeft(count,'0');
                String shi = Convert.ToString(i).PadLeft(4, '0');
                writer1.WriteLine(shi + "------" + key + "------>" + friendTable[i]);
            }
            writer1.Close();
            firendTableStream.Close();

            FileStream myTableStream = new FileStream("0x" + count + "myTable.txt", FileMode.OpenOrCreate);
            StreamWriter writer2 = new StreamWriter(myTableStream);
            for (int i = 0; i < max; i++)
            {
                String key = Convert.ToString(i, 2).PadLeft(count, '0');
                writer2.WriteLine("情况： "+ i + "------" + key);
                writer2.WriteLine("翻法：");
                Hashtable sonTable = (Hashtable)myTable[i];
                ArrayList ksys = new ArrayList(sonTable.Keys);
                ksys.Sort();
                foreach (int item in ksys)
                {
                    String action = Convert.ToString(item, 2).PadLeft(count, '0');
                    String shi = Convert.ToString(item).PadLeft(4, '0');
                    writer2.WriteLine("    " + shi + "------" + action);
                    int result = i ^ item;
                    String result2x = Convert.ToString(result, 2).PadLeft(count, '0');
                    String result10x = Convert.ToString(result).PadLeft(4, '0');

                    writer2.WriteLine("    " + result10x + "      " + result2x + "------>" + sonTable[item]);
                }
                writer2.WriteLine(" ");
            }
            writer2.Close();
            myTableStream.Close();
        }
    }
}
