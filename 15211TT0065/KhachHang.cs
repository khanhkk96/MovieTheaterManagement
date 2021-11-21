using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using _15211TT0065.Model;

namespace _15211TT0065
{
    public partial class KhachHang : Form
    {
        //connection string and variable
        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-95AI01F\SQLEXPRESS;Initial Catalog=QUANLYRAPCHIEUPHIM;Integrated Security=True");
        static string id;
        bool flag;
        public KhachHang()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Lấy dữ liệu từ bảng khách hàng trong database
        /// </summary>
        /// <param name="con">kết nối csdl</param>
        /// <returns>danh sách khách hàng</returns>
        /// Created by Vu Dinh Khanh - 01/06/2018 : Lấy tất cả thông tin khách hàng
        public static DataTable LayDSKH(SqlConnection con)
        {
            id = null;
            try
            {
                //Collection<Client> list = new Collection<Client>();
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayDSKH", con);
                cmd.CommandText = "sp_LayDSKH";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[dt.Rows.Count - 1].ItemArray[0].ToString();
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

        /// <summary>
        /// Lấy dữ liệu từ bảng khách hàng trong database
        /// Trong cột "họ tên" có kèm theo mã khách hàng
        /// </summary>
        /// <param name="con">kết nối csdl</param>
        /// <returns>danh sách khách hàng</returns>
        /// Created by Vu Dinh Khanh - 01/06/2018 : Lấy tất cả thông tin khách hàng
        public static DataTable LayDSKH2(SqlConnection con)
        {
            id = null;
            try
            {
                //Collection<Client> list = new Collection<Client>();
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayDSKH2", con);
                cmd.CommandText = "sp_LayDSKH2";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[dt.Rows.Count - 1].ItemArray[0].ToString();
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

        //Created by Vu Dinh Khanh - 01/06/2018 : Kiểm tra điệu kiện trước khi thao tác với csdl
        private void test()
        {
            //không được để trống
            flag = true;
            if (txtMa.Text.Length == 0 | txtPhone.Text.Length == 0 | txtTen.Text.Length == 0 | txtEmail.Text.Length == 0 | txtAddress.Text.Length == 0 | cboType.Text.Length == 0)
            {
                MessageBox.Show("Nhập đầy đủ thông tin!");
                flag = false;
                return;
            }

            //xác thực sdt
            if (txtPhone.TextLength < 10 || txtPhone.TextLength > 11)
            {
                MessageBox.Show("Kiểm tra số điện thoại!");
                flag = false;
                return;
            }

            if (!txtPhone.Text.Trim().IsInterger())
            {
                MessageBox.Show("Nhập số điện thoại chưa đúng!");
                flag = false;
                return;
            }

            //xác thực tên khách hàng
            if (!txtTen.Text.Trim().IsString())
            {
                MessageBox.Show("Kiểm tra họ tên khách hàng!");
                flag = false;
                return;
            }

            //kiểm tra email khách hàng
            if (!txtEmail.Text.Trim().Contains('.') || !txtEmail.Text.Trim().Contains('@') ||  txtEmail.Text.Trim().Contains(' '))
            {
                MessageBox.Show("Kiểm tra email khách hàng!");
                flag = false;
                return;
            }
        }

        //Created by Vu Dinh Khanh - 01/06/2018: Thêm khách hàng
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int kq = 0;
            try
            {
                test();
                if (!flag)
                {
                    return;
                }
                // mo ket noi
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_themKH", con);
                cmd.CommandText = "sp_themKH";
                cmd.CommandType = CommandType.StoredProcedure;

                //Nhận các parameters cho procedure
                SqlParameter para_ma = new SqlParameter("@MaKH", txtMa.Text.Trim());
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@TenKH", txtTen.Text.Trim());
                cmd.Parameters.Add(para_ten);
                SqlParameter para_type = new SqlParameter("@LoaiKh", cboType.Text.Trim());
                cmd.Parameters.Add(para_type);
                SqlParameter para_email = new SqlParameter("@Email", txtEmail.Text.Trim());
                cmd.Parameters.Add(para_email);
                SqlParameter para_sdt = new SqlParameter("@SDT", txtPhone.Text.Trim());
                cmd.Parameters.Add(para_sdt);
                SqlParameter para_dc = new SqlParameter("@DiaChi", txtAddress.Text.Trim());
                cmd.Parameters.Add(para_dc);

                // thuc thi thanh cong cong hay khong?
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Thêm thông tin thành công!", "Thông báo");
                    clear();
                    kq++;
                }
                else
                {
                    MessageBox.Show("Thêm thông tin không thành công!", "Thông báo");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra! \n" + ex.Message, "Thông báo");
            }
            finally
            {
                // dong ket noi
                con.Close();
            }
            //kiểm tra kết quả thao tác và load dữ liệu lên view
            if (kq != 0)
            {
                dgvList.DataSource = LayDSKH(con);
                AutoCode code = new AutoCode("KH", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh Khanh - 01/06/2018 : Sửa thông tin khách hàng
        private void btnEdit_Click(object sender, EventArgs e)
        {
            int kq = 0;
            try
            {
                test();
                if (!flag)
                {
                    return;
                }
                // mo ket noi
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SuaKH", con);
                cmd.CommandText = "sp_SuaKH";
                cmd.CommandType = CommandType.StoredProcedure;

                //Nhận các parameters cho procedure
                SqlParameter para_ma = new SqlParameter("@MaKH", txtMa.Text.Trim());
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@TenKH", txtTen.Text.Trim());
                cmd.Parameters.Add(para_ten);
                SqlParameter para_type = new SqlParameter("@LoaiKh", cboType.Text.Trim());
                cmd.Parameters.Add(para_type);
                SqlParameter para_email = new SqlParameter("@Email", txtEmail.Text.Trim());
                cmd.Parameters.Add(para_email);
                SqlParameter para_sdt = new SqlParameter("@SDT", txtPhone.Text.Trim());
                cmd.Parameters.Add(para_sdt);
                SqlParameter para_dc = new SqlParameter("@DiaChi", txtAddress.Text.Trim());
                cmd.Parameters.Add(para_dc);

                // thuc thi thanh cong cong hay khong?
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Sửa thông tin thành công!", "Thông báo");
                    clear();
                    kq++;
                }
                else
                {
                    MessageBox.Show("Sửa thông tin không thành công!", "Thông báo");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra! \n" + ex.Message, "Thông báo");
            }
            finally
            {
                // dong ket noi
                con.Close();
            }

            //kiểm tra kết quả thao tác và load dữ liệu lên view
            if (kq != 0)
            {
                dgvList.DataSource = LayDSKH(con);
                AutoCode code = new AutoCode("KH", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh Khanh - 01/06/2018 : Xóa thông tin khách hàng
        private void btnDel_Click(object sender, EventArgs e)
        {
            //xác thực yêu cầu
            DialogResult result = MessageBox.Show("Bạn muốn xóa thông tin khách hàng!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                int kq = 0;
                try
                {
                    // mo ket noi
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_xoaKH", con);
                    cmd.CommandText = "sp_xoaKH";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Nhận mã để xóa
                    SqlParameter para_ma = new SqlParameter("@ma", txtMa.Text);
                    cmd.Parameters.Add(para_ma);

                    // thuc thi thanh cong cong hay khong?
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo");
                        clear();
                        kq++;
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công!", "Thông báo");
                    }
                }
                catch (Exception ex)
                {
                    //try
                    //{
                    //    SqlCommand cmd2 = new SqlCommand("sp_xoaKH2", con);
                    //    cmd2.CommandText = "sp_xoaKH2";
                    //    cmd2.CommandType = CommandType.StoredProcedure;

                    //    SqlParameter para_ma2 = new SqlParameter("@ma", txtMa.Text);
                    //    cmd2.Parameters.Add(para_ma2);

                    //    // thuc thi thanh cong cong hay khong?
                    //    if (cmd2.ExecuteNonQuery() > 0)
                    //    {
                    //        MessageBox.Show("Xóa thành công!", "Thông báo");
                    //        clear();
                    //        kq++;
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Xóa không thành công!", "Thông báo");
                    //    }
                    //}
                    //catch (Exception ex2)
                    //{
                        MessageBox.Show("Có lỗi xảy ra! \n" + ex.Message, "Thông báo");
                    //}
                }
                finally
                {
                    // dong ket noi
                    con.Close();
                }

                //kiểm tra kết quả thao tác và load dữ liệu lên view
                if (kq != 0)
                {
                    dgvList.DataSource = LayDSKH(con);
                    AutoCode code = new AutoCode("KH", id);
                    txtMa.Text = code.ToString();
                }
            }
        }

        //Created by Vu Dinh Khanh - 01/06/2018 : show dữ liệu theo dòng được chọn
        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cboType.SelectedItem = null;
            int dong = dgvList.CurrentCell.RowIndex;
            txtMa.Text = dgvList.Rows[dong].Cells[0].Value.ToString();
            txtTen.Text = dgvList.Rows[dong].Cells[1].Value.ToString();
            cboType.Text = dgvList.Rows[dong].Cells[2].Value.ToString();
            txtEmail.Text = dgvList.Rows[dong].Cells[3].Value.ToString();
            txtPhone.Text = dgvList.Rows[dong].Cells[4].Value.ToString();
            txtAddress.Text = dgvList.Rows[dong].Cells[5].Value.ToString();

            //dòng trống dữ liệu, hiển thị mã khách hàng tiếp theo
            if (dong > dgvList.RowCount - 2)
            {
                AutoCode code = new AutoCode("KH", id);
                txtMa.Text = code.ToString();
            }
        }

        //clear dữ liễu trên các trường nhập và chọn
        private void clear()
        {
            txtAddress.Clear();
            txtEmail.Clear();
            txtMa.Clear();
            txtPhone.Clear();
            txtTen.Clear();
            cboType.SelectedItem = null;
        }


        //Created by Vu Dinh Khanh - 01/06/2018 : Load data for form
        private void KhachHang_Load(object sender, EventArgs e)
        {
            dgvList.DataSource = LayDSKH(con);
            AutoCode code = new AutoCode("KH", id);
            txtMa.Text = code.ToString();
        }

        //Confirm exit request
        private void KhachHang_FormClosing(object sender, FormClosingEventArgs e)
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
            else{
                e.Cancel = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            AutoCode code = new AutoCode("KH", id);
            txtMa.Text = code.ToString();
        }
    }
}
