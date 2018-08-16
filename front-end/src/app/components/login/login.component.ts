import { Component, OnInit } from '@angular/core';
import { TokenParams } from '../../models/token-params';
import { LoginModel } from "../../models/login-model";
import { AccountService } from '../../services/account/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  login: string;
  password: string;
  tokenParam: TokenParams;


  constructor(private router: Router,
    private accService: AccountService) { }

  onLoginInput(value: string) {
    this.login = value;
  }
  onPasswordInput(value: string) {
    this.password = value;
  }

  onSignInClick() {
    const loginModel: LoginModel = {
      username: this.login,
      password: this.password
    };

    this.accService.signIn(loginModel)
      .subscribe(data => {
        this.tokenParam = data;
        this.accService.AccessToken = data.access_token;
        localStorage.setItem("Authorization", data.access_token);

        this.accService.authentication.userName = this.login;
        this.accService.authentication.isAuth = true;
        console.log(this.accService.authentication.isAuth);

        this.accService.getLoggedIn.emit(true);
        if (this.accService.authentication.userName == 'admin') {
          this.router.navigate(['/admin']);
        }
        else { this.router.navigate(['/posts']); }
      });
  }

  ngOnInit() {
  }

}
