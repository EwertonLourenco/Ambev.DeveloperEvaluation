import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  // garante não-standalone, pois é declarado no AuthModule
  standalone: false
})
export class LoginComponent {
  email = '';
  password = '';
  error = '';

  constructor(private http: HttpClient, private router: Router) { }

  login() {
    this.error = '';
    const body = { email: this.email, password: this.password };

    this.http.post<any>('/api/Auth', body).subscribe({
      next: (res) => {
        const token = res?.token ?? res?.data?.token;
        if (!token) {
          this.error = 'Resposta inválida da API.';
          return;
        }
        localStorage.setItem('token', token);
        this.router.navigateByUrl('/home');
      },
      error: (err) => {
        this.error = err?.error?.message ?? 'Falha no login.';
      }
    });
  }
}
