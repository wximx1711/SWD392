import { useEffect, useRef, useState } from 'react'
import type { FormEvent, ReactNode } from 'react'
import { ApiError, statusLabel } from '../lib/api'
import type { FieldErrors } from '../lib/types'
import { Icon } from './Icon'

export function Badge({ status }: { status: string }) { return <span className={'badge badge-' + status.replaceAll(' ', '-').toLowerCase()}><i/>{statusLabel(status)}</span> }
export function Empty({ title, detail, children }: { title: string; detail: string; children?: ReactNode }) { return <div className="empty"><span className="empty-icon"><Icon name="court" size={30}/></span><h3>{title}</h3><p>{detail}</p>{children}</div> }
export function Field({ label, name, errors, children, hint, required = false }: { label: string; name: string; errors?: FieldErrors; children: ReactNode; hint?: string; required?: boolean }) {
  const messages = errors?.[name]
  return <div className={'field' + (messages ? ' invalid' : '')}><label htmlFor={name}>{label}{required && <span className="required" aria-hidden="true"> *</span>}</label>{children}{messages ? <small className="field-error" id={name + '-error'}>{messages[0]}</small> : hint ? <small>{hint}</small> : null}</div>
}
export function Modal({ title, subtitle, onClose, children }: { title: string; subtitle: string; onClose: () => void; children: ReactNode }) {
  const ref = useRef<HTMLDialogElement>(null)
  useEffect(() => { ref.current?.showModal(); const original = document.body.style.overflow; document.body.style.overflow = 'hidden'; return () => { document.body.style.overflow = original } }, [])
  return <dialog ref={ref} className="modal" aria-labelledby="dialog-title" onCancel={onClose}><div className="modal-heading"><div><h2 id="dialog-title">{title}</h2><p>{subtitle}</p></div><button type="button" className="icon-button" aria-label="Đóng" onClick={onClose}><Icon name="close"/></button></div>{children}</dialog>
}
export function useSave(onSuccess: () => void) {
  const [busy, setBusy] = useState(false), [error, setError] = useState(''), [errors, setErrors] = useState<FieldErrors>({})
  async function save(e: FormEvent, action: () => Promise<void>) {
    e.preventDefault()
    if (busy) return
    setBusy(true); setError(''); setErrors({})
    try { await action(); onSuccess() } catch (e) { setError(e instanceof Error ? e.message : 'Không thể lưu.'); if (e instanceof ApiError) setErrors(e.fields) } finally { setBusy(false) }
  }
  return { busy, error, errors, save }
}
export function FormError({ message }: { message: string }) {
  const ref = useRef<HTMLDivElement>(null)
  useEffect(() => { if (message) ref.current?.focus() }, [message])
  return message ? <div ref={ref} tabIndex={-1} className="alert error" role="alert">{message}</div> : null
}
export function FormActions({ busy, onClose, label }: { busy: boolean; onClose: () => void; label: string }) {
  return <footer className="form-actions"><button type="button" className="button secondary" disabled={busy} onClick={onClose}>Hủy</button><button className="button primary" type="submit" disabled={busy}>{busy ? 'Đang lưu...' : label}<Icon name="check" size={17}/></button></footer>
}

