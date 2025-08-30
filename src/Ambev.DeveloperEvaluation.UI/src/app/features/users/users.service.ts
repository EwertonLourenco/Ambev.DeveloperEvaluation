import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface CreateUser {
  username: string;
  password: string;
  phone?: string;
  email?: string;
  status?: number;
  role?: number;
}

@Injectable({ providedIn: 'root' })
export class UsersService {
  constructor(private http: HttpClient) { }

  create(body: CreateUser) {
    return this.http.post(`/api/Users`, body);
  }

  getById(id: number) {
    return this.http.get(`/api/Users/${id}`);
  }
}
