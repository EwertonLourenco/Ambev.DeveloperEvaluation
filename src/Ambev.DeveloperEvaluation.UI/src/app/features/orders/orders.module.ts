import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { OrdersComponent } from './orders.component';
import { OrderDetailComponent } from './order-detail.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild([
      { path: '', component: OrdersComponent },
      { path: 'detail/:id', component: OrderDetailComponent }
    ])
  ],
  declarations: [OrdersComponent, OrderDetailComponent]
})
export class OrdersModule { }
