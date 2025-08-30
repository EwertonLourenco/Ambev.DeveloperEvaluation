import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UsersService, CreateUser } from './users.service';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './users.component.html',
})
export class UsersComponent {
  create: CreateUser = { username: '', password: '', status: 1, role: 0, email: '', phone: '' };
  createMsg = '';
  userId: number | null = null;
  userJson = '';

  constructor(private users: UsersService) { }

  submitCreate() {
    this.createMsg = '';
    this.users.create(this.create).subscribe({
      next: () => (this.createMsg = 'Usuário criado.'),
      error: () => (this.createMsg = 'Erro ao criar usuário.')
    });
  }

  loadUser() {
    this.userJson = '';
    if (this.userId == null) return;
    this.users.getById(this.userId).subscribe({
      next: (u) => (this.userJson = JSON.stringify(u, null, 2)),
      error: () => (this.userJson = 'Erro ao buscar usuário.')
    });
  }
}
