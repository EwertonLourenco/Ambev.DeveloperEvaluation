import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface UserCreate {
  username: string;
  password: string;
  phone?: string;
  email?: string;
  status: number; // 0=inactive, 1=active
  role: number;   // 0=user, 1=admin
}

@Injectable({ providedIn: 'root' })
export class UsersService {
  private base = '/api/Users';
  constructor(private http: HttpClient) { }
  create(user: UserCreate) { return this.http.post<any>(this.base, user); }
  getById(id: string | number) { return this.http.get<any>(`${this.base}/${id}`); }
}
