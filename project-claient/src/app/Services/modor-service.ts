import { inject, Injectable } from '@angular/core';
import { modorModel } from '../Models/modorModel';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ModorService {
  private http = inject(HttpClient);
  // וודאי שהכתובת הזו נכונה (Donor או Modor?)
  private readonly apiUrl = "https://localhost:7289/api/Donor";
public refreshList$ = new Subject<void>();
  getAll(): Observable<modorModel[]> {
    return this.http.get<modorModel[]>(this.apiUrl);
  }

  add(m: modorModel): Observable<modorModel> {
    return this.http.post<modorModel>(this.apiUrl, m);
  }

  remove(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getById(id: number): Observable<modorModel> {
    return this.http.get<modorModel>(`${this.apiUrl}/${id}`);
  }

update(g: any): Observable<any> {
  const id = g.Id || g.id; 
  return this.http.put<any>(`${this.apiUrl}/${id}`, g);
}
getByName(name: string): Observable<modorModel> {
  const params = new HttpParams().set('name', name);
  
  
  return this.http.get<modorModel>(`${this.apiUrl}/by-name`, { params });
}

getByEmail(email: string): Observable<modorModel> {
  const params = new HttpParams().set('email',email);
  return this.http.get<modorModel>(`${this.apiUrl}/by-email`, { params });
}
getByGift(name: string): Observable<modorModel[]> {
  const params = new HttpParams().set('name',name);
  return this.http.get<modorModel[]>(`${this.apiUrl}/by-gift`, { params });
}
 }