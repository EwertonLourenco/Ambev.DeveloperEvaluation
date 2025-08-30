import { Component } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { OrdersService, Order } from './orders.service';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, DecimalPipe],
  templateUrl: './order-detail.component.html',
})
export class OrderDetailComponent {
  id = 0;
  order?: Order;

  constructor(private route: ActivatedRoute, private orders: OrdersService) {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    this.load();
  }

  load() {
    this.orders.get(this.id).subscribe({ next: (o) => (this.order = o) });
  }

  save() {
    if (!this.order) return;
    this.orders.update(this.id, this.order).subscribe({
      next: () => alert('Salvo'),
      error: () => alert('Erro ao salvar')
    });
  }
}
