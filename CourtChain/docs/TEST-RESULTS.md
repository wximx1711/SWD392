# Kết quả kiểm tra đã thực hiện

Hồ Tuấn Minh · HE191253 · SE1990NJ

- Backend build: thành công.
- Frontend TypeScript/Vite build: thành công.
- API integration tests với SQLite: 26/26 passed. Bao gồm các nhóm UC51–UC55, anonymous access, non-owner, logout, audit, batch atomic, cập nhật phiên bản cũ và các lỗi giá.
- Browser test: đã chạy một phần và chụp màn hình; dừng theo yêu cầu người dùng trước khi có kết quả xác nhận hoàn tất toàn bộ. Không công bố E2E passed.
- Booking conflict với reservation thật: chưa có module booking nên chưa xác minh.
- NuGet có cảnh báo cho Microsoft.OpenApi 2.0.0 và SQLitePCLRaw.lib.e_sqlite3 2.1.11; đây là bản demo local.

Lệnh chạy lại khi cần: `dotnet test tests/CourtChain.Tests.csproj`; frontend: `npm run build`, `npx playwright test`.
