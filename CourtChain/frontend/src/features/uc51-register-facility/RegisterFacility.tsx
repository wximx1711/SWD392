// Hồ Tuấn Minh | HE191253 | SE1990NJ | SWD392
import { useState } from 'react'
import { api } from '../../lib/api'
import type { Facility, Options } from '../../lib/types'
import { Modal, FormActions, FormError, useSave } from '../../components/UI'
import { blankFacility, FacilityFields } from '../facilities/FacilityFields'
export function RegisterFacility({ options, onClose, onCreated }: { options: Options; onClose: () => void; onCreated: (f: Facility) => void }) {
  const [value, setValue] = useState({ ...blankFacility })
  const { busy, error, errors, save } = useSave(() => {})
  return <Modal title="Đăng ký cơ sở mới" subtitle="Thêm chi nhánh vào hệ thống CourtChain." onClose={onClose}><form onSubmit={e => save(e, async () => onCreated(await api<Facility>('/facilities', 'POST', value)))}><div className="form-body"><FormError message={error}/><FacilityFields value={value} setValue={setValue} errors={errors} options={options}/></div><FormActions busy={busy} onClose={onClose} label="Đăng ký cơ sở"/></form></Modal>
}

