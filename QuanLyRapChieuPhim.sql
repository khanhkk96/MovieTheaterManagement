--tao database
create database QUANLYRAPCHIEUPHIM
go

--thao tac voi database
use QUANLYRAPCHIEUPHIM
go

--tao bang phim
create table PHIM(MaPhim varchar(10) primary key not null, TenPhim nvarchar(128), ThoiLuong time, TheLoai varchar(10), NoiDung nvarchar(MAX), HinhAnh image, IsDel int)
go

--tao bang the loai phim
create table THELOAI(MaTheLoai varchar(10) primary key not null, TenTheLoaiPhim nvarchar(128), IsDel int)
go


--tao bang khach hang
create table KHACHHANG(MaKH varchar(10) primary key not null, TenKH nvarchar(128), LoaiKh nvarchar(50), Email nvarchar(128), SDT nvarchar(20), DiaChi nvarchar(128), IsDel int)
go


--tao bang khach hang
create table NHANVIEN(MaNV varchar(10) primary key not null, TenNV nvarchar(128), GioiTinh nvarchar(20), Email nvarchar(128), SDT nvarchar(20), DiaChi nvarchar(128), HinhAnh image, IsDel int)
go


--tao bang phong chieu phim
create table PHONG(MaPhong varchar(10) primary key not null, TenPhong nvarchar(128),LoaiPhong nvarchar(50),Gia float, SoGhe int, IsDel int)
go


--tao bang lich chieu phim
create table LICHCHIEUPHIM(MaLich varchar(10) primary key not null, MaPhim varchar(10), BatDau datetime, MaNV varchar(10), MaPhong varchar(10), IsDel int)
go


--tao bang ve
create table VE(MaVe varchar(10) primary key not null, MaLich varchar(10), KhachHang varchar(10), NhanVien varchar(10), SaleTime datetime, Slot int, Tien float)
go

--tao rang buoc

--tao unique cho bang phong
alter table PHONG add constraint un_name_room unique(TenPhong)

--tao khoa ngoai giua bang PHIM va THELOAI
alter table PHIM 
Add constraint fk_phim_theloai foreign key (TheLoai) references THELOAI(MaTheLoai)
go

--tao khoa ngoai giua bang VE va KHACHHANG
alter table VE 
Add constraint fk_ve_khachhang foreign key (KhachHang) references KHACHHANG(MaKH)
go

--tao khoa ngoai giua bang VE va NHANVIEN
alter table VE 
Add constraint fk_ve_nhanvien foreign key (NhanVien) references NHANVIEN(MaNV)
go


--tao khoa ngoai giua bang VE va LICHCHIEUPHIM
alter table VE 
Add constraint fk_ve_lich foreign key (MaLich) references LICHCHIEUPHIM(MaLich)
go

--tao khoa ngoai giua bang LICHCHIEUPHIM va PHIM
alter table LICHCHIEUPHIM 
Add constraint fk_lich_phim foreign key (MaPhim) references PHIM(MaPhim)
go

--tao khoa ngoai giua bang LICHCHIEUPHIM va NHANVIEN
alter table LICHCHIEUPHIM 
Add constraint fk_lich_nhanvien foreign key (MaNV) references NHANVIEN(MaNV)
go

--tao khoa ngoai giua bang LICHCHIEUPHIM va PHONG
alter table LICHCHIEUPHIM 
Add constraint fk_lich_phong foreign key (MaPhong) references PHONG(MaPhong)
go


-- viet store

------------------------- Phim -------------------------------

--store them phim
go
create proc sp_themPhim(@MaPhim varchar(10),
 @TenPhim nvarchar(128),
 @ThoiLuong time,
   @TheLoai varchar(10),
    @NoiDung nvarchar(MAX),
	@HinhAnh image)
 as
 insert into PHIM values (@MaPhim, @TenPhim,Convert(time, @ThoiLuong, 108), @TheLoai, @NoiDung, @HinhAnh, 0)
 
-- store lay danh sach phim
go
create proc sp_LayDSPhim
as 
select MaPhim as N'Mã phim', TenPhim as N'Tên phim', ThoiLuong as N'Thời lượng', t.TenTheLoaiPhim as N'Thể Loại', NoiDung as N'Nội dung', HinhAnh as N'Hình ảnh'  from PHIM p, THELOAI t where p.TheLoai = t.MaTheLoai and p.IsDel = 0
	
