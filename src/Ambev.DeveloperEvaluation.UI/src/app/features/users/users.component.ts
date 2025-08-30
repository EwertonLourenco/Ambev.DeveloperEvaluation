import { Component } from '@angular/core';
import { UsersService, UserCreate } from './users.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  standalone: false
})
export class UsersComponent {
  // criar
  create: UserCreate = { username: '', password: '', phone: '', email: '', status: 1, role: 0 };
  createMsg = '';

  // buscar
  userId: string | number = '';
  userJson = '';

  loadingCreate = false;
  loadingGet = false;

  constructor(private api: UsersService) { }

  doCreate() {
    this.loadingCreate = true; this.createMsg = '';
    this.api.create(this.create).subscribe({
      next: r => { this.loadingCreate = false; this.createMsg = 'Usuário criado com sucesso.'; },
      error: _ => { this.loadingCreate = false; this.createMsg = 'Falha ao criar.'; }
    });
  }

  getById() {
    if (!this.userId) { this.userJson = 'Informe o ID'; return; }
    this.loadingGet = true; this.userJson = '';
    this.api.getById(this.userId).subscribe({
      next: r => { this.loadingGet = false; this.userJson = JSON.stringify(r, null, 2); },
      error: _ => { this.loadingGet = false; this.userJson = 'Não encontrado.'; }
    });
  }
}
