import { CommonModule } from '@angular/common';
import { Component, inject, Input, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ModorService } from '../../Services/modor-service';
import { AuthService } from 'src/app/Services/auth-service'; 
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-modor',
  standalone: true,
  imports: [ReactiveFormsModule,
     CommonModule,
MatCardModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
  MatFormFieldModule],
  templateUrl: './modor.html',
  styleUrl: './modor.scss',
})
export class modor {
  modorSrv: ModorService = inject(ModorService);
  authSrv: AuthService = inject(AuthService); 

  frmModor: FormGroup = new FormGroup({
    id: new FormControl(0),
    firstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    phone: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.email]),
    address: new FormControl('') 
  });

  @Input() id: number = -1;

  ngOnChanges(c: SimpleChanges) {
    if (c['id'] && this.id > 0) {
      this.modorSrv.getById(this.id).subscribe(modorFromServer => {
        if (modorFromServer) {
          this.frmModor.patchValue({
            id: modorFromServer.id,
            firstName: modorFromServer.firstName,
            lastName: modorFromServer.lastName,
            phone: modorFromServer.phone,
            email: modorFromServer.email,
            address: modorFromServer.address 
          });
        }
      });
    } else {
      this.frmModor.reset({ id: 0 });
    }
  }

  addModor() {
    // בדיקת הרשאה לפני שליחה
    if (!this.authSrv.isAdmin()) {
      alert('אין לך הרשאה לביצוע פעולה זו');
      return;
    }

    if (this.frmModor.invalid) {
      this.frmModor.markAllAsTouched();
      return;
    }

    const donorToSave = this.frmModor.value;

    if (this.id <= 0) {
      this.modorSrv.add(donorToSave).subscribe(() => {
        alert('נוסף בהצלחה');
        this.modorSrv.refreshList$.next();
      });
    } else {
      this.modorSrv.update(donorToSave).subscribe(() => {
        alert('עודכן בהצלחה');
        this.modorSrv.refreshList$.next();
      });
    }
  }
}