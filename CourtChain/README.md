# CourtChain — Hồ Tuấn Minh

**MSV:** HE191253 · **Lớp:** SE1990NJ · **Môn:** SWD392

Demo quản trị chuỗi sân cho Court Owner: UC51 đăng ký cơ sở, UC52 cập nhật cơ sở, UC53 đăng ký sân, UC54 cập nhật sân, UC55 tạo chính sách giá.

## Chạy nhanh

Cần .NET SDK 10 và Node.js 22.12 trở lên. Mở terminal tại thư mục giải nén:

```powershell
$env:COURTCHAIN_OWNER_PASSWORD='DemoMinh-2026!'
dotnet run --project backend --no-launch-profile --urls http://localhost:5000
```

Terminal thứ hai:

```powershell
cd frontend
npm ci
npm run dev
```

Mở http://127.0.0.1:5173. Email: `owner@courtchain.local`. Mật khẩu là giá trị bạn đặt ở terminal đầu. Ví dụ trên chỉ dùng cho demo local. Tài khoản được tạo một lần khi database chưa có người dùng.

Database SQLite tự tạo và migrate tại `backend/courtchain.db`; không cần cài SQL Server. File `database/CourtChain.sql` dành cho SQLite, dùng khi muốn tạo cấu trúc thủ công trên database trống. Không chạy lại file SQL lên database đã có bảng. OpenAPI: http://localhost:5000/openapi/v1.json.

## Cấu trúc nộp bài

```text
backend/
  Controllers/          API và phân quyền
  UseCases/             UC51_... đến UC55_...: nghiệp vụ riêng từng UC
  Contracts/            Request/response DTO
  Domain/               Entity và danh mục
  Data/                 Repository, DbContext, seed
  Infrastructure/       Auth, lỗi, audit, thời gian
  Integration/          Điểm tích hợp kiểm tra booking
  Migrations/           Migration không xóa dữ liệu hiện có
frontend/src/
  features/uc51-.../     Form UC51
  features/uc52-.../     Form UC52
  features/uc53-.../     Form UC53
  features/uc54-.../     Form UC54
  features/uc55-.../     Form UC55
  pages/                Giới thiệu, danh sách, chi tiết, giá
  components/           Thành phần dùng chung
  lib/                  Kiểu dữ liệu và API client
frontend/e2e/           Kịch bản kiểm thử trình duyệt
 tests/                Kiểm thử API cho từng UC
 database/             CourtChain.sql
 docs/                 Hướng dẫn, pattern, kết quả test, sơ đồ DB
```

Xem `docs/LOCAL-DEMO-GUIDE.md` để demo; `docs/DESIGN-PATTERNS.md` để giải thích kiến trúc. Giới hạn: chưa có module đặt chỗ, nên chưa kiểm tra xung đột booking thật; ảnh cơ sở dùng URL HTTPS, chưa có upload. Các giới hạn và mâu thuẫn của báo cáo được ghi trong tài liệu triển khai.
