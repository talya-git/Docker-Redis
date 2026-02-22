import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CustomerDetailsService } from 'src/app/Services/customer-details-service';
import { GiftService } from '../../Services/gift-service'; 
import { AuthService } from 'src/app/Services/auth-service';

// ייבוא מודולים של Material לעיצוב היוקרתי
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-customer-details',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    MatIconModule, 
    MatButtonModule, 
    MatTooltipModule
  ],
  templateUrl: './customer-details.html',
  styleUrl: './customer-details.scss',
})
export class CustomerDetails implements OnInit {
  private customerDatailsSrv = inject(CustomerDetailsService);
  private giftSrv = inject(GiftService); 
  public authSrv: AuthService = inject(AuthService); 

  arrDetailsByGiftId: any[] = []; 
  arrGifts: any[] = [];         
  selectedGiftId: number | null = null; 
  totalAmount: number = 0;

  ngOnInit() {
    this.getAll(); 
    this.loadAllGifts(); 
    
    // האזנה לרענון אוטומטי של הרשימה
    this.customerDatailsSrv.refreshList$.subscribe(() => {
      this.getAll();
    });
  }

  loadAllGifts() {
    this.giftSrv.getAll().subscribe({
      next: (res) => this.arrGifts = res,
      error: (err) => console.error("שגיאה בטעינת מתנות:", err)
    });
  }

  getAll() {
    this.selectedGiftId = null; 

    if (this.authSrv.isAdmin()) {
      this.customerDatailsSrv.get().subscribe((data) => {
        this.arrDetailsByGiftId = data; 
      });
    } else {
      this.customerDatailsSrv.GetMyPurchases().subscribe((data) => {
        this.arrDetailsByGiftId = data;
        this.GetTotalAmount(); // עדכון הסכום רק ללקוח
      });
    }
  }

  GetTotalAmount() {
    this.customerDatailsSrv.GetTotalAmount().subscribe({
      next: (data: any) => this.totalAmount = data,
      error: (err) => console.error("שגיאה בטעינת סכום כולל:", err)
    });
  }

  getDetailsByGiftId(id: number) {
    this.selectedGiftId = id; 
    this.customerDatailsSrv.getDetailsByGiftId(id).subscribe({
      next: (res) => this.arrDetailsByGiftId = res,
      error: (err) => console.error("שגיאה בטעינת פרטי מתנה:", err)
    });
  }

  ConfirmPurchase(id: number) {
    this.customerDatailsSrv.ConfirmPurchase(id).subscribe({
      next: () => {
        // במקום alert פשוט, אפשר להוסיף כאן לוגיקה של הודעה יפה
        this.getAll(); 
      },
      error: (err) => console.error("שגיאה באישור הרכישה", err)
    }); 
  }

  Delete(id: number) {
    if (confirm('האם להסיר פריט זה מהסל?')) {
      this.customerDatailsSrv.Delete(id).subscribe({
        next: () => this.getAll(),
        error: (err) => console.error("שגיאה במחיקה", err)
      }); 
    }
  }

  isCustomer(): boolean {
    return this.authSrv.isCustomer();
  }
}