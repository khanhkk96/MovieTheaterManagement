using System;

namespace _15211TT0065.Model
{
    //lop ma tu dong cho database
    public class AutoCode
    {
        //variable
        private string str;
        private string code;


        //contructor
        public AutoCode(string str, string code)
        {
            this.str = str;
            this.code = code;
        }


        //override ToString() for class
        public override string ToString()
        {
            string id = null;
            if (code == null)
            {
                id = str + "0001";
            }
            else
            {
                try
                {
                    id += str;
                    string num = code.Substring(2, 4);
                    int so = int.Parse(num);
                    so++;
                    if (so < 10)
                    {
                        id += "000";
                    }
                    else if (so < 100)
                    {
                        id += "00";
                    }
                    else if (so < 1000)
                    {
                        id += "0";
                    }
                    id += so.ToString();
                }
                catch (Exception ex)
                {
                }
            }

            return id;
        }
    }
}
