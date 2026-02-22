import { Component, inject, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router'; // הוספנו RouterLinkActive לסימון דף פעיל
import { CommonModule } from '@angular/common';

// רכיבי הפרויקט שלך
import { GiftList } from './Components/gift-list/gift-list';
import { ListModor } from './Components/list-modor/list-modor';
import { ListCustomer } from './Components/list-customer/list-customer';

// Angular Material - פותר את השגיאות של ה-Navbar
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';   // חשוב! פותר את ה-mat-icon
import { MatBadgeModule } from '@angular/material/badge'; // חשוב! פותר את ה-matBadge
import { AuthService } from './Services/auth-service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet, 
    RouterLink, 
    RouterLinkActive, // מאפשר עיצוב קישור פעיל ב-Navbar
    GiftList, 
    ListModor, 
    ListCustomer, 
    MatSlideToggleModule, 
    MatButtonModule,
    MatIconModule,
    MatBadgeModule
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.scss'] 
})
export class App {
  protected readonly title = signal('Luxury Gifts Project');
    public authSrv = inject(AuthService);
    
   isCustomer(): boolean {
    return this.authSrv.isCustomer();
  }
}