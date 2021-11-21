using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _15211TT0065
{
    public partial class SelectFilm : Form
    {
        //connection database and variable
        public delegate void getData(Lich lich);

        public getData sendCode;

        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-95AI01F\SQLEXPRESS;Initial Catalog=QUANLYRAPCHIEUPHIM;Integrated Security=True");
        //Collection<Film> list;

        bool flag;

        public SelectFilm()
        {
            InitializeComponent();
            //list = new Collection<Film>();
            flag = false;
        }

        //Created by Vu Dinh khanh - 31/05/2018 : Lấy danh sách lịch chiếu phim
        public static DataTable LayDSLich(SqlConnection con)
        {
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayLich3", con);
                cmd.CommandText = "sp_LayLich3";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt
                return dt; // tra ve bang
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Loi " + ex.Message, "Thong bao");
                return null;
            }
            finally
            {
                con.Close(); // dong ket noi
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : Chọn lịch chiếu phim
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (clbList.CheckedItems.Count == 1)
            {
                CheckListBoxItem item = (CheckListBoxItem)clbList.CheckedItems[0];
                if (sendCode != null)
                {
                    sendCode(item.getTag() as Lich);
                    flag = true;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Mời bạn chọn một bộ phim!");
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : xác nhận và đóng form
        private void SearchTicket_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!flag)
            {
                e.Cancel = true;
                if (Main.flag == false)
                {
                    DialogResult dr = MessageBox.Show("Bạn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        e.Cancel = false;
                    }
                }
                else
                {
                    e.Cancel = false;
                }
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : Load dữ liệu lên form
        private void SelectFilm_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayLich3", con);
                cmd.CommandText = "sp_LayLich3";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt

                foreach(DataRow item in dt.Rows)
                {
                    Lich l = new Lich();
                    l.Code = item.ItemArray[0].ToString();
                    l.Film = item.ItemArray[1].ToString();
                    l.Start = DateTime.Parse(item.ItemArray[2].ToString());
                    l.Staff = item.ItemArray[3].ToString();
                    l.Room = item.ItemArray[4].ToString();

                    //string film = searchFilm(l, con);
                    string film = l.Film + " - " + l.Start + " - " + l.Room;
                    clbList.Items.Add(new CheckListBoxItem(l, film));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi\n" + ex.Message);
            }
            finally
            {
                con.Close(); // dong ket noi
            }
        }
    }

    //Created by Vu Dinh khanh - 31/05/2018 : tạo lớp item cho checklistbox
    public class CheckListBoxItem
    {
        private object Tag;
        private string Text;

        public CheckListBoxItem(object obj, string text)
        {
            this.Tag = obj;
            this.Text = text;
        }

        public object getTag()
        {
            return Tag;
        }
        
        public override string ToString() { return Text; }
    }

}
