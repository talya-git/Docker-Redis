import { Injectable, inject } from '@angular/core';
import { customerModel } from '../Models/customerModel';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7289/api/Customer';
  private readonly apiUrl2 = 'https://localhost:7289/api/Auth/register';

  getAll(): Observable<customerModel[]> {
    return this.http.get<customerModel[]>(this.apiUrl);
  }

  getById(id: number): Observable<customerModel> {
    return this.http.get<customerModel>(`${this.apiUrl}/${id}`);
  }
  register(m: customerModel): Observable<customerModel> {
    return this.http.post<customerModel>(this.apiUrl2, m);
  }

    login(): Observable<customerModel[]> {
    return this.http.get<customerModel[]>(this.apiUrl2);
  }
 

  // remove(id: number): Observable<void> {
  //   return this.http.delete<void>(`${this.apiUrl}/${id}`);
  // }
}