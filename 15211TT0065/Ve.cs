using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using _15211TT0065.Model;

namespace _15211TT0065
{
    public partial class Ve : Form
    {
        //connection database and variable
        SqlConnection con = new SqlConnection(AppConst.AppConnectionString);
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-95AI01F\SQLEXPRESS;Initial Catalog=QUANLYRAPCHIEUPHIM;Integrated Security=True");
        static string id;
        //Lich lich = null;
        string code;
        int soGhe = 0;
        double gia = 0;
        double discount = 0;
        DateTime time;
        Collection<Client> clients;
        bool? isEdit = null;

        public Ve()
        {
            InitializeComponent();
        }

        //Created by Vu Dinh khanh - 31/05/2018 : lấy danh sách vé bán ra(tên khách hàng và nhân viên có kèm theo mã)
        public static DataTable LayDSVe(SqlConnection con)
        {
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayDSVe", con);
                cmd.CommandText = "sp_LayDSVe";
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

        //Created by Vu Dinh khanh - 31/05/2018 : Lấy danh sách vé bán ra
        public DataTable LayDSVe2()
        {
            code = null;
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayDSVe2", con);
                cmd.CommandText = "sp_LayDSVe2";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);// fill cho bang dt
                if (dt.Rows.Count > 0)
                {
                    if (txtMa.Text.Length != 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i].ItemArray[0].ToString() == txtMa.Text)
                            {
                                code = dt.Rows[i].ItemArray[1].ToString();
                                break;
                            }
                        }
                    }
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

