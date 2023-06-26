-- create database
CREATE DATABASE DQTSinhVien;
GO

USE DQTSinhVien
GO
-- create tables
CREATE TABLE PhanQuyen (
    MaPhanQuyen nchar(10) PRIMARY KEY,
    TenPhanQuyen nvarchar(50) NOT NULL,
);

CREATE TABLE DonVi (
    MaDonVi nchar(10) PRIMARY KEY,
    TenDonVi nvarchar(50)
);

CREATE TABLE GiangVien (
    MaGiangVien nchar(10) PRIMARY KEY,
    HoTen nvarchar(50) NOT NULL,
    GioiTinh nvarchar(50),
    TrinhDo nvarchar(50),
	ChuyenMon nvarchar(50),
	MaDonVi nchar(10),
	MaPhanQuyen nchar(10),
    TaiKhoan nvarchar(50) NOT NULL,
  	MatKhau nvarchar(50) NOT NULL,
	HoatDong smallint
	FOREIGN KEY (MaDonVi) REFERENCES DonVi (MaDonVi) ON UPDATE CASCADE,
	FOREIGN KEY (MaPhanQuyen) REFERENCES PhanQuyen (MaPhanQuyen) ON UPDATE CASCADE,
);

CREATE TABLE SinhVien (
    MaSinhVien nchar(10) PRIMARY KEY,
    HoTen nvarchar(50) NOT NULL,
    GioiTinh nvarchar(50),
    LopNienChe nvarchar(50),
    TaiKhoan nvarchar(50) NOT NULL,
  	MatKhau nvarchar(50) NOT NULL,
	HoatDong smallint
);

CREATE TABLE MonHoc (
    MaMonHoc nchar(10) PRIMARY KEY,
    TenMonHoc nvarchar(50) NOT NULL,
    TongSoTiet nvarchar(50),
    SoTietLyThuyet nvarchar(50) NOT NULL,
    SoTietThucHanh nvarchar(50) NOT NULL
);

CREATE TABLE LopMonHoc (
    MaLopMonHoc nchar(10) PRIMARY KEY,
    TenLopMonHoc nvarchar(50),
    MaGiangVien nchar(10) NOT NULL,
	FOREIGN KEY (MaGiangVien) REFERENCES GiangVien (MaGiangVien) ON UPDATE CASCADE,
);

CREATE TABLE CaHoc (
    MaCaHoc nchar(10) PRIMARY KEY,
    GioBD nchar(20) NOT NULL,
    GioKT nchar(20),
	BuoiHoc nvarchar(20)
);

CREATE TABLE MonHoc_LopMonHoc (
    MaLopMonHoc nchar(10) NOT NULL,
	HocKy nchar(10),
	NamHoc nchar(10),
	MaMonHoc nchar(10) NOT NULL,
	SoSinhVien int,
	FOREIGN KEY (MaLopMonHoc) REFERENCES LopMonHoc (MaLopMonHoc) ON UPDATE CASCADE,
	FOREIGN KEY (MaMonHoc) REFERENCES MonHoc (MaMonHoc) ON UPDATE CASCADE,
);

CREATE TABLE SinhVien_Hoc_LopMonHoc (
    MaLopMonHoc nchar(10) NOT NULL,
	MaSinhVien nchar(10) NOT NULL,
	HocKy nchar(10),
	NamHoc nchar(10),
	FOREIGN KEY (MaLopMonHoc) REFERENCES LopMonHoc (MaLopMonHoc) ON UPDATE CASCADE,
	FOREIGN KEY (MaSinhVien) REFERENCES SinhVien (MaSinhVien) ON UPDATE CASCADE,
);

CREATE TABLE BuoiHoc (
    MaBuoiHoc nchar(10) PRIMARY KEY,
    BuoiHocSo nchar(10) NOT NULL,
);

CREATE TABLE DiemDanh (
    IDDiemDanh INT IDENTITY (1, 1) PRIMARY KEY,
	MaGiangVien nchar(10) NOT NULL,
	MaSinhVien nchar(10) NOT NULL,
	MaMonHoc nchar(10) NOT NULL,
	MaLopMonHoc nchar(10) NOT NULL,
	NgayDiemDanh Date,
	MaBuoiHoc nchar(10),
	MaCaHoc nchar(10),
	TrangThai bit,
	FOREIGN KEY (MaBuoiHoc) REFERENCES BuoiHoc (MaBuoiHoc) ON UPDATE NO ACTION,
	FOREIGN KEY (MaCaHoc) REFERENCES CaHoc (MaCaHoc) ON UPDATE NO ACTION,
	FOREIGN KEY (MaSinhVien) REFERENCES SinhVien (MaSinhVien) ON UPDATE CASCADE,
	FOREIGN KEY (MaLopMonHoc) REFERENCES LopMonHoc (MaLopMonHoc) ON UPDATE NO ACTION,
	FOREIGN KEY (MaGiangVien) REFERENCES GiangVien (MaGiangVien) ON UPDATE NO ACTION,
);

CREATE TABLE KiemTra (
    MaBuoiKiemTra nchar(10) PRIMARY KEY,
    BuoiKiemTra nchar(10) NOT NULL,
);

CREATE TABLE LopMonHoc_KiemTra (
    IDKiemTra INT IDENTITY (1, 1) PRIMARY KEY,
	MaGiangVien nchar(10) NOT NULL,
	MaSinhVien nchar(10) NOT NULL,
	MaLopMonHoc nchar(10) NOT NULL,
	MaBuoiKiemTra nchar(10) NOT NULL,
	DiemKiemTra float,
	FOREIGN KEY (MaBuoiKiemTra) REFERENCES KiemTra (MaBuoiKiemTra) ON UPDATE NO ACTION,
	FOREIGN KEY (MaSinhVien) REFERENCES SinhVien (MaSinhVien) ON UPDATE CASCADE,
	FOREIGN KEY (MaLopMonHoc) REFERENCES LopMonHoc (MaLopMonHoc) ON UPDATE NO ACTION,
	FOREIGN KEY (MaGiangVien) REFERENCES GiangVien (MaGiangVien) ON UPDATE NO ACTION,
);


-- create stored procedures

-- =============================================
-- Author:		TungLX
-- Create date: 6/4/2023
-- Description:	Kiểm tra tình trạng tài khoản login giảng viên, phòng đào tạo, admin
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_CheckLogin]
	-- Add the parameters for the stored procedure here
	@taikhoan VARCHAR(100),
	@matkhau VARCHAR(100),

	-- Return Result
	@Code INT OUTPUT,
	@Message NVARCHAR(100) OUTPUT,
	@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @CodeCheck SMALLINT;
	DECLARE @ActicveCheck SMALLINT;
	-- Insert statements for procedure here
	SET NOCOUNT ON;
	IF(@taikhoan IS NULL OR @matkhau IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Tài Khoản hoặc mật khẩu không được để rỗng!', @Data = NULL;
			RETURN;
		END
	
	SET @CodeCheck = (SELECT Count(1) FROM GiangVien WHERE TaiKhoan = @taikhoan AND MatKhau = @matkhau);

	SET @ActicveCheck = (SELECT GV.HoatDong FROM GiangVien GV WHERE TaiKhoan = @taikhoan AND MatKhau = @matkhau);

	IF (@ActicveCheck <> 0)
		BEGIN
			SELECT @Code = -1, @Message = N'Tài khoản của bạn hiện đang bị khoá liên hệ nhà trường để kích hoạt lại!', @Data = NULL;
			RETURN;
		END

	IF(@CodeCheck <> 0)
		BEGIN
			SELECT @Code = 0, @Message = N'Đăng nhập thành công!', @Data = (SELECT GiangVien.MaGiangVien FROM GiangVien WHERE TaiKhoan = @taikhoan AND MatKhau = @matkhau);
			RETURN;
		END
	ELSE
		BEGIN
			SELECT @Code = -1, @Message = N'Đăng nhập không thành công!', @Data = NULL;
			RETURN;
		END;
END

-- =============================================
-- Author:		TungLX
-- Create date: 16/5/2023
-- Description:	Tắt hoạt động của tài khoản giảng viên hoặc nhân viên phòng đào tạo
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_DeleteEmployee]
	-- Add the parameters for the stored procedure here
	@MaGiangVien nchar(10)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@MaGiangVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Lỗi không xoá được!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	BEGIN TRY
		UPDATE GiangVien 
		SET HoatDong = 1
		WHERE MaGiangVien = @MaGiangVien

		SELECT @Code = 0, @Message = N'Tắt hoạt động tài khoản thành công!', @Data = null;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Tắt hoạt động tài khoản thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 16/5/2023
