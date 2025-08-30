import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrdersService, Order, OrderItem } from './orders.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  standalone: false
})
export class OrderDetailComponent implements OnInit {
  id!: string;
  order: Order = {
    subtotal: 0,
    discountPercent: 0,
    discountAmount: 0,
    total: 0,
    items: [{ description: '', unitPrice: 0, quantity: 1 }]
  };
  loading = true;
  saving = false;

  constructor(
    private route: ActivatedRoute,
    private api: OrdersService,
    private router: Router
  ) { }

  ngOnInit() {
    this.id = String(this.route.snapshot.paramMap.get('id') || '');
    this.api.getById(this.id).subscribe({
      next: (o) => {
        // garantias pra binding
        this.order = o ?? ({
          subtotal: 0, discountPercent: 0, discountAmount: 0, total: 0, items: []
        } as Order);
        this.order.items ||= [];
        if (this.order.items.length === 0) {
          this.order.items.push({ description: '', unitPrice: 0, quantity: 1 });
        }
        this.loading = false;
      },
      error: _ => {
        this.loading = false;
        alert('Pedido não encontrado');
        this.router.navigate(['/orders']);
      }
    });
  }

  // --- Totais em tempo real (UI usa esses getters) ---
  get subtotal(): number {
    if (!this.order?.items?.length) return 0;
    return this.order.items.reduce(
      (s, it) => s + (+it.unitPrice || 0) * (+it.quantity || 0), 0
    );
  }

  get discountAmount(): number {
    const pct = (+this.order?.discountPercent || 0) / 100; // UI usa %
    return this.subtotal * pct;
  }

  get total(): number {
    return this.subtotal - this.discountAmount;
  }

  // --- Itens ---
  addItem() {
    this.order.items.push({ description: '', unitPrice: 0, quantity: 1 } as OrderItem);
  }

  removeItem(i: number) {
    this.order.items.splice(i, 1);
  }

  // --- Salvar ---
  save() {
    this.saving = true;

    // backend espera fração para desconto; UI está em %
    const body: Partial<Order> = {
      id: this.id,
      customerName: this.order.customerName,
      discountPercent: (+this.order.discountPercent || 0) / 100,
      items: (this.order.items || []).map(it => ({
        description: (it.description || '').trim(),
        unitPrice: +it.unitPrice || 0,
        quantity: +it.quantity || 0
      }))
    };

    this.api.update(this.id, body).subscribe({
      next: _ => {
        this.saving = false;
        alert('Atualizado com sucesso!');
        this.router.navigate(['/orders']);
      },
      error: _ => {
        this.saving = false;
        alert('Falha ao salvar');
      }
    });
  }
}
