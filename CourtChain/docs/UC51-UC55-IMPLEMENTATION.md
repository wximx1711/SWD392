# Triển khai UC51–UC55

Hồ Tuấn Minh · HE191253 · SE1990NJ

## Đối chiếu yêu cầu

| UC | Frontend | Backend service | Luồng chính |
|---|---|---|---|
| UC51 | `features/uc51-register-facility` | `UC51_RegisterFacility.cs` | Form cơ sở → validate → Active + giờ/bước thời gian → audit → mở chi tiết. |
| UC52 | `features/uc52-update-facility` | `UC52_UpdateFacilityProfile.cs` | Prefill → sửa → lý do → xác nhận giảm giờ → kiểm tra version → lưu và audit before/after. |
| UC53 | `features/uc53-register-court` | `UC53_RegisterCourt.cs` | Chọn cấu hình sân trong cơ sở Active → kiểm tra tên → tạo một hoặc nhiều sân atomic. |
| UC54 | `features/uc54-update-court` | `UC54_UpdateCourtProfile.cs` | Sửa tên/mặt sân/đèn/ghi chú/trạng thái → xác nhận ngừng hoạt động → audit; giữ ID, cơ sở, môn. |
| UC55 | `features/uc55-create-pricing` | `UC55_CreateCourtPricingPolicy.cs` | Chọn Facility/Chain + sport → giá/chiết khấu/thời gian → kiểm tra BR233/244/245 → công bố + audit. |

## Quy tắc

- Chỉ Court Owner được đọc/ghi dữ liệu quản trị; token ngẫu nhiên, mật khẩu PBKDF2, hết phiên sau 30 phút không hoạt động; logout thu hồi token.
- Tên cơ sở duy nhất toàn chuỗi; tên sân duy nhất trong cơ sở, so sánh chuẩn hóa Unicode và không phân biệt hoa thường trong service.
- Cơ sở tối thiểu 4 giờ hoạt động; giờ mở trước đóng; điện thoại 9–15 số; email hợp lệ; tọa độ có giới hạn.
- Môn: Badminton, Pickleball, Table Tennis. Mặt sân: Wood, Rubber, Acrylic. Sân mới Active và bắt buộc thuộc một cơ sở Active.
- Giá là decimal; các mức giá >0, tối đa 2 chữ số thập phân; peak >= 1,20 regular. Không gắn giá riêng court_id.
- UC55 tạo khoảng hiệu lực mở [start, vô hạn); không được trùng scope + facility (nếu có) + sport với bất kỳ phiên bản giao nhau nào, kể cả Scheduled. Mỗi policy chứa đủ ba loại khung giờ nên kiểm tra ở mức policy bao phủ cả ba.
- Ngày nghiệp vụ theo UTC+7. Status trả ra được tính theo ngày hiện tại; Scheduled tự hiển thị Active khi tới ngày.
- Mỗi thay đổi có audit before/after, actor, UTC timestamp, lý do; không có API xóa vật lý. UC52/54 dùng Version để chặn ghi đè dữ liệu cũ.
- Facility lưu template giờ mở/đóng/bước thời gian. Court tham chiếu lịch qua FacilityId; endpoint schedule sinh các khung đầy đủ nằm trong giờ hoạt động.

## Mâu thuẫn / giả định trong báo cáo

1. UC54 được lặp hai lần. Dùng chung một triển khai.
2. UC55 cho phép toàn chuỗi; entity CourtPrice nói thuộc đúng một Facility. Ưu tiên UC55: Chain có FacilityId=null, là mặc định cho mọi cơ sở; Facility có khóa ngoại.
3. UC55 mô tả cuối tuần cao điểm cả ngày; BR27 chung nói cao điểm 17–22 hằng ngày. Ưu tiên UC55: form/list thể hiện Weekend rate cả ngày T7/CN. Chưa có checkout để tính tiền đặt chỗ.
4. BR244/245 tại bảng BR tổng quát nói inventory, nhưng UC55 diễn giải cho giá sân. Dùng diễn giải UC55.
5. UC55 không cho ngày kết thúc và UC56 riêng sửa giá; vì vậy UC55 không tự đóng policy cũ. Policy trùng phải bị từ chối. Sửa/ngừng policy thuộc UC56/78 ngoài phạm vi.
6. Báo cáo không chỉ rõ trần chiết khấu định kỳ hoặc danh sách slot granularity. Demo dùng 0–100%, tối đa 2 số lẻ; bước 30/60/90/120 phút. Đây là quyết định triển khai, không tự gán số BR.
7. Môn thể thao là danh mục master, không bắt buộc cơ sở phải có sân trước mới tạo giá.

## Giới hạn tích hợp

Không có booking module: `IBookingConflictChecker` là điểm tích hợp, implementation hiện tại không đối chiếu reservation thật. UI cảnh báo và yêu cầu xác nhận khi giảm giờ hoặc ngừng sân; không được coi xác nhận này là thay thế kiểm tra booking trong hệ thống đầy đủ. Ảnh dùng URL HTTPS; chưa upload cloud. NFR production (JWT refresh, HTTPS, tải 200 người, Redis, backup tự động) ngoài demo local.

## Database / API

SQLite: Users, Facilities, Courts, Policies, Audits. Hai migration InitialCreate và AdministrationRevisions; migration sau chỉ thêm cột/index. FK Courts→Facilities bắt buộc; Policies→Facilities tùy scope. Audits.ActorId/EntityId là tham chiếu logic, không phải FK. Decimal được SQLite provider lưu TEXT để giữ giá trị chính xác.

POST login/logout; GET me/options; GET/POST facilities; GET/PUT facilities/{id}; GET facilities/{id}/courts và /schedule; GET courts/{id}; POST courts (trả array, hỗ trợ Count); PUT courts/{id}; GET/POST pricing-policies; GET audits?entity=&entityId=. OpenAPI tại /openapi/v1.json.
