using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _15211TT0065
{
    public partial class SearchSchedule : Form
    {
        //connection database and variable
        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        bool flag;
        public SearchSchedule()
        {
            InitializeComponent();
            flag = false;
        }

        //Created by Vu Dinh khanh - 31/05/2018 : Tìm kiếm lịch chiếu phim
        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = null;
                SqlParameter para = null;

                //kiểm tra xác nhận phương pháp tìm kiếm
                if (rbtPhim.Checked)
                {
                    cmd = new SqlCommand("sp_SearchScheduleByFilm", con);
                    cmd.CommandText = "sp_SearchScheduleByFilm";
                    para = new SqlParameter("@TenPhim", txtText.Text);
                }
                else
                {
                    cmd = new SqlCommand("sp_SearchScheduleByRoom", con);
                    cmd.CommandText = "sp_SearchScheduleByRoom";
                    para = new SqlParameter("@TenPhong", txtText.Text);
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(para);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt
                dgList.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Loi " + ex.Message, "Thong bao");
            }
            finally
            {
                con.Close(); // dong ket noi
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : Load dữ liệu cho form
        private void SearchSchedule_Load(object sender, EventArgs e)
        {
            dgList.DataSource = LichChieuPhim.LayDSLich(con);
        }

        //Created by Vu Dinh khanh - 31/05/2018 : xác nhận và đóng form
        private void SearchSchedule_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
