import { Component, inject, OnInit } from '@angular/core';
import { CustomerService } from '../../Services/customer-service';
import { customerModel } from '../../Models/customerModel';
import { AuthService } from 'src/app/Services/auth-service';
import { CommonModule } from '@angular/common'; // הוספת ייבוא חיוני
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-list-customer',
  standalone: true,
  imports: [CommonModule,
    MatTableModule,   // עבור הטבלה
    MatIconModule,    // עבור האייקונים
    MatButtonModule,  // עבור הכפתורים
    MatTooltipModule
  ],
  templateUrl: './list-customer.html',
  styleUrl: './list-customer.scss',
})
export class ListCustomer implements OnInit {
  customerSrv = inject(CustomerService);
  public authSrv = inject(AuthService);

  // שינוי שם המערך לזה שמופיע ב-HTML
  arrModor: customerModel[] = []; 
  degel: number = 0;
  selectedId: number = -1;

  ngOnInit() {
    if (this.authSrv.isAdmin()) {
      this.customerSrv.getAll().subscribe(data => {
        this.arrModor = data;
      });
    }
  }

  removeCustomer(id: number) {
    console.log('מחיקה לא פעילה כרגע');
  }

  updateModor(id: number) {
    this.degel = id;
    this.selectedId = id;
  }

  closeEdit(id: number) {
    this.selectedId = -1;
    this.degel = 0;
  }
}