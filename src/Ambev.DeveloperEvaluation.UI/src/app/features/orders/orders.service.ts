import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';

export interface OrderItem {
  description: string;
  unitPrice: number;
  quantity: number;
}

export interface Order {
  id?: string;
  createdAt?: string;
  customerName?: string;
  subtotal: number;
  discountPercent: number;   // no detalhe vamos exibir em %
  discountAmount: number;
  total: number;
  items: OrderItem[];
}

export interface OrderSummary {
  id: string;
  createdAt: string;
  subtotal: number;
  discountPercent: number;   // convertemos 0.2 → 20 para lista
  discountAmount: number;
  total: number;
  itemsCount: number;
  customerName?: string;
}

export interface PagedResult<T> {
  items: T[];
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}

@Injectable({ providedIn: 'root' })
export class OrdersService {
  private base = '/api/Orders';
  constructor(private http: HttpClient) { }

  list(opts: { search?: string; page?: number; pageSize?: number } = {}): Observable<PagedResult<OrderSummary>> {
    const page = opts.page ?? 1;
    const pageSize = opts.pageSize ?? 10;

    let params = new HttpParams()
      .set('page', String(page))
      .set('pageNumber', String(page))
      .set('pageSize', String(pageSize));
    if (opts.search) params = params.set('search', opts.search);

    return this.http.get<any>(this.base, { params }).pipe(
      tap(raw => console.debug('[OrdersService] raw GET /api/Orders', raw)),
      map(resp => {
        let data = resp?.data;

        if (!data && Array.isArray(resp?.data)) {
          data = {
            items: resp.data, currentPage: 1, pageSize: resp.data.length, totalCount: resp.data.length,
            totalPages: 1, hasNext: false, hasPrevious: false
          };
        }
        if (!data && Array.isArray(resp)) {
          data = {
            items: resp, currentPage: 1, pageSize: resp.length, totalCount: resp.length,
            totalPages: 1, hasNext: false, hasPrevious: false
          };
        }
        if (!data) {
          data = { items: [], currentPage: page, pageSize, totalCount: 0, totalPages: 0, hasNext: false, hasPrevious: false };
        }

        // normaliza fração → %
        data.items = (data.items ?? []).map((o: OrderSummary) => ({
          ...o,
          discountPercent: o?.discountPercent <= 1 ? (o.discountPercent * 100) : o.discountPercent
        }));

        return data as PagedResult<OrderSummary>;
      })
    );
  }

  getById(id: string | number): Observable<Order> {
    return this.http.get<any>(`${this.base}/${id}`).pipe(
      tap(raw => console.debug('[OrdersService] raw GET /api/Orders/', id, raw)),
      map(resp => {
        const o: Order = resp?.data ?? resp;
        // se vier fração (0.2), converte para % para exibição
        if (o && typeof o.discountPercent === 'number' && o.discountPercent <= 1) {
          o.discountPercent = o.discountPercent * 100;
        }
        return o;
      })
    );
  }

  create(order: any) {
    return this.http.post<any>(this.base, order).pipe(
      tap(raw => console.debug('[OrdersService] raw POST /api/Orders', raw)),
      map(r => r?.data ?? r)
    );
  }

  // <<< aqui aceita string | number >>>
  update(id: string | number, order: any) {
    return this.http.put<any>(`${this.base}/${id}`, order).pipe(
      tap(raw => console.debug('[OrdersService] raw PUT /api/Orders/', id, raw)),
      map(r => r?.data ?? r)
    );
  }
}