-- store xoa 1 phim
go
create proc sp_xoaPhim(@ma varchar(10))
as
  delete
  From PHIM
  where MaPhim = @ma
  
    --store xoa hien thi thong tin phim
  go
  create proc sp_xoaPhim2(@ma varchar(10))
  as 
  update PHIM
  set IsDel = -1
  where MaPhim = @ma

  --store sua thong tin phim
  go
  create proc sp_SuaPhim(@MaPhim varchar(10),
 @TenPhim nvarchar(128),
  @ThoiLuong time,
   @TheLoai varchar(10),
    @NoiDung nvarchar(MAX),
	@HinhAnh image)
  as 
  update PHIM
  set TenPhim = @TenPhim, ThoiLuong = Convert(time, @ThoiLuong, 108), TheLoai = @TheLoai, NoiDung = @NoiDung, HinhAnh = @HinhAnh
  where MaPhim = @MaPhim


------------------------- TheLoai -------------------------------

--store them the loai
go
create proc sp_themTheLoai(@MaTheLoai varchar(10), @TenTheLoaiPhim nvarchar(128))
 as
 insert into THELOAI values (@MaTheLoai, @TenTheLoaiPhim, 0)
 
-- store lay danh sach the loai
go
create proc sp_LayDSTheLoai
as 
select MaTheLoai as N'Mã thể loại', TenTheLoaiPhim as N'Tên thể loại' from THELOAI where IsDel = 0
	
-- store xoa 1 the loai
go
create proc sp_xoaTheLoai(@ma varchar(10))
as
  delete
  From THELOAI
  where MaTheLoai = @ma
  
--store xoa hien thi thong tin the loai
  go
  create proc sp_xoaTL2(@ma varchar(10))
  as 
  update THELOAI
  set IsDel = -1
  where MaTheLoai = @ma
  
  --store sua ten the loai
  go
  create proc sp_SuaTheLoai(@MaTheLoai varchar(10), @TenTheLoaiPhim nvarchar(128))
  as 
  update THELOAI
  set TenTheLoaiPhim = @TenTheLoaiPhim
  where MaTheLoai = @MaTheLoai


------------------------- Khach hang -------------------------------

--store them khach hang
go
create proc sp_themKH(@MaKH varchar(10), @TenKH nvarchar(128), @LoaiKh nvarchar(50), @Email nvarchar(128), @SDT nvarchar(20), @DiaChi nvarchar(128))
 as
 insert into KHACHHANG values (@MaKH, @TenKH, @LoaiKh, @Email, @SDT, @DiaChi, 0)
 
-- store lay danh sach khach hang
go
create proc sp_LayDSKH
as 
select MaKH as N'Mã khách hàng', TenKH as N'Họ tên', LoaiKH as N'Loại khách hàng', Email, SDT as N'Số điện thoại', DiaChi as N'Địa chỉ' from KHACHHANG where IsDel = 0

--store lay danh sach khach hang 2
go
create proc sp_LayDSKH2
as 
select MaKH as N'Mã khách hàng', TenKH + ' ['+ MaKH +']' as N'Họ tên', LoaiKH as N'Loại khách hàng', Email, SDT as N'Số điện thoại', DiaChi as N'Địa chỉ' from KHACHHANG where IsDel = 0

	
-- store xoa 1 khach hang
go
create proc sp_xoaKH(@ma varchar(10))
as
  delete
  From KHACHHANG
  where MaKH = @ma
  
   --store xoa hien thi thong tin kh
  go
  create proc sp_xoaKH2(@ma varchar(10))
  as 
  update KHACHHANG
  set IsDel = -1
  where MaKH = @ma
  
  --store sua thong tin kh
  go
  create proc sp_SuaKH(@MaKH varchar(10), @TenKH nvarchar(128), @LoaiKh nvarchar(50), @Email nvarchar(128), @SDT nvarchar(20), @DiaChi nvarchar(128))
  as 
  update KHACHHANG
  set TenKH = @TenKH, LoaiKh = @LoaiKh, Email = @Email, SDT = @SDT, DiaChi = @DiaChi
  where MaKH = @MaKH

