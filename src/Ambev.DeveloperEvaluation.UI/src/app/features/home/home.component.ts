import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <section class="container">
      <div class="grid">
        <div class="card span-8">
          <h3>Bem-vindo 👋</h3>
          <p class="muted">
            Este é o painel principal. Use os atalhos para navegar e checar rapidamente o backend.
          </p>

          <div style="display:flex;gap:10px;margin-top:10px;flex-wrap:wrap">
            <a class="btn btn-primary" routerLink="/orders">Abrir Pedidos</a>
            <a class="btn" routerLink="/users">Gerenciar Usuários</a>
            <button class="btn btn-outline" (click)="logout()">Sair</button>
          </div>
        </div>

        <div class="card span-4">
          <h3>Status da API</h3>
          <p class="muted">Ping simples (GET /api/Orders?pageSize=1)</p>
          <div style="display:flex;gap:8px;align-items:center;margin:10px 0">
            <span class="badge" [class.ok]="apiOk" [class.err]="apiOk===false">
              {{ apiOk === null ? 'Aguardando...' : (apiOk ? 'Online' : 'Offline') }}
            </span>
            <button class="btn" (click)="pingApi()">Verificar</button>
          </div>
          <small class="muted">Base URL: {{ baseUrl }}</small>
        </div>

        <div class="card span-6">
          <h3>Pedidos em foco</h3>
          <p class="muted">Crie, edite e acompanhe pedidos do desafio.</p>
          <a class="btn btn-primary" routerLink="/orders">Ir para Pedidos</a>
        </div>

        <div class="card span-6">
          <h3>Usuários</h3>
          <p class="muted">Cadastro e consulta de usuários.</p>
          <a class="btn btn-primary" routerLink="/users">Ir para Usuários</a>
        </div>
      </div>
    </section>
  `
})
export class HomeComponent {
  apiOk: boolean | null = null;
  baseUrl = (window as any).__env?.API_BASE_URL ?? 'http://ambev.developerevaluation.webapi:8080';

  constructor(private http: HttpClient, private router: Router) { }

  pingApi() {
    this.apiOk = null;
    this.http.get('/api/Orders', { params: { pageSize: 1 } })
      .subscribe({
        next: () => this.apiOk = true,
        error: () => this.apiOk = false
      });
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigateByUrl('/login');
  }
}
