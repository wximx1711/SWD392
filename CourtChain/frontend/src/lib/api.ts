import type { FieldErrors } from './types'
const base = import.meta.env.VITE_API_URL || '/api'
export class ApiError extends Error {
  status: number
  fields: FieldErrors
  constructor(message: string, status: number, fields: FieldErrors = {}) { super(message); this.status = status; this.fields = fields }
}
export async function api<T>(path: string, method = 'GET', body?: unknown): Promise<T> {
  const token = sessionStorage.getItem('courtchain.token')
  let response: Response
  try {
    response = await fetch(base + path, {
      method, headers: { 'Content-Type': 'application/json', ...(token ? { Authorization: 'Bearer ' + token } : {}) },
      body: body === undefined ? undefined : JSON.stringify(body),
    })
  } catch { throw new ApiError('Không kết nối được máy chủ. Kiểm tra backend đang chạy.', 0) }
  if (response.status === 204) return undefined as T
  const data = await response.json().catch(() => ({}))
  if (!response.ok) {
    if (response.status === 401 && path !== '/login') window.dispatchEvent(new Event('courtchain:expired'))
    throw new ApiError(data.error || (response.status === 401 ? 'Phiên đăng nhập đã hết hạn.' : response.status === 403 ? 'Bạn không có quyền thực hiện thao tác này.' : response.status === 404 ? 'Không tìm thấy dữ liệu.' : 'Thông tin không hợp lệ. Vui lòng kiểm tra lại.'), response.status, data.errors || {})
  }
  return data as T
}
export const money = (value: number) => new Intl.NumberFormat('vi-VN', { maximumFractionDigits: 2 }).format(value)
export const time = (value: string) => value.slice(0, 5)
export const date = (value: string) => value.split('-').reverse().join('/')
export const sportLabel = (value: string) => ({ Badminton: 'Cầu lông', Pickleball: 'Pickleball', 'Table Tennis': 'Bóng bàn' }[value] || value)
export const surfaceLabel = (value: string) => ({ Wood: 'Gỗ', Rubber: 'Cao su', Acrylic: 'Acrylic' }[value] || value)
export const statusLabel = (value: string) => ({ Active: 'Đang hoạt động', Scheduled: 'Chờ hiệu lực', Inactive: 'Ngừng hoạt động', 'Under Maintenance': 'Đang bảo trì', Expired: 'Hết hiệu lực' }[value] || value)
