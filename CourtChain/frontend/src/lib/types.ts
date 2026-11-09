export type Facility = {
  id: number; name: string; address: string; cityDistrict: string; phone: string; email: string;
  opening: string; closing: string; slotMinutes: number; parking: boolean; shower: boolean; proShop: boolean; wifi: boolean;
  latitude: number | null; longitude: number | null; status: string; updatedAt: string; version: number; imageUrl: string | null;
}
export type FacilityInput = Omit<Facility, 'id' | 'status' | 'updatedAt' | 'version'> & { version?: number; rationale?: string }
export type Court = { id: number; facilityId: number; name: string; sport: string; surface: string; indoor: boolean; notes: string | null; lighting: boolean; status: string; updatedAt: string; version: number }
export type CourtInput = Pick<Court, 'facilityId' | 'name' | 'sport' | 'surface' | 'indoor' | 'notes' | 'lighting'> & { count: number }
export type CourtUpdate = Pick<Court, 'name' | 'surface' | 'status' | 'notes' | 'lighting' | 'version'> & { rationale: string }
export type Policy = { id: number; name: string; scope: 'Facility' | 'Chain'; facilityId: number | null; sport: string; regular: number; peak: number; weekend: number; discount: number; peakStart: string; peakEnd: string; effectiveStart: string; effectiveEnd: string | null; status: string }
export type PolicyInput = Omit<Policy, 'id' | 'effectiveEnd' | 'status'>
export type Options = { sports: string[]; surfaces: string[]; statuses: string[]; slotMinutes: number[]; today: string; bookingChecksAvailable: boolean }
export type Audit = { id: number; entity: string; entityId: number; action: string; actorId: number; at: string; before: string | null; after: string; rationale: string }
export type FieldErrors = Record<string, string[]>
