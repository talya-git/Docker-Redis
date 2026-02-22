import { Component, inject, OnInit } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { giftModel } from '../../Models/GiftModel';
import { GiftService } from '../../Services/gift-service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CustomerDetailsService } from 'src/app/Services/customer-details-service';
import { AuthService } from 'src/app/Services/auth-service';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-gift-list',
  templateUrl: './gift-list.html',
  styleUrls: ['./gift-list.scss'],
  standalone: true,
  imports: [
    MatDialogModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    FormsModule,
    CommonModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatToolbarModule, 
    RouterLink,
    MatFormFieldModule,
    MatInputModule
  ]
})
export class GiftList implements OnInit {
  giftSrv: GiftService = inject(GiftService);
  private customerDatailsSrv = inject(CustomerDetailsService);
  public authSrv: AuthService = inject(AuthService);
  
  displayedColumns: string[] = ['id', 'name', 'donorName', 'priceCard', 'category', 'actions'];
  arrGift: giftModel[] = [];
  gift: giftModel = {} as giftModel;
  degel: number = 0;
  showAddForm: boolean = false; 

  // --- משתני הסינון החדשים (כאן נפתרת השגיאה שלך) ---
  filterName: string = '';
  filterDonor: string = '';
  filterCount: number = 0;
  // -----------------------------------------------

  newPurchase: any = {
    customerId: null,
    giftId: null,
    quntity: 1
  };

  ngOnInit() {
    this.loadAllGifts();
  }

  loadAllGifts() {
    // איפוס שדות הסינון בטעינה מחדש
    this.filterName = '';
    this.filterDonor = '';
    this.filterCount = 0;

    this.giftSrv.getAll().subscribe(data => {
      this.arrGift = data;
      console.log('All Gifts:', this.arrGift);
    });
  }

  removeGift(g: giftModel) {
    if (!this.authSrv.isAdmin()) return;
    if (confirm(`האם למחוק את ${g.name}?`)) {
      this.giftSrv.remove(g.id).subscribe(() => {
        this.arrGift = this.arrGift.filter(c => c.id != g.id);
      });
    }
  }

  hasWinner(g: giftModel): boolean {
    if (!g.customerName) return false;
    const name = g.customerName.toLowerCase().trim();
    const invalidValues = ['null', '', 'אין זוכה עדיין', 'string', 'undefined'];
    return !invalidValues.includes(name) && !name.includes('string');
  }

  updateGift(g: giftModel) {
    this.degel = g.id;
    this.gift = { ...g };
    this.showAddForm = true; 
  }

  saveUpdate() {
    if (this.gift) {
      this.giftSrv.update(this.gift).subscribe(() => {
        this.closeEdit();
        this.loadAllGifts(); 
      });
    }
  }

  closeEdit() {
    this.degel = 0;
    this.gift = {} as giftModel; 
    this.showAddForm = false;    
  }

  getById(id: number) {
    this.giftSrv.getById(id).subscribe((data) => {
      if (data) {
        this.arrGift = [data];
      }
    });
  }

  getByName(name: string) {
    if (!name) {
      this.loadAllGifts();
      return;
    }
    this.giftSrv.getByName(name).subscribe((data) => {
      this.arrGift = data ? [data] : [];
    });
  }

  getByDonor(name: string) {
    if (!name) {
      this.loadAllGifts();
      return;
    }
    this.giftSrv.getByDonor(name).subscribe((data) => {
      this.arrGift = data || [];
    });
  }

  GetByPurchasesCount(num: number) {
    this.giftSrv.GetByPurchasesCount(num).subscribe((data) => {
      this.arrGift = data || [];
    });
  }

  giftExpensive() {
    this.giftSrv.giftExpensive().subscribe((data) => {
      this.arrGift = data || [];
    });
  }

  addToCart(g: giftModel) {
    if (!this.authSrv.isCustomer()) {
      alert("רק לקוחות רשומים יכולים להוסיף לסל!");
      return;
    }
    
    this.newPurchase = {
      customerId: 1, 
      giftId: g.id,
      quntity: 1, 
      status: 'Draft',
      customerName: "", 
      giftName: ""
    };
    
    this.customerDatailsSrv.add(this.newPurchase).subscribe({
      next: () => alert("נוסף לסל בהצלחה!"),
      error: (err) => {
        console.log("פירוט שגיאה:", err);
        alert("שגיאה בהוספה לסל");
      }
    });
  }

  Winner(giftId: number) {
    if (!this.authSrv.isAdmin()) return;
    this.giftSrv.Winner(giftId).subscribe({
      next: (res) => {
        if (res && res.winnerFullName) {
          alert(`🎉 יש לנו זוכה! \nהזוכה במתנה "${res.giftName}" הוא/היא: ${res.winnerFullName}`);
          this.loadAllGifts(); // רענון כדי לראות את הזוכה על הכרטיס
        } else {
          alert("לא נמצא זוכה למתנה זו.");
        }
      },
      error: (err) => {
        alert("פעולת ההגרלה נכשלה.");
      }
    });
  }
}