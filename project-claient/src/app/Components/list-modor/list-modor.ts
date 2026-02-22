import { Component, inject, OnInit } from '@angular/core';
import { ModorService } from '../../Services/modor-service';
import { modorModel } from '../../Models/modorModel';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { FormsModule } from '@angular/forms';
import { AuthService } from 'src/app/Services/auth-service';
import { CommonModule } from '@angular/common'; 
import { MatChipsModule } from '@angular/material/chips';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BidiModule } from '@angular/cdk/bidi';

@Component({
  selector: 'app-list-modor',
  standalone: true, 
  imports: [
    MatTableModule, 
    MatButtonModule, 
    MatIconModule,
    FormsModule,
    CommonModule,
    MatChipsModule ,
    MatChipsModule,
    MatCardModule,       
    MatFormFieldModule,  
    MatInputModule,
    BidiModule
  ],
  templateUrl: './list-modor.html',
  styleUrl: './list-modor.scss',
})
export class ListModor implements OnInit {
  private modorSrv = inject(ModorService);
  public authSrv = inject(AuthService);

  displayedColumns: string[] = ['id', 'fullName', 'contact', 'address', 'gifts', 'actions', 'email'];
  arrModor: modorModel[] = [];
  donor: modorModel | null = null;
  selectedId: number = -1;
  degel: number = 0;

  ngOnInit() {
    // טעינת נתונים רק אם המשתמש מנהל
    if (this.authSrv.isAdmin()) {
      this.getAll(); 

      this.modorSrv.refreshList$.subscribe(() => {
        this.selectedId = -1; 
        this.degel = 0;
        this.getAll();
      });
    }
  }

  getAll() {
    if (!this.authSrv.isAdmin()) return; // הגנת הרשאה
    this.modorSrv.getAll().subscribe((data) => {
      this.arrModor = data;
    });
  }

  removeModor(m: modorModel) {
    if (!this.authSrv.isAdmin()) {
      alert("אין לך הרשאה למחוק תורמים");
      return;
    }
    if (confirm(`האם את בטוחה שברצונך למחוק את ${m.firstName}?`)) {
      this.modorSrv.remove(m.id).subscribe(() => {
        this.arrModor = this.arrModor.filter(c => c.id !== m.id);
      });
    }
  }

  updateModor(g: modorModel) {
    if (!this.authSrv.isAdmin()) return;
    this.degel = g.id;
    this.donor = { ...g };
  }

  saveUpdate() {
    if (this.donor && this.authSrv.isAdmin()) {
      this.modorSrv.update(this.donor).subscribe(() => {
        this.closeEdit();
        this.getAll(); 
      });
    }
  }

  closeEdit() {
    this.degel = 0;
    this.donor = null;
  }

  // פונקציות חיפוש מוגנות
  getByName(name: string) {
    if (!this.authSrv.isAdmin()) return;
    this.modorSrv.getByName(name).subscribe((res) => {
      this.arrModor = res ? [res] : []; 
    });
  }

  getByEmail(email: string) {
    if (!this.authSrv.isAdmin()) return;
    this.modorSrv.getByEmail(email).subscribe((res) => {
      this.arrModor = res ? [res] : []; 
    });
  }

  getByGift(gift: string) {
    if (!this.authSrv.isAdmin()) return;
    this.modorSrv.getByGift(gift).subscribe((res) => {
      this.arrModor = res ? res : []; 
    });
  }
}