import { inject, Injectable } from '@angular/core';
import { giftModel } from '../Models/GiftModel';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GiftService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7289/api/Gift';
  public refreshList$ = new Subject<void>();

  getAll(): Observable<giftModel[]> {
    return this.http.get<giftModel[]>(this.apiUrl);
  }

  add(g: giftModel): Observable<any> {
    return this.http.post(this.apiUrl, g);
  }

  remove(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getById(id: number): Observable<giftModel> {
    return this.http.get<giftModel>(`${this.apiUrl}/${id}`);
  }
  getByName(name: string): Observable<giftModel> {
    const params = new HttpParams().set('name', name);
    return this.http.get<giftModel>(`${this.apiUrl}/by-name`, { params });
}
  getByDonor(name: string): Observable<giftModel[]> {
    const params = new HttpParams().set('name', name);
    return this.http.get<giftModel[]>(`${this.apiUrl}/by-donor`, { params });
}
 GetByPurchasesCount(num: number): Observable<giftModel[]> {
    const params = new HttpParams().set('num', num);
    return this.http.get<giftModel[]>(`${this.apiUrl}/by-parches-count`, { params });
}

  update(g: giftModel): Observable<any> {
    return this.http.put(`${this.apiUrl}/${g.id}`, g);
  }

giftExpensive() {
  return this.http.get<giftModel[]>(`${this.apiUrl}/giftExpensive`);
}
Winner(giftId: number): Observable<any> {
  return this.http.post<any>(`${this.apiUrl}/winner/${giftId}`, {});
}
reportWinners() {
  return this.http.get<any>(`${this.apiUrl}/reportWinners`);

}
reportAchnasot(){
  return this.http.get<any>(`${this.apiUrl}/reportAchnasot`);
}

}