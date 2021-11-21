using System;

namespace _15211TT0065
{
    public class Lich
    {
        //variable
        private string code, film, staff, room;
        private DateTime start;

        //contructor
        public Lich() { }

        public Lich(string code, string film, DateTime start, string staff, string room)
        {
            this.code = code;
            this.film = film;
            this.start = start;
            this.staff = staff;
            this.room = room;
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

        public string Film
        {
            get
            {
                return film;
            }

            set
            {
                film = value;
            }
        }

        public string Room
        {
            get
            {
                return room;
            }

            set
            {
                room = value;
            }
        }

        public string Staff
        {
            get
            {
                return staff;
            }

            set
            {
                staff = value;
            }
        }

        public DateTime Start
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
            }
        }
    }
}