------------------------- Nhan vien -------------------------------

--store them nv
go
create proc sp_themNV(@MaNV varchar(10), @TenNV nvarchar(128), @GioiTinh nvarchar(20), @Email nvarchar(128), @SDT nvarchar(20), @DiaChi nvarchar(128), @HinhAnh image)
 as
 insert into NHANVIEN values (@MaNV, @TenNV, @GioiTinh, @Email, @SDT, @DiaChi, @HinhAnh, 0)

-- store lay danh sach nv
go
create proc sp_LayDSNV
as 
select MaNV as N'Mã nhân viên', TenNV as N'Họ tên', GioiTinh as N'Giới tính', Email, SDT as N'Số điện thoại', DiaChi as N'Địa chỉ', HinhAnh as N'Hình ảnh' from NHANVIEN where IsDel = 0

-- store lay danh sach nv 2
go
create proc sp_LayDSNV2
as 
select MaNV as N'Mã nhân viên', TenNV + ' [' + MaNV + ']' as N'Họ tên', GioiTinh as N'Giới tính', Email, SDT as N'Số điện thoại', DiaChi as N'Địa chỉ', HinhAnh as N'Hình ảnh' from NHANVIEN where IsDel = 0

	
-- store xoa 1 nv
go
create proc sp_xoaNV(@ma varchar(10))
as
  delete
  From NHANVIEN
  where MaNV = @ma

  --store xoa hien thi thong tin nv
  go
  create proc sp_xoaNV2(@MaNV varchar(10))
  as 
  update NHANVIEN
  set IsDel = -1
  where MaNV = @MaNV
  
  --store sua thong tin nv
  go
  create proc sp_SuaNV(@MaNV varchar(10), @TenNV nvarchar(128), @GioiTinh nvarchar(20), @Email nvarchar(128), @SDT nvarchar(20), @DiaChi nvarchar(128), @HinhAnh image)
  as 
  update NHANVIEN
  set TenNV = @TenNV, GioiTinh = @GioiTinh, Email = @Email, SDT = @SDT, DiaChi = @DiaChi , HinhAnh = @HinhAnh
  where MaNV = @MaNV


  ------------------------- Lich chieu phim -------------------------------

--store them lich
go
create proc sp_themLich(@MaLich varchar(10), @MaPhim varchar(10), @BatDau datetime, @MaNV varchar(10), @MaPhong varchar(10))
 as
 insert into LICHCHIEUPHIM values (@MaLich, @MaPhim, Convert(datetime, @BatDau, 100), @MaNV, @MaPhong, 0)

-- store lay danh sach lich
go
create proc sp_LayLich
as 
select MaLich as N'Mã', p.TenPhim as N'Phim', BatDau as N'Bắt đầu', nv.TenNV + ' [' + nv.MaNV + ']' as N'Nhân viên', ph.TenPhong as N'Phòng chiếu' from LICHCHIEUPHIM l, PHIM p, NHANVIEN nv, PHONG ph where l.MaNV = nv.MaNV and l.MaPhim = p.MaPhim and l.MaPhong = ph.MaPhong and (l.IsDel = 0)

-- store lay danh sach lich
go
create proc sp_LayLich3
as 
select MaLich as N'Mã', p.TenPhim as N'Phim', BatDau as N'Bắt đầu', nv.TenNV + ' [' + nv.MaNV + ']' as N'Nhân viên', ph.TenPhong as N'Phòng chiếu' from LICHCHIEUPHIM l, PHIM p, NHANVIEN nv, PHONG ph where l.MaNV = nv.MaNV and l.MaPhim = p.MaPhim and l.MaPhong = ph.MaPhong and (l.IsDel = 0) and l.BatDau > CURRENT_TIMESTAMP