-- Description: Xoá (Tắt hoạt động) tài khoản sinh viên
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_DeleteSinhVien]
	-- Add the parameters for the stored procedure here
	@MaSinhVien nchar(10)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@MaSinhVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Lỗi không xoá được!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	BEGIN TRY
		UPDATE SinhVien 
		SET HoatDong = 1
		WHERE MaSinhVien = @MaSinhVien

		SELECT @Code = 0, @Message = N'Tắt hoạt động tài khoản thành công!', @Data = null;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Tắt hoạt động tài khoản thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 21/6/2023
-- Description:	Xoá lớp môn học
-- =============================================
CREATE PROCEDURE GV_SP_DeleteLopMonHoc
	-- Add the parameters for the stored procedure here
	@MaLopMonHoc nchar(10)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@MaLopMonHoc IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Lỗi không xoá được!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	BEGIN TRY
		DELETE FROM SinhVien_Hoc_LopMonHoc 
		WHERE MaLopMonHoc = @MaLopMonHoc

		DELETE FROM LopMonHoc 
		WHERE MaLopMonHoc = @MaLopMonHoc

		SELECT @Code = 0, @Message = N'Xoá sinh viên của lớp môn học thành công!', @Data = null;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Xoá sinh viên khỏi lớp môn học thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 19/5/2023
-- Description:	Xoá sinh viên của 1 lớp môn học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_DeleteSinhVienCuaLopMH]
	-- Add the parameters for the stored procedure here
	@MaSinhVien nchar(10)
	,@MaLopMonHoc nchar(10)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@MaSinhVien IS NULL OR @MaLopMonHoc IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Lỗi không xoá được!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	BEGIN TRY
		DELETE FROM SinhVien_Hoc_LopMonHoc 
		WHERE MaSinhVien = @MaSinhVien AND MaLopMonHoc = @MaLopMonHoc

		SELECT @Code = 0, @Message = N'Xoá sinh viên của lớp môn học thành công!', @Data = null;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Xoá sinh viên khỏi lớp môn học thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 16/6/2023
-- Description:	Điểm danh sinh viên
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_DiemDanhSinhVien]
	-- Add the parameters for the stored procedure here
	@MaGiangVien nchar(10), 
	@MaSinhVien nchar(10),
	@MaLopMonHoc nchar(10),
    @MaMonHoc nchar(10),
    @NgayDiemDanh datetime,
    @MaBuoiHoc nchar(10),
    @MaCaHoc nchar(10),
    @TrangThai bit

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @CodeCheck SMALLINT;
	DECLARE @BuoiHocCheck SMALLINT;
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF(@MaGiangVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã Giảng Viên!', @Data = NULL;
			RETURN;
		END

	IF(@MaSinhVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã sinh viên!', @Data = NULL;
			RETURN;
		END

	IF(@MaLopMonHoc IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã lớp môn học!', @Data = NULL;
			RETURN;
		END

	IF(@NgayDiemDanh IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Chưa chọn ngày điểm danh!', @Data = NULL;
			RETURN;
		END

	IF(@MaCaHoc IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã ca học!', @Data = NULL;
			RETURN;
		END

	IF(@MaBuoiHoc IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã buổi học!', @Data = NULL;
			RETURN;
		END

	IF(@TrangThai IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Chưa chọn trạng thái cho sinh viên ' + @MaSinhVien + '!', @Data = NULL;
			RETURN;
		END

	SET @CodeCheck = (SELECT Count(1) FROM DiemDanh 
	WHERE exists (SELECT MaSinhVien FROM DiemDanh 
			WHERE TRIM(MaCaHoc) LIKE TRIM(@MaCaHoc) AND TRIM(MaBuoiHoc) LIKE TRIM(@MaBuoiHoc) AND TRIM(MaLopMonHoc) = TRIM(@MaLopMonHoc) AND NgayDiemDanh = @NgayDiemDanh AND TRIM(MaSinhVien) LIKE TRIM(@MaSinhVien)));

	IF(@CodeCheck > 0)
		BEGIN 
			SELECT @Code = -2, @Message = N'Buổi học này đã được điểm danh vui lòng chọn buổi học sau đó!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	BEGIN TRY
    -- Insert statements for procedure here
		INSERT INTO DiemDanh(MaGiangVien, MaSinhVien, MaLopMonHoc, MaMonHoc, NgayDiemDanh, MaBuoiHoc, MaCaHoc, TrangThai)
		VALUES (@MaGiangVien, @MaSinhVien, @MaLopMonHoc, @MaMonHoc, @NgayDiemDanh, @MaBuoiHoc, @MaCaHoc, @TrangThai);

		SELECT @Code = 0, @Message = N'Điểm danh thành công!', @Data = null;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Điểm danh thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 11/4/2023
-- Description:	Lấy ra tất cả nhân viên trong trường
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAll]
	@MaNhanVien nchar(10)
AS
BEGIN
	DECLARE @CompareString VARCHAR(10)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @CompareString = CONCAT(TRIM(@MaNhanVien), '%');

    -- Insert statements for procedure here
	SELECT MaGiangVien
	  ,HoTen
	  ,GioiTinh
      ,TrinhDo
      ,ChuyenMon
      ,GV.MaDonVi
	  ,DV.TenDonVi
      ,GV.MaPhanQuyen
	  ,PQ.TenPhanQuyen FROM GiangVien GV
	  INNER JOIN PhanQuyen PQ ON PQ.MaPhanQuyen = GV.MaPhanQuyen
	  LEFT JOIN DonVi DV ON DV.MaDonVi = GV.MaDonVi
	  WHERE (GV.MaPhanQuyen = '2' OR GV.MaPhanQuyen = '1') AND GV.MaGiangVien LIKE @CompareString
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 14/6/2023
-- Description:	Lấy ra tất cả buổi học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllBuoiHoc]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaBuoiHoc, BuoiHocSo FROM BuoiHoc
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 14/6/2023
-- Description:	Lấy ra tất cả ca học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllCaHoc]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaCaHoc, GioBD, GioKT, BuoiHoc FROM CaHoc
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 13/4/2023
-- Description:	Lấy ra các đơn vị trong trường
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllDonVi]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaDonVi ,TenDonVi FROM DonVi
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 13/4/2023
-- Description:	Lấy ra toàn bộ giảng viên
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllGiangVien]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaGiangVien
	  ,HoTen
	  ,GioiTinh
      ,TrinhDo
      ,ChuyenMon
      ,GV.MaDonVi
	  ,DV.TenDonVi
      ,GV.MaPhanQuyen
	  ,PQ.TenPhanQuyen
	  ,GV.HoatDong FROM GiangVien GV
	  INNER JOIN PhanQuyen PQ ON PQ.MaPhanQuyen = GV.MaPhanQuyen
	  LEFT JOIN DonVi DV ON DV.MaDonVi = GV.MaDonVi
	  WHERE GV.MaPhanQuyen = '2'
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 15/5/2023
-- Description:	Lấy ra tất cả các lớp môn học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllLopMonHoc]
	-- Add the parameters for the stored procedure here
	@MaGiangVien NCHAR(12) NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MH_LMH.MaLopMonHoc
	  ,LMH.TenLopMonHoc
	  ,LMH.MaGiangVien
	  ,GV.HoTen
	  ,MH_LMH.HocKy
	  ,MH_LMH.NamHoc
      ,MH_LMH.MaMonHoc
      ,MH_LMH.SoSinhVien
	  ,MH.TongSoTiet
	  ,MH.SoTietLyThuyet
	  ,MH.SoTietThucHanh
	FROM MonHoc_LopMonHoc MH_LMH
	INNER JOIN MonHoc MH ON MH.MaMonHoc = MH_LMH.MaMonHoc
	INNER JOIN LopMonHoc LMH ON LMH.MaLopMonHoc = MH_LMH.MaLopMonHoc
	LEFT JOIN GiangVien GV ON GV.MaGiangVien = LMH.MaGiangVien
	WHERE (@MaGiangVien IS NULL OR @MaGiangVien = LMH.MaGiangVien)
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: <15/5/2023
-- Description:	Lấy ra mã lớp môn học giống với tham số truyền vào
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllMaLopMH]
	-- Add the parameters for the stored procedure here
	@MaLopMonHoc nchar(10)
