import { useEffect, useState } from 'react'
import { api } from './lib/api'
import type { Court, Facility, Options, Policy } from './lib/types'
import { Icon } from './components/Icon'
import { FormError } from './components/UI'
import { Login } from './features/auth/Login'
import { Introduction } from './pages/Introduction'
import { Facilities } from './pages/Facilities'
import { FacilityDetails } from './pages/FacilityDetails'
import { Pricing } from './pages/Pricing'
import { RegisterFacility } from './features/uc51-register-facility/RegisterFacility'
import { UpdateFacility } from './features/uc52-update-facility/UpdateFacility'
import { RegisterCourt } from './features/uc53-register-court/RegisterCourt'
import { UpdateCourt } from './features/uc54-update-court/UpdateCourt'
import { CreatePricingPolicy } from './features/uc55-create-pricing/CreatePricingPolicy'
import './App.css'

export default function App() {
  const [signedIn, setSignedIn] = useState(!!sessionStorage.getItem('courtchain.token'))
  const [expired, setExpired] = useState(false), [route, setRoute] = useState(location.hash || '#introduction')
  const [facilities, setFacilities] = useState<Facility[]>([]), [policies, setPolicies] = useState<Policy[]>([]), [options, setOptions] = useState<Options | null>(null)
  const [loading, setLoading] = useState(false), [error, setError] = useState(''), [toast, setToast] = useState(''), [modal, setModal] = useState(''), [court, setCourt] = useState<Court | null>(null)
  const [revision, setRevision] = useState(0), [menu, setMenu] = useState(false)
  const id = Number(route.split('/')[1]), selected = facilities.find(f => f.id === id)
  const section = route.startsWith('#facilities') ? 'facilities' : route === '#pricing' ? 'pricing' : 'introduction'
  useEffect(() => {
    const hash = () => { setRoute(location.hash || '#introduction'); setMenu(false); setModal('') }
    const logout = () => { sessionStorage.removeItem('courtchain.token'); setSignedIn(false); setExpired(true); setModal('') }
    window.addEventListener('hashchange', hash); window.addEventListener('courtchain:expired', logout)
    return () => { window.removeEventListener('hashchange', hash); window.removeEventListener('courtchain:expired', logout) }
  }, [])
  async function refresh() {
    setLoading(true); setError('')
    try { const [f, p, o] = await Promise.all([api<Facility[]>('/facilities'), api<Policy[]>('/pricing-policies'), api<Options>('/options')]); setFacilities(f); setPolicies(p); setOptions(o) }
    catch (e) { setError(e instanceof Error ? e.message : 'Không tải được dữ liệu.') } finally { setLoading(false) }
  }
  useEffect(() => { if (signedIn) void refresh() }, [signedIn])
  useEffect(() => { if (!toast) return; const t = setTimeout(() => setToast(''), 5500); return () => clearTimeout(t) }, [toast])
  function saved(message: string) { setModal(''); setToast(message); setRevision(n => n + 1); void refresh() }
  async function logout() { try { await api('/logout', 'POST') } finally { sessionStorage.removeItem('courtchain.token'); setSignedIn(false); setExpired(false) } }
  if (!signedIn) return <Login expired={expired} onLogin={() => { setSignedIn(true); setExpired(false) }}/>
  return <div className="app-shell"><aside className={'sidebar ' + (menu ? 'open' : '')}><a href="#introduction" className="brand"><span className="brand-icon"><Icon name="court" size={24}/></span>CourtChain<span className="brand-dot">.</span></a><span className="sidebar-caption">QUẢN TRỊ CHUỖI SÂN</span><nav>{([['introduction', 'Giới thiệu', 'grid'], ['facilities', 'Cơ sở & sân', 'building'], ['pricing', 'Chính sách giá', 'price']] as const).map(([key, label, icon]) => <a href={'#' + key} key={key} className={section === key ? 'active' : ''} aria-current={section === key ? 'page' : undefined}><Icon name={icon}/>{label}{section === key && <i/>}</a>)}</nav><div className="sidebar-bottom"><div className="profile"><span>TM</span><div><strong>Hồ Tuấn Minh</strong><small>HE191253 · SE1990NJ</small></div></div><button onClick={logout}><Icon name="logout" size={18}/>Đăng xuất</button></div></aside>{menu && <button className="sidebar-scrim" aria-label="Đóng menu" onClick={() => setMenu(false)}/>}<div className="workspace"><header className="topbar"><div><button className="icon-button menu-toggle" aria-label="Mở menu" onClick={() => setMenu(!menu)}><Icon name="menu"/></button><span>CourtChain <b>/</b> {section === 'facilities' ? 'Cơ sở & sân' : section === 'pricing' ? 'Chính sách giá' : 'Giới thiệu'}</span></div><span className="owner-chip"><Icon name="shield" size={16}/>Court Owner</span></header><main className="main-content">{toast && <div className="toast" role="status"><Icon name="check"/>{toast}<button className="icon-button" aria-label="Đóng thông báo" onClick={() => setToast('')}><Icon name="close" size={16}/></button></div>}<FormError message={error}/>{error && <button className="button secondary" onClick={refresh}>Thử tải lại</button>}{loading && !options ? <div className="loading">Đang tải dữ liệu...</div> : options && <>{section === 'introduction' && <Introduction/>}{section === 'facilities' && (!id ? <Facilities facilities={facilities} onCreate={() => setModal('uc51')}/> : selected ? <FacilityDetails key={selected.id} facility={selected} revision={revision} onEdit={() => setModal('uc52')} onAddCourt={() => setModal('uc53')} onEditCourt={c => { setCourt(c); setModal('uc54') }}/> : <p>Không tìm thấy cơ sở. <a href="#facilities">Về danh sách</a></p>)}{section === 'pricing' && <Pricing facilities={facilities} policies={policies} onCreate={() => setModal('uc55')}/>}</>}</main><footer className="app-footer">CourtChain · SWD392<span>Hồ Tuấn Minh · HE191253 · SE1990NJ</span></footer></div>{options && <>{modal === 'uc51' && <RegisterFacility options={options} onClose={() => setModal('')} onCreated={f => { setFacilities(list => [...list, f]); location.hash = '#facilities/' + f.id; saved('Đã đăng ký cơ sở. Bạn có thể thêm sân ngay.')}}/>}{modal === 'uc52' && selected && <UpdateFacility facility={selected} options={options} onClose={() => setModal('')} onUpdated={f => { setFacilities(list => list.map(x => x.id === f.id ? f : x)); saved('Đã cập nhật hồ sơ cơ sở.') }}/>} {modal === 'uc53' && selected && <RegisterCourt facility={selected} options={options} onClose={() => setModal('')} onCreated={() => saved('Đã đăng ký sân thành công.')}/>} {modal === 'uc54' && court && <UpdateCourt court={court} options={options} onClose={() => setModal('')} onUpdated={() => saved('Đã cập nhật hồ sơ sân.')}/>} {modal === 'uc55' && <CreatePricingPolicy facilities={facilities} options={options} onClose={() => setModal('')} onCreated={() => saved('Đã công bố chính sách giá.')}/>}</>}</div>
}
