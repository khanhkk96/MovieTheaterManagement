namespace _15211TT0065.Model
{
    public class Client
    {
        //variable
        private string code, name, type, email, phone, address;
        
        //contructor
        public Client() { }

        public Client(string code, string name, string type, string email, string phone, string address)
        {
            this.code = code;
            this.name = name;
            this.type = type;
            this.email = email;
            this.phone = phone;
            this.address = address;
        }
        
        //getter and setter
        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
            }
        }

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

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
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

        public string Phone
        {
            get
            {
                return phone;
            }

            set
            {
                phone = value;
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
