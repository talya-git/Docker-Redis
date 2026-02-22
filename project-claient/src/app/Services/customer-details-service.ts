import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { CustomerDetails } from '../Models/CustomerDetails';

@Injectable({
  providedIn: 'root',
})
export class CustomerDetailsService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7289/api/CustomerDetails';
  public refreshList$ = new Subject<void>();

getDetailsByGiftId(giftId: number) {
return this.http.get<any[]>(`${this.apiUrl}/by-gift/${giftId}`);
}

get() {
  return this.http.get<CustomerDetails[]>(`${this.apiUrl}/all`);
}

ConfirmPurchase(id: number) {
  return this.http.get<any[]>(`${this.apiUrl}/ConfirmPurchase?id=${id}`);
}

  add(c: CustomerDetails): Observable<CustomerDetails> {
     return this.http.post<CustomerDetails>(this.apiUrl, c);
  }
 Delete(id: number): Observable<CustomerDetails> {
  return this.http.delete<CustomerDetails>(`${this.apiUrl}/${id}`);
}
GetMyPurchases(): Observable<CustomerDetails[]> {
  return this.http.get<CustomerDetails[]>(`${this.apiUrl}/GetMyPurchases`);
  }

GetTotalAmount(): Observable<number> { 
  return this.http.get<number>(`${this.apiUrl}/GetTotalAmount`);
}
}
