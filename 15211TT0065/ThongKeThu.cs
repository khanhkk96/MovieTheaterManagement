﻿using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _15211TT0065
{
    public partial class ThongKeThu : Form
    {
        //connection database and variable
        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-95AI01F\SQLEXPRESS;Initial Catalog=QUANLYRAPCHIEUPHIM;Integrated Security=True");
        public float tong;
        public ThongKeThu()
        {
            InitializeComponent();

            
        }

        //Created by Vu Dinh khanh - 31/05/2018 : thống kê doanh thu theo thời gian đã chọn
        public DataTable LayDSVe()
        {
            tong = 0;
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_thongTienBanVe", con);
                cmd.CommandText = "sp_thongTienBanVe";
                cmd.CommandType = CommandType.StoredProcedure;

                //tham số truyền vào store procedure
                SqlParameter dateFrom = new SqlParameter("@from", dtpStart.Value);
                cmd.Parameters.Add(dateFrom);
                SqlParameter dateEnd = new SqlParameter("@to", dtpEnd.Value);
                cmd.Parameters.Add(dateEnd);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt
                foreach (DataRow item in dt.Rows)
                {
                    float s = float.Parse(item.ItemArray[1].ToString());
                    tong += s;
                }
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

        //Created by Vu Dinh khanh - 31/05/2018 : thống kê doanh thu trong tháng này
        public DataTable ThongKeThuTien()
        {
            tong = 0;
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_thongTienBanVe2", con);
                cmd.CommandText = "sp_thongTienBanVe2";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt
                foreach (DataRow item in dt.Rows)
                {
                    float s = float.Parse(item.ItemArray[1].ToString());
                    tong += s;
                }
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

        //Created by Vu Dinh khanh - 31/05/2018 : thống kê doanh thu theo thời gian
        private void btnThongke_Click(object sender, EventArgs e)
        {
            if (dtpStart.Value >= dtpEnd.Value)
            {
                MessageBox.Show("Kiểm tra lại thời gian đã chọn!");
            }
            else
            {
                dgvList.DataSource = LayDSVe();
                txtTong.Text = tong.ToString();
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : xác nhận và đóng form
        private void ThongKeThu_FormClosing(object sender, FormClosingEventArgs e)
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

        //Created by Vu Dinh khanh - 31/05/2018 : load dữ liệu lên form
        private void ThongKeThu_Load(object sender, EventArgs e)
        {
            dgvList.DataSource = ThongKeThuTien();
            txtTong.Text = tong.ToString();

            dtpStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime dtResult = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtResult = dtResult.AddMonths(1);
            dtResult = dtResult.AddDays(-(dtResult.Day));
            dtpEnd.Value = dtResult;
        }
    }
}