AS
BEGIN
	DECLARE @CompareString VARCHAR(10)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @CompareString = CONCAT(TRIM(@MaLopMonHoc), '%');

    -- Insert statements for procedure here
	SELECT MaLopMonHoc
	FROM LopMonHoc
	WHERE MaLopMonHoc LIKE @CompareString
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 5/5/2023
-- Description:	Lấy ra tất cả mã môn học để thêm mới lớp môn học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllMaMH]
	-- Add the parameters for the stored procedure here
	@MaMonHoc nchar(10)
AS
BEGIN
	DECLARE @CompareString VARCHAR(10)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @CompareString = CONCAT(TRIM(@MaMonHoc), '%');

    -- Insert statements for procedure here
	SELECT MaMonHoc
	FROM MonHoc
	WHERE MaMonHoc LIKE @CompareString
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 14/4/2023
-- Description:	Lấy ra các mã sinh viên cùng năm học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllMaSV]
	-- Add the parameters for the stored procedure here
	@MaSinhVien nchar(10)
AS
BEGIN
	DECLARE @CompareString VARCHAR(10)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @CompareString = CONCAT(TRIM(@MaSinhVien), '%');

    -- Insert statements for procedure here
	SELECT MaSinhVien 
		,HoTen
		,LopNienChe
	FROM SinhVien
	WHERE MaSinhVien LIKE @CompareString
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 4/5/2023
-- Description:	Lấy ra tất cả các môn học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllMonHoc]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaMonHoc
	  ,TenMonHoc
	  ,TongSoTiet
      ,SoTietLyThuyet
      ,SoTietThucHanh FROM MonHoc MH
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 12/4/2023
-- Description:	Lấy ra tất cả nhân viên phòng đào tạo
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllPhongDaoTao]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT MaGiangVien
	  ,HoTen
	  ,GioiTinh
      ,TrinhDo
      ,ChuyenMon
      ,GV.MaDonVi
	  ,DV.TenDonVi
      ,GV.MaPhanQuyen
	  ,PQ.TenPhanQuyen
	  ,GV.TaiKhoan
	  ,GV.HoatDong FROM GiangVien GV
	  INNER JOIN PhanQuyen PQ ON PQ.MaPhanQuyen = GV.MaPhanQuyen
	  LEFT JOIN DonVi DV ON DV.MaDonVi = GV.MaDonVi
	  WHERE GV.MaPhanQuyen = '1'
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 12/4/2023
-- Description:	Lấy ra tất cả sinh viên từ bên giảng viên
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllSinhVien]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SV.MaSinhVien
	  , HoTen
	  , GioiTinh
	  , SV.LopNienChe
	  FROM SinhVien SV
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 19/5/2023
-- Description:	Lấy ra các sinh viên học lớp môn học được thêm vào
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetAllSVHocLopMonHoc]
	-- Add the parameters for the stored procedure here
	@MaLopMonHoc nchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SVHLMH.MaSinhVien
	, SV.HoTen
	, SV.HoatDong
	, SV.LopNienChe
	FROM SinhVien_Hoc_LopMonHoc SVHLMH
	INNER JOIN SinhVien SV ON SV.MaSinhVien = SVHLMH.MaSinhVien
	WHERE MaLopMonHoc = @MaLopMonHoc
	ORDER BY SVHLMH.MaLopMonHoc
	
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 17/6/2023
-- Description:	Lấy ra các buổi học của lớp môn học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetBuoiHocByLopMonHoc]
	-- Add the parameters for the stored procedure here
	@MaLopMonHoc NCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DD.MaBuoiHoc, BH.BuoiHocSo FROM DiemDanh DD
	INNER JOIN BuoiHoc BH ON BH.MaBuoiHoc = DD.MaBuoiHoc
	WHERE TRIM(DD.MaLopMonHoc) = TRIM(@MaLopMonHoc)
	GROUP BY DD.MaBuoiHoc, BH.BuoiHocSo
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 20/6/2023
-- Description:	Lấy ra các buổi kiểm tra
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetBuoiKiemTra]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaBuoiKiemTra, BuoiKiemTra FROM KiemTra
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 20/6/2023
-- Description:	Lấy ra danh sách điểm của lớp môn học theo buổi kiểm tra
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetBuoiKiemTraByBuoi]
	-- Add the parameters for the stored procedure here
	@MaLopMonHoc NCHAR(10),
	@MaBuoiKT NCHAR(10) NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT IDKiemTra
      ,DiemKT.MaSinhVien
	  ,SV.HoTen AS TenSinhVien
      ,DiemKT.MaBuoiKiemTra
      ,DiemKT.MaLopMonHoc
      ,BKT.BuoiKiemTra
      ,DiemKT.DiemKiemTra
	FROM LopMonHoc_KiemTra DiemKT
	INNER JOIN SinhVien SV ON SV.MaSinhVien = DiemKT.MaSinhVien
	INNER JOIN KiemTra BKT ON BKT.MaBuoiKiemTra = DiemKT.MaBuoiKiemTra
	WHERE TRIM(MaLopMonHoc) = TRIM(@MaLopMonHoc) AND (@MaBuoiKT IS NULL OR TRIM(@MaBuoiKT) = TRIM(DiemKT.MaBuoiKiemTra))
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 20/6/2023
-- Description:	Lấy ra tất cả buổi kiểm tra của lớp môn học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetBuoiKiemTraByLMH] 
	-- Add the parameters for the stored procedure here
	@MaLopMonHoc NCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DKT.MaBuoiKiemTra, BKT.BuoiKiemTra FROM LopMonHoc_KiemTra DKT
	INNER JOIN KiemTra BKT ON BKT.MaBuoiKiemTra = DKT.MaBuoiKiemTra
	WHERE TRIM(DKT.MaLopMonHoc) = TRIM(@MaLopMonHoc)
	GROUP BY DKT.MaBuoiKiemTra, BKT.BuoiKiemTra
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 16/6/2023
-- Description:	Lấy ra điểm chuyên cần của sinh viên
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetDiemChuyenCanSV]
	-- Add the parameters for the stored procedure here
	@MaLopMonHoc NCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here	
	SELECT A.MaSinhVien, A.MaLopMonHoc, A.HoTen, A.LopNienChe, SUM(A.CoMat) AS TongCoMat, SUM(A.VangMat) AS TongVangMat FROM (
		SELECT DD.[MaSinhVien]
			,SinhVien.HoTen
			,SinhVien.LopNienChe
			,DD.MaLopMonHoc
			,COUNT(case [TrangThai] when 0 then 1 else null end ) AS CoMat
			,COUNT(case [TrangThai] when 1 then 1 else null end ) AS VangMat
		FROM [DDSinhVien].[dbo].[DiemDanh] DD
		LEFT JOIN SinhVien ON DD.[MaSinhVien] = SinhVien.[MaSinhVien]
		WHERE TRIM(MaLopMonHoc) LIKE TRIM(@MaLopMonHoc)
		group by DD.[MaSinhVien], [TrangThai], MaLopMonHoc, SinhVien.HoTen, SinhVien.LopNienChe) A
	GROUP BY MaSinhVien, MaLopMonHoc, HoTen, LopNienChe