        //Created by Vu Dinh khanh - 31/05/2018 : tìm kiếm lịch chiếu phim
        public string LayDSLich()
        {
            string name = null;
            try
            {
                con.Open();// mo ket noi
                SqlCommand cmd = new SqlCommand("sp_LayLich", con);
                cmd.CommandText = "sp_LayLich";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt
                //tìm kiếm bộ phim và thời lượng
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[dt.Rows.Count - 1].ItemArray[0].ToString();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i].ItemArray[0].ToString() == code)
                        {
                            name = dt.Rows[i].ItemArray[4].ToString();
                            time = DateTime.Parse(dt.Rows[i].ItemArray[2].ToString());
                            if (time.CompareTo(DateTime.Now) > 0)
                            {
                                isEdit = true;
                            }
                            else
                            {
                                isEdit = false;
                            }
                            break;
                        }
                    }
                }
                return name; // tra ve bang
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

        //Created by Vu Dinh khanh - 31/05/2018 : load dữ liệu lên các trường nhập theo dòng dữ liệu đã chọn
        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //clear selectedItem của các combobox
            cboSlot.SelectedItem = null;
            cboStaff.SelectedItem = null;
            cboClient.SelectedItem = null;

            int dong = dgvList.CurrentCell.RowIndex;
            if (dong == dgvList.RowCount - 1)
            {
                txtLich.Clear();
                txtTien.Clear();
                return;
            }
            txtMa.Text = dgvList.Rows[dong].Cells[0].Value.ToString();
            txtLich.Text = dgvList.Rows[dong].Cells[1].Value.ToString();

            //tìm kiếm selected value cho cboClient
            string kh = dgvList.Rows[dong].Cells[2].Value.ToString();
            foreach (DataRow item in (cboClient.DataSource as DataTable).Rows)
            {
                if (item.ItemArray[1].ToString() == kh)
                {
                    cboClient.SelectedValue = item.ItemArray[0].ToString();
                    break;
                }
            }

            //tìm kiếm selected value cho cboStaff
            string nv = dgvList.Rows[dong].Cells[3].Value.ToString();
            foreach (DataRow item in (cboStaff.DataSource as DataTable).Rows)
            {
                if (item.ItemArray[1].ToString() == nv)
                {
                    cboStaff.SelectedValue = item.ItemArray[0].ToString();
                    break;
                }
            }

            //tìm kiếm lịch chiếu phim khi biết mã vé
            string slot = dgvList.Rows[dong].Cells[5].Value.ToString();
            LayDSVe2();
            searchSlotNotAvailable();
            string name = LayDSLich();
            searchRoom(name);
            takeData();
            cboSlot.Items.Add(slot);
            cboSlot.SelectedItem = slot;

            txtTien.Text = dgvList.Rows[dong].Cells[6].Value.ToString();
            
            if (dong > dgvList.RowCount - 2)
            {
                AutoCode code = new AutoCode("VX", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : đặt vé mới
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int kq = 0;
            try
            {
                if (txtMa.Text.Length == 0 | cboSlot.SelectedItem == null | txtLich.Text.Length == 0 | cboStaff.SelectedValue == null | cboClient.Text.Length == 0)
                {
                    MessageBox.Show("Nhập đầy đủ thông tin!");
                    return;
                }

                // mo ket noi
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_themVe", con);
                cmd.CommandText = "sp_themVe";
                cmd.CommandType = CommandType.StoredProcedure;

                //tham số truyền vào store procedure
                SqlParameter para_ma = new SqlParameter("@MaVe", txtMa.Text);
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@MaLich", code);
                cmd.Parameters.Add(para_ten);
                SqlParameter para_kh = new SqlParameter("@KhachHang", cboClient.SelectedValue);
                cmd.Parameters.Add(para_kh);
                SqlParameter para_loai = new SqlParameter("@NhanVien", cboStaff.SelectedValue);
                cmd.Parameters.Add(para_loai);
                SqlParameter para_tg = new SqlParameter("@SaleTime", DateTime.Now);
                cmd.Parameters.Add(para_tg);
                SqlParameter para_nd = new SqlParameter("@Slot", cboSlot.Text);
                cmd.Parameters.Add(para_nd);
                SqlParameter para_t = new SqlParameter("@Tien", float.Parse(txtTien.Text));
                cmd.Parameters.Add(para_t);

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
                dgvList.DataSource = LayDSVe(con);
                AutoCode code = new AutoCode("VX", id);
                txtMa.Text = code.ToString();
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : Sửa lại thông tin vé
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (isEdit == null)
            {
                MessageBox.Show("Chọn vé muốn chỉnh sửa!");
                return;
            }
            else if (isEdit == false)
            {
                MessageBox.Show("Vé hết hạn!");
                return;
            }

            int kq = 0;
            try
            {
                if (txtMa.Text.Length == 0 | cboSlot.SelectedItem == null | txtLich.Text.Length == 0 | cboStaff.SelectedValue == null | cboClient.Text.Length == 0)
                {
                    MessageBox.Show("Nhập đầy đủ thông tin!");
                    return;
                }
                
                // mo ket noi
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SuaVe", con);
                cmd.CommandText = "sp_SuaVe";
                cmd.CommandType = CommandType.StoredProcedure;

                //tham số truyền vào store procedure
                SqlParameter para_ma = new SqlParameter("@MaVe", txtMa.Text);
                cmd.Parameters.Add(para_ma);
                SqlParameter para_ten = new SqlParameter("@MaLich", code);
                cmd.Parameters.Add(para_ten);
                SqlParameter para_kh = new SqlParameter("@KhachHang", cboClient.SelectedValue);
                cmd.Parameters.Add(para_kh);
                SqlParameter para_loai = new SqlParameter("@NhanVien", cboStaff.SelectedValue);
                cmd.Parameters.Add(para_loai);
                SqlParameter para_tg = new SqlParameter("@SaleTime", DateTime.Now);
                cmd.Parameters.Add(para_tg);
                SqlParameter para_nd = new SqlParameter("@Slot", cboSlot.Text);
                cmd.Parameters.Add(para_nd);
                SqlParameter para_t = new SqlParameter("@Tien", float.Parse(txtTien.Text));
                cmd.Parameters.Add(para_t);

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

            //kiểm tra kết quả và load lại dữ liệu
            if (kq != 0)
            {
                dgvList.DataSource = LayDSVe(con);
                AutoCode code = new AutoCode("VX", id);
                txtMa.Text = code.ToString();
            }

            isEdit = null;
        }

        //Created by Vu Dinh khanh - 31/05/2018 : hủy vé có điều kiện
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (isEdit == null)
            {
                MessageBox.Show("Chọn vé muốn xóa!");
                return;
            }
            else if (isEdit == false)
            {
                MessageBox.Show("Vé hết hạn!");
                return;
            }

            DialogResult result = MessageBox.Show("Bạn muốn hủy vé?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                if ((time.Subtract(DateTime.Now)).Hours >= 1)
                {
                    int kq = 0;
                    try
                    {
                        // mo ket noi
                        con.Open();
                        SqlCommand cmd = new SqlCommand("sp_xoaVe", con);
                        cmd.CommandText = "sp_xoaVe";
                        cmd.CommandType = CommandType.StoredProcedure;

                        //tham số truyền vào store
                        SqlParameter para_ma = new SqlParameter("@ma", txtMa.Text);
                        cmd.Parameters.Add(para_ma);

                        // thuc thi thanh cong cong hay khong?
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Hủy thành công!", "Thông báo");
                            clear();
                            kq++;
                        }
                        else
                        {
                            MessageBox.Show("Hủy không thành công!", "Thông báo");
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
                        dgvList.DataSource = LayDSVe(con);
                        AutoCode code = new AutoCode("VX", id);
                        txtMa.Text = code.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Không đủ điều kiện để hủy vé!");

                }
            }

            isEdit = null;
        }

        //Created by Vu Dinh khanh - 31/05/2018 : xóa dữ liệu trên các trường nhập
        private void clear()
        {
            txtMa.Clear();
            txtLich.Clear();
            cboSlot.SelectedItem = null;
            cboStaff.SelectedItem = null;
            cboClient.SelectedItem = null;
            txtTien.Clear();
            cboSlot.Items.Clear();
        }

        //Created by Vu Dinh khanh - 31/05/2018 : load dữ liệu lên form
        private void Ve_Load(object sender, EventArgs e)
        {
            clients = new Collection<Client>();
            dgvList.DataSource = LayDSVe(con);
            cboStaff.DataSource = NhanVien.LayDSNV2(con);
            DataTable dt = KhachHang.LayDSKH2(con);
            foreach (DataRow item in dt.Rows)
            {
                Client l = new Client();
                l.Code = item.ItemArray[0].ToString();
                l.Name = item.ItemArray[1].ToString();
                l.Type = item.ItemArray[2].ToString();
                l.Email = item.ItemArray[3].ToString();
                l.Phone = item.ItemArray[4].ToString();
                l.Address = item.ItemArray[5].ToString();
                clients.Add(l);
            }
            cboClient.DataSource = dt;
            AutoCode code = new AutoCode("VX", id);
            txtMa.Text = code.ToString();

            //clear selectedItem của các combobox
            cboSlot.SelectedItem = null;
            cboStaff.SelectedItem = null;
            cboClient.SelectedItem = null;
        }

        //Created by Vu Dinh khanh - 31/05/2018 : mở form để chọn lịch chiếu phim
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SelectFilm p = new SelectFilm();
            p.MdiParent = ParentForm;
            p.Show();
            p.sendCode = receiveFilm;
        }

        //Created by Vu Dinh khanh - 31/05/2018 : nhận lịch chiếu phim đã chọn
        private void receiveFilm(Lich lich)
        {
            code = null;
            if (lich != null)
            {
                this.code = lich.Code;
                txtLich.Text = lich.Film + " - " + lich.Start.ToString();
                searchRoom(lich.Room);
                takeData();
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : lấy danh sách ghế còn trống
        private void takeData()
        {
            cboSlot.Items.Clear();
            if (soGhe > 0)
            {
                Collection<string> list = searchSlotNotAvailable();
                if (list != null)
                {
                    if (list.Count() > 0)
                    {
                        for (int i = 1; i <= soGhe; i++)
                        {
                            int test = 0;
                            foreach (string s in list)
                            {
                                if (s == i.ToString())
                                {
                                    test++;
                                    break;
                                }
                            }
                            if (test == 0)
                            {
                                cboSlot.Items.Add(i + "");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= soGhe; i++)
                        {
                            cboSlot.Items.Add(i + "");
                        }
                    }
                }
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : xác nhận và đóng form
        private void Ve_FormClosing(object sender, FormClosingEventArgs e)
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

        //private void searchShchen

        //Created by Vu Dinh khanh - 31/05/2018 : Lấy danh sách ghế ngồi KHÔNG còn trống
        public Collection<string> searchSlotNotAvailable()
        {
            Collection<string> list = null;
            if (code != null)
            {
                list = new Collection<string>();
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_SearchTicketSaled", con);
                    cmd.CommandText = "sp_SearchTicketSaled";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter ma = new SqlParameter("@MaLich", code);
                    cmd.Parameters.Add(ma);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);// fill cho bang dt
                    foreach (DataRow item in dt.Rows)
                    {
                        int slot = int.Parse(item.ItemArray[0].ToString());
                        list.Add(slot.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi! \n" + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return list;
        }

        //Created by Vu Dinh khanh - 31/05/2018 : tìm kiếm phòng chiếu phim khi biết tên phòng
        public void searchRoom(string name)
        {
            soGhe = 0;
            gia = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SearchRoomByName", con);
                cmd.CommandText = "sp_SearchRoomByName";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter ma = new SqlParameter("@TenPhong", name);
                cmd.Parameters.Add(ma);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);// fill cho bang dt
                foreach (DataRow item in dt.Rows)
                {
                    soGhe = int.Parse(item.ItemArray[0].ToString());
                    gia = double.Parse(item.ItemArray[1].ToString());
                    txtTien.Text = (gia - gia * discount).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi! \n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        //Created by Vu Dinh khanh - 31/05/2018 : cập nhật giá tiền theo loại kh
        private void cboClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cboClient.SelectedIndex;
            if (index > -1)
            {
                if (gia > 0)
                {
                    double money = 0;

                    if (clients.ElementAt(index).Type == "Normal")
                    {
                        money = gia;
                        
                    }
                    else if (clients.ElementAt(index).Type == "Friendly")
                    {
                        discount = 0.2d;
                        money = gia - gia * 0.2;
                    }
                    else
                    {
                        discount = 0.4d;
                        money = gia - gia * 0.4;
                    }

                    txtTien.Text = money.ToString();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            AutoCode code = new AutoCode("VX", id);
            txtMa.Text = code.ToString();
        }
    }
}
