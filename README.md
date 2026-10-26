# CSLab08 - LINQ to SQL

Ứng dụng Windows Forms quản lý học sinh theo nội dung LAB 08 - LINQ: kết nối SQL Server, lọc học sinh theo lớp/tên và thực hiện thêm, sửa, xóa.

## Yêu cầu
- Windows
- Visual Studio 2019/2022
- .NET Framework 4.8 Developer Pack
- SQL Server (LocalDB hoặc SQL Server Express)

## Cài đặt
1. Mở SQL Server Management Studio và chạy `database/QLHS.sql`.
2. Mở `CSLab08-LINQ.csproj` bằng Visual Studio.
3. Sửa chuỗi kết nối trong `App.config` nếu SQL Server của bạn không dùng LocalDB mặc định.
4. Build và chạy dự án.

> Dự án dùng LINQ to SQL (`System.Data.Linq`). Nếu Visual Studio không có sẵn reference này, hãy dùng workload .NET desktop development và .NET Framework 4.8 Developer Pack.

## Chức năng
- Hiển thị lớp trong ComboBox.
- Lọc học sinh theo lớp và tên gần đúng.
- Thêm học sinh.
- Sửa thông tin học sinh theo mã.
- Xóa học sinh theo mã.
- Chọn một dòng trong bảng để nạp thông tin lên biểu mẫu.

## Cấu trúc
- `Program.cs`: điểm khởi chạy.
- `frmQLHocSinh.cs`: giao diện và xử lý LINQ.
- `Models.cs`: các lớp ánh xạ dữ liệu LINQ to SQL.
- `database/QLHS.sql`: tạo cơ sở dữ liệu, bảng và dữ liệu mẫu.
- `App.config`: cấu hình kết nối.

## Ghi chú
Tên bảng và cột được định nghĩa trong script SQL đi kèm. Dữ liệu mẫu có thể thay đổi theo nhu cầu.
