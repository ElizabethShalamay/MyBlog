import { Component, OnInit } from '@angular/core';
import { RegisterModel } from "../../models/register-model";
import { AccountService } from "../../services/account/account.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  email: string;
  password: string;
  confirmPassword: string;

  constructor(private accService: AccountService) { }

  onLoginInput(value: string) {
    this.email = value;
  }

  onPasswordInput(value: string) {
    this.password = value;
  }

  onConfirmPasswordInput(value: string) {
    this.confirmPassword = value;
  }

  onRegisterClick(){
    const registerModel: RegisterModel = {
      email : this.email,
      password : this.password,
      confirmPassword: this.confirmPassword
    };

    this.accService.register(registerModel).subscribe();   
  }

  ngOnInit() {
  }

}