END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 18/6/2023
-- Description:	Lấy ra trạng thái điểm danh của lớp môn học theo buổi học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetDiemDanhByBuoiHoc]
	-- Add the parameters for the stored procedure here
	@MaLopMonHoc NCHAR(10),
	@MaBuoiHoc NCHAR(10) NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT IDDiemDanh
      ,DD.MaSinhVien
	  ,SV.HoTen AS TenSinhVien
      ,NgayDiemDanh
      ,DD.MaBuoiHoc
	  ,BH.BuoiHocSo
      ,DD.MaCaHoc
	  ,CH.GioBD AS GioBatDau
	  ,CH.GioKT AS GioKetThuc
      ,TrangThai
	FROM DiemDanh DD
	INNER JOIN SinhVien SV ON SV.MaSinhVien = DD.MaSinhVien
	INNER JOIN BuoiHoc BH ON BH.MaBuoiHoc = DD.MaBuoiHoc
	INNER JOIN CaHoc CH ON CH.MaCaHoc = DD.MaCaHoc
	WHERE TRIM(MaLopMonHoc) = TRIM(@MaLopMonHoc) AND (@MaBuoiHoc IS NULL OR TRIM(@MaBuoiHoc) = TRIM(DD.MaBuoiHoc))
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 20/6/2023
-- Description:	Lấy ra thông tin điểm kiểm tra của lớp môn học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetDiemKiemTraSV]
	-- Add the parameters for the stored procedure here
	@MaLopMonHoc NCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT A.MaSinhVien, A.MaLopMonHoc, A.HoTen, A.LopNienChe, SUM(A.LanKT1) AS BaiKiemTra1, SUM(A.LanKT2) AS BaiKiemTra2, SUM(A.LanKT3) AS BaiKiemTra3 FROM (
		SELECT DQT.MaSinhVien
			,SinhVien.HoTen
			,SinhVien.LopNienChe
			,DQT.MaLopMonHoc
			,(case MaBuoiKiemTra when 'KT01' then DiemKiemTra else null end) AS LanKT1
			,(case MaBuoiKiemTra when 'KT02' then DiemKiemTra else null end) AS LanKT2
			,(case MaBuoiKiemTra when 'KT00' then DiemKiemTra else null end) AS LanKT3
		FROM LopMonHoc_KiemTra DQT
		LEFT JOIN SinhVien ON DQT.[MaSinhVien] = SinhVien.[MaSinhVien]
		WHERE TRIM(MaLopMonHoc) LIKE TRIM(@MaLopMonHoc)
		group by DQT.MaSinhVien, DQT.MaBuoiKiemTra, MaLopMonHoc, SinhVien.HoTen, SinhVien.LopNienChe, DiemKiemTra) A
	GROUP BY MaSinhVien, MaLopMonHoc, HoTen, LopNienChe
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 20/6/2023
-- Description:	Lấy ra các thông tin cần có để tính điểm quá trình
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetDiemQuaTrinh]
	-- Add the parameters for the stored procedure here
	@MaLopMonHoc NCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT t1.MaSinhVien, t1.HoTen, t1. LopNienChe, t1.MaLopMonHoc, t1.BaiKiemTra1 AS BaiKiemTra1, t1.BaiKiemTra2 AS BaiKiemTra2, t1.BaiKiemTra3 AS BaiKiemTra3, t2.TongCoMat AS TongCoMat, t2.TongVangMat AS TongVangMat
	FROM 
		(SELECT A.MaSinhVien, A.MaLopMonHoc, A.HoTen, A.LopNienChe, SUM(A.LanKT1) AS BaiKiemTra1, SUM(A.LanKT2) AS BaiKiemTra2, SUM(A.LanKT3) AS BaiKiemTra3 FROM (
			SELECT DQT.MaSinhVien
				,SinhVien.HoTen
				,SinhVien.LopNienChe
				,DQT.MaLopMonHoc
				,(case MaBuoiKiemTra when 'KT01' then DiemKiemTra else null end) AS LanKT1
				,(case MaBuoiKiemTra when 'KT02' then DiemKiemTra else null end) AS LanKT2
				,(case MaBuoiKiemTra when 'KT03' then DiemKiemTra else null end) AS LanKT3
			FROM LopMonHoc_KiemTra DQT
			LEFT JOIN SinhVien ON DQT.[MaSinhVien] = SinhVien.[MaSinhVien]
			WHERE TRIM(MaLopMonHoc) LIKE TRIM(@MaLopMonHoc)
			group by DQT.MaSinhVien, DQT.MaBuoiKiemTra, MaLopMonHoc, SinhVien.HoTen, SinhVien.LopNienChe, DiemKiemTra) A
		GROUP BY MaSinhVien, MaLopMonHoc, HoTen, LopNienChe) t1
	LEFT JOIN
		(SELECT A.MaSinhVien, A.MaLopMonHoc, A.HoTen, A.LopNienChe, SUM(A.CoMat) AS TongCoMat, SUM(A.VangMat) AS TongVangMat FROM (
			SELECT DD.[MaSinhVien]
				,SinhVien.HoTen
				,SinhVien.LopNienChe
				,DD.MaLopMonHoc
				,COUNT(case [TrangThai] when 0 then 1 else null end ) AS CoMat
				,COUNT(case [TrangThai] when 1 then 1 else null end ) AS VangMat
			FROM [DDSinhVien].[dbo].[DiemDanh] DD
			LEFT JOIN SinhVien ON DD.[MaSinhVien] = SinhVien.[MaSinhVien]
			WHERE TRIM(MaLopMonHoc) LIKE TRIM(@MaLopMonHoc)
			group by DD.[MaSinhVien], [TrangThai], MaLopMonHoc, SinhVien.HoTen, SinhVien.LopNienChe) A
		GROUP BY MaSinhVien, MaLopMonHoc, HoTen, LopNienChe) t2
	ON (t1.MaSinhVien = t2.MaSinhVien);
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 15/5/2023
-- Description:	Lấy ra thông tin phòng đào tạo hoặc giảng viên theo mã giảng viên (mã nhân viên)
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetGiangVien]
	-- Add the parameters for the stored procedure here
	@MaNhanVien nchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaGiangVien
	  ,HoTen
	  ,GioiTinh
      ,TrinhDo
      ,ChuyenMon
      ,GV.MaDonVi
	  ,DV.TenDonVi
      ,GV.MaPhanQuyen
	  ,PQ.TenPhanQuyen
	  ,GV.TaiKhoan FROM GiangVien GV
	  INNER JOIN PhanQuyen PQ ON PQ.MaPhanQuyen = GV.MaPhanQuyen
	  LEFT JOIN DonVi DV ON DV.MaDonVi = GV.MaDonVi
	  WHERE GV.MaGiangVien = @MaNhanVien
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 16/5/2023
-- Description:	Lấy ra thông tin sinh viên theo mã sinh viên
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_GetSinhVien]
	-- Add the parameters for the stored procedure here
	@MaSinhVien nchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaSinhVien
	  , HoTen
	  , GioiTinh
	  , LopNienChe
      , TaiKhoan
	  FROM SinhVien SV
	  WHERE SV.MaSinhVien = @MaSinhVien
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 16/6/2023
-- Description:	Huỷ bỏ điểm danh nếu trạng thái chạy điểm danh lỗi
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_HuyBoDD]
	-- Add the parameters for the stored procedure here
	@MaGiangVien nchar(10), 
	@MaLopMonHoc nchar(10),
    @NgayDiemDanh datetime,
    @MaBuoiHoc nchar(10),
    @MaCaHoc nchar(10)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRY
		DELETE FROM DiemDanh 
		WHERE MaGiangVien LIKE @MaGiangVien 
		AND MaLopMonHoc LIKE @MaLopMonHoc 
		AND NgayDiemDanh LIKE @NgayDiemDanh 
		AND MaBuoiHoc LIKE @MaBuoiHoc
		AND MaCaHoc LIKE @MaCaHoc

	SELECT @Code = 0, @Message = N'Trạng thái điểm danh đã được reset lại do lỗi dữ liệu!', @Data = null;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Reset dữ liệu thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH		
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 20/6/2023
-- Description:	Thêm điểm cho sinh viên theo bài kiểm tra
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_InsertDiemKiemTra]
	-- Add the parameters for the stored procedure here
	@MaGiangVien NCHAR(10),
	@MaSinhVien NCHAR(10),
	@MaLopMonHoc NCHAR(10),
	@MaBuoiKiemTra NCHAR(10),
	@DiemKiemTra INT

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@MaGiangVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã Giảng Viên!', @Data = NULL;
			RETURN;
		END

	IF(@MaSinhVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã sinh viên!', @Data = NULL;
			RETURN;
		END

	IF(@MaLopMonHoc IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã lớp môn học!', @Data = NULL;
			RETURN;
		END

	IF(@MaBuoiKiemTra IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã buổi học!', @Data = NULL;
			RETURN;
		END

	IF(@DiemKiemTra IS NULL)
		BEGIN 
			SET @DiemKiemTra = 0;
		END

    -- Insert statements for procedure here
	BEGIN TRY
    -- Insert statements for procedure here
		INSERT INTO LopMonHoc_KiemTra(MaGiangVien, MaSinhVien, MaLopMonHoc, MaBuoiKiemTra, DiemKiemTra)
		VALUES (@MaGiangVien, @MaSinhVien, @MaLopMonHoc, @MaBuoiKiemTra, @DiemKiemTra);

		SELECT @Code = 0, @Message = N'Thêm điểm thành công!', @Data = null;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Thêm điểm thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 13/4/2023
-- Description:	Thêm mới Giảng Viên hoặc Nhân viên phòng đào tạo
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_InsertEmployee] 
	-- Add the parameters for the stored procedure here
	@MaGiangVien nchar(10)
	,@HoTen nvarchar(50)
	,@GioiTinh nvarchar(50)
	,@TrinhDo nvarchar(50)
	,@ChuyenMon nvarchar(50)
	,@MaDonVi nchar(10)
	,@MaPhanQuyen nchar(10)
	,@TaiKhoan nvarchar(50)
	,@MatKhau nvarchar(50)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @CodeCheck SMALLINT;
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF(@MaGiangVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Mã Giảng Viên không được để trống!', @Data = NULL;
			RETURN;
		END

	IF(@HoTen IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Họ Tên không được để trống!', @Data = NULL;
			RETURN;
		END

	IF(@MaDonVi IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Đơn vị không được để trống!', @Data = NULL;
			RETURN;
		END

	IF(@MaPhanQuyen IS NULL OR @MaPhanQuyen = '0')
		BEGIN 
			SELECT @Code = -1, @Message = N'Phân quyền không được để trống!', @Data = NULL;
			RETURN;
		END

	SET @CodeCheck = (SELECT Count(1) FROM GiangVien WHERE exists (SELECT TaiKhoan FROM GiangVien WHERE TaiKhoan LIKE @TaiKhoan));

	IF(@CodeCheck > 0)
		BEGIN 
			SELECT @Code = -1, @Message = N'Email đã có người sử dụng, vui lòng chọn email khác!', @Data = NULL;
			RETURN;
		END

	BEGIN TRY
    -- Insert statements for procedure here
	INSERT INTO GiangVien (MaGiangVien, HoTen, GioiTinh, TrinhDo, ChuyenMon, MaDonVi, MaPhanQuyen, TaiKhoan, MatKhau)
		VALUES (@MaGiangVien, @HoTen, @GioiTinh, @TrinhDo, @ChuyenMon, @MaDonVi, @MaPhanQuyen, @TaiKhoan, @MatKhau);

	SELECT @Code = 0, @Message = N'Thêm mới thành công!', @Data = null;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Thêm mới thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 15/5/2023
-- Description:	Thêm mới 1 lớp môn học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_InsertLopMonHoc] 
	-- Add the parameters for the stored procedure here
	@MaLopMonHoc nchar(10),
	@TenLopMonHoc nvarchar(50),
	@MaGiangVien nvarchar(50),
	@HocKy nvarchar(50),
	@NamHoc nvarchar(50),
	@MaMonHoc nvarchar(50),
	@SoSinhVien nvarchar(50)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @CodeCheck SMALLINT;
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@MaMonHoc IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Chưa chọn môn học!', @Data = NULL;
			RETURN;
		END

	IF(@MaGiangVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Chưa chọn giảng viên!', @Data = NULL;
			RETURN;
		END

	IF(@HocKy IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Chưa chọn học kỳ!', @Data = NULL;
			RETURN;
		END

	IF(@NamHoc IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Chưa chọn năm học!', @Data = NULL;
			RETURN;
		END

	IF(@SoSinhVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Chưa nhập số sinh viên của lớp!', @Data = NULL;
			RETURN;
		END

	SET @CodeCheck = (SELECT Count(1) FROM LopMonHoc WHERE exists (SELECT MaLopMonHoc FROM LopMonHoc WHERE MaLopMonHoc LIKE @MaLopMonHoc));

	IF(@CodeCheck > 0)
		BEGIN 
			SELECT @Code = -1, @Message = N'Lỗi trùng mã lớp môn học!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	BEGIN TRY
		BEGIN TRANSACTION AddClass

		INSERT INTO LopMonHoc (MaLopMonHoc, TenLopMonHoc, MaGiangVien)
			VALUES (@MaLopMonHoc, @TenLopMonHoc, @MaGiangVien);

		IF(@@ROWCOUNT > 0)
			BEGIN
				COMMIT TRANSACTION AddClass
				INSERT INTO MonHoc_LopMonHoc (MaLopMonHoc, HocKy, NamHoc, MaMonHoc, SoSinhVien)
					VALUES (@MaLopMonHoc, @HocKy, @NamHoc, @MaMonHoc, @SoSinhVien);

				SELECT @Code = 0, @Message = N'Thêm mới lớp thành công!', @Data = null;
				RETURN;
			END
		ELSE
			BEGIN
				ROLLBACK TRANSACTION AddClass
				SELECT @Code = 0, @Message = N'Thêm mới lớp thất bại!', @Data = null;
				RETURN;
			END

	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Thêm mới thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 4/5/2023
-- Description:	Thêm mới lớp môn học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_InsertMonHoc]
	-- Add the parameters for the stored procedure here
	@MaMonHoc nchar(10),
	@TenMonHoc nvarchar(50),
	@TongSoTiet nvarchar(50),
	@SoTietLyThuyet nvarchar(50),
	@SoTietThucHanh nvarchar(50)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @CodeCheck SMALLINT;
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@MaMonHoc IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Mã môn học không được để trống!', @Data = NULL;
			RETURN;
		END

	IF(@TenMonHoc IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Tên môn học không được để trống!', @Data = NULL;
			RETURN;
		END

	IF(@SoTietLyThuyet IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Số tiết lý thuyết không được để trống!', @Data = NULL;
			RETURN;
		END

	IF(@SoTietThucHanh IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Số tiết thực hành không được để trống!', @Data = NULL;
			RETURN;
		END

	SET @CodeCheck = (SELECT Count(1) FROM MonHoc WHERE exists (SELECT MaMonHoc FROM MonHoc WHERE MaMonHoc LIKE @MaMonHoc));

	IF(@CodeCheck > 0)
		BEGIN 
			SELECT @Code = -1, @Message = N'Lỗi trùng mã môn học!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	BEGIN TRY
		INSERT INTO MonHoc(MaMonHoc, TenMonHoc, TongSoTiet, SoTietLyThuyet, SoTietThucHanh)
			VALUES (@MaMonHoc, @TenMonHoc, @TongSoTiet, @SoTietLyThuyet, @SoTietThucHanh);

		SELECT @Code = 0, @Message = N'Thêm mới thành công!', @Data = null;
			RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Thêm mới thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 16/6/2023
-- Description:	Thêm mới 1 sinh viên vào lớp môn học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_InsertMotSinhVienToLopMH]
	-- Add the parameters for the stored procedure here
	@MaSinhVien nchar(10),
	@MaLopMonHoc nchar(10)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @CodeCheck SMALLINT;
	DECLARE @MonHocCheck SMALLINT;
	DECLARE @HocKy NVARCHAR(50);
	DECLARE @NamHoc NVARCHAR(50);
	DECLARE @MonHoc	NCHAR(10);
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @HocKy = ( SELECT MH_LMH.HocKy FROM MonHoc_LopMonHoc MH_LMH WHERE TRIM(MH_LMH.MaLopMonHoc) = TRIM(@MaLopMonHoc));
	SET @NamHoc = ( SELECT MH_LMH.NamHoc FROM MonHoc_LopMonHoc MH_LMH WHERE TRIM(MH_LMH.MaLopMonHoc) = TRIM(@MaLopMonHoc));
	SET	@MonHoc	= ( SELECT MH_LMH.MaMonHoc FROM MonHoc_LopMonHoc MH_LMH WHERE TRIM(MH_LMH.MaLopMonHoc) = TRIM(@MaLopMonHoc));

	SET @MonHocCheck = (SELECT Count(1) FROM SinhVien_Hoc_LopMonHoc WHERE exists (SELECT LMH.MaMonHoc FROM MonHoc_LopMonHoc LMH WHERE MaSinhVien LIKE @MaSinhVien AND MaMonHoc = @MonHoc));
	IF(@MonHocCheck <> 0)
		BEGIN 
			SELECT @Code = -1, @Message = N'Sinh viên này đã đăng ký môn học này!', @Data = NULL;
			RETURN;
		END


	SET @CodeCheck = (SELECT Count(1) FROM SinhVien_Hoc_LopMonHoc WHERE exists (SELECT MaSinhVien FROM SinhVien_Hoc_LopMonHoc WHERE MaSinhVien LIKE @MaSinhVien AND TRIM(MaLopMonHoc) = TRIM(@MaLopMonHoc)));
	IF(@CodeCheck <> 0)
		BEGIN 
			SELECT @Code = -1, @Message = N'Sinh viên này đã có trong lớp môn học!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	INSERT INTO SinhVien_Hoc_LopMonHoc (MaLopMonHoc, MaSinhVien, HocKy, NamHoc)
			VALUES (@MaLopMonHoc, @MaSinhVien, @HocKy, @NamHoc)
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 14/4/2023
-- Description:	Thêm mới sinh viên
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_InsertSinhVien]
	-- Add the parameters for the stored procedure here
	@MaSinhVien nchar(10)
	,@HoTen nvarchar(50)
	,@GioiTinh nvarchar(50)
	,@LopNienChe nvarchar(50)
	,@TaiKhoan nvarchar(50)
	,@MatKhau nvarchar(50)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @CodeCheck SMALLINT;

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@MaSinhVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Mã Sinh Viên không được để trống!', @Data = NULL;
			RETURN;
		END

	IF(@HoTen IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Họ Tên không được để trống!', @Data = NULL;
			RETURN;
		END

	SET @CodeCheck = (SELECT Count(1) FROM GiangVien WHERE exists (SELECT TaiKhoan FROM SinhVien WHERE TaiKhoan LIKE @TaiKhoan));

	IF(@CodeCheck > 0)
		BEGIN 
			SELECT @Code = -1, @Message = N'Email đã có người sử dụng, vui lòng chọn email khác!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	BEGIN TRY
    -- Insert statements for procedure here
	INSERT INTO SinhVien (MaSinhVien, HoTen, GioiTinh, LopNienChe, TaiKhoan, MatKhau)
		VALUES (@MaSinhVien, @HoTen, @GioiTinh, @LopNienChe, @TaiKhoan, @MatKhau);

	SELECT @Code = 0, @Message = N'Thêm mới thành công!', @Data = null;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Thêm mới thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 19/5/2023
-- Description:	Thêm tất cả sinh viên trong 1 lớp niên chế vào lớp môn học
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_InsertSinhVienToLopMH]
	-- Add the parameters for the stored procedure here
	@LopNienChe nchar(10),
	@MaLopMonHoc nchar(10)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN

	DECLARE @MyCursor CURSOR;
	DECLARE @MaSinhVien NCHAR(10);
	DECLARE @HocKy NVARCHAR(50);
	DECLARE @NamHoc NVARCHAR(50);
	DECLARE @MonHoc	NCHAR(10);
	DECLARE @CodeCheck SMALLINT;
	DECLARE @MonHocCheck SMALLINT;
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @HocKy = ( SELECT MH_LMH.HocKy FROM MonHoc_LopMonHoc MH_LMH WHERE TRIM(MH_LMH.MaLopMonHoc) = TRIM(@MaLopMonHoc));
	SET @NamHoc = ( SELECT MH_LMH.NamHoc FROM MonHoc_LopMonHoc MH_LMH WHERE TRIM(MH_LMH.MaLopMonHoc) = TRIM(@MaLopMonHoc));
	SET	@MonHoc	= ( SELECT MH_LMH.MaMonHoc FROM MonHoc_LopMonHoc MH_LMH WHERE TRIM(MH_LMH.MaLopMonHoc) = TRIM(@MaLopMonHoc));

	--SET @CodeCheck = (SELECT Count(1) FROM SinhVien_Hoc_LopMonHoc WHERE exists (SELECT MaLopMonHoc FROM SinhVien_Hoc_LopMonHoc WHERE MaLopMonHoc LIKE @MaLopMonHoc));

	--IF(@CodeCheck = 0)
	--	BEGIN 
	--		SELECT @Code = -1, @Message = N'Lỗi không tìm được mã lớp môn học!', @Data = NULL;
	--		RETURN;
	--	END

	SET @MyCursor = CURSOR FOR
    SELECT TOP 100 MaSinhVien FROM SinhVien
        WHERE LopNienChe = @LopNienChe

	OPEN @MyCursor 
    FETCH NEXT FROM @MyCursor 
    INTO @MaSinhVien

    -- Insert statements for procedure here
	WHILE @@FETCH_STATUS = 0
    BEGIN
		SET @MonHocCheck = (SELECT Count(1) FROM SinhVien_Hoc_LopMonHoc WHERE exists (SELECT LMH.MaMonHoc FROM MonHoc_LopMonHoc LMH WHERE MaSinhVien LIKE @MaSinhVien AND MaMonHoc = @MonHoc));
		IF(@MonHocCheck <> 0)
			BEGIN 
				SELECT @Code = -1, @Message = N'Một Sinh viên trong lớp đã đăng ký môn học này!', @Data = NULL;
				RETURN;
			END

		SET @CodeCheck = (SELECT Count(1) FROM SinhVien_Hoc_LopMonHoc WHERE exists (SELECT MaSinhVien FROM SinhVien_Hoc_LopMonHoc WHERE MaSinhVien LIKE @MaSinhVien AND TRIM(MaLopMonHoc) = TRIM(@MaLopMonHoc)));
		IF(@CodeCheck <> 0)
			BEGIN 
				SELECT @Code = -1, @Message = N'Lớp niên chế này đã có trong lớp môn học!', @Data = NULL;
				RETURN;
			END

		/*YOUR ALGORITHM GOES HERE*/
		INSERT INTO SinhVien_Hoc_LopMonHoc (MaLopMonHoc, MaSinhVien, HocKy, NamHoc)
			VALUES (@MaLopMonHoc, @MaSinhVien, @HocKy, @NamHoc)
		
		IF(@@ROWCOUNT = 0)
			BEGIN
				SELECT @Code = -1, @Message = N'Lỗi không tìm được mã lớp môn học!', @Data = NULL;
				RETURN;
			END

		FETCH NEXT FROM @MyCursor 
		INTO @MaSinhVien 
    END;

	IF (@@FETCH_STATUS <> 0)
		BEGIN
			SELECT @Code = 0, @Message = N'Thêm mới tất cả lớp thành công!', @Data = NULL;
			RETURN;
		END
	ELSE
		

    CLOSE @MyCursor ;
    DEALLOCATE @MyCursor;
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 20/6/2023
-- Description:	Thêm điểm kiểmt ra cho sinh viên
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_ThemDiemKiemTra]
	-- Add the parameters for the stored procedure here
	@MaGiangVien nchar(10), 
	@MaSinhVien nchar(10),
	@MaLopMonHoc nchar(10),
    @MaBuoiKiemTra nchar(10),
    @Diem float

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@MaGiangVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã Giảng Viên!', @Data = NULL;
			RETURN;
		END

	IF(@MaSinhVien IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã sinh viên!', @Data = NULL;
			RETURN;
		END

	IF(@MaLopMonHoc IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Không nhận dạng được Mã lớp môn học!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	BEGIN TRY
    -- Insert statements for procedure here
		INSERT INTO LopMonHoc_KiemTra(MaGiangVien, MaSinhVien, MaLopMonHoc, MaBuoiKiemTra, DiemKiemTra)
		VALUES (@MaGiangVien, @MaSinhVien, @MaLopMonHoc, @MaBuoiKiemTra, @Diem);

		SELECT @Code = 0, @Message = N'Thêm điểm thành công!', @Data = null;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Thêm điểm thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 10/4/2023
-- Description:	Lấy ra các thông tin tài khoản
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_ThongTinTaiKhoan] 
	-- Add the parameters for the stored procedure here
	@MaNhanVien nchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	-- lấy ra thông tin của tài khoản vừa đăng nhập
	SELECT GV.MaGiangVien, GV.HoTen, GV.GioiTinh, GV.TrinhDo, GV.ChuyenMon, GV.MaDonVi, DV.TenDonVi, GV.MaPhanQuyen, PQ.TenPhanQuyen
	FROM GiangVien GV
	INNER JOIN PhanQuyen PQ ON PQ.MaPhanQuyen = GV.MaPhanQuyen
	LEFT JOIN DonVi DV ON DV.MaDonVi = GV.MaDonVi OR GV.MaDonVi = NULL
	WHERE MaGiangVien = @MaNhanVien
	RETURN;
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 16/5/2023
-- Description:	Update Giảng Viên hoặc Nhân Viên Phòng đào tạo
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_UpdateEmployee]
	-- Add the parameters for the stored procedure here
	@MaGiangVien nchar(10)
	,@HoTen nvarchar(50)
	,@GioiTinh nvarchar(50)
	,@TrinhDo nvarchar(50)
	,@MaPhanQuyen nchar(10)
	,@ChuyenMon nvarchar(50)
	,@MaDonVi nchar(10)


	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@HoTen IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Họ Tên không được để trống!', @Data = NULL;
			RETURN;
		END

	IF(@MaDonVi IS NULL AND @MaPhanQuyen = 2)
		BEGIN 
			SELECT @Code = -1, @Message = N'Đơn vị không được để trống!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	BEGIN TRY
		IF (@MaPhanQuyen = 1)
		BEGIN
			UPDATE GiangVien 
			SET HoTen = @HoTen, GioiTinh = @GioiTinh, TrinhDo = @TrinhDo
			WHERE MaGiangVien = @MaGiangVien

			SELECT @Code = 0, @Message = N'Cập nhật Nhân viên thành công!', @Data = null;
			RETURN;
		END
		ELSE IF (@MaPhanQuyen = 2)
		BEGIN
			UPDATE GiangVien 
			SET HoTen = @HoTen, GioiTinh = @GioiTinh, TrinhDo = @TrinhDo, ChuyenMon = @ChuyenMon, MaDonVi = @MaDonVi
			WHERE MaGiangVien = @MaGiangVien

			SELECT @Code = 0, @Message = N'Cập nhật Giảng viên thành công!', @Data = null;
			RETURN;
		END
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Cập nhật thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 16/5/2023
-- Description:	Cập nhật thông tin sinh viên
-- =============================================
CREATE PROCEDURE [dbo].[GV_SP_UpdateSinhVien]
	-- Add the parameters for the stored procedure here
	@MaSinhVien nchar(10)
	,@HoTen nvarchar(50)
	,@GioiTinh nvarchar(50)
	,@LopNienChe nvarchar(50)

	-- Return Result
	,@Code INT OUTPUT
	,@Message NVARCHAR(100) OUTPUT
	,@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@HoTen IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Họ Tên không được để trống!', @Data = NULL;
			RETURN;
		END

    -- Insert statements for procedure here
	BEGIN TRY
		UPDATE SinhVien 
		SET HoTen = @HoTen, GioiTinh = @GioiTinh, LopNienChe = @LopNienChe
		WHERE MaSinhVien = @MaSinhVien

		SELECT @Code = 0, @Message = N'Cập nhật Sinh viên thành công!', @Data = null;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT @Code = -1, @Message = N'Cập nhật Sinh viên thất bại!', @Data = ERROR_MESSAGE(); 
	END CATCH
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 21/2/2023
-- Description:	Kiểm tra tình trạng tài khoản login
-- =============================================
CREATE PROCEDURE [dbo].[SV_SP_CheckLogin] 
	-- Add the parameters for the stored procedure here
	@taikhoan VARCHAR(100),
	@matkhau VARCHAR(100),

	-- Return Result
	@Code INT OUTPUT,
	@Message NVARCHAR(100) OUTPUT,
	@Data NVARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @CodeCheck SMALLINT;
	DECLARE @ActicveCheck SMALLINT;
	-- Insert statements for procedure here
	SET NOCOUNT ON;
	IF(@taikhoan IS NULL OR @matkhau IS NULL)
		BEGIN 
			SELECT @Code = -1, @Message = N'Tài Khoản hoặc mật khẩu không được để rỗng!', @Data = NULL;
			RETURN;
		END
	
	SET @CodeCheck = (SELECT Count(1) FROM SinhVien WHERE TaiKhoan = @taikhoan AND MatKhau = @matkhau);

	SET @ActicveCheck = (SELECT SV.HoatDong FROM SinhVien SV WHERE TaiKhoan = @taikhoan AND MatKhau = @matkhau);

	IF(@CodeCheck <> 0)
		BEGIN
			SELECT @Code = 0, @Message = N'Đăng nhập thành công!', @Data = (SELECT SinhVien.MaSinhVien FROM SinhVien WHERE TaiKhoan = @taikhoan AND MatKhau = @matkhau);
			RETURN;
		END
	ELSE
		BEGIN
			SELECT @Code = -1, @Message = N'Đăng nhập không thành công!', @Data = NULL;
			RETURN;
		END;
END 
GO


-- =============================================
-- Author:		TungLX
-- Create date: 16/6/2023
-- Description:	Lấy ra lớp môn học mà sinh viên chọn để xem chi tiết điểm danh
-- =============================================
CREATE PROCEDURE [dbo].[SV_SP_GetAllDiemDanhBySinhVien]
	-- Add the parameters for the stored procedure here
	@MaSinhVien NCHAR(12) NULL,
	@MaLopMonHOc NCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DD.IDDiemDanh
	  ,DD.MaSinhVien
	  ,LMH.TenLopMonHoc
	  ,GV.HoTen AS TenGV
	  ,SV.HoTen AS TenSV
	  ,CH.GioBD
	  ,CH.GioKT
	  ,BH.BuoiHocSo
	  ,DD.NgayDiemDanh
	  ,DD.TrangThai
	FROM DiemDanh DD
	INNER JOIN LopMonHoc LMH ON LMH.MaLopMonHoc = DD.MaLopMonHoc
	INNER JOIN CaHoc CH ON CH.MaCaHoc = DD.MaCaHoc
	INNER JOIN BuoiHoc BH ON BH.MaBuoiHoc = DD.MaBuoiHoc
	INNER JOIN GiangVien GV ON GV.MaGiangVien = DD.MaGiangVien
	LEFT JOIN SinhVien SV ON SV.MaSinhVien = DD.MaSinhVien
	WHERE (@MaSinhVien IS NULL OR @MaSinhVien = DD.MaSinhVien) 
	AND (@MaLopMonHoc IS NULL OR TRIM(@MaLopMonHoc) = TRIM(DD.MaLopMonHoc))
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 16/6/2023
-- Description:	Lấy ra các lớp môn học mà sinh viên đang theo học
-- =============================================
CREATE PROCEDURE [dbo].[SV_SP_GetAllLopMonHoc]
	-- Add the parameters for the stored procedure here
	@MaSinhVien NCHAR(12) NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SV_H_LMH.MaSinhVien
	  ,SV_H_LMH.MaLopMonHoc
	  ,LMH.TenLopMonHoc
	  ,LMH.MaGiangVien
	  ,SV.HoTen
	  ,SV_H_LMH.HocKy
	  ,SV_H_LMH.NamHoc
      ,SV.LopNienChe
      ,SV.HoatDong
	FROM SinhVien_Hoc_LopMonHoc SV_H_LMH
	INNER JOIN LopMonHoc LMH ON LMH.MaLopMonHoc = SV_H_LMH.MaLopMonHoc
	LEFT JOIN SinhVien SV ON SV.MaSinhVien = SV_H_LMH.MaSinhVien
	--WHERE ISNULL(@MaGiangVien, ' ') = ' ';
	WHERE (@MaSinhVien IS NULL OR @MaSinhVien = SV_H_LMH.MaSinhVien)
END
GO

-- =============================================
-- Author:		TungLX
-- Create date: 20/6/2023
-- Description:	Sinh viên lấy ra điểm quá trình của bản thân
-- =============================================
CREATE PROCEDURE [dbo].[SV_SP_GetDiemQuaTrinh]
	-- Add the parameters for the stored procedure here
	@MaSinhVien NCHAR(10),
	@MaLopMonHoc NCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT t1.MaSinhVien, t1.TenGV, t1.TenSV, t1.LopNienChe, t1.MaLopMonHoc, t1.TenLopMonHoc, t1.BaiKiemTra1 AS BaiKiemTra1, t1.BaiKiemTra2 AS BaiKiemTra2, t1.BaiKiemTra3 AS BaiKiemTra3, t2.TongCoMat AS TongCoMat, t2.TongVangMat AS TongVangMat
	FROM 
		(SELECT A.MaSinhVien, A.MaLopMonHoc, A.TenLopMonHoc, A.TenGV, TenSV, A.LopNienChe, SUM(A.LanKT1) AS BaiKiemTra1, SUM(A.LanKT2) AS BaiKiemTra2, SUM(A.LanKT3) AS BaiKiemTra3 FROM (
			SELECT DQT.MaSinhVien
				,SinhVien.HoTen AS TenSV
				,GiangVien.HoTen AS TenGV
				,SinhVien.LopNienChe
				,DQT.MaLopMonHoc
				,LMH.TenLopMonHoc
				,(case MaBuoiKiemTra when 'KT01' then DiemKiemTra else null end) AS LanKT1
				,(case MaBuoiKiemTra when 'KT02' then DiemKiemTra else null end) AS LanKT2
				,(case MaBuoiKiemTra when 'KT03' then DiemKiemTra else null end) AS LanKT3
			FROM LopMonHoc_KiemTra DQT
			LEFT JOIN SinhVien ON DQT.[MaSinhVien] = SinhVien.[MaSinhVien]
			LEFT JOIN GiangVien ON DQT.MaGiangVien = GiangVien.MaGiangVien
			INNER JOIN LopMonHoc LMH ON DQT.MaLopMonHoc = LMH.MaLopMonHoc
			WHERE TRIM(DQT.MaLopMonHoc) LIKE TRIM(@MaLopMonHoc)
			group by DQT.MaSinhVien, DQT.MaBuoiKiemTra, DQT.MaLopMonHoc, LMH.TenLopMonHoc, GiangVien.HoTen, SinhVien.HoTen, SinhVien.LopNienChe, DiemKiemTra) A
		GROUP BY MaSinhVien, MaLopMonHoc, TenLopMonHoc, TenGV, TenSV, LopNienChe) t1
	LEFT JOIN
		(SELECT A.MaSinhVien, A.MaLopMonHoc,  A.TenLopMonHoc, A.TenGV, TenSV, A.LopNienChe, SUM(A.CoMat) AS TongCoMat, SUM(A.VangMat) AS TongVangMat FROM (
			SELECT DD.[MaSinhVien]
				,GiangVien.HoTen AS TenGV
				,SinhVien.HoTen AS TenSV
				,SinhVien.LopNienChe
				,DD.MaLopMonHoc
				,LMH.TenLopMonHoc
				,COUNT(case [TrangThai] when 0 then 1 else null end ) AS CoMat
				,COUNT(case [TrangThai] when 1 then 1 else null end ) AS VangMat
			FROM [DDSinhVien].[dbo].[DiemDanh] DD
			LEFT JOIN SinhVien ON DD.[MaSinhVien] = SinhVien.[MaSinhVien]
			LEFT JOIN GiangVien ON DD.MaGiangVien = GiangVien.MaGiangVien
			INNER JOIN LopMonHoc LMH ON DD.MaLopMonHoc = LMH.MaLopMonHoc
			WHERE TRIM(DD.MaLopMonHoc) LIKE TRIM(@MaLopMonHoc)
			group by DD.[MaSinhVien], [TrangThai], DD.MaLopMonHoc, LMH.TenLopMonHoc, GiangVien.HoTen, SinhVien.HoTen, SinhVien.LopNienChe) A
		GROUP BY MaSinhVien, MaLopMonHoc, TenLopMonHoc, TenGV, TenSV, LopNienChe) t2
	ON (t1.MaSinhVien = t2.MaSinhVien)
	WHERE TRIM(t1.MaSinhvien) = TRIM(@MaSinhVien);
END
GO


-- =============================================
-- Author:		TungLX
-- Create date: 18/6/2023
-- Description:	Lấy ra thông tin của sinh viên khi đăng nhập thành công
-- =============================================
CREATE PROCEDURE [dbo].[SV_SP_GetThongTinTK]
	-- Add the parameters for the stored procedure here
	@MaSinhVien NCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaSinhVien, HoTen, LopNienChe, HoatDong FROM SinhVien WHERE MaSinhVien LIKE @MaSinhVien
END
GO
