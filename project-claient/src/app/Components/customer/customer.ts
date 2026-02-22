import { Component, inject, Input, SimpleChanges } from '@angular/core';
import { ModorService } from '../../Services/modor-service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { customerModel } from '../../Models/customerModel';
import { CommonModule } from '@angular/common';
import { CustomerService } from '../../Services/customer-service';

@Component({
  selector: 'app-customer',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './customer.html',
  styleUrl: './customer.scss',
})
export class Customer {
 frmCostomer: FormGroup = new FormGroup({
    id: new FormControl(0, [Validators.required, Validators.max(99999999)]),
    firstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    phone: new FormControl('', [Validators.required, Validators.minLength(9)]),
    email: new FormControl('', [Validators.required, Validators.minLength(7)]),
    adress: new FormControl('', [Validators.required, Validators.minLength(5)]),
    userName:new FormControl('', [Validators.required, Validators.minLength(3)]),
    password:new FormControl('', [Validators.required, Validators.minLength(3)])
  });

castomerSrv:CustomerService = inject(CustomerService);

@Input()
id:number=-1;


ngOnChanges(c: SimpleChanges) {
  if (c['id'] && this.id > 0) {
    this.castomerSrv.getById(this.id).subscribe(data => {
      if (data) {
        this.frmCostomer.setValue({
          id: data.id,
          firstName: data.firstName,
          lastName: data.lastName,
          phone: data.phone,
          email: data.email,
          adress: data.address,
          password:data.password,
          userName:data.userName 
        });
      }
    });
  }
}

addCustomer(){
   
    let castomer: customerModel = new customerModel();
    castomer.id = this.frmCostomer.controls['id'].value;
    castomer.firstName = this.frmCostomer.controls['firstName'].value;
    castomer.lastName = this.frmCostomer.controls['lastName'].value;
    castomer.phone = this.frmCostomer.controls['phone'].value;
    castomer.address = this.frmCostomer.controls['adress'].value;
    castomer.email = this.frmCostomer.controls['email'].value;
    castomer.password = this.frmCostomer.controls['password'].value;
    castomer.userName = this.frmCostomer.controls['userName'].value;



    if(castomer.id == 0) {
    
        this.castomerSrv.register(castomer).subscribe(res => {
            alert("הלקוח נוסף בהצלחה!");
        });
    }
   
    

  }

}
