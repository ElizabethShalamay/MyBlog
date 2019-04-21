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
  badRequest: string = "";

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
        if (!this.accService.bagRequest) {
          this.tokenParam = data;

          this.accService.getLoggedIn.emit(true);
          if (this.accService.authentication.userName == 'admin') {
            this.router.navigate(['/admin']);
          }
          else { 
            this.router.navigate(['/posts']); 
          }
        }
      },
        error => {
          this.badRequest = error.error.error_description;
          this.password = "";
        }
      );
  }

  ngOnInit() {
    if (sessionStorage.getItem("Authorization")) {
        this.accService.authentication.isAuth = true;
        this.router.navigate(["/posts"]);
    }
  }
}
