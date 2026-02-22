import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Customer } from '../Components/customer/customer';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
    private readonly apiUrl = 'https://localhost:7289/api/Auth';

     Register(c: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, c);
}


Login(userName: string, password: string): Observable<any> {
  return this.http.get(`${this.apiUrl}/login?userName=${userName}&password=${password}`);
}
getRole(): string | null {
    return localStorage.getItem('userRole'); 
  }

isAdmin(): boolean {
  const role = localStorage.getItem('userRole');
  return role?.toLowerCase().trim() === 'manager';
}

  isCustomer(): boolean {
    return this.getRole() === 'user';
  }

  logout() {
    localStorage.clear();
  }
}
