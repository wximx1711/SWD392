// Hồ Tuấn Minh | HE191253 | SE1990NJ | SWD392
import { useState } from 'react'
import { api } from '../../lib/api'
import type { Facility, FacilityInput, Options } from '../../lib/types'
import { Modal, FormActions, FormError, Field, useSave } from '../../components/UI'
import { FacilityFields } from '../facilities/FacilityFields'
export function UpdateFacility({ facility, options, onClose, onUpdated }: { facility: Facility; options: Options; onClose: () => void; onUpdated: (f: Facility) => void }) {
  const { id: _id, status: _status, updatedAt: _at, ...profile } = facility
  const [value, setValue] = useState<FacilityInput>({ ...profile, rationale: '' })
  const [confirmed, setConfirmed] = useState(false)
  const reduced = value.opening.slice(0, 5) > facility.opening.slice(0, 5) || value.closing.slice(0, 5) < facility.closing.slice(0, 5)
  const { busy, error, errors, save } = useSave(() => {})
  return <Modal title="Cập nhật hồ sơ cơ sở" subtitle={facility.name + ' · Mã CS-' + facility.id.toString().padStart(3, '0')} onClose={onClose}><form onSubmit={e => save(e, async () => { onUpdated(await api<Facility>('/facilities/' + facility.id, 'PUT', value)) })}><div className="form-body"><FormError message={error}/><FacilityFields value={value} setValue={setValue} errors={errors} options={options}/><Field name="rationale" label="Lý do cập nhật" required errors={errors}><textarea id="rationale" required maxLength={1000} value={value.rationale} onChange={e => setValue({ ...value, rationale: e.target.value })}/></Field>{reduced && <div className="alert warning"><strong>Bạn đang thu hẹp giờ hoạt động.</strong>{!options.bookingChecksAvailable && <p>Bản demo chưa kết nối dữ liệu đặt chỗ. Cần kiểm tra đặt chỗ thực tế trước khi áp dụng.</p>}<label className="check-line"><input type="checkbox" required checked={confirmed} onChange={e => setConfirmed(e.target.checked)}/>Tôi xác nhận thay đổi giờ hoạt động.</label></div>}</div><FormActions busy={busy} onClose={onClose} label="Lưu thay đổi"/></form></Modal>
}