-- store lay danh sach lich theo phong va ngay
go
create proc sp_LayLich2(@MaPhong varchar(10), @Ngay datetime)
as 
select MaLich, BatDau, p.ThoiLuong from LICHCHIEUPHIM l, PHIM p where l.MaPhim = p.MaPhim and MaPhong = @MaPhong and cast(BatDau as date) =  cast(@Ngay as date) order by(BatDau)

	
-- store xoa 1 lich
go
create proc sp_xoaLich(@ma varchar(10))
as
  delete
  From LICHCHIEUPHIM
  where MaLich = @ma
  
   --store xoa hien thi thong tin lich chieu phim
  go
  create proc sp_xoaLich2(@ma varchar(10))
  as 
  update LICHCHIEUPHIM
  set IsDel = -1
  where MaLich = @ma
  
  --store sua thong tin lich
  go
  create proc sp_SuaLich(@MaLich varchar(10), @MaPhim varchar(10), @BatDau datetime, @MaNV varchar(10), @MaPhong varchar(10))
  as 
  update LICHCHIEUPHIM
  set MaPhim = @MaPhim, BatDau = Convert(datetime, @BatDau, 100), MaPhong = @MaPhong, MaNV = @MaNV
  where MaLich = @MaLich


  ------------------------- ve -------------------------------

--store them ve
go
create proc sp_themVe(@MaVe varchar(10), @MaLich varchar(10), @KhachHang varchar(10), @NhanVien varchar(10), @SaleTime datetime, @Slot int, @Tien float)
 as
 insert into VE values (@MaVe, @MaLich, @KhachHang, @NhanVien, Convert(datetime, @SaleTime, 100), @Slot, @Tien)

-- store lay danh sach ve
go
create proc sp_LayDSVe
as 
select MaVe as N'Mã vé', (p.TenPhim + ' - ' + CONVERT(VARCHAR, l.BatDau, 20)) as N'Lịch chiếu', k.TenKH + ' [' + k.MaKH + ']' as N'Khách hàng', nv.TenNV + ' [' + nv.MaNV + ']' as N'Nhân viên', SaleTime as N'Thời gian', Slot as N'Ghế', Tien as N'Thanh toán' from VE v, KHACHHANG k, NHANVIEN nv, LICHCHIEUPHIM l, PHIM p where v.KhachHang = k.MaKH and v.MaLich = l.MaLich and v.NhanVien = nv.MaNV and l.MaPhim = p.MaPhim

-- store lay danh sach ve2
go
create proc sp_LayDSVe2
as 
select * from Ve


-- store xoa 1 ve
go
create proc sp_xoaVe(@ma varchar(10))
as
  delete
  From VE
  where MaVe = @ma
  
  --store sua thong tin ve
  go
  create proc sp_SuaVe(@MaVe varchar(10), @MaLich varchar(10), @KhachHang varchar(10), @NhanVien varchar(10), @SaleTime datetime, @Slot int, @Tien float)
  as 
  update VE
  set MaLich = @MaLich, KhachHang = @KhachHang, NhanVien = @NhanVien, SaleTime = Convert(datetime, @SaleTime, 100), Slot = @Slot, Tien = @Tien
  where MaVe = @MaVe
  
------------------------- phong -------------------------------

--store them phong
go
create proc sp_themPhong(@MaPhong varchar(10), @TenPhong nvarchar(128), @LoaiPhong nvarchar(50), @Gia float, @SoGhe int)
 as
 insert into PHONG values (@MaPhong, @TenPhong, @LoaiPhong, @Gia, @SoGhe, 0)
 
-- store lay danh sach phong
go
create proc sp_LayDSPhong
as 
select MaPhong as N'Mã phòng', TenPhong as N'Tên phòng', LoaiPhong as N'Loại phòng', Gia as N'Giá', SoGhe as N'Số ghế' from PHONG where IsDel = 0
	
