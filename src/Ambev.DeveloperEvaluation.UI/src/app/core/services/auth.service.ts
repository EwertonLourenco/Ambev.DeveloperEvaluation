import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

type AuthResponse = { token?: string } | { data?: { token?: string } };

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient) { }

  login(email: string, password: string) {
    // Com o BaseUrlInterceptor, a URL relativa será prefixada com API_BASE_URL
    return this.http.post<AuthResponse>('/api/Auth', { email, password });
  }

  saveToken(resp: AuthResponse) {
    const token = (resp as any)?.data?.token || (resp as any)?.token;
    if (token) localStorage.setItem('token', token);
    return !!token;
  }

  logout() {
    localStorage.removeItem('token');
  }
}
