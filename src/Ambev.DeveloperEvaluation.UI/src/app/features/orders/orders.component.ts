import { Component } from '@angular/core';
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { OrdersService, Order } from './orders.service';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, DatePipe, DecimalPipe],
  templateUrl: './orders.component.html',
})
export class OrdersComponent {
  items: Order[] = [];
  search = '';
  pageSize = 10;

  creating = false;
  newItems: { description: string; unitPrice: number; quantity: number }[] = [
    { description: '', unitPrice: 0, quantity: 1 }
  ];

  constructor(private orders: OrdersService) {
    this.load();
  }

  load() {
    this.orders.list({ search: this.search, pageSize: this.pageSize }).subscribe({
      next: (data) => (this.items = data || []),
      error: () => (this.items = [])
    });
  }

  openCreate() { this.creating = true; }
  addItem() { this.newItems.push({ description: '', unitPrice: 0, quantity: 1 }); }
  removeItem(i: number) { this.newItems.splice(i, 1); }

  saveNew() {
    const order = {
      items: this.newItems.map(x => ({ ...x })),
    };
    this.orders.create(order as any).subscribe({
      next: () => { this.creating = false; this.newItems = [{ description: '', unitPrice: 0, quantity: 1 }]; this.load(); },
      error: () => alert('Erro ao criar pedido')
    });
  }
}
