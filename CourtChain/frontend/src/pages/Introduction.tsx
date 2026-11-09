import { Icon } from '../components/Icon'
export function Introduction() {
  return <><section className="intro-hero"><div><span className="eyebrow">SWD392 · CHAIN ADMINISTRATION</span><h2>Xin chào, mình là<br/><span>Hồ Tuấn Minh.</span></h2><p>Đồ án CourtChain — hệ thống quản lý và vận hành chuỗi sân thể thao. Phần triển khai tập trung vào 5 use case dành cho chủ sân.</p><div className="identity-tags"><span>MSV <strong>HE191253</strong></span><span>Lớp <strong>SE1990NJ</strong></span></div><a className="button primary" href="#facilities">Bắt đầu quản lý cơ sở<Icon name="arrow"/></a></div><div className="intro-mark" aria-hidden="true"><Icon name="court" size={160}/><span>COURTCHAIN</span><small>ONE CHAIN. EVERY COURT.</small></div></section><div className="section-heading"><div><span className="eyebrow">PHẠM VI TRIỂN KHAI</span><h2>Năm luồng quản trị</h2></div><span className="muted">Court Owner</span></div><div className="usecase-grid">{[
    ['UC51', 'Đăng ký cơ sở', 'Tạo chi nhánh, khai báo liên hệ, tiện ích và lịch hoạt động.', '#facilities', 'building'],
    ['UC52', 'Cập nhật hồ sơ cơ sở', 'Điều chỉnh thông tin, lưu phiên bản và lý do thay đổi.', '#facilities', 'edit'],
    ['UC53', 'Đăng ký sân', 'Thêm một hoặc nhiều sân trong cơ sở đang hoạt động.', '#facilities', 'court'],
    ['UC54', 'Cập nhật hồ sơ sân', 'Đổi tên, mặt sân, chiếu sáng và trạng thái hoạt động.', '#facilities', 'edit'],
    ['UC55', 'Tạo chính sách giá', 'Giá theo cơ sở hoặc toàn chuỗi, môn thể thao và khung giờ.', '#pricing', 'price'],
  ].map(([code, title, desc, href]) => <a href={href} className="usecase-card" key={code}><span className="uc-code">{code}</span><h3>{title}</h3><p>{desc}</p><Icon name="arrow" size={18}/></a>)}</div></>
}
