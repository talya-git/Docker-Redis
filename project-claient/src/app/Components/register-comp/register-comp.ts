import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from 'src/app/Services/auth-service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-register-comp',
  standalone: true, 
  imports: [
    ReactiveFormsModule, CommonModule, RouterLink,
    MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule
  ], 
  templateUrl: './register-comp.html',
  styleUrl: './register-comp.scss',
})
export class RegisterComp implements OnInit { 
  private fb = inject(FormBuilder);
  public authSrv = inject(AuthService);
  private router = inject(Router); 

  registerForm!: FormGroup;

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      Id: [null, [Validators.required]], 
      FirstName: ['', [Validators.required, Validators.minLength(2)]], 
      LastName: ['', [Validators.required]], 
      Phone: ['', [Validators.required]], 
      Email: ['', [Validators.required, Validators.email]],
      Address: ['', [Validators.required]], // השדה חזר להיות חובה בטופס
      UserName: ['', [Validators.required, Validators.minLength(4)]],
      Password: ['', [Validators.required, Validators.minLength(6)]],
      Role: ['Customer'] 
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      this.register(this.registerForm.value);
    } else {
      this.registerForm.markAllAsTouched(); 
      alert("נא למלא את כל השדות, כולל כתובת.");
    }
  }

  register(customerData: any) {
    this.authSrv.Register(customerData).subscribe({
      next: (data: any) => {
        alert("נרשמת בהצלחה! מעביר אותך לדף ההתחברות.");
        this.router.navigate(['/login']); 
      },
      error: (err) => {
        console.error("Register error:", err);
        alert("ההרשמה נכשלה. בדקי שהנתונים תקינים.");
      }
    });
  }
}