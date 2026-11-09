using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
public static class Validation
{
    public static void Text(string? value, string field, int max = 200)
    {
        if (string.IsNullOrWhiteSpace(value)) throw ApiException.Field(field, "Vui lòng nhập thông tin này.");
        if (value.Trim().Length > max) throw ApiException.Field(field, $"Tối đa {max} ký tự.");
    }
    public static void Facility(FacilityInput x)
    {
        Text(x.Name, "name"); Text(x.Address, "address", 500); Text(x.CityDistrict, "cityDistrict");
        if (!Regex.IsMatch(x.Phone ?? "", @"^\+?[0-9]{9,15}$")) throw ApiException.Field("phone", "Số điện thoại cần 9–15 chữ số, có thể bắt đầu bằng +.");
        if (string.IsNullOrWhiteSpace(x.Email) || x.Email.Length > 254 || !new EmailAddressAttribute().IsValid(x.Email)) throw ApiException.Field("email", "Email không hợp lệ.");
        if (x.Closing.ToTimeSpan() - x.Opening.ToTimeSpan() < TimeSpan.FromHours(4)) throw ApiException.Field("closing", "Giờ đóng cửa phải sau giờ mở cửa ít nhất 4 tiếng.");
        if (!Catalog.SlotMinutes.Contains(x.SlotMinutes)) throw ApiException.Field("slotMinutes", "Chọn bước thời gian 30, 60, 90 hoặc 120 phút.");
        if (x.Latitude is < -90 or > 90) throw ApiException.Field("latitude", "Vĩ độ phải trong khoảng -90 đến 90.");
        if (x.Longitude is < -180 or > 180) throw ApiException.Field("longitude", "Kinh độ phải trong khoảng -180 đến 180.");
        if (!string.IsNullOrWhiteSpace(x.ImageUrl) && (!Uri.TryCreate(x.ImageUrl, UriKind.Absolute, out var uri) || uri.Scheme != "https"))
            throw ApiException.Field("imageUrl", "Ảnh cần có đường dẫn HTTPS hợp lệ.");
    }
    public static void Court(string name, string surface, string? notes)
    {
        Text(name, "name");
        if (!Catalog.Surfaces.Contains(surface)) throw ApiException.Field("surface", "Loại mặt sân không hợp lệ.");
        if (notes?.Length > 2000) throw ApiException.Field("notes", "Ghi chú tối đa 2000 ký tự.");
    }
    public static void Version(int? submitted, int current)
    {
        if (submitted != current) throw new ApiException(409, "Dữ liệu đã thay đổi. Hãy tải lại trang trước khi lưu.");
    }
    public static void Policy(PolicyInput x, DateOnly today)
    {
        Text(x.Name, "name");
        if (x.Scope is not ("Facility" or "Chain")) throw ApiException.Field("scope", "Phạm vi không hợp lệ.");
        if (x.Scope == "Facility" && x.FacilityId is not > 0) throw ApiException.Field("facilityId", "Vui lòng chọn cơ sở.");
        if (x.Scope == "Chain" && x.FacilityId != null) throw ApiException.Field("facilityId", "Chính sách toàn chuỗi không gắn riêng một cơ sở.");
        if (!Catalog.Sports.Contains(x.Sport)) throw ApiException.Field("sport", "Môn thể thao không được hỗ trợ.");
        foreach (var (key, value) in new[] { ("regular", x.Regular), ("peak", x.Peak), ("weekend", x.Weekend) })
            if (value <= 0 || value > 999999999999.99m || decimal.Round(value, 2) != value)
                throw ApiException.Field(key, "Giá phải lớn hơn 0, tối đa 999.999.999.999,99 và có tối đa 2 chữ số thập phân.");
        if (x.Peak < x.Regular * 1.2m) throw ApiException.Field("peak", "Giá giờ cao điểm phải ít nhất bằng 1,20 lần giá thường (BR233).");
        if (x.Discount < 0 || x.Discount > 100 || decimal.Round(x.Discount, 2) != x.Discount) throw ApiException.Field("discount", "Chiết khấu từ 0–100%, tối đa 2 chữ số thập phân.");
        if (x.PeakEnd <= x.PeakStart) throw ApiException.Field("peakEnd", "Giờ kết thúc cao điểm phải sau giờ bắt đầu.");
        if (x.EffectiveStart < today) throw ApiException.Field("effectiveStart", "Ngày hiệu lực không được ở trong quá khứ (BR245).");
    }
}
