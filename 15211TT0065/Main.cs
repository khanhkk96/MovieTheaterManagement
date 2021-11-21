using System;
using System.Windows.Forms;

namespace _15211TT0065
{
    public partial class Main : Form
    {
        public static bool flag;
        public Main()
        {
            InitializeComponent();
            flag = false;
        }

        //Created by Vu Dinh khanh - 30/05/2018: Mở form quản lý thông tin các phòng chiếu phim
        private void tshPhong_Click(object sender, EventArgs e)
        {
            Phong p = new Phong();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }

        }

        //Created by Vu Dinh Khanh - 30/05/2018 : Mở form quản lý thông tin các bộ phim
        private void tshPhim_Click(object sender, EventArgs e)
        {
            Phim p = new Phim();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form quản lý thông tin nhân viên
        private void tshNhanVien_Click(object sender, EventArgs e)
        {
            NhanVien p = new NhanVien();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form quản lý thông tin lịch chiếu phim
        private void tshLich_Click(object sender, EventArgs e)
        {
            LichChieuPhim p = new LichChieuPhim();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form quản lý thông tin khách hàng
        private void tshKhachHang_Click(object sender, EventArgs e)
        {
            KhachHang p = new KhachHang();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }


        //Created by Vu Dinh khanh - 30/05/2018 : Thoát chương trình
        private void tshThoat_Click(object sender, EventArgs e)
        {
            flag = true;
            this.Close();
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form tìm kiếm thông tin nhân viên
        private void tstNhanvien_Click(object sender, EventArgs e)
        {
            TimKiemNhanvien p = new TimKiemNhanvien();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form tìm kiếm thông tin vé đã bán
        private void tstVe_Click(object sender, EventArgs e)
        {
            TimKiemVe p = new TimKiemVe();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form tìm kiếm lịch chiếu phim
        private void tstLich_Click(object sender, EventArgs e)
        {
            TimKiemPhim p = new TimKiemPhim();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form thống kê vé bán ra
        private void tskVe_Click(object sender, EventArgs e)
        {
            ThongKeVe p = new ThongKeVe();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form thống ke doanh thu
        private void tskTien_Click(object sender, EventArgs e)
        {
            ThongKeThu p = new ThongKeThu();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form quản lý danh mục phim
        private void tshTheLoai_Click(object sender, EventArgs e)
        {
            TheLoaiPhim p = new TheLoaiPhim();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form tìm kiếm thông tin khách hàng
        private void tskKhach_Click(object sender, EventArgs e)
        {
            TimKiemKhachHang p = new TimKiemKhachHang();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form quản lý vé
        private void tshVe_Click(object sender, EventArgs e)
        {
            Ve p = new Ve();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }

        //Created by Vu Dinh khanh - 30/05/2018 : thoát chương trình
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            DialogResult dr = MessageBox.Show("Bạn muốn thoát chương trình?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                foreach (Form form in this.MdiChildren)
                {
                    form.Close();
                }
                e.Cancel = false;
            }
            else
            {
                flag = false;
            }
        }

        /// <summary>
        /// kiểm tra sự tồn tại của form
        /// </summary>
        /// <param name="formType">loại form</param>
        /// <returns>bool</returns>
        /// Created by Vu Dinh khanh - 30/05/2018 : Quản lý form của ứng dụng
        public bool IsFormOpen(Type formType)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType().Name == formType.Name)
                {
                    form.Activate();
                    return true;
                }
            }
            return false;
        }

        //Created by Vu Dinh khanh - 30/05/2018 : Mở form tìm kiếm lịch chiếu phim
        private void timKiếmLichChiếuPhimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchSchedule p = new SearchSchedule();
            bool winTest = IsFormOpen(p.GetType());
            if (winTest == false)
            {
                p.MdiParent = this;
                p.Show();
            }
        }
    }
}
