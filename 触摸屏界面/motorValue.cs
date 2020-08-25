using System;
using System.Collections.Generic;
using System.IO;

namespace 触摸屏界面
{
    class motorValue
    {
        public List<double> time = new List<double>();
        public List<Single>[] pos = new List<float>[3];
        public List<Single>[] vel = new List<float>[3];
        public List<Single>[] tor = new List<float>[3];
        public motorValue()
        {
            for (int i = 0; i < 3; i++)
            {
                pos[i] = new List<float>();
                vel[i] = new List<float>();
                tor[i] = new List<float>();
            }
        }
        public void save(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int i = 0; i < pos[0].Count; i++)
                {
                    string tmp = time[i].ToString("F3");
                    for (int j = 0; j < 3; j++)
                    {
                        tmp += "," + pos[j][i].ToString("F3");
                        tmp += "," + vel[j][i].ToString("F3");
                        tmp += "," + tor[j][i].ToString("F3");
                    }
                    sw.WriteLine(tmp);
                }
            }
        }
        public void read(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');
                    time.Add(Convert.ToDouble(values[0]));
                    pos[0].Add(Convert.ToSingle(values[1]));
                    pos[1].Add(Convert.ToSingle(values[2]));
                    pos[2].Add(Convert.ToSingle(values[3]));
                    vel[0].Add(Convert.ToSingle(values[4]));
                    vel[1].Add(Convert.ToSingle(values[5]));
                    vel[2].Add(Convert.ToSingle(values[6]));
                    tor[0].Add(Convert.ToSingle(values[7]));
                    tor[1].Add(Convert.ToSingle(values[8]));
                    tor[2].Add(Convert.ToSingle(values[9]));
                }
            }
        }
    }
}
