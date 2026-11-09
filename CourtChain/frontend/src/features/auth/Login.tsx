import { useState } from 'react'
import type { FormEvent } from 'react'
import { api } from '../../lib/api'
import { FormError } from '../../components/UI'
import { Icon } from '../../components/Icon'
export function Login({ onLogin, expired }: { onLogin: () => void; expired: boolean }) {
  const [email, setEmail] = useState('owner@courtchain.local'), [password, setPassword] = useState('')
  const [busy, setBusy] = useState(false), [error, setError] = useState('')
  async function submit(e: FormEvent) {
    e.preventDefault(); setBusy(true); setError('')
    try { const data = await api<{ token: string }>('/login', 'POST', { email, password }); sessionStorage.setItem('courtchain.token', data.token); onLogin() }
    catch (e) { setError(e instanceof Error ? e.message : 'Đăng nhập không thành công.') } finally { setBusy(false) }
  }
  return <main className="login-page"><section className="login-story"><div className="brand"><span className="brand-icon"><Icon name="court" size={27}/></span>CourtChain<span className="brand-dot">.</span></div><div className="login-copy"><span className="eyebrow">KHÔNG GIAN THỂ THAO. VẬN HÀNH THÔNG MINH.</span><h1>Mọi cơ sở.<br/>Một kết nối.</h1><p>Quản lý chi nhánh, sân thể thao và chính sách giá của bạn trong một không gian thống nhất.</p><div className="court-illustration" aria-hidden="true"><div className="court-lines"><i/><b/><em/></div><span className="court-ball"/></div></div><div className="login-credit">SWD392 · Hồ Tuấn Minh · HE191253 · SE1990NJ</div></section><section className="login-form-side"><form className="login-form" onSubmit={submit}><span className="login-shield"><Icon name="shield" size={25}/></span><span className="eyebrow">CỔNG QUẢN TRỊ CHỦ SÂN</span><h2>Chào mừng trở lại</h2><p>Đăng nhập để quản lý chuỗi sân CourtChain.</p>{expired && <div className="alert info">Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại.</div>}<FormError message={error}/><label htmlFor="email">Email</label><input id="email" type="email" autoComplete="username" required value={email} onChange={e => setEmail(e.target.value)}/><label htmlFor="password">Mật khẩu</label><input id="password" type="password" autoComplete="current-password" required value={password} onChange={e => setPassword(e.target.value)}/><button className="button primary" disabled={busy}>{busy ? 'Đang đăng nhập...' : 'Đăng nhập'}<Icon name="arrow"/></button><small><Icon name="shield" size={15}/>Chỉ dành cho tài khoản Court Owner.</small></form><p className="login-footer">CourtChain · Multi-Sport Court Management</p></section></main>
}
