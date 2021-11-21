using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using _15211TT0065.Model;

namespace _15211TT0065
{
    public partial class NhanVien : Form
    {
        //connection and variable
        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-95AI01F\SQLEXPRESS;Initial Catalog=QUANLYRAPCHIEUPHIM;Integrated Security=True");
        static string id;
        bool flag;
        public NhanVien()
        {
            InitializeComponent();
        }

       
        //Created by Vu Dinh khanh - 30/05/2018 : Lấy danh sách nhân viên
        public static DataTable LayDSNV(SqlConnection con)
        {
            id = null;
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayDSNV", con);
                cmd.CommandText = "sp_LayDSNV";
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

        //Created by Vu Dinh khanh - 30/05/2018 : Lấy danh sách nhân viên(cột họ tên có kèm mã)
        public static DataTable LayDSNV2(SqlConnection con)
        {
            id = null;
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayDSNV2", con);
                cmd.CommandText = "sp_LayDSNV2";
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

        //Created by Vu Dinh khanh - 30/05/2018 : kiểm tra các trường nhập thông tin
        private void test()
        {
            flag = true;
            if (txtMa.Text.Length == 0 | txtPhone.Text.Length == 0 | txtTen.Text.Length == 0 | txtEmail.Text.Length == 0 | txtAddress.Text.Length == 0)
            {
                MessageBox.Show("Nhập đầy đủ thông tin!");
                flag = false;
                return;
            }

            if (!txtPhone.Text.Trim().IsInterger() || txtPhone.Text.Length < 10 || txtPhone.Text.Length > 11)
            {
                MessageBox.Show("Nhập số điện thoại chưa đúng!");
                flag = false;
                return;
            }

            if (!txtTen.Text.Trim().IsString())
            {
                MessageBox.Show("Kiểm tra họ tên khách hàng!");
                flag = false;
                return;
            }

            if (!txtEmail.Text.Trim().Contains('.') || !txtEmail.Text.Trim().Contains('@') || txtEmail.Text.Trim().Contains(' '))
            {
                MessageBox.Show("Kiểm tra email khách hàng!");
                flag = false;
                return;
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Thêm một nhân viên mới
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int kq = 0;
            try
            {

                test();
                if(!flag)
                {
                    return;
                }
                // mo ket noi
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_themNV", con);
                cmd.CommandText = "sp_themNV";
                cmd.CommandType = CommandType.StoredProcedure;

                //Cac parameters trong store procedure
                SqlParameter para_ma = new SqlParameter("@MaNV", txtMa.Text.Trim());
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@TenNV", txtTen.Text.Trim());
                cmd.Parameters.Add(para_ten);

                string i = null;
                if (rbtNu.Checked)
                {
                    i = "Nữ";
                }
                else
                {
                    i = "Nam";
                }

                SqlParameter para_sex = new SqlParameter("@GioiTinh", i);
                cmd.Parameters.Add(para_sex);
                SqlParameter para_email = new SqlParameter("@Email", txtEmail.Text.Trim());
                cmd.Parameters.Add(para_email);
                SqlParameter para_sdt = new SqlParameter("@SDT", txtPhone.Text.Trim());
                cmd.Parameters.Add(para_sdt);
                SqlParameter para_dc = new SqlParameter("@DiaChi", txtAddress.Text.Trim());
                cmd.Parameters.Add(para_dc);
                SqlParameter para_ha = new SqlParameter("@HinhAnh", pbImage.Image.ImageToByteArray());
                cmd.Parameters.Add(para_ha);

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

            //kiểm tra kết quả và load lại dữ liệu
            if (kq != 0)
            {
                dgvList.DataSource = LayDSNV(con);
                AutoCode code = new AutoCode("NV", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Sửa thông tin nhân viên
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
                SqlCommand cmd = new SqlCommand("sp_SuaNV", con);
                cmd.CommandText = "sp_SuaNV";
                cmd.CommandType = CommandType.StoredProcedure;

                //Cac parameters trong store procedure
                SqlParameter para_ma = new SqlParameter("@MaNV", txtMa.Text);
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@TenNV", txtTen.Text.Trim());
                cmd.Parameters.Add(para_ten);

                string i = null;
                if (rbtNu.Checked)
                {
                    i = "Nữ";
                }
                else
                {
                    i = "Nam";
                }

                SqlParameter para_sex = new SqlParameter("@GioiTinh", i);
                cmd.Parameters.Add(para_sex);
                SqlParameter para_email = new SqlParameter("@Email", txtEmail.Text.Trim());
                cmd.Parameters.Add(para_email);
                SqlParameter para_sdt = new SqlParameter("@SDT", txtPhone.Text.Trim());
                cmd.Parameters.Add(para_sdt);
                SqlParameter para_dc = new SqlParameter("@DiaChi", txtAddress.Text.Trim());
                cmd.Parameters.Add(para_dc);
                SqlParameter para_ha = new SqlParameter("@HinhAnh", pbImage.Image.ImageToByteArray());
                cmd.Parameters.Add(para_ha);

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

            //kiểm  tra kết quả và load lại dữ liệu
            if (kq != 0)
            {
                dgvList.DataSource = LayDSNV(con);
                AutoCode code = new AutoCode("NV", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : xóa thông tin nhân viên
        private void btnDel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn xóa nhân viên!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                int kq = 0;
                try
                {
                    // mo ket noi
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_xoaNV", con);
                    cmd.CommandText = "sp_xoaNV";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Cac parameters trong store procedure
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
                    //    SqlCommand cmd = new SqlCommand("sp_xoaNV2", con);
                    //    cmd.CommandText = "sp_xoaNV2";
                    //    cmd.CommandType = CommandType.StoredProcedure;

                    //    SqlParameter para_ma = new SqlParameter("@MaNV", txtMa.Text);
                    //    cmd.Parameters.Add(para_ma);

                    //    // thuc thi thanh cong cong hay khong?
                    //    if (cmd.ExecuteNonQuery() > 0)
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

                //kiểm  tra kết quả và load lại dữ liệu
                if (kq != 0)
                {
                    dgvList.DataSource = LayDSNV(con);
                    AutoCode code = new AutoCode("NV", id);
                    txtMa.Text = code.ToString();
                }
            }
        }

        private void NhanVien_Load(object sender, EventArgs e)
        {
            dgvList.DataSource = LayDSNV(con);

            AutoCode code = new AutoCode("NV", id);
            txtMa.Text = code.ToString();

            DataGridViewImageColumn imageCol = (DataGridViewImageColumn)dgvList.Columns[6];
            imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom; // will do the trick
        }

        //Created by Vu Dinh khanh - 30/05/2018 : load dữ liệu lên các control trong form theo dòng dữ liệu đã chọn
        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int dong = dgvList.CurrentCell.RowIndex;
            txtMa.Text = dgvList.Rows[dong].Cells[0].Value.ToString();
            txtTen.Text = dgvList.Rows[dong].Cells[1].Value.ToString();
            string i = dgvList.Rows[dong].Cells[2].Value.ToString();
            if (i == "Nữ")
            {
                rbtNu.Checked = true;
            }
            else
            {
                rbtNam.Checked = true;
            }

            txtEmail.Text = dgvList.Rows[dong].Cells[3].Value.ToString();
            txtPhone.Text = dgvList.Rows[dong].Cells[4].Value.ToString();
            txtAddress.Text = dgvList.Rows[dong].Cells[5].Value.ToString();

            //hình ảnh
            if (dgvList.Rows[dong].Cells[5].Value.ToString().Length > 0)
            {
                byte[] img = (byte[])dgvList.Rows[dong].Cells[6].Value;
                pbImage.Image = Test.ByteArrayToImage(img);
            }
            else
            {
                pbImage.Image = null;
            }

            //auto code
            if (dong > dgvList.RowCount - 2)
            {
                AutoCode code = new AutoCode("NV", id);
                txtMa.Text = code.ToString();
            }

        }

        //Created by Vu Dinh khanh - 30/05/2018 : xóa dữ liệu đang hiển thị trên các control
        private void clear()
        {
            txtAddress.Clear();
            txtEmail.Clear();
            txtMa.Clear();
            txtPhone.Clear();
            txtTen.Clear();
            rbtNu.Checked = true;
            pbImage.Image = null;
        }
        //Created by Vu Dinh khanh - 30/05/2018 : xác nhận và đóng form
        private void NhanVien_FormClosing(object sender, FormClosingEventArgs e)
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

        //Created by Vu Dinh khanh - 30/05/2018 : chọn ảnh cho nhân viên
        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "Image File|*.png; *.bmp; *.gif; *.jpg; *.jpeg; ";
            op.ShowDialog();
            if (op.FileName.Length > 0)
            {
                pbImage.Image = Image.FromFile(op.FileName);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            AutoCode code = new AutoCode("NV", id);
            txtMa.Text = code.ToString();
        }
        
    }

}
