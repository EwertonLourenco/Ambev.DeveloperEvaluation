import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule) },
  { path: 'orders', canActivate: [AuthGuard], loadChildren: () => import('./features/orders/orders.module').then(m => m.OrdersModule) },
  { path: 'users', canActivate: [AuthGuard], loadChildren: () => import('./features/users/users.module').then(m => m.UsersModule) },
  { path: '', pathMatch: 'full', redirectTo: 'orders' },
  { path: '**', redirectTo: 'orders' }
];
