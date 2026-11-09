type Name = 'grid' | 'building' | 'court' | 'price' | 'plus' | 'arrow' | 'edit' | 'logout' | 'search' | 'check' | 'clock' | 'pin' | 'close' | 'menu' | 'shield'
const paths: Record<Name, string> = {
  grid: 'M3 3h7v7H3z M14 3h7v7h-7z M3 14h7v7H3z M14 14h7v7h-7z',
  building: 'M4 21V7l8-4 8 4v14 M2 21h20 M9 21v-5h6v5 M8 9h1 M15 9h1 M8 12h1 M15 12h1',
  court: 'M3 4h18v16H3z M12 4v16 M3 12h18 M6 4v16 M18 4v16',
  price: 'M3 3h8l10 10-8 8L3 11z M7 7h.01',
  plus: 'M12 5v14 M5 12h14', arrow: 'M5 12h14 M13 6l6 6-6 6', edit: 'M16 3l5 5-12 12H4v-5z M13 6l5 5',
  logout: 'M9 3H4v18h5 M10 12h11 M17 8l4 4-4 4', search: 'M21 21l-5-5 M18 10a8 8 0 1 1-16 0 8 8 0 0 1 16 0',
  check: 'M5 12l4 4L19 6', clock: 'M12 7v5l3 2 M22 12a10 10 0 1 1-20 0 10 10 0 0 1 20 0',
  pin: 'M20 10c0 6-8 12-8 12S4 16 4 10a8 8 0 1 1 16 0 M15 10a3 3 0 1 1-6 0 3 3 0 0 1 6 0',
  close: 'M6 6l12 12 M18 6L6 18', menu: 'M3 6h18 M3 12h18 M3 18h18', shield: 'M12 3l8 3v6c0 5-8 9-8 9s-8-4-8-9V6z M8 12l3 3 5-6',
}
export function Icon({ name, size = 20 }: { name: Name; size?: number }) {
  return <svg width={size} height={size} viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true"><path d={paths[name]}/></svg>
}
