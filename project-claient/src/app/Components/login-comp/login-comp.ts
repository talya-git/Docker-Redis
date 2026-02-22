import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Services/auth-service';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

// ייבוא רכיבי Angular Material
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-login-comp',
  standalone: true, 
  // הוספת המודולים של Material לכאן
  imports: [
    ReactiveFormsModule, 
    CommonModule, 
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ], 
  templateUrl: './login-comp.html',
  styleUrl: './login-comp.scss',
})
export class LoginComp implements OnInit {
  private fb = inject(FormBuilder);
  public authSrv = inject(AuthService); 
  private router = inject(Router);
  
  loginForm!: FormGroup;

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      UserName: ['', [Validators.required]],
      Password: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const { UserName, Password } = this.loginForm.value;
      this.login(UserName, Password);
    } else {
      this.loginForm.markAllAsTouched();
    }
  }

  login(userName: string, password: string) {
    this.authSrv.Login(userName, password).subscribe({
      next: (data: any) => {
        // חילוץ נתונים עם הגנה ממקרים של Null/Undefined
        const token = data?.token || data?.Token;
        const role = data?.role || data?.Role;
        const name = data?.firstName || data?.FirstName || userName;

        if (token) {
          localStorage.setItem('token', token);
          localStorage.setItem('userRole', role || 'User');
          localStorage.setItem('userName', name);

          alert(`שלום ${name}, התחברת בהצלחה!`);
          this.router.navigate(['/home']); 
        } else {
          alert("שגיאה: לא התקבל טוקן מהשרת");
        }
      },
      error: (err) => {
        console.error("Login Error:", err);
        alert("שם משתמש או סיסמה שגויים");
      }
    });
  }
}