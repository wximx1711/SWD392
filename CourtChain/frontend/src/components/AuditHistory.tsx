import { useEffect, useState } from 'react'
import { api } from '../lib/api'
import type { Audit } from '../lib/types'
import { FormError } from './UI'
export function AuditHistory({ entity, id }: { entity: string; id: number }) {
  const [rows, setRows] = useState<Audit[]>([]), [error, setError] = useState(''), [loading, setLoading] = useState(true)
  useEffect(() => { let active = true; api<Audit[]>('/audits?entity=' + entity + '&entityId=' + id).then(r => { if (active) setRows(r) }).catch(e => { if (active) setError(e.message) }).finally(() => { if (active) setLoading(false) }); return () => { active = false } }, [entity, id])
  return <div className="audit-list"><FormError message={error}/>{loading ? <p>Đang tải lịch sử...</p> : !rows.length ? <p>Chưa có lịch sử.</p> : rows.map(a => {
    const before = a.before ? JSON.parse(a.before) as Record<string, unknown> : {}
    const after = JSON.parse(a.after) as Record<string, unknown>
    const changes = Object.entries(after).filter(([k, v]) => !['UpdatedAt', 'Version'].includes(k) && before[k] !== v)
    return <article className="audit-item" key={a.id}><span className="audit-dot"/><div><div className="audit-heading"><strong>{a.action}</strong><time>{new Date(a.at.endsWith('Z') ? a.at : a.at + 'Z').toLocaleString('vi-VN', { timeZone: 'Asia/Ho_Chi_Minh' })}</time></div><p>{a.rationale || 'Bản ghi từ phiên bản trước.'}</p><small>Người thực hiện #{a.actorId} · Bản ghi #{a.id}</small><details><summary>Xem dữ liệu thay đổi</summary><div className="table-wrap"><table><thead><tr><th>Thuộc tính</th><th>Trước</th><th>Sau</th></tr></thead><tbody>{changes.map(([k, v]) => <tr key={k}><td>{k}</td><td>{String(before[k] ?? '—')}</td><td>{String(v ?? '—')}</td></tr>)}</tbody></table></div></details></div></article>
  })}</div>
}
