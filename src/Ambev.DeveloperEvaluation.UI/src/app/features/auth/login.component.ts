import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  email = '';
  password = '';
  error = '';

  constructor(private auth: AuthService, private router: Router) { }

  submit() {
    this.error = '';
    this.auth.login(this.email, this.password).subscribe({
      next: (resp) => {
        if (this.auth.saveToken(resp)) {
          this.router.navigateByUrl('/orders');
        } else {
          this.error = 'Credenciais inválidas';
        }
      },
      error: () => (this.error = 'Falha no login'),
    });
  }
}
