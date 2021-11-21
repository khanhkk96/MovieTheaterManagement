using System;

namespace _15211TT0065
{
    public class Film
    {
        //variable
        private string code, name, content, type;
        private TimeSpan time;

        //contructor
        public Film() { }

        public Film(string code, string name, string content, string type, TimeSpan time)
        {
            this.code = code;
            this.name = name;
            this.content = content;
            this.type = type;
            this.time = time;
        }

        //getter and setter
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
            }
        }

        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public TimeSpan Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }
    }
}