-- store xoa 1 phong
go
create proc sp_xoaPhong(@ma varchar(10))
as
  delete
  From PHONG
  where MaPhong = @ma
  
   --store xoa hien thi thong tin phong
  go
  create proc sp_xoaPhong2(@ma varchar(10))
  as 
  update PHONG
  set IsDel = -1
  where MaPhong = @ma
  
  --store sua thong tin phong
  go
  create proc sp_SuaPhong(@MaPhong varchar(10), @TenPhong nvarchar(128), @LoaiPhong nvarchar(50), @Gia float, @SoGhe int)
  as 
  update PHONG
  set TenPhong = @TenPhong, SoGhe = @SoGhe, LoaiPhong = @LoaiPhong, Gia = @gia
  where MaPhong = @MaPhong

  ------------------------------tim kiem------------------------------------

  --tim kiem thong tin khach hang theo ma
  go
  create proc sp_SearchClientByCode(@MaKH varchar(10))
  as 
  select MaKH as N'Mã khách hàng', TenKH as N'Họ tên', LoaiKH as N'Loại khách hàng', Email, SDT as N'Số điện thoại', DiaChi as N'Địa chỉ' from KHACHHANG
  where MaKH like '%' +  @MaKH + '%' and IsDel = 0

  --tim kiem thong tin khach hang theo ten
  go
  create proc sp_SearchClientByName(@TenKH varchar(128))
  as 
  select MaKH as N'Mã khách hàng', TenKH as N'Họ tên', LoaiKH as N'Loại khách hàng', Email, SDT as N'Số điện thoại', DiaChi as N'Địa chỉ' from KHACHHANG
  where TenKH like '%' +  @TenKH + '%' and IsDel = 0

  --tim kiem nhan vien theo ma
  go
  create proc sp_SearchEmployeeByCode(@MaNV varchar(10))
  as 
  select MaNV as N'Mã nhân viên', TenNV as N'Họ tên', GioiTinh as N'Giới tính', Email, SDT as N'Số điện thoại', DiaChi as N'Địa chỉ' from NHANVIEN
  where (MaNV like '%' +  @MaNV + '%') and (IsDel = 0)

  --tim kiem nhan vien theo ten
  go
  create proc sp_SearchEmployeeByName(@TenNV varchar(128))
  as 
  select MaNV as N'Mã nhân viên', TenNV as N'Họ tên', GioiTinh as N'Giới tính', Email, SDT as N'Số điện thoại', DiaChi as N'Địa chỉ' from NHANVIEN
  where (TenNV like '%' +  @TenNV + '%') and (IsDel = 0)

  --tim kiem phim theo ten
  go
  create proc sp_SearchFilmByName(@TenPhim varchar(128))
  as 
  select MaPhim as N'Mã phim', TenPhim as N'Tên phim', ThoiLuong as N'Thời lượng', t.TenTheLoaiPhim as N'Thể Loại', NoiDung as N'Nội dung'  from PHIM p, THELOAI t where p.TheLoai = t.MaTheLoai
  and TenPhim like '%' +  @TenPhim + '%' and p.IsDel = 0

  --tim kiem phim theo ma
  go
  create proc sp_SearchFilmByCode(@MaPhim varchar(10))
  as 
  select MaPhim as N'Mã phim', TenPhim as N'Tên phim', ThoiLuong as N'Thời lượng', t.TenTheLoaiPhim as N'Thể Loại', NoiDung as N'Nội dung'  from PHIM p, THELOAI t where p.TheLoai = t.MaTheLoai
  and MaPhim like '%' +  @MaPhim + '%' and p.IsDel = 0

  --tim kiem ve bang ma ve
  go
  create proc sp_SearchTicketByCode(@MaVe varchar(10))
  as 
  select MaVe as N'Mã vé', (p.TenPhim + ' - ' + CONVERT(VARCHAR, l.BatDau, 20)) as N'Lịch chiếu', k.TenKH + ' [' + k.MaKH + ']' as N'Khách hàng', nv.TenNV + ' [' + nv.MaNV + ']' as N'Nhân viên', SaleTime as N'Thời gian', Slot as N'Ghế', Tien as N'Thanh toán' from VE v, KHACHHANG k, NHANVIEN nv, LICHCHIEUPHIM l, PHIM p where v.KhachHang = k.MaKH and v.MaLich = l.MaLich and v.NhanVien = nv.MaNV and l.MaPhim = p.MaPhim
  and MaVe like '%' +  @MaVe + '%'
  
  --tim kiem ve bang ma khach hang
  go
  create proc sp_SearchTicketByClient(@KH varchar(10))
  as 
  select MaVe as N'Mã vé', (p.TenPhim + ' - ' + CONVERT(VARCHAR, l.BatDau, 20)) as N'Lịch chiếu', k.TenKH + ' [' + k.MaKH + ']' as N'Khách hàng', nv.TenNV + ' [' + nv.MaNV + ']' as N'Nhân viên', SaleTime as N'Thời gian', Slot as N'Ghế', Tien as N'Thanh toán' from VE v, KHACHHANG k, NHANVIEN nv, LICHCHIEUPHIM l, PHIM p where v.KhachHang = k.MaKH and v.MaLich = l.MaLich and v.NhanVien = nv.MaNV and l.MaPhim = p.MaPhim
  and k.TenKH like '%' +  @KH + '%'
 
 --tim kiem phong bang ten
  go
  create proc sp_SearchRoomByName(@TenPhong varchar(10))
  as 
  select SoGhe, Gia from PHONG
  where TenPhong = @TenPhong

  --tim kiem ve da ban
  go
  create proc sp_SearchTicketSaled(@MaLich varchar(10))
  as
  select Slot from VE
  where MaLich = @MaLich

  --thong ke ve ban ra
  go 
  create proc sp_thongVeBanRa(@from date, @to date)
  as
  select p.TenPhim as N'Phim' ,Count(MaVe) as N'Số lượng' from PHIM p, VE v, LICHCHIEUPHIM l where p.MaPhim = l.MaPhim and v.MaLich = l.MaLich and (convert (datetime ,v.SaleTime, 103) >  convert(date,@from, 103)) and (convert (datetime ,v.SaleTime, 103) < convert(date,@to,103)) group by p.TenPhim

  --thong ke ve ban ra thang hien tai
  go 
  create proc sp_thongVeBanRa2
  as
  select p.TenPhim as N'Phim' ,Count(MaVe) as N'Số lượng' from PHIM p, VE v, LICHCHIEUPHIM l where p.MaPhim = l.MaPhim and v.MaLich = l.MaLich and MONTH(v.SaleTime) = MONTH(CURRENT_TIMESTAMP) group by p.TenPhim

 --thong ke doanh thu
  go 
  create proc sp_thongTienBanVe(@from date, @to date)
  as
  select p.TenPhim as N'Phim' ,Sum(Tien) as N'Tổng tiền' from PHIM p, VE v, LICHCHIEUPHIM l where p.MaPhim = l.MaPhim and v.MaLich = l.MaLich and (convert (datetime ,v.SaleTime, 103) >  convert(date,@from, 103)) and (convert (datetime ,v.SaleTime, 103) < convert(date,@to,103)) group by p.TenPhim

