declare global {
  interface Window { __env?: { API_BASE_URL?: string }; }
}
export const API_BASE_URL =
  (typeof window !== 'undefined' && window.__env?.API_BASE_URL) ||
  'http://ambev.developerevaluation.webapi:8080';
