import { Component, OnInit } from '@angular/core';
import { OrdersService, OrderSummary, PagedResult } from './orders.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  standalone: false
})
export class OrdersComponent implements OnInit {
  data!: PagedResult<OrderSummary>;
  items: OrderSummary[] = [];
  loading = false;
  errorMsg = ''; // ← debug

  search = '';
  page = 1;
  pageSize = 10;

  creating = false;
  customerName = '';
  discountPercent = 0;
  newItems = [{ description: '', unitPrice: 0, quantity: 1 }];

  constructor(private api: OrdersService, private router: Router) { }
  ngOnInit() { this.load(); }

  load() {
    this.loading = true;
    this.errorMsg = '';
    this.api.list({ search: this.search, page: this.page, pageSize: this.pageSize })
      .subscribe({
        next: (res) => {
          this.data = res;
          this.items = res.items;
          this.loading = false;
          console.debug('[OrdersComponent] received', res.items?.length, 'items');
        },
        error: (err) => {
          this.items = [];
          this.loading = false;
          this.errorMsg = (err?.error?.message || err?.message || 'Falha ao carregar pedidos.');
          console.error('[OrdersComponent] load error:', err);
        }
      });
  }

  prev() { if (this.page > 1) { this.page--; this.load(); } }
  next() { if (this.data?.hasNext) { this.page++; this.load(); } }

  addItem() { this.newItems.push({ description: '', unitPrice: 0, quantity: 1 }); }
  removeItem(i: number) { this.newItems.splice(i, 1); }

  get newSubtotal() { return this.newItems.reduce((s, it) => s + (+it.unitPrice || 0) * (+it.quantity || 0), 0); }
  get newDiscountAmount() { return (this.newSubtotal * (+this.discountPercent || 0)) / 100; }
  get newTotal() { return this.newSubtotal - this.newDiscountAmount; }

  create() {
    const body = {
      customerName: this.customerName || undefined,
      discountPercent: +this.discountPercent || 0,
      items: this.newItems.filter(i => i.description?.trim())
    };
    this.api.create(body).subscribe({
      next: _ => { this.creating = false; this.customerName = ''; this.discountPercent = 0; this.newItems = [{ description: '', unitPrice: 0, quantity: 1 }]; this.load(); },
      error: (err) => { console.error('[OrdersComponent] create error', err); alert('Falha ao criar pedido'); }
    });
  }

  openDetail(id: string) { this.router.navigate(['/orders', 'detail', id]); }
}
