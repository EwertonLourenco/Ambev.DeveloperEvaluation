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
  discountPercent: number | null = null;
  newItems: { description: string; unitPrice: number | null; quantity: number | null }[] = [
    { description: '', unitPrice: null, quantity: null },
  ];

  constructor(private api: OrdersService, private router: Router) { }
  ngOnInit() { this.load(); }

  toggleCreate() {
    this.creating = !this.creating;
    if (this.creating) this.resetNewForm();
  }

  resetNewForm() {
    this.discountPercent = null;
    this.newItems = [{ description: '', unitPrice: null, quantity: null }];
  }

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

  get newSubtotal() {
    return this.newItems.reduce((s, it) => s + (+(it.unitPrice ?? 0) * +(it.quantity ?? 0)), 0);
  }
  get newDiscountAmount() {
    return (this.newSubtotal * +(this.discountPercent ?? 0)) / 100;
  }
  get newTotal() {
    return this.newSubtotal - this.newDiscountAmount;
  }

  create() {
    const items = this.newItems
      .map(i => ({
        description: (i.description ?? '').trim(),
        unitPrice: +(i.unitPrice ?? 0),
        quantity: +(i.quantity ?? 0)
      }))
      .filter(i => i.description && i.unitPrice > 0 && i.quantity > 0);

    const body = {
      discountPercent: +(this.discountPercent ?? 0),
      items
    };

    this.api.create(body).subscribe({
      next: _ => {
        this.creating = false;
        this.resetNewForm();
        this.load();
      },
      error: _ => alert('Falha ao criar pedido')
    });
  }

  openDetail(id: string) { this.router.navigate(['/orders', 'detail', id]); }
}