--thong ke doanh thu thang hien tai
  go 
  create proc sp_thongTienBanVe2
  as
  select p.TenPhim as N'Phim' ,Sum(Tien) as N'Tổng tiền' from PHIM p, VE v, LICHCHIEUPHIM l where p.MaPhim = l.MaPhim and v.MaLich = l.MaLich and MONTH(v.SaleTime) = MONTH(CURRENT_TIMESTAMP)   group by p.TenPhim


--tim kiem lich chieu phim theo phim
go
create proc sp_SearchScheduleByFilm(@TenPhim nvarchar(128))
as 
select MaLich as N'Mã', p.TenPhim as N'Phim', BatDau as N'Bắt đầu', nv.TenNV + ' [' + nv.MaNV + ']' as N'Nhân viên', ph.TenPhong as N'Phòng chiếu' from LICHCHIEUPHIM l, PHIM p, NHANVIEN nv, PHONG ph where l.MaNV = nv.MaNV and l.MaPhim = p.MaPhim and l.MaPhong = ph.MaPhong and p.TenPhim like '%' +  @TenPhim + '%' and p.IsDel = 0

--tim kiem lich chieu phim theo phong
go
create proc sp_SearchScheduleByRoom(@TenPhong nvarchar(128))
as 
select MaLich as N'Mã', p.TenPhim as N'Phim', BatDau as N'Bắt đầu', nv.TenNV + ' [' + nv.MaNV + ']' as N'Nhân viên', ph.TenPhong as N'Phòng chiếu' from LICHCHIEUPHIM l, PHIM p, NHANVIEN nv, PHONG ph where l.MaNV = nv.MaNV and l.MaPhim = p.MaPhim and l.MaPhong = ph.MaPhong and ph.TenPhong like '%' +  @TenPhong + '%' and p.IsDel = 0
