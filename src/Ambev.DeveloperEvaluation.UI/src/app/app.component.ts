import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, NgIf],
  template: `
    <header style="padding:12px;border-bottom:1px solid #eee;display:flex;gap:16px;align-items:center">
      <strong>Ambev Developer Evaluation</strong>
      <a routerLink="/orders" routerLinkActive="active">Orders</a>
      <a routerLink="/users"  routerLinkActive="active">Users</a>
      <span style="margin-left:auto"></span>
      <a routerLink="/login">Login</a>
      <button (click)="logout()">Logout</button>
    </header>
    <main style="padding:16px">
      <router-outlet></router-outlet>
    </main>
  `,
})
export class AppComponent {
  logout() {
    localStorage.removeItem('token');
    location.href = '/login';
  }
}
