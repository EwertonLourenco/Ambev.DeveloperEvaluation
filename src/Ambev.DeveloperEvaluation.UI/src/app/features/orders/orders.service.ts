import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface OrderItem { description: string; unitPrice: number; quantity: number; }
export interface Order {
  id: number;
  subtotal: number;
  discountPercent: number;
  discountAmount: number;
  total: number;
  items: OrderItem[];
  createdAt?: string;
}

@Injectable({ providedIn: 'root' })
export class OrdersService {
  constructor(private http: HttpClient) { }

  list(params: { search?: string; pageSize?: number } = {}) {
    const query = new URLSearchParams(params as any).toString();
    return this.http.get<Order[]>(`/api/Orders${query ? '?' + query : ''}`);
  }

  get(id: number) {
    return this.http.get<Order>(`/api/Orders/${id}`);
  }

  create(order: Partial<Order>) {
    return this.http.post<Order>(`/api/Orders`, order);
  }

  update(id: number, order: Partial<Order>) {
    return this.http.put<Order>(`/api/Orders/${id}`, order);
  }
}
