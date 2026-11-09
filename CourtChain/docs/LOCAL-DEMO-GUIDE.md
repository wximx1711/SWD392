# Hướng dẫn chạy demo

Hồ Tuấn Minh · HE191253 · SE1990NJ

1. Cài .NET SDK 10 và Node.js 22.12 trở lên.
2. Tại thư mục gốc, PowerShell:

```powershell
$env:COURTCHAIN_OWNER_PASSWORD='DemoMinh-2026!'
dotnet run --project backend --no-launch-profile --urls http://localhost:5000
```

3. Terminal mới:

```powershell
cd frontend
npm ci
npm run dev
```

4. Mở http://127.0.0.1:5173. Đăng nhập `owner@courtchain.local` với mật khẩu đã đặt. Mật khẩu chỉ được dùng khi seed lần đầu; đổi env không đổi mật khẩu tài khoản cũ.
5. Trang Giới thiệu hiển thị Hồ Tuấn Minh / HE191253 / SE1990NJ.
6. Cơ sở & sân → Đăng ký cơ sở: CourtChain Hòa Lạc, địa chỉ, quận/huyện, 0912345678, hoalac@example.com, 06:00–22:00, 60 phút, tiện ích. Sau lưu mở chi tiết. Refresh để kiểm tra.
7. Chỉnh sửa cơ sở: đổi hotline và giờ đóng 21:00, nhập lý do, xác nhận giảm giờ. Lưu và refresh.
8. Tab Sân thể thao → đăng ký Badminton 01, Badminton 02, Pickleball 01. Thử trùng Badminton 01: bị từ chối. Có thể chọn số lượng >1 để đăng ký batch theo tiền tố 01, 02... trong một transaction.
9. Chỉnh sửa Badminton 01 thành Badminton VIP 01; chọn Rubber, Under Maintenance; nhập lý do và xác nhận. Kiểm tra lịch sử sân.
10. Chính sách giá → Tạo chính sách giá: chọn cơ sở và Cầu lông, 80.000 thường / 120.000 cao điểm / 140.000 cuối tuần, 17:00–22:00, ngày hiện tại hoặc tương lai. Công bố và refresh.
11. Thử 90.000 cao điểm: báo lỗi 1,20x. Thử ngày cũ: lỗi BR245. Thử policy cùng scope/sport: lỗi BR244. Có thể tạo policy Toàn chuỗi cho môn khác để demo phạm vi.

## SQL

`database/CourtChain.sql` là SQLite SQL tạo database trống từ hai migration, kèm lịch sử EF. Không chứa mật khẩu hoặc dữ liệu cá nhân. Cách đơn giản nhất vẫn là chạy backend để tự migrate. Nếu có sqlite3 CLI, có thể tạo database bằng `.read database/CourtChain.sql` trong database trống trước khi chạy backend.

## Test

```powershell
dotnet test tests/CourtChain.Tests.csproj
cd frontend
npm run build
npx playwright test
```

E2E dùng Microsoft Edge trên Windows theo đường dẫn mặc định; có thể đặt BROWSER_PATH trỏ tới Chromium/Chrome khác. Test tự dùng port 5100/5175 và SQLite tạm, không sửa database demo.

## Khắc phục

- Backend cũ đang chạy: Ctrl+C trước khi build/chạy bản mới.
- Sai mật khẩu trên database cũ: dùng mật khẩu đã seed lần đầu, không xóa database đang cần giữ.
- API không kết nối: kiểm tra cổng 5000, khởi động lại Vite sau khi thay cấu hình.
- Không cần import SQL nếu đã để backend tự migrate.